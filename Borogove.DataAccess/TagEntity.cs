using System.Collections.Generic;
using System.Linq;
using Borogove.Model;

namespace Borogove.DataAccess
{
    public class TagEntity
    {
        private string _tagName;

        public TagEntity()
        {
            TagName = null;
            Aliases = new List<TagAliasEntity>();
            Implications = new List<TagEntity>();
        }

        public TagEntity(string tagName = null, params string[] aliases)
        {
            TagName = tagName;
            Aliases = aliases.Select(a => new TagAliasEntity(tagName, a)).ToList();
            Implications = new List<TagEntity>();
        }

        public string TagName
        {
            get
            {
                return _tagName;
            }

            set
            {
                _tagName = string.IsNullOrWhiteSpace(value) ? null : Tag.CanonicalizeTagName(value);
            }
        }
        public virtual ICollection<TagAliasEntity> Aliases { get; set;}
        public virtual ICollection<TagEntity> Implications { get; set; }

        public static explicit operator Tag(TagEntity tagEntity)
        {
            if (tagEntity == null)
            {
                return null;
            }

            return new Tag(tagEntity.TagName)
            {
                Aliases = tagEntity.Aliases?.Select(tae => tae.TagName).ToList(),
                Implications = tagEntity.Implications?.Select(i => (Tag)i).ToList(),
            };
        }

        public static explicit operator TagEntity(Tag tag)
        {
            if (tag == null)
            {
                return null;
            }

            return new TagEntity(tag.Name)
            {
                Aliases = tag.Aliases?.Select(a => new TagAliasEntity(tag.Name, a)).ToList(),
                Implications = tag.Implications?.Select(i => (TagEntity)i).ToList(),
            };
        }
    }
}
