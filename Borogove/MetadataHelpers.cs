using System;
using System.Collections.Generic;
using System.Linq;

namespace Borogove
{
    public static class MetadataHelpers
    {
        public const char ListSeparator = ',';

        public static readonly char[] ListSeparatorArray = new char[] { ListSeparator };

        public static string CanonicalizeString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return input.ToLowerInvariant().Replace(" ", string.Empty);
        }

        public static IEnumerable<string> SeparateList(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new List<string>();
            }

            return input
                .Split(ListSeparatorArray, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s?.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s));

        }
    }
}
