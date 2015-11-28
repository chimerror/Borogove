using System;
using System.Collections.Generic;
using System.Linq;

namespace Borogove.Model
{
    public enum ContentRating
    {
        NotRated,
        General3,
        ParentalGuidance7,
        Teen12,
        Mature16,
        Adult18,
    }

    public static class ContentRatingUtilities
    {
        private static readonly Dictionary<ContentRating, string> _ratingToLongNameDictionary =
            new Dictionary<ContentRating, string>()
        {
            { ContentRating.General3, "General Audiences (3+)" },
            { ContentRating.ParentalGuidance7, "Parental Guidance Recommended (7+)" },
            { ContentRating.Teen12, "Teenaged Audiences (12+)" },
            { ContentRating.Mature16, "Mature Audiences (16+)" },
            { ContentRating.Adult18, "Adult Audiences (18+)" },
            { ContentRating.NotRated, "Not Rated" },
        };

        public static string GetLongName(this ContentRating contentRating)
        {
            if (!_ratingToShortNameDictionary.ContainsKey(contentRating))
            {
                throw new ArgumentException($"Unknown Content Rating: {contentRating}");
            }

            return _ratingToLongNameDictionary[contentRating];
        }

        private static readonly Dictionary<string, ContentRating> _longNameToRatingDictionary =
            _ratingToLongNameDictionary.ToDictionary(kvp => kvp.Value.ToLowerInvariant(), kvp => kvp.Key);

        public static ContentRating ParseLongName(string longName)
        {
            if (string.IsNullOrEmpty(longName))
            {
                throw new ArgumentNullException(nameof(longName));
            }

            string longNameLower = longName.ToLowerInvariant();
            if (!_longNameToRatingDictionary.ContainsKey(longNameLower))
            {
                throw new ArgumentException($"Unknown Content Rating Long Name: {longName}");
            }

            return _longNameToRatingDictionary[longNameLower];
        }

        private static readonly Dictionary<ContentRating, string> _ratingToShortNameDictionary =
            new Dictionary<ContentRating, string>()
        {
                { ContentRating.General3, "GE3" },
                { ContentRating.ParentalGuidance7, "PG7" },
                { ContentRating.Teen12, "T12" },
                { ContentRating.Mature16, "M16" },
                { ContentRating.Adult18, "X18" },
                { ContentRating.NotRated, "NR-" },
        };

        public static string GetShortName(this ContentRating contentRating)
        {
            if (!_ratingToShortNameDictionary.ContainsKey(contentRating))
            {
                throw new ArgumentException($"Unknown Content Rating: {contentRating}");
            }

            return _ratingToShortNameDictionary[contentRating];
        }

        private static readonly Dictionary<string, ContentRating> _shortNameToRatingDictionary =
            _ratingToShortNameDictionary.ToDictionary(kvp => kvp.Value.ToLowerInvariant(), kvp => kvp.Key);

        public static ContentRating ParseShortName(string shortName)
        {
            if (string.IsNullOrEmpty(shortName))
            {
                throw new ArgumentNullException(nameof(shortName));
            }

            string shortNameLower = shortName.ToLowerInvariant();
            if (!_shortNameToRatingDictionary.ContainsKey(shortNameLower))
            {
                throw new ArgumentException($"Unknown Content Rating Short Name: {shortName}");
            }

            return _shortNameToRatingDictionary[shortNameLower];
        }
    }
}
