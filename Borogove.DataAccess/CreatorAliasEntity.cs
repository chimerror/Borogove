using System;
using System.ComponentModel.DataAnnotations;

namespace Borogove.DataAccess
{
    [CustomValidation(typeof(CreatorAliasEntity), "Validate")]
    public class CreatorAliasEntity
    {
        private string _alias;
        private string _aliasOf;

        public CreatorAliasEntity() { }

        public CreatorAliasEntity(string alias, string aliasOf)
        {
            Alias = alias;
            AliasOf = aliasOf;
        }

        public string Alias
        {
            get
            {
                return _alias;
            }

            set
            {
                _alias = string.IsNullOrEmpty(value) ? CreatorInfoEntity.AnonymousName : value;
            }
        }

        public string AliasOf
        {
            get
            {
                return _aliasOf;
            }
            set
            {
                _aliasOf = string.IsNullOrEmpty(value) ? CreatorInfoEntity.AnonymousName : value;
            }
        }

        public static ValidationResult Validate(CreatorAliasEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity.Alias.Equals(entity.AliasOf))
            {
                return new ValidationResult("Alias must be different from AliasOf");
            }

            return ValidationResult.Success;
        }
    }
}
