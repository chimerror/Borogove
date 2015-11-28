using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

        private readonly Dictionary<string, Tag> _tagDictionary = new Dictionary<string, Tag>();
        private readonly Dictionary<Tag, HashSet<Tag>> _implicationDictionary = new Dictionary<Tag, HashSet<Tag>>();

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

        public IEnumerable<Tag> ResolveTagList(string tagList, bool updateTagSet = false, bool resolveImplications = true)
        {
            var result = SplitTagList(tagList)?.Select(ts => ResolveTag(ts, updateTagSet)).Distinct();
            if (resolveImplications)
            {
                result = result.Concat(result.SelectMany(t => t.Implications)).Distinct();
            }
            return result;
        }

        public Tag ResolveTag(string tagString, bool updateTagSet = false)
        {
            if (string.IsNullOrWhiteSpace(tagString))
            {
                throw new ArgumentNullException(nameof(tagString));
            }

            string canonicalizedTagSpecification = Tag.CanonicalizeTagSpecification(tagString);
            bool hasAlias = canonicalizedTagSpecification.Contains(AliasSeparator);
            bool hasImplication = canonicalizedTagSpecification.Contains(ImplicationSeparator);
            Tag resultTag = null;

            if (hasAlias)
            {
                List<string> splitAlias = canonicalizedTagSpecification
                    .Split(AliasSeparatorArray, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => Tag.CanonicalizeTagName(s))
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToList();

                if (splitAlias.Count != 2)
                {
                    throw new ArgumentException($"Invalid alias specification supplied: Supplied string was: {tagString}");
                }

                string canonicalizedAliasName = splitAlias[0];
                Tag aliasTag;
                bool aliasExists = _tagDictionary.TryGetValue(canonicalizedAliasName, out aliasTag);
                string canonicalizedResultTagName = splitAlias[1];
                bool resultTagExists = _tagDictionary.TryGetValue(canonicalizedResultTagName, out resultTag);

                if (updateTagSet)
                {
                    if (aliasExists && resultTagExists)
                    {
                        if (!aliasTag.Equals(resultTag))
                        {
                            _tagDictionary[canonicalizedAliasName] = resultTag;
                        }
                    }
                    else if (resultTagExists)
                    {
                        _tagDictionary[canonicalizedAliasName] = resultTag;
                    }
                    else if (aliasExists)
                    {
                        // Favor the existing tag
                        _tagDictionary[canonicalizedResultTagName] = aliasTag;
                        resultTag = aliasTag;
                    }
                    else
                    {
                        resultTag = new Tag(canonicalizedResultTagName);
                        _tagDictionary[canonicalizedResultTagName] = resultTag;
                        _tagDictionary[canonicalizedAliasName] = resultTag;
                        resultTag.Aliases = GetAliases(resultTag);
                        resultTag.Implications = GetImplications(resultTag);
                    }
                }
                else
                {
                    if (!aliasExists || !resultTagExists || !aliasTag.Equals(resultTag))
                    {
                        throw new InvalidOperationException($"Could not resolve alias specification without updating: ${tagString}");
                    }
                }

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
                    throw new ArgumentException($"Invalid Implication specification supplied: Supplied string was: {tagString}");
                }

                string canonicalizedResultTagName = splitImplication[0];
                bool resultTagExists = _tagDictionary.TryGetValue(canonicalizedResultTagName, out resultTag);
                bool implicationsExist = resultTagExists ? _implicationDictionary.ContainsKey(resultTag) : false;
                string canonicalizedImpliedTagName = splitImplication[1];
                Tag impliedTag;
                bool impliedTagExists = _tagDictionary.TryGetValue(canonicalizedImpliedTagName, out impliedTag);
                bool impliedTagConnected = (impliedTagExists && implicationsExist)
                    ? _implicationDictionary[resultTag].Contains(impliedTag)
                    : false;

                if (!impliedTagConnected)
                {
                    if (!updateTagSet)
                    {
                        throw new InvalidOperationException($"Could not resolve implication specification without updating: {tagString}");
                    }

                    if (!resultTagExists)
                    {
                        resultTag = new Tag(canonicalizedResultTagName);
                        _tagDictionary[canonicalizedResultTagName] = resultTag;
                        resultTag.Aliases = GetAliases(resultTag);
                        resultTag.Implications = GetImplications(resultTag);
                    }

                    if (!impliedTagExists)
                    {
                        impliedTag = new Tag(canonicalizedImpliedTagName);
                        _tagDictionary[canonicalizedImpliedTagName] = impliedTag;
                        impliedTag.Aliases = GetAliases(impliedTag);
                        impliedTag.Implications = GetImplications(impliedTag);
                    }

                    if (!implicationsExist)
                    {
                        _implicationDictionary.Add(resultTag, new HashSet<Tag>());
                    }

                    // Since it's a hash set, no error if we add it twice.
                    _implicationDictionary[resultTag].Add(impliedTag);
                }
            }
            else
            {
                string canonicalizedTagName = Tag.CanonicalizeTagName(tagString);
                bool tagExists = _tagDictionary.TryGetValue(canonicalizedTagName, out resultTag);
                if (!tagExists && updateTagSet)
                {
                    resultTag = new Tag(canonicalizedTagName);
                    _tagDictionary[canonicalizedTagName] = resultTag;
                    resultTag.Aliases = GetAliases(resultTag);
                    resultTag.Implications = GetImplications(resultTag);
                }
            }

            if (resultTag == null)
            {
                throw new InvalidOperationException($"Unable to resolve tag specification: {tagString}");
            }

            return resultTag;
        }

        private IEnumerable<string> GetAliases(Tag tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }

            if (!_tagDictionary.ContainsKey(tag.Name))
            {
                throw new ArgumentException($"Unrecognized tag provided: {tag.Name}");
            }

            var tagName = tag.Name;
            var rootTag = _tagDictionary[tagName];
            return _tagDictionary
                .Where(kvp => kvp.Value == rootTag && kvp.Key != tagName)
                .Select(kvp => kvp.Key)
                .Distinct();

        }

        private IEnumerable<Tag> GetImplications(Tag tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }

            if (!_implicationDictionary.ContainsKey(tag))
            {
                yield break;
            }

            var stack = new Stack<Tag>();
            foreach (var implication in _implicationDictionary[tag])
            {
                stack.Push(implication);
            }

            var seenTags = new HashSet<Tag>();
            while (stack.Count != 0)
            {
                var current = stack.Pop();
                if (seenTags.Contains(current))
                {
                    continue;
                }

                yield return current;
                seenTags.Add(current);

                if (!_implicationDictionary.ContainsKey(current))
                {
                    continue;
                }

                foreach (var implication in _implicationDictionary[current])
                {
                    if (!implication.Equals(tag) && !seenTags.Contains(implication))
                    {
                        stack.Push(implication);
                    }
                }
            }
        }

        public IEnumerator<Tag> GetEnumerator()
        {
            return _tagDictionary.Values.Distinct().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
