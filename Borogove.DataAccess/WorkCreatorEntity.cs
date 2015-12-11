using Borogove.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Borogove.DataAccess
{
    public class WorkCreatorEntity
    {
        public WorkEntity WorkEntity { get; set; }
        public Role Role { get; set; }
        public CreatorInfoEntity CreatorInfoEntity { get; set; }
        public string WorkedAs { get; set; }
        public Guid WorkIdentifier
        {
            get
            {
                return WorkEntity.Identifier;
            }

            set
            {
                WorkEntity.Identifier = value;
            }
        }
        public string CreatorName
        {
            get
            {
                return CreatorInfoEntity.Name;
            }

            set
            {
                CreatorInfoEntity.Name = value;
            }
        }
    }

    public static class WorkCreatorUtilities
    {
        public static IEnumerable<WorkCreatorEntity> GetWorkCreatorEntities(this WorkEntity workEntity)
        {
            if (workEntity == null)
            {
                throw new ArgumentNullException(nameof(workEntity));
            }

            return workEntity.Creators
                ?.Select(c =>
                {
                    var workCreatorEntity = new WorkCreatorEntity()
                    {
                        WorkEntity = workEntity,
                        Role = c.Role,
                        CreatorInfoEntity = new CreatorInfoEntity(c),
                    };

                    if (string.IsNullOrEmpty(c.Text))
                    {
                        workCreatorEntity.WorkedAs = string.IsNullOrEmpty(c.FileAs) ? CreatorInfoEntity.AnonymousName : c.FileAs;
                    }
                    else
                    {
                        workCreatorEntity.WorkedAs = c.Text;
                    }

                    return workCreatorEntity;
                });
        }
    }
}
