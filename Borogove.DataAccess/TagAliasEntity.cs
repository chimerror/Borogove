using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borogove.DataAccess
{
    public class TagAliasEntity
    {
        public TagAliasEntity()
        {
            TagName = null;
            Alias = null;
        }

        public TagAliasEntity(string tagName = null, string alias = null)
        {
            TagName = tagName;
            Alias = alias;
        }

        public string TagName { get; set; }
        public string Alias { get; set; }
    }
}
