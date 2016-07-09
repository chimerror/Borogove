using System;

namespace Borogove.DataAccess
{
    public class WorkWhitelistEntryEntity
    {
        public WorkWhitelistEntryEntity()
        {
        }

        public WorkWhitelistEntryEntity(WorkEntity work, SubjectType subjectType, string subjectName)
        {
            if (work == null)
            {
                throw new ArgumentNullException(nameof(work));
            }

            if (subjectName == null)
            {
                throw new ArgumentNullException(nameof(subjectName));
            }

            Work = work;
            WorkIdentifier = Work.Identifier;
            SubjectType = subjectType;
            SubjectName = subjectName;
        }

        public Guid WorkIdentifier { get; set; }
        public WorkEntity Work { get; set; }
        public SubjectType SubjectType { get; set; }
        public string SubjectName { get; set; }
    }
}
