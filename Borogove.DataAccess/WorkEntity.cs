using System.Collections.Generic;
using System.Linq;
using Borogove.Model;

namespace Borogove.DataAccess
{
    public class WorkEntity : Work
    {
        public WorkEntity(Work work)
        {
            Identifier = work.Identifier;
            Title = work.Title;
            Description = work.Description;
            Creators = work.Creators;
            CreatedDate = work.CreatedDate;
            ModifiedDate = work.ModifiedDate;
            PublishedDate = work.PublishedDate;
            Rights = work.Rights;
            License = work.License;
            Language = work.Language;
            WorkType = work.WorkType;
            ContentRating = work.ContentRating;
            ContentDescriptor = work.ContentDescriptor;
            Tags = work.Tags;
            Content = work.Content;
            Parent = work.Parent;
            Children = work.Children;
            PreviousWorks = work.PreviousWorks;
            NextWorks = work.NextWorks;
            DraftIdentifier = work.DraftIdentifier;
            DraftOf = work.DraftOf;
            Drafts = work.Drafts;
            ArtifactOf = work.ArtifactOf;
            Artifacts = work.Artifacts;
            CommentsOn = work.CommentsOn;
            Comments = work.Comments;
        }

        public LanguageEntity LanguageEntity => Language == null ? null : new LanguageEntity(Language);
        public IEnumerable<TagEntity> TagEntities => Tags?.Select(t => new TagEntity(t));
        public IEnumerable<WorkCreatorEntity> WorkCreatorEntities => this.GetWorkCreatorEntities();
    }
}
