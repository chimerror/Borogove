using Borogove.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace Borogove.DataAccess
{
    [CustomValidation(typeof(WorkCreatorEntity), "Validate")]
    public class WorkCreatorEntity
    {
        private string _workedAsName;

        public virtual WorkEntity Work { get; set; }
        public virtual CreatorInfoEntity Creator { get; set; }
        public Role Role { get; set; }

        public Guid WorkIdentifier
        {
            get
            {
                return Work?.Identifier ?? Guid.Empty;
            }

            set
            {
                if (Work == null)
                {
                    Work = new WorkEntity();
                }

                Work.Identifier = value;
            }
        }

        public string CreatorName
        {
            get
            {
                return Creator?.Name;
            }

            set
            {
                if (Creator == null)
                {
                    Creator = new CreatorInfoEntity();
                }

                Creator.Name = value;
            }
        }

        public string WorkedAsName
        {
            get
            {
                return _workedAsName ?? CreatorName;
            }

            set
            {
                _workedAsName = value;
            }
        }

        public CreatorAliasEntity WorkedAs
        {
            get
            {
                return CreatorName.Equals(WorkedAsName) ? null : new CreatorAliasEntity(CreatorName, WorkedAsName);
            }

            set
            {
                WorkedAsName = value?.Alias;
            }
        }

        public static explicit operator Creator(WorkCreatorEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new Creator()
            {
                Role = entity.Role,
                FileAs = entity.CreatorName,
                Text = entity.WorkedAsName,
            };
        }

        public static ValidationResult Validate(WorkCreatorEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity.Work == null)
            {
                return new ValidationResult("Work must not be null.");
            }

            if (entity.Creator == null)
            {
                return new ValidationResult("Creator must not be null.");
            }

            return ValidationResult.Success;
        }
    }
}
