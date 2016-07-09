using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using Wyam.Common.Documents;
using Wyam.Common.Execution;
using Wyam.Common.Modules;

using Borogove.DataAccess;
using Borogove.Model;
using Names = Borogove.WorkMetadataCanonicalNames;
using AuthorizationNames = Borogove.AuthorizationMetadataNames;

using static Borogove.MetadataHelpers;

namespace Borogove
{
    public class WorkMetadataPersister : IModule
    {
        public const string DefaultDatabaseName = "name=WorkContext";

        private readonly Dictionary<Guid, WorkEntity> _workDictionary = new Dictionary<Guid, WorkEntity>();
        private readonly string _nameOrConnectionString;

        public WorkMetadataPersister() : this(DefaultDatabaseName)
        {
        }

        public WorkMetadataPersister(string nameOrConnectionString)
        {
            _nameOrConnectionString = nameOrConnectionString;
        }

        public IEnumerable<IDocument> Execute(IReadOnlyList<IDocument> inputs, IExecutionContext context)
        {
            var finalProcessedDocuments = new List<IDocument>();
            using (var workContext = new WorkContext(_nameOrConnectionString))
            {
                foreach (var document in inputs)
                {
                    Dictionary<string, object> newMetadata = new Dictionary<string, object>();

                    Guid? workGuid = document
                        .SingleOrDefault(kvp => CanonicalizeString(kvp.Key).Equals(Names.Identifier) && kvp.Value is Guid)
                        .Value as Guid?;
                    if (!workGuid.HasValue)
                    {
                        workGuid = Guid.NewGuid();
                        newMetadata.Add(Names.Identifier, workGuid.Value);
                    }

                    WorkEntity work = workContext.Works.Find(workGuid.Value);
                    if (work == null)
                    {
                        work = workContext.Works.Add(new WorkEntity(workGuid.Value));
                    }
                    else if (workContext.Entry(work).State != EntityState.Added)
                    {
                        workContext.Entry(work).Collection(w => w.WorkCreators).Load();
                        workContext.Entry(work).Reference(w => w.Language).Load();
                        workContext.Entry(work).Collection(w => w.Tags).Load();
                        workContext.Entry(work).Reference(w => w.Parent).Load();
                        workContext.Entry(work).Collection(w => w.PreviousWorks).Load();
                        workContext.Entry(work).Collection(w => w.NextWorks).Load();
                        workContext.Entry(work).Reference(w => w.DraftOf).Load();
                        workContext.Entry(work).Reference(w => w.ArtifactOf).Load();
                        workContext.Entry(work).Reference(w => w.CommentsOn).Load();
                        workContext.Entry(work).Collection(w => w.WhitelistEntries).Load();
                    }

                    work.Path = document.Get<string>(Names.Path, null);
                    work.Content = document.Content;
                    UpdateWorkEntityFromMetadata(work, document, workContext);
                    UpdateAuhtorizationEntitiesFromMetadata(work, document, workContext);
                    finalProcessedDocuments.Add(context.GetDocument(document, newMetadata));
                }

                workContext.SaveChanges();
            }

            return finalProcessedDocuments;
        }

        private static void UpdateWorkEntityFromMetadata(WorkEntity work, IEnumerable<KeyValuePair<string, object>> metadata, WorkContext workContext)
        {
            if (work == null)
            {
                throw new ArgumentNullException(nameof(work));
            }

            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            if (workContext == null)
            {
                throw new ArgumentNullException(nameof(workContext));
            }

            foreach (var keyValuePair in metadata)
            {
                string canonicalizedKey = CanonicalizeString(keyValuePair.Key);
                object value = keyValuePair.Value;
                switch (canonicalizedKey)
                {
                    case Names.Title:
                        work.Title = value as string ?? string.Empty;
                        continue;

                    case Names.Description:
                        work.Description = value as string ?? string.Empty;
                        continue;

                    case Names.Creator:
                        var metadataCreators = value as IEnumerable<Creator> ?? new List<Creator>();

                        var workCreatorEntities = new List<WorkCreatorEntity>();
                        foreach (Creator creator in metadataCreators)
                        {
                            var creatorName = string.IsNullOrEmpty(creator.FileAs) ? creator.Text : creator.FileAs;
                            creatorName = string.IsNullOrEmpty(creatorName) ?
                                CreatorInfoEntity.AnonymousName :
                                creatorName;
                            var creatorInfo =
                                workContext.Creators.Find(creatorName) ??
                                workContext.Creators.Add(new CreatorInfoEntity(creator));

                            var workedAsName = string.IsNullOrEmpty(creator.Text) ? creatorName : creator.Text;
                            CreatorAliasEntity aliasEntity =
                                creatorInfo.Aliases.FirstOrDefault(a => a.Alias.Equals(workedAsName));
                            if (aliasEntity == null && !workedAsName.Equals(creatorName))
                            {
                                aliasEntity = workContext.CreatorAliases.Find(workedAsName) ??
                                    workContext.CreatorAliases.Add(new CreatorAliasEntity(workedAsName, creatorName));
                                creatorInfo.Aliases.Add(aliasEntity);
                            }

                            var workCreator = workContext.WorkCreators
                                .Find(work.Identifier, creatorName, creator.Role, workedAsName) ??
                                workContext.WorkCreators.Add(
                                    new WorkCreatorEntity(work, creatorInfo, creator.Role, aliasEntity?.Alias));
                            workCreatorEntities.Add(workCreator);
                        }

                        work.WorkCreators = workCreatorEntities;
                        continue;

                    case Names.CreatedDate:
                        work.CreatedDate = value is DateTime ? (DateTime)value : WorkEntity.MinimumDateTime;
                        continue;

                    case Names.ModifiedDate:
                        work.ModifiedDate = value is DateTime ? (DateTime)value : WorkEntity.MinimumDateTime;
                        continue;

                    case Names.PublishedDate:
                        work.PublishedDate = value is DateTime ? (DateTime)value : WorkEntity.MinimumDateTime;
                        continue;

                    case Names.Rights:
                        work.Rights = value as string ?? string.Empty;
                        continue;

                    case Names.License:
                        work.License = (value is License && Enum.IsDefined(typeof(License), value)) ? (License)value : License.None;
                        continue;

                    case Names.Language:
                        var language = value as CultureInfo ?? CultureInfo.CurrentCulture;
                        work.Language = workContext.Languages.Find(language.Name.ToLowerInvariant()) ??
                            workContext.Languages.Add(new LanguageEntity(language));
                        continue;

                    case Names.WorkType:
                        work.WorkType = (value is WorkType && Enum.IsDefined(typeof(WorkType), value)) ? (WorkType)value : WorkType.Other;
                        continue;

                    case Names.ContentRating:
                        work.ContentRating = (value is ContentRating && Enum.IsDefined(typeof(ContentRating), value)) ? (ContentRating)value : ContentRating.NotRated;
                        continue;

                    case Names.ContentDescriptors:
                        work.ContentDescriptor = (value is ContentDescriptor && Enum.IsDefined(typeof(ContentDescriptor), value)) ? (ContentDescriptor)value : ContentDescriptor.None;
                        continue;

                    case Names.Tags:
                        var tags = value as IEnumerable<Tag> ?? new List<Tag>();
                        var tagEntities = new List<TagEntity>();
                        foreach(Tag tag in tags)
                        {
                            var tagEntity = workContext.Tags.Find(tag.Name);
                            if (tagEntity == null)
                            {
                                tagEntity = workContext.Tags.Add(new TagEntity(tag.Name));
                            }
                            else if (workContext.Entry(tagEntity).State != EntityState.Added)
                            {
                                workContext.Entry(tagEntity).Collection(t => t.Aliases).Load();
                                workContext.Entry(tagEntity).Collection(t => t.Implications).Load();
                            }

                            var tagAliases = new List<TagAliasEntity>();
                            foreach (string alias in tag.Aliases)
                            {
                                var aliasEntity = workContext.TagAliases.Find(alias) ?? workContext.TagAliases.Add(new TagAliasEntity(tag.Name, alias));
                                tagAliases.Add(aliasEntity);
                            }
                            tagEntity.Aliases = tagAliases;

                            var tagImplications = new List<TagEntity>();
                            foreach (Tag implication in tag.Implications)
                            {
                                var implicationEntity = workContext.Tags.Find(implication.Name) ?? workContext.Tags.Add(new TagEntity(implication.Name));
                                tagImplications.Add(implicationEntity);
                                // We assume that the original tag list includes the implication, so we'll get around to setting its aliases and implications eventually.
                            }
                            tagEntity.Implications = tagImplications;

                            tagEntities.Add(tagEntity);
                        }

                        work.Tags = tagEntities;
                        continue;

                    case Names.Parent:
                        var parentGuid = value as Guid?;
                        if (parentGuid.HasValue)
                        {
                            work.Parent = workContext.Works.Find(parentGuid.Value) ??
                                workContext.Works.Add(new WorkEntity(parentGuid.Value));
                        }
                        else
                        {
                            work.Parent = null;
                        }
                        continue;

                    case Names.Previous:
                        var previousWorks = value as IEnumerable<Guid>;
                        if (work.PreviousWorks == null)
                        {
                            work.PreviousWorks = new List<WorkEntity>();
                        }

                        if (previousWorks == null)
                        {
                            work.PreviousWorks.Clear();
                        }
                        else
                        {
                            foreach (Guid previousWorkGuid in previousWorks)
                            {
                                var previousWorkEntity = workContext.Works.Find(previousWorkGuid) ??
                                    workContext.Works.Add(new WorkEntity(previousWorkGuid));
                                work.PreviousWorks.Add(previousWorkEntity);
                            }
                        }
                        continue;

                    case Names.Next:
                        var nextWorks = value as IEnumerable<Guid>;
                        if (work.NextWorks == null)
                        {
                            work.NextWorks = new List<WorkEntity>();
                        }

                        if (nextWorks == null)
                        {
                            work.NextWorks.Clear();
                        }
                        else
                        {
                            foreach (Guid nextWorkGuid in nextWorks)
                            {
                                var nextWorkEntity = workContext.Works.Find(nextWorkGuid) ??
                                    workContext.Works.Add(new WorkEntity(nextWorkGuid));
                                work.NextWorks.Add(nextWorkEntity);
                            }
                        }
                        continue;

                    case Names.DraftOf:
                        var draftOfGuid = value as Guid?;
                        if (draftOfGuid.HasValue)
                        {
                            work.DraftOf = workContext.Works.Find(draftOfGuid.Value) ??
                                workContext.Works.Add(new WorkEntity(draftOfGuid.Value));
                        }
                        else
                        {
                            work.DraftOf = null;
                        }
                        continue;

                    case Names.ArtifactOf:
                        var artifactOfGuid = value as Guid?;
                        if (artifactOfGuid.HasValue)
                        {
                            work.ArtifactOf = workContext.Works.Find(artifactOfGuid.Value) ??
                                workContext.Works.Add(new WorkEntity(artifactOfGuid.Value));
                        }
                        else
                        {
                            work.ArtifactOf = null;
                        }
                        continue;

                    case Names.CommentsOn:
                        var commentsOnGuid = value as Guid?;
                        if (commentsOnGuid.HasValue)
                        {
                            work.CommentsOn = workContext.Works.Find(commentsOnGuid.Value) ??
                                workContext.Works.Add(new WorkEntity(commentsOnGuid.Value));
                        }
                        else
                        {
                            work.CommentsOn = null;
                        }
                        continue;

                    case Names.DraftIdentifier:
                        work.DraftIdentifier = value as string ?? string.Empty;
                        continue;

                    default:
                        // Ignore anything else
                        continue;
                }
            }
        }
        private static void UpdateAuhtorizationEntitiesFromMetadata(WorkEntity work, IDocument document, WorkContext workContext)
        {
            if (work == null)
            {
                throw new ArgumentNullException(nameof(work));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (workContext == null)
            {
                throw new ArgumentNullException(nameof(workContext));
            }

            var whitelistedUsers = document
                .FirstOrDefault(kvp => CanonicalizeString(kvp.Key).Equals(AuthorizationNames.WhitelistedUsers))
                .Value as IEnumerable<string> ?? new List<string>();
            var whitelistedGroups = document
                .FirstOrDefault(kvp => CanonicalizeString(kvp.Key).Equals(AuthorizationNames.WhitelistedGroups))
                .Value as IEnumerable<string> ?? new List<string>();

            var whitelistedUserEntities = whitelistedUsers
                .Select(u => workContext.WorkWhitelistEntries.Find(work.Identifier, SubjectType.User, u) ??
                            new WorkWhitelistEntryEntity(work, SubjectType.User, u));
            var whitelistedGroupEntities = whitelistedGroups
                .Select(g => workContext.WorkWhitelistEntries.Find(work.Identifier, SubjectType.Group, g) ??
                            new WorkWhitelistEntryEntity(work, SubjectType.Group, g));
            work.WhitelistEntries = whitelistedUserEntities.Concat(whitelistedGroupEntities).ToList();
        }
    }
}
