using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Borogove.Model
{
    public class TagSet : IEnumerable<Tag>
    {
        public const char AliasSeparator = '=';
        public const char ImplicationSeparator = '>';
        public const char ListSeparator = ',';

        private static readonly char[] AliasSeparatorArray = new char[] { AliasSeparator };
        private static readonly char[] ImplicationSeparatorArray = new char[] { ImplicationSeparator };
        private static readonly char[] ListSeparatorArray = new char[] { ListSeparator };

        public TagSet(string initialTagList) : this(SplitTagList(initialTagList))
        {
        }

        public TagSet(IEnumerable<string> initialTagList = null)
        {
            if (initialTagList != null)
            {
                foreach(string tag in initialTagList)
                {
                    ResolveTag(tag, true);
                }
            }
        }

        public static IEnumerable<string> SplitTagList(string tagList)
        {
            return tagList?.Split(ListSeparatorArray, StringSplitOptions.RemoveEmptyEntries);
        }


        public IEnumerable<Tag> ResolveTagList(string tagList, bool updateTagSet = false)
        {
            return SplitTagList(tagList)?.SelectMany(ts => ResolveTag(ts, updateTagSet));
        }

        public IEnumerable<Tag> ResolveTag(string tagString, bool updateTagSet = false, bool resolveImplications = true)
        {
            if (string.IsNullOrWhiteSpace(tagString))
            {
                throw new ArgumentNullException(nameof(tagString));
            }

            string canonicalizedTagSpecification = Tag.CanonicalizeTagSpecification(tagString);
            bool hasAlias = canonicalizedTagSpecification.Contains(AliasSeparator);
            bool hasImplication = canonicalizedTagSpecification.Contains(ImplicationSeparator);

            if (hasAlias)
            {
                List<string> splitAlias = canonicalizedTagSpecification
                    .Split(AliasSeparatorArray, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => Tag.CanonicalizeTagName(s))
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList();

                if (splitAlias.Count != 2)
                {
                    throw new ArgumentException($"Invalid alias specification supplied: Supplied string was: ${tagString}");
                }

                // TODO: Implement
            }
            else if (hasImplication)
            {
                List<string> splitImplication = canonicalizedTagSpecification
                    .Split(ImplicationSeparatorArray, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => Tag.CanonicalizeTagName(s))
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList();

                if (splitImplication.Count != 2)
                {
                    throw new ArgumentException($"Invalid Implication specification supplied: Supplied string was: ${tagString}");
                }

                // TODO: Implement
            }
            else
            {
                string canonicalizedTagName = Tag.CanonicalizeTagName(tagString);
            }
        }

        public IEnumerator<Tag> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
