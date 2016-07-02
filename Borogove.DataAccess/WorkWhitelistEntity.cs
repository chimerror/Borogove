using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borogove.DataAccess
{
    public class WorkWhitelistEntryEntity
    {
        public Guid WorkIdentifier { get; set; }
        public WorkEntity Work { get; set; }
        public SubjectType SubjectType { get; set; }
        public string SubjectName { get; set; }
    }
}
