using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borogove.Model
{
    [Flags]
    public enum ContentDescriptor
    {
        None,
        AlcoholReference,
        AnimatedBlood,
        Blood,
        BloodAndGore,
        CartoonViolence,
        ComicMischief,
        CrudeHumor,
        DrugReference,
        FantasyViolence,
        IntenseViolence,
        Language,
        Lyrics,
        MatureHumor,
        Nudity,
        PartialNudity,
        RealGambling,
        SexualContent,
        SexualThemes,
        SexualViolence,
        SimulatedGambling,
        StrongLanguage,
        StrongLyrics,
        StrongSexualContent,
        SuggestiveThemes,
        TobaccoReference,
        UseOfAlcohol,
        UseOfDrugs,
        UseOfTobacco,
        Violence,
        ViolentReferences,
    }

    public static class ContentDescriptorUtilities
    {
        private readonly static Dictionary<ContentDescriptor, string> _descriptorToFriendlyNameDictionary =
            new Dictionary<ContentDescriptor, string>()
            {
                { ContentDescriptor.None, "None" },
                { ContentDescriptor.AlcoholReference, "Alcohol Reference" },
                { ContentDescriptor.AnimatedBlood, "Animated Blood" },
                { ContentDescriptor.Blood, "Blood" },
                { ContentDescriptor.BloodAndGore, "Blood and Gore" },
                { ContentDescriptor.CartoonViolence, "Cartoon Violence" },
                { ContentDescriptor.ComicMischief, "Comic Mischief" },
                { ContentDescriptor.CrudeHumor, "Crude Humor" },
                { ContentDescriptor.DrugReference, "Drug Reference" },
                { ContentDescriptor.FantasyViolence, "Fantasy Violence" },
                { ContentDescriptor.IntenseViolence, "Intense Violence" },
                { ContentDescriptor.Language, "Language" },
                { ContentDescriptor.Lyrics, "Lyrics" },
                { ContentDescriptor.MatureHumor, "Mature Humor" },
                { ContentDescriptor.Nudity, "Nudity" },
                { ContentDescriptor.PartialNudity, "Partial Nudity" },
                { ContentDescriptor.RealGambling, "Real Gambling" },
                { ContentDescriptor.SexualContent, "Sexual Content" },
                { ContentDescriptor.SexualThemes, "Sexual Themes" },
                { ContentDescriptor.SexualViolence, "Sexual Violence" },
                { ContentDescriptor.SimulatedGambling, "Simulated Gambling" },
                { ContentDescriptor.StrongLanguage, "Strong Language" },
                { ContentDescriptor.StrongLyrics, "Strong Lyrics" },
                { ContentDescriptor.StrongSexualContent, "Strong Sexual Content" },
                { ContentDescriptor.SuggestiveThemes, "Suggestive Themes" },
                { ContentDescriptor.TobaccoReference, "Tobacco Reference" },
                { ContentDescriptor.UseOfAlcohol, "Use of Alcohol" },
                { ContentDescriptor.UseOfDrugs, "Use of Drugs" },
                { ContentDescriptor.UseOfTobacco, "Use of Tobacco" },
                { ContentDescriptor.Violence, "Violence" },
                { ContentDescriptor.ViolentReferences, "Violent References" },
            };

        private readonly static ContentDescriptor _knownFriendlyNames = _descriptorToFriendlyNameDictionary
            .Aggregate(ContentDescriptor.None, (cd, kvp) => cd | kvp.Key);

        private readonly static Dictionary<string, ContentDescriptor> _friendlyNameToDescriptorDictionary =
            _descriptorToFriendlyNameDictionary.ToDictionary(kvp => kvp.Value.ToLowerInvariant(), kvp => kvp.Key);

        public static bool HasFriendlyName(this ContentDescriptor contentDescriptor)
        {
            return contentDescriptor == ContentDescriptor.None || _knownFriendlyNames.HasFlag(contentDescriptor);
        }

        public static string GetFriendlyName(this ContentDescriptor contentDescriptor)
        {
            if (!contentDescriptor.HasFriendlyName())
            {
                throw new ArgumentException($"Content Descriptor Does Not Have Friendly Name: {contentDescriptor}");
            }

            var friendlyNames = _descriptorToFriendlyNameDictionary
                .Where(kvp => contentDescriptor.HasFlag(kvp.Key))
                .Select(kvp => kvp.Value)
                .ToList();

            if (friendlyNames.Count == 0)
            {
                throw new ArgumentException($"Unable to create friendly name for content descriptor: {contentDescriptor}");
            }

            return string.Join(",", friendlyNames);
        }

        public static ContentDescriptor ParseContentDescriptor(string input)
        {
            var splitInput = input.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(e => e.Trim())
                .ToList();

            ContentDescriptor result = ContentDescriptor.None;
            foreach (string entry in splitInput)
            {
                ContentDescriptor parsedContentDescriptor = ContentDescriptor.None;
                if (_friendlyNameToDescriptorDictionary.ContainsKey(entry.ToLowerInvariant()))
                {
                    result |= _friendlyNameToDescriptorDictionary[entry];
                }
                else if (Enum.TryParse(entry, true, out parsedContentDescriptor))
                {
                    result |= parsedContentDescriptor;
                }
                else
                {
                    throw new ArgumentException($"Could not parse entry '{entry}' in input: {input}");
                }
            }

            return result;
        }
    }
}
