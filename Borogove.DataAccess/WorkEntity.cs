using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Borogove.Model;

namespace Borogove.DataAccess
{
    public class WorkEntity
    {
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

        public Guid Identifier { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<WorkCreatorEntity> WorkCreators { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Rights { get; set; }
        public License License { get; set; }
        public LanguageEntity LanguageEntity { get; set; }
        public WorkType WorkType { get; set; }
        public ContentRating ContentRating { get; set; }
        public ContentDescriptor ContentDescriptor { get; set; }
        public List<TagEntity> TagEntities { get; set; }
        public string Content { get; set; }
        public WorkEntity ParentEntity { get; set; }
        public List<WorkEntity> ChildrenEntities { get; set; }
        public List<WorkEntity> PreviousWorkEntities { get; set; }
        public List<WorkEntity> NextWorkEntities { get; set; }
        public string DraftIdentifier { get; set; }
        public WorkEntity DraftOfEntity { get; set; }
        public List<WorkEntity> DraftEntities { get; set; }
        public WorkEntity ArtifactOfEntity { get; set; }
        public List<WorkEntity> ArtifactEntities { get; set; }
        public WorkEntity CommentsOnEntity { get; set; }
        public List<WorkEntity> CommentEntities { get; set; }

        public IEnumerable<Creator> Creators
        {
            get
            {
                return WorkCreators?.Select(wce => (Creator)wce);
            }

            set
            {
                WorkCreators = value
                    ?.Select(c =>
                    {
                        var wce = new WorkCreatorEntity();
                        wce.Work = this;
                        wce.Role = c.Role;

                        var ce = new CreatorInfoEntity(c.Text);
                        bool hasText = string.IsNullOrEmpty(c.Text);
                        if (!string.IsNullOrEmpty(c.FileAs))
                        {
                            ce = hasText ? new CreatorInfoEntity(c.FileAs, c.Text) : new CreatorInfoEntity(c.FileAs);
                        }
                        wce.Creator = ce;
                        wce.WorkedAs = ce.Aliases.FirstOrDefault();

                        return wce;
                    })
                    .ToList();
            }
        }

        public CultureInfo Language
        {
            get
            {
                return (CultureInfo)LanguageEntity;
            }

            set
            {
                LanguageEntity = (LanguageEntity)value;
            }
        }

        public IEnumerable<Tag> Tags
        {
            get
            {
                return TagEntities?.Select(te => (Tag)te);
            }

            set
            {
                TagEntities = value?.Select(t => (TagEntity)t).ToList();
            }
        }
    }

    public static class WorkExtensions
    {
        public static WorkEntity ToWorkEntity(this Work work)
        {
            if (work == null)
            {
                return null;
            }

            return new WorkEntity()
            {
                Identifier = work.Identifier,
                Title = work.Title,
                Description = work.Description,
                Creators = work.Creators,
                CreatedDate = work.CreatedDate,
                ModifiedDate = work.ModifiedDate,
                PublishedDate = work.PublishedDate,
                Rights = work.Rights,
                License = work.License,
                Language = work.Language,
                WorkType = work.WorkType,
                ContentRating = work.ContentRating,
                ContentDescriptor = work.ContentDescriptor,
                Tags = work.Tags,
                Content = work.Content,
                ParentEntity = work.Parent.ToWorkEntity(),
                ChildrenEntities = work.Children?.Select(w => w.ToWorkEntity()).ToList(),
                PreviousWorkEntities = work.PreviousWorks?.Select(w => w.ToWorkEntity()).ToList(),
                NextWorkEntities = work.NextWorks?.Select(w => w.ToWorkEntity()).ToList(),
                DraftIdentifier = work.DraftIdentifier,
                DraftOfEntity = work.DraftOf.ToWorkEntity(),
                DraftEntities = work.Drafts?.Select(w => w.ToWorkEntity()).ToList(),
                ArtifactOfEntity = work.ArtifactOf.ToWorkEntity(),
                ArtifactEntities = work.Artifacts?.Select(w => w.ToWorkEntity()).ToList(),
                CommentsOnEntity = work.CommentsOn.ToWorkEntity(),
                CommentEntities = work.Comments?.Select(w => w.ToWorkEntity()).ToList(),
            };
        }
    }
}
