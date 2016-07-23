using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace Borogove
{
    public class PersisterDbConfiguration : DbConfiguration
    {
        public PersisterDbConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
            AddInterceptor(new AdministratorsDbConnectionInterceptor());
        }
    }
}
