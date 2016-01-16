using Borogove.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace Borogove.DataAccess
{
    [CustomValidation(typeof(WorkCreatorEntity), "Validate")]
    public class WorkCreatorEntity
    {
        public WorkEntity Work { get; set; }
        public CreatorInfoEntity Creator { get; set; }
        public Role Role { get; set; }
        public CreatorAliasEntity WorkedAs { get; set; }

        public Guid WorkIdentifier
        {
            get
            {
                return Work.Identifier;
            }

            set
            {
                Work.Identifier = value;
            }
        }

        public string CreatorName
        {
            get
            {
                return Creator.Name;
            }

            set
            {
                Creator.Name = value;
            }
        }

        public string WorkedAsName
        {
            get
            {
                return WorkedAs?.Alias ?? CreatorName;
            }

            set
            {
                if (WorkedAs == null)
                {
                    CreatorName = value;
                }
                else if (!CreatorName.Equals(value))
                {
                    WorkedAs.Alias = value;
                }
                else
                {
                    throw new ArgumentException("Cannot set alias to be same as creator name.");
                }
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

            if (entity.WorkedAs != null && entity.CreatorName.Equals(entity.WorkedAs.Alias))
            {
                return new ValidationResult("Alias cannot match creator name if supplied");
            }

            return ValidationResult.Success;
        }
    }
}
