using System;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using Borogove.DataAccess;
using Borogove.Model;
using System.Web.OData.Routing;

namespace Borogove.API.Controllers
{
    [ODataRoutePrefix("WorkCreators")]
    public class WorkCreatorsController : ODataController
    {
        WorkContext db = new WorkContext();

        [EnableQuery]
        [ODataRoute]
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
