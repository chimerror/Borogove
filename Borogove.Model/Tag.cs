using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Borogove.Model
{
    public sealed class Tag : IEquatable<Tag>
    {
        public const char WhitespaceReplacement = '_';
        public static readonly Regex WhitespaceRegex = new Regex(@"[\s_]+");

        internal Tag(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = CanonicalizeTagName(name);
        }

        public string Name { get; }
        public IEnumerable<string> Aliases { get; internal set; }
        public IEnumerable<Tag> Implications { get; internal set; }

        public static string CanonicalizeTagName(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                throw new ArgumentNullException(nameof(tagName));
            }

            string canonicalizedTagName = CanonicalizeTagSpecification(tagName);

            if (canonicalizedTagName.Contains(TagSet.AliasSeparator))
            {
                throw new ArgumentException($"Canonicalized tag name cannot contain Alias Separator '${TagSet.AliasSeparator}'. Supplied string was: ${tagName}");
            }

            if (canonicalizedTagName.Contains(TagSet.ImplicationSeparator))
            {
                throw new ArgumentException($"Canonicalized tag name cannot contain Implication Separator '${TagSet.ImplicationSeparator}'. Supplied string was: ${tagName}");
            }

            return canonicalizedTagName;
        }

        public static string CanonicalizeTagSpecification(string tagSpecification)
        {
            if (string.IsNullOrWhiteSpace(tagSpecification))
            {
                throw new ArgumentNullException(nameof(tagSpecification));
            }

            string canonicalizedTagSpecification =
                WhitespaceRegex.Replace(tagSpecification.ToLowerInvariant().Trim(), WhitespaceReplacement.ToString())
                .Trim(WhitespaceReplacement);

            if (string.IsNullOrWhiteSpace(canonicalizedTagSpecification))
            {
                throw new ArgumentException($"Canonicalized tag specification cannot be null or whitespace. Supplied string was: ${tagSpecification}");
            }

            if (canonicalizedTagSpecification.Contains(TagSet.ListSeparator))
            {
                throw new ArgumentException($"Canonicalized tag specification cannot contain List Separator '${TagSet.ListSeparator}'. Supplied string was: ${tagSpecification}");
            }

            int aliasCount = canonicalizedTagSpecification.Count(c => c.Equals(TagSet.AliasSeparator));
            int implicationCount = canonicalizedTagSpecification.Count(c => c.Equals(TagSet.ImplicationSeparator));
            if (aliasCount > 0 && implicationCount > 0)
            {
                throw new ArgumentException($"Canonicalized tag specification cannot contain both Alias ('${TagSet.AliasSeparator}') and Implication ('${TagSet.ImplicationSeparator}') specifications. Supplied string was: ${tagSpecification}");
            }

            if (aliasCount != 0 && aliasCount != 1)
            {
                throw new ArgumentException($"Canonicalized tag specification cannot contain multiple Alias ('${TagSet.AliasSeparator}') specifications. Supplied string was: ${tagSpecification}");
            }

            if (implicationCount != 0 && implicationCount != 1)
            {
                throw new ArgumentException($"Canonicalized tag specification cannot contain multiple Implication ('${TagSet.ImplicationSeparator}') specifications. Supplied string was: ${tagSpecification}");
            }

            return canonicalizedTagSpecification;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Tag);
        }

        public bool Equals(Tag other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            // As this class is sealed, we can already assume that the names have been canonicalized, so we can just
            // check that they are equal.
            return other.Name.Equals(Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return $"Borogove Tag '{Name}'";
        }
    }
}
