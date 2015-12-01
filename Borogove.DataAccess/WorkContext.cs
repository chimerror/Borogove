using System.Data.Entity;
using Borogove.Model;

namespace Borogove.DataAccess
{
    public class WorkContext : DbContext
    {
        public DbSet<Work> Works { get; set; }
    }
}
