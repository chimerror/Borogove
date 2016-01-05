using System;
using System.Globalization;

namespace Borogove.DataAccess
{
    public class LanguageEntity : IEquatable<LanguageEntity>
    {
        private string _name;

        public LanguageEntity()
        {
            Name = "Unknown Language";
        }

        public LanguageEntity(CultureInfo cultureInfo)
        {
            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            Name = cultureInfo.Name;
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _name = value.ToLowerInvariant();
            }
        }

        public static implicit operator LanguageEntity(CultureInfo cultureInfo)
        {
            return cultureInfo == null ? null : new LanguageEntity(cultureInfo);
        }

        public static implicit operator CultureInfo(LanguageEntity languageEntity)
        {
            return languageEntity == null ? null : CultureInfo.GetCultureInfo(languageEntity.Name);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LanguageEntity);
        }

        public bool Equals(LanguageEntity other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.Name.Equals(Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
