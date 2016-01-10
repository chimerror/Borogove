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
        public AliasEntity WorkedAs { get; set; }

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

    //public static class WorkCreatorUtilities
    //{
    //    public static IEnumerable<WorkCreatorEntity> GetWorkCreatorEntities(this WorkEntity workEntity)
    //    {
    //        if (workEntity == null)
    //        {
    //            throw new ArgumentNullException(nameof(workEntity));
    //        }

    //        return workEntity.Creators
    //            ?.Select(c =>
    //            {
    //                var workCreatorEntity = new WorkCreatorEntity()
    //                {
    //                    WorkEntity = workEntity,
    //                    Role = c.Role,
    //                    CreatorInfoEntity = new CreatorInfoEntity(c),
    //                };

    //                if (string.IsNullOrEmpty(c.Text))
    //                {
    //                    workCreatorEntity.WorkedAs = string.IsNullOrEmpty(c.FileAs) ? CreatorInfoEntity.AnonymousName : c.FileAs;
    //                }
    //                else
    //                {
    //                    workCreatorEntity.WorkedAs = c.Text;
    //                }

    //                return workCreatorEntity;
    //            });
    //    }
    //}
}
