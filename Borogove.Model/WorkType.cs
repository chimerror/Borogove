using System;

namespace Borogove.Model
{
    public enum WorkType
    {
        Other,
        Series,
        Writing,
        BlogPost,
        Audio,
        Image,
        Video,
        BrowserSoftware,
        DownloadableSoftware,
    }

    public static class WorkTypeUtilities
    {
        public static string GetFriendlyName(this WorkType workType)
        {
            switch (workType)
            {
                case WorkType.BlogPost:
                    return "Blog Post";

                case WorkType.BrowserSoftware:
                    return "Browser Software";

                    case WorkType.DownloadableSoftware:
                    return "Downloadable Software";

                case WorkType.Other:
                case WorkType.Series:
                case WorkType.Writing:
                case WorkType.Audio:
                case WorkType.Image:
                case WorkType.Video:
                    return Enum.GetName(typeof(WorkType), workType);

                default:
                    throw new ArgumentException($"Unknown WorkType: {workType}");
            }
        }

        public static WorkType ParseFriendlyName(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            string canonicalizedString = input.Replace(" ", string.Empty);
            return (WorkType)Enum.Parse(typeof(WorkType), canonicalizedString, true);
        }
    }
}
