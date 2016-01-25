using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using Wyam.Common.Documents;
using Wyam.Common.Modules;
using Wyam.Common.Pipelines;

using Borogove.DataAccess;
using Borogove.Model;
using Names = Borogove.WorkMetadataCanonicalNames;

namespace Borogove
{
    class WorkMetadataPersister : IModule
    {
        private readonly Dictionary<Guid, WorkEntity> _workDictionary = new Dictionary<Guid, WorkEntity>();

        public IEnumerable<IDocument> Execute(IReadOnlyList<IDocument> inputs, IExecutionContext context)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<WorkContext>());

            var finalProcessedDocuments = new List<IDocument>();
            using (var workContext = new WorkContext())
            {
                foreach (var document in inputs)
                {
                    Dictionary<string, object> metadata = document.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                    Guid? workGuid = metadata
                        .SingleOrDefault(kvp => Names.CanonicalizeString(kvp.Key).Equals(Names.Identifier) && kvp.Value is Guid)
                        .Value as Guid?;
                    if (!workGuid.HasValue)
                    {
                        workGuid = Guid.NewGuid();
                        metadata.Add(Names.Identifier, workGuid.Value);
                    }

                    WorkEntity work = workContext.Works
                        .Include(w => w.WorkCreators)
                        .Include(w => w.LanguageEntity)
                        .Include(w => w.TagEntities)
                        .Include(w => w.ParentEntity)
                        .Include(w => w.PreviousWorkEntities)
                        .Include(w => w.NextWorkEntities)
                        .Include(w => w.DraftOfEntity)
                        .Include(w => w.ArtifactOfEntity)
                        .Include(w => w.CommentsOnEntity)
                        .SingleOrDefault(w => w.Identifier.Equals(workGuid)) ??
                        workContext.Works.Add(new WorkEntity(workGuid));
                    work.Content = document.Content;
                    UpdateWorkEntityFromMetadata(work, metadata, workContext);
                    finalProcessedDocuments.Add(document.Clone(metadata));
                }

                workContext.SaveChanges();
            }

            return finalProcessedDocuments;
        }

        static private void UpdateWorkEntityFromMetadata(WorkEntity work, Dictionary<string, object> metadata, WorkContext workContext)
        {
            if (work == null)
            {
                throw new ArgumentNullException(nameof(work));
            }

            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            foreach (var keyValuePair in metadata)
            {
                string canonicalizedKey = Names.CanonicalizeString(keyValuePair.Key);
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
                        var langauge = value as CultureInfo ?? CultureInfo.CurrentCulture;
                        work.LanguageEntity = workContext.Languages.Find(langauge.Name) ??
                            workContext.Languages.Add(new LanguageEntity(langauge));
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

                        work.TagEntities = tagEntities;
                        continue;

                    case Names.Parent:
                        var parentGuid = value as Guid?;
                        if (parentGuid.HasValue)
                        {
                            work.ParentEntity = workContext.Works.Find(parentGuid.Value) ??
                                workContext.Works.Add(new WorkEntity(parentGuid.Value));
                        }
                        else
                        {
                            work.ParentEntity = null;
                        }
                        continue;

                    case Names.Previous:
                        var previousWorks = value as IEnumerable<Guid>;
                        if (work.PreviousWorkEntities == null)
                        {
                            work.PreviousWorkEntities = new List<WorkEntity>();
                        }

                        if (previousWorks == null)
                        {
                            work.PreviousWorkEntities.Clear();
                        }
                        else
                        {
                            foreach (Guid previousWorkGuid in previousWorks)
                            {
                                var previousWorkEntity = workContext.Works.Find(previousWorkGuid) ?? workContext.Works.Add(new WorkEntity(previousWorkGuid));
                                work.PreviousWorkEntities.Add(previousWorkEntity);
                            }
                        }
                        continue;

                    case Names.Next:
                        var nextWorks = value as IEnumerable<Guid>;
                        if (work.NextWorkEntities == null)
                        {
                            work.NextWorkEntities = new List<WorkEntity>();
                        }

                        if (nextWorks == null)
                        {
                            work.NextWorkEntities.Clear();
                        }
                        else
                        {
                            foreach (Guid nextWorkGuid in nextWorks)
                            {
                                var nextWorkEntity = workContext.Works.Find(nextWorkGuid) ?? workContext.Works.Add(new WorkEntity(nextWorkGuid));
                                work.NextWorkEntities.Add(nextWorkEntity);
                            }
                        }
                        continue;

                    case Names.DraftOf:
                        var draftOfGuid = value as Guid?;
                        if (draftOfGuid.HasValue)
                        {
                            work.DraftOfEntity = workContext.Works.Find(draftOfGuid.Value) ??
                                workContext.Works.Add(new WorkEntity(draftOfGuid.Value));
                        }
                        else
                        {
                            work.DraftOfEntity = null;
                        }
                        continue;

                    case Names.ArtifactOf:
                        var artifactOfGuid = value as Guid?;
                        if (artifactOfGuid.HasValue)
                        {
                            work.ArtifactOfEntity = workContext.Works.Find(artifactOfGuid.Value) ??
                                workContext.Works.Add(new WorkEntity(artifactOfGuid.Value));
                        }
                        else
                        {
                            work.ArtifactOfEntity = null;
                        }
                        continue;

                    case Names.CommentsOn:
                        var commentsOnGuid = value as Guid?;
                        if (commentsOnGuid.HasValue)
                        {
                            work.CommentsOnEntity = workContext.Works.Find(commentsOnGuid.Value) ??
                                workContext.Works.Add(new WorkEntity(commentsOnGuid.Value));
                        }
                        else
                        {
                            work.CommentsOnEntity = null;
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
    }
}
