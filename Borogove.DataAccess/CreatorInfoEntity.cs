using System;
using System.Collections.Generic;
using System.Linq;
using Borogove.Model;

namespace Borogove.DataAccess
{
    public class CreatorInfoEntity
    {
        public const string AnonymousName = "Anonymous";

        private string _name;

        public CreatorInfoEntity()
        {
            Name = AnonymousName;
            Aliases = new List<AliasEntity>();
        }

        public CreatorInfoEntity(string name, params string[] aliases)
        {
            Name = string.IsNullOrEmpty(name) ? AnonymousName : name;
            Aliases = aliases.Select(a => new AliasEntity(a, Name)).ToList();
        }

        public CreatorInfoEntity(Creator creator)
        {
            if (creator == null)
            {
                throw new ArgumentNullException(nameof(creator));
            }

            Aliases = new List<AliasEntity>();

            bool textExists = string.IsNullOrEmpty(creator.Text);
            if (string.IsNullOrEmpty(creator.FileAs))
            {
                Name = textExists ? creator.Text : AnonymousName;
            }
            else
            {
                Name = creator.FileAs;
                if (textExists)
                {
                    Aliases.Add(new AliasEntity(creator.Text, Name));
                }
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = string.IsNullOrEmpty(value) ? AnonymousName : value;
            }
        }
        public List<AliasEntity> Aliases { get; set; }
    }
}
