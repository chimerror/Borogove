using System;
using System.Collections.Generic;
using System.Globalization;

namespace Borogove.Model
{
    public class Work
    {
        public Guid Identifier { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Creator> Creators { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Rights { get; set; }
        public License License { get; set; }
        public virtual CultureInfo Language { get; set; }
        public WorkType WorkType { get; set; }
        public ContentRating ContentRating { get; set; }
        public ContentDescriptor ContentDescriptor { get; set; }
        public List<Tag> Tags { get; set; }
        public string Content { get; set; }
        public Work Parent { get; set; }
        public List<Work> Children { get; set; }
        public List<Work> PreviousWorks { get; set; }
        public List<Work> NextWorks { get; set; }
        public string DraftIdentifier { get; set; }
        public Work DraftOf { get; set; }
        public List<Work> Drafts { get; set; }
        public Work ArtifactOf { get; set; }
        public List<Work> Artifacts { get; set; }
        public Work CommentsOn { get; set; }
        public List<Work> Comments { get; set; }
    }
}
