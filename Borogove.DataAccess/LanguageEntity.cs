using System;
using System.Globalization;

namespace Borogove.DataAccess
{
    public class LanguageEntity
    {
        internal LanguageEntity(CultureInfo cultureInfo)
        {
            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            Id = cultureInfo.LCID;
            Name = cultureInfo.Name;
        }

        int Id { get; }
        string Name { get; }
    }
}
