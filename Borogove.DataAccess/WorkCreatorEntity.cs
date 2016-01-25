using Borogove.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace Borogove.DataAccess
{
    public class WorkCreatorEntity
    {
        private string _workedAsName;

        public WorkCreatorEntity()
        {
        }

        public WorkCreatorEntity(WorkEntity work, CreatorInfoEntity creator, Role role, string workedAs = null)
        {
            if (work == null)
            {
                throw new ArgumentNullException(nameof(work));
            }

            if (creator == null)
            {
                throw new ArgumentNullException(nameof(creator));
            }

            Work = work;
            WorkIdentifier = Work.Identifier;
            Creator = creator;
            CreatorName = creator.Name;
            Role = role;
            WorkedAsName = workedAs;
        }

        public Guid WorkIdentifier { get; set; }
        public WorkEntity Work { get; set; }
        public string CreatorName { get; set; }
        public CreatorInfoEntity Creator { get; set; }
        public Role Role { get; set; }

        public string WorkedAsName
        {
            get
            {
                return _workedAsName ?? CreatorName;
            }

            set
            {
                if (CreatorName == null)
                {
                    _workedAsName = value;
                }
                else
                {
                    _workedAsName = CreatorName.Equals(value) ? null : value;
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

            return ValidationResult.Success;
        }
    }
}
