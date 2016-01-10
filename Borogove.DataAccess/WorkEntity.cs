using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Borogove.Model;

namespace Borogove.DataAccess
{
    public class WorkEntity : Work
    {
        private LanguageEntity _languageEntity;

        public WorkEntity()
        {
        }

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

        public override CultureInfo Language
        {
            get
            {
                return _languageEntity;
            }

            set
            {
                _languageEntity = value;
            }
        }

        public LanguageEntity LanguageEntity
        {
            get
            {
                return _languageEntity;
            }

            set
            {
                _languageEntity = value;
            }
        }

        public IEnumerable<TagEntity> TagEntities => Tags?.Select(t => new TagEntity(t));
    }
}
