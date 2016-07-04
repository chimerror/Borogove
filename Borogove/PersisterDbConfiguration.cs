using System.Data.Entity;

namespace Borogove
{
    public class PersisterDbConfiguration : DbConfiguration
    {
        public PersisterDbConfiguration()
        {
            AddInterceptor(new AdministratorsDbConnectionInterceptor());
        }
    }
}
