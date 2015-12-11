using System.Collections.Generic;
using Borogove.Model;

namespace Borogove.DataAccess
{
    public class CreatorInfoEntity
    {
        public const string AnonymousName = "Anonymous";

        public CreatorInfoEntity()
        {
            Name = AnonymousName;
        }

        public CreatorInfoEntity(Creator creator)
        {
            bool textIsEmpty = string.IsNullOrEmpty(creator.Text);
            if (string.IsNullOrEmpty(creator.FileAs))
            {
                Name = textIsEmpty ? AnonymousName : creator.Text;
            }
            else
            {
                Name = creator.FileAs;
                if (!textIsEmpty)
                {
                    Aliases = new List<string>() { creator.Text };
                }
            }
        }

        public string Name { get; set; }
        public IEnumerable<string> Aliases { get; set; }
    }
}
