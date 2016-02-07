using System;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using Borogove.DataAccess;

namespace Borogove.API.Controllers
{
    [ODataRoutePrefix("Works")]
    public class WorksController : ODataController
    {
        WorkContext db = new WorkContext();

        [EnableQuery]
        public IQueryable<WorkEntity> Get()
        {
            return db.Works;
        }

        [EnableQuery]
        public SingleResult<WorkEntity> Get([FromODataUri] Guid key)
        {
            IQueryable<WorkEntity> result = db.Works.Where(w => w.Identifier.Equals(key));
            return SingleResult.Create(result);
        }

        [EnableQuery]
        public SingleResult<string> GetContent([FromODataUri] Guid key)
        {
            var result = db.Works.Where(w => w.Identifier.Equals(key)).Select(w => w.Content);
            return SingleResult.Create(result);
        }

        [EnableQuery]
        public IQueryable<WorkCreatorEntity> GetWorkCreators([FromODataUri] Guid key)
        {
            return db.Works.Single(w => w.Identifier.Equals(key)).WorkCreators.AsQueryable();
        }

        private bool WorkExists(Guid key)
        {
            return db.Works.Any(w => w.Identifier.Equals(key));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}