using System;
using System.Globalization;

namespace Borogove.DataAccess
{
    public class Language
    {
        public int Id { get; }
        public string Name { get; }

        public Language(CultureInfo cultureInfo)
        {
            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            Id = cultureInfo.LCID;
            Name = cultureInfo.Name;
        }
    }

    public static class CultureInfoExtensions
    {
        public static Language ToBorogoveLanguage(this CultureInfo cultureInfo)
        {
            return cultureInfo == null ? null : new Language(cultureInfo);
        }
    }
}
