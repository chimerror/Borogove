using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Wyam.Common;
using System.Data.Entity;
using Borogove.Model;
using Borogove.DataAccess;
using Names = Borogove.WorkMetadataCanonicalNames;

namespace Borogove
{
    class WorkMetadataPersister : IModule
    {
        private readonly Dictionary<Guid, Work> _workDictionary = new Dictionary<Guid, Work>();

        public IEnumerable<IDocument> Execute(IReadOnlyList<IDocument> inputs, IExecutionContext context)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<WorkContext>());

            var finalProcessedDocuments = new List<IDocument>();
            using (var workContext = new WorkContext())
            {
                var firstProcessedDocuments = new List<IDocument>();
                foreach (var document in inputs)
                {
                    Dictionary<string, object> metadata = document.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                    Guid? workGuid = metadata.SingleOrDefault(kvp => Names.CanonicalizeString(kvp.Key).Equals(Names.Identifier) && kvp.Value is Guid).Value as Guid?;
                    if (!workGuid.HasValue)
                    {
                        workGuid = Guid.NewGuid();
                        metadata.Add(Names.Identifier, workGuid.Value);
                    }

                    WorkEntity work = workContext.Works.FindOrCreateEntity(workGuid.Value);
                    work.Identifier = workGuid.Value;
                    work.Content = document.Content;
                    UpdateWorkFromMetadata(work, metadata);
                    firstProcessedDocuments.Add(document.Clone(metadata));
                }

                foreach (var document in firstProcessedDocuments)
                {
                    Dictionary<string, object> metadata = document.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                    Guid targetWorkGuid = (Guid)metadata.Single(kvp => Names.CanonicalizeString(kvp.Key).Equals(Names.Identifier) && kvp.Value is Guid).Value;
                    WorkEntity targetWork = workContext.Works.Find(targetWorkGuid);

                    if(targetWork == null)
                    {
                        throw new InvalidOperationException($"Unable to find expected target work {targetWorkGuid} in context.");
                    }

                    foreach (var keyValuePair in metadata)
                    {
                        var canonicalizedKey = Names.CanonicalizeString(keyValuePair.Key);
                        object value = keyValuePair.Value;
                        Guid? guidValue = value as Guid?;
                        List<Guid> guidListValue = value as List<Guid>;
                        switch (canonicalizedKey)
                        {
                            case Names.Parent:
                                if (!guidValue.HasValue)
                                {
                                    continue;
                                }
                                var parentWork = workContext.Works.FindOrCreateEntity(guidValue.Value);
                                parentWork.Identifier = guidValue.Value;
                                parentWork.ChildrenEntities.AddOrCreateList(targetWork);
                                if (targetWork.ParentEntity != null)
                                {
                                    context.Trace.Warning($"Overwriting parent of Work {targetWorkGuid} to {guidValue.Value}");
                                }
                                targetWork.ParentEntity = parentWork;
                                continue;

                            default:
                                // Ignore anything else
                                continue;
                        }
                    }
                }
            }

            return finalProcessedDocuments;
        }

        static private void UpdateWorkFromMetadata(WorkEntity work, Dictionary<string, object> metadata)
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
                        work.Creators = value as List<Creator> ?? new List<Creator>();
                        continue;

                    case Names.CreatedDate:
                        work.CreatedDate = value is DateTime ? (DateTime)value : DateTime.MinValue;
                        continue;

                    case Names.ModifiedDate:
                        work.ModifiedDate = value is DateTime ? (DateTime)value : DateTime.MinValue;
                        continue;

                    case Names.PublishedDate:
                        work.PublishedDate = value is DateTime ? (DateTime)value : DateTime.MinValue;
                        continue;

                    case Names.Rights:
                        work.Rights = value as string ?? string.Empty;
                        continue;

                    case Names.License:
                        work.License = (value is License && Enum.IsDefined(typeof(License), value)) ? (License)value : License.None;
                        continue;

                    case Names.Language:
                        work.Language = value as CultureInfo ?? CultureInfo.CurrentCulture;
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
                        work.Tags = value as List<Tag> ?? new List<Tag>();
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
