using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using Borogove.DataAccess;

namespace Borogove.API.Controllers
{
    [ODataRoutePrefix("Creators")]
    public class CreatorsController : ODataController
    {
        WorkContext db = new WorkContext();

        [EnableQuery]
        public IQueryable<CreatorInfoEntity> Get()
        {
            return db.Creators;
        }

        [EnableQuery]
        public SingleResult<CreatorInfoEntity> Get([FromODataUri] string key)
        {
            var result = db.Creators.Where(c => c.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase));
            return SingleResult.Create(result);
        }

        [EnableQuery]
        public IQueryable<CreatorAliasEntity> GetAliases([FromODataUri] string key)
        {
            return GetCreatorOrThrowNotFound(key).Aliases.AsQueryable();
        }

        [EnableQuery]
        public IQueryable<WorkEntity> GetWorks([FromODataUri] string key)
        {
            var creator = GetCreatorOrThrowNotFound(key);
            return db.WorkCreators
                .Where(wc => wc.CreatorName.Equals(creator.Name, StringComparison.InvariantCultureIgnoreCase))
                .Select(wc => wc.Work)
                .Distinct();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private CreatorInfoEntity GetCreatorOrThrowNotFound(string creatorName)
        {
            var result = db.Creators.SingleOrDefault(c => c.Name.Equals(creatorName, StringComparison.InvariantCultureIgnoreCase));
            if (result == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return result;
        }
    }
}
