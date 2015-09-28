using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Borogove.Model
{
    public class Tag
    {
        public const string AliasSeparator = "=";
        public const string ImplicationSeparator = "->";
        public const char WhitespaceReplacement = '_';
        public const char ListSeparator = ',';

        public static readonly Regex WhitespaceRegex = new Regex(@"[\s_]+");

        public string Name { get; set; }
        public HashSet<string> Aliases { get; set; }
        public List<Tag> Implications { get; set; }

        public static string CanonicalizeTagName(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                throw new ArgumentNullException(nameof(tagName));
            }

            string canonicalizedTagName =
                WhitespaceRegex.Replace(tagName.ToLowerInvariant().Trim(), WhitespaceReplacement.ToString())
                .Trim(WhitespaceReplacement);
            if (string.IsNullOrWhiteSpace(canonicalizedTagName))
            {
                throw new ArgumentException($"Canonicalized tag name cannot be null or whitespace. Supplied string was: ${tagName}");
            }

            if (canonicalizedTagName.Contains(AliasSeparator))
            {
                throw new ArgumentException($"Canonicalized tag name cannot contain Alias Separator '${AliasSeparator}'. Supplied string was: ${tagName}");
            }

            if (canonicalizedTagName.Contains(ImplicationSeparator))
            {
                throw new ArgumentException($"Canonicalized tag name cannot contain Implication Separator '${ImplicationSeparator}'. Supplied string was: ${tagName}");
            }

            if (canonicalizedTagName.Contains(ListSeparator))
            {
                throw new ArgumentException($"Canonicalized tag name cannot contain List Separator '${ListSeparator}'. Supplied string was: ${tagName}");
            }

            return canonicalizedTagName;

        }
    }
}
