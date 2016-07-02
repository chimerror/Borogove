using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using Borogove.Model;

namespace Borogove.DataAccess
{
    public class WorkEntity
    {
        public static readonly DateTime MinimumDateTime = (DateTime)SqlDateTime.MinValue;

        public WorkEntity()
        {
            WorkCreators = new List<WorkCreatorEntity>();
            Tags = new List<TagEntity>();
            Children = new List<WorkEntity>();
            PreviousWorks = new List<WorkEntity>();
            NextWorks = new List<WorkEntity>();
            Drafts = new List<WorkEntity>();
            Artifacts = new List<WorkEntity>();
            Comments = new List<WorkEntity>();
            WhitelistEntries = new List<WorkWhitelistEntryEntity>();
        }

        public WorkEntity(Guid? identifier = null,
                          DateTime? createdDate = null,
                          DateTime? modifiedDate = null,
                          DateTime? publishedDate = null,
                          License license = License.None,
                          WorkType workType = WorkType.Other,
                          ContentRating contentRating = ContentRating.NotRated,
                          ContentDescriptor contentDescriptor = ContentDescriptor.None)
            : this()
        {
            Identifier = identifier ?? Guid.Empty;
            CreatedDate = createdDate ?? MinimumDateTime;
            ModifiedDate = modifiedDate ?? MinimumDateTime;
            PublishedDate = publishedDate ?? MinimumDateTime;
            License = license;
            WorkType = workType;
            ContentRating = contentRating;
            ContentDescriptor = contentDescriptor;
        }

        public Guid Identifier { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public virtual ICollection<WorkCreatorEntity> WorkCreators { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Rights { get; set; }
        public License License { get; set; }
        public virtual LanguageEntity Language { get; set; }
        public WorkType WorkType { get; set; }
        public ContentRating ContentRating { get; set; }
        public ContentDescriptor ContentDescriptor { get; set; }
        public virtual ICollection<TagEntity> Tags { get; set; }
        public string Content { get; set; }
        public virtual WorkEntity Parent { get; set; }
        public virtual ICollection<WorkEntity> Children { get; set; }
        public virtual ICollection<WorkEntity> PreviousWorks { get; set; }
        public virtual ICollection<WorkEntity> NextWorks { get; set; }
        public string DraftIdentifier { get; set; }
        public virtual WorkEntity DraftOf { get; set; }
        public virtual ICollection<WorkEntity> Drafts { get; set; }
        public virtual WorkEntity ArtifactOf { get; set; }
        public virtual ICollection<WorkEntity> Artifacts { get; set; }
        public virtual WorkEntity CommentsOn { get; set; }
        public virtual ICollection<WorkEntity> Comments { get; set; }
        public virtual ICollection<WorkWhitelistEntryEntity> WhitelistEntries { get; set; }
    }
}
