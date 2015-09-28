using System;
using System.Text.RegularExpressions;

namespace Borogove.Model
{
    [Flags]
    public enum License
    {
        None = 0,
        AllRightsReserved = 0,
        Attribution = 1,
        NoDerivatives = 2,
        NonCommercial = 4,
        ShareAlike = 8,
        PublicDomain = 16,
    }

    public static class LicenseUtilities
    {
        public static string GetFriendlyName(this License license)
        {
            if (license.HasFlag(License.PublicDomain))
            {
                // Public domain overrides all other licenses
                return "CC0 (Public Domain)";
            }
            else if (license == License.AllRightsReserved)
            {
                return "All rights reserved";
            }
            else if (license.HasFlag(License.Attribution))
            {
                License restOfLicense = license ^ License.Attribution;
                if (restOfLicense == License.None)
                {
                    return "CC BY";
                }
                else if ((restOfLicense ^ License.ShareAlike) == License.None)
                {
                    return "CC BY-SA";
                }
                else if ((restOfLicense ^ License.NoDerivatives) == License.None)
                {
                    return "CC BY-ND";
                }
                else if ((restOfLicense ^ License.NonCommercial) == License.None)
                {
                    return "CC BY-NC";
                }
                else if ((restOfLicense ^ (License.NonCommercial | License.ShareAlike)) == License.None)
                {
                    return "CC BY-NC-SA";
                }
                else if ((restOfLicense ^ (License.NonCommercial | License.NoDerivatives)) == License.None)
                {
                    return "CC BY-NC-ND";
                }
                else
                {
                    throw new ArgumentException($"Unrecognized License: {license}");
                }
            }
            else
            {
                throw new ArgumentException($"Unrecognized License: {license}");
            }
        }

        /// <summary>
        /// Regex to retrieve a license value from the end of a string.
        /// </summary>
        /// <remarks>
        /// Here are all the valid, case-insensitive substrings allowed, assuming they are at the end of the string:
        /// <list type="bulleted">
        ///     <item>
        ///         <description>all rights reserved</description>
        ///     </item>
        ///     <item>
        ///         <description>public domain</description>
        ///     </item>
        ///     <item>
        ///         <description>cc0</description>
        ///     </item>
        ///     <item>
        ///         <description>cc0 (public domain)</description>
        ///     </item>
        ///     <item>
        ///         <description>cc by</description>
        ///     </item>
        ///     <item>
        ///         <description>cc by-sa</description>
        ///     </item>
        ///     <item>
        ///         <description>cc by-nd</description>
        ///     </item>
        ///     <item>
        ///         <description>cc by-nc</description>
        ///     </item>
        ///     <item>
        ///         <description>cc by-nc-sa</description>
        ///     </item>
        ///     <item>
        ///         <description>cc by-nc-nd</description>
        ///     </item>
        /// </list>
        /// </remarks>
        public static readonly Regex LicenseRegex =
            new Regex(@"(all rights reserved|public domain|cc(0( \(public domain\))?|( by(-(sa|nd|(nc(-(sa|nd))?)))?)))$",
                RegexOptions.IgnoreCase);

        public static bool TryParseLicense(string licenseString, out License license)
        {
            bool parsed = false;
            license = License.None;
            var match = LicenseRegex.Match(licenseString);
            if (match.Success)
            {
                try
                {
                    license = ParseFriendlyName(match.Value);
                    parsed = true;
                }
                catch (ArgumentNullException)
                {
                }
                catch (ArgumentException)
                {
                }
            }
            return parsed;
        }

        public static License ParseFriendlyName(string licenseString)
        {
            if (string.IsNullOrWhiteSpace(licenseString))
            {
                throw new ArgumentNullException(nameof(licenseString));
            }

            var licenseStringLower = licenseString.ToLowerInvariant();
            switch (licenseStringLower)
            {
                case "all rights reserved":
                    return License.AllRightsReserved;
                case "cc0":
                case "public domain":
                case "cc0 (public domain)":
                    return License.PublicDomain;
                case "cc by":
                    return License.Attribution;
                case "cc by-sa":
                    return License.Attribution | License.ShareAlike;
                case "cc by-nd":
                    return License.Attribution | License.NoDerivatives;
                case "cc by-nc":
                    return License.Attribution | License.NonCommercial;
                case "cc by-nc-sa":
                    return License.Attribution | License.NonCommercial | License.ShareAlike;
                case "cc by-nc-nd":
                    return License.Attribution | License.NonCommercial | License.NoDerivatives;
                default:
                    throw new ArgumentException($"Unknown license string: {licenseString}");
            }
        }
    }
}
