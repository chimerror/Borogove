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
            TagEntities = new List<TagEntity>();
            ChildrenEntities = new List<WorkEntity>();
            PreviousWorkEntities = new List<WorkEntity>();
            NextWorkEntities = new List<WorkEntity>();
            DraftEntities = new List<WorkEntity>();
            ArtifactEntities = new List<WorkEntity>();
            CommentEntities = new List<WorkEntity>();
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
        public virtual ICollection<WorkCreatorEntity> WorkCreators { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Rights { get; set; }
        public License License { get; set; }
        public virtual LanguageEntity LanguageEntity { get; set; }
        public WorkType WorkType { get; set; }
        public ContentRating ContentRating { get; set; }
        public ContentDescriptor ContentDescriptor { get; set; }
        public virtual ICollection<TagEntity> TagEntities { get; set; }
        public string Content { get; set; }
        public virtual WorkEntity ParentEntity { get; set; }
        public virtual ICollection<WorkEntity> ChildrenEntities { get; set; }
        public virtual ICollection<WorkEntity> PreviousWorkEntities { get; set; }
        public virtual ICollection<WorkEntity> NextWorkEntities { get; set; }
        public string DraftIdentifier { get; set; }
        public virtual WorkEntity DraftOfEntity { get; set; }
        public virtual ICollection<WorkEntity> DraftEntities { get; set; }
        public virtual WorkEntity ArtifactOfEntity { get; set; }
        public virtual ICollection<WorkEntity> ArtifactEntities { get; set; }
        public virtual WorkEntity CommentsOnEntity { get; set; }
        public virtual ICollection<WorkEntity> CommentEntities { get; set; }
    }
}
