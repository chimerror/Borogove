using System.Linq;
using System.Web.OData;
using System.Web.OData.Routing;
using Borogove.DataAccess;

namespace Borogove.API.Controllers
{
    [ODataRoutePrefix("WorkCreators")]
    public class WorkCreatorsController : ODataController
    {
        WorkContext db = new WorkContext("name=WorkContext");

        [EnableQuery]
        public IQueryable<WorkCreatorEntity> Get()
        {
            return db.WorkCreators;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
