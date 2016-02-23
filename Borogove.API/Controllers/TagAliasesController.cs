using System.Linq;
using System.Web.OData;
using System.Web.OData.Routing;
using Borogove.DataAccess;

namespace Borogove.API.Controllers
{
    [ODataRoutePrefix("TagAliases")]
    public class TagAliasesController : ODataController
    {
        WorkContext db = new WorkContext("name=WorkContext");

        [EnableQuery]
        public IQueryable<TagAliasEntity> Get()
        {
            return db.TagAliases;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
