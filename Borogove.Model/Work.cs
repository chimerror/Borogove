using System;
using System.Collections.Generic;
using System.Globalization;

namespace Borogove.Model
{
    public class Work
    {
        public virtual Guid Identifier { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual ICollection<Creator> Creators { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual DateTime PublishedDate { get; set; }
        public virtual string Rights { get; set; }
        public virtual License License { get; set; }
        public virtual CultureInfo Language { get; set; }
        public virtual WorkType WorkType { get; set; }
        public virtual ContentRating ContentRating { get; set; }
        public virtual ContentDescriptor ContentDescriptor { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual string Content { get; set; }
        public virtual Work Parent { get; set; }
        public virtual ICollection<Work> Children { get; set; }
        public virtual ICollection<Work> PreviousWorks { get; set; }
        public virtual ICollection<Work> NextWorks { get; set; }
        public virtual string DraftIdentifier { get; set; }
        public virtual Work DraftOf { get; set; }
        public virtual ICollection<Work> Drafts { get; set; }
        public virtual Work ArtifactOf { get; set; }
        public virtual ICollection<Work> Artifacts { get; set; }
        public virtual Work CommentsOn { get; set; }
        public virtual ICollection<Work> Comments { get; set; }
    }
}
