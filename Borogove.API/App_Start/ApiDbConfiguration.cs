using System.Data.Entity;

namespace Borogove.API
{
    internal class ApiDbConfiguration : DbConfiguration
    {
        internal ApiDbConfiguration()
        {
            AddInterceptor(new SessionContextDbConnectionInterceptor());
        }
    }
}
