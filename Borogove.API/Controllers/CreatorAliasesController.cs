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
    [ODataRoutePrefix("CreatorAliases")]
    public class CreatorAliasesController : ODataController
    {
        WorkContext db = new WorkContext("name=WorkContext");

        [EnableQuery]
        public IQueryable<CreatorAliasEntity> Get()
        {
            return db.CreatorAliases;
        }

        [EnableQuery]
        public SingleResult<CreatorAliasEntity> Get([FromODataUri] string key)
        {
            var result = db.CreatorAliases.Where(ca => ca.Alias.Equals(key, StringComparison.InvariantCultureIgnoreCase));
            return SingleResult.Create(result);
        }

        [EnableQuery]
        public SingleResult<CreatorInfoEntity> GetCreator([FromODataUri] string key)
        {
            var creatorAlias = GetCreatorAliasOrThrowNotFound(key);
            var creator = db.Creators
                .Where(c => c.Name.Equals(creatorAlias.AliasOf, StringComparison.InvariantCultureIgnoreCase));
            return SingleResult.Create(creator);
        }

        [EnableQuery]
        public IQueryable<CreatorAliasEntity> GetOtherAliases([FromODataUri] string key)
        {
            var creatorAlias = GetCreatorAliasOrThrowNotFound(key);
            return db.CreatorAliases
                .Where(ca => ca.AliasOf.Equals(creatorAlias.AliasOf, StringComparison.InvariantCultureIgnoreCase) && !ca.Alias.Equals(creatorAlias.Alias));
        }

        [EnableQuery]
        public IQueryable<WorkEntity> GetWorks([FromODataUri] string key)
        {
            var creatorAlias = GetCreatorAliasOrThrowNotFound(key);
            return db.WorkCreators
                .Where(wc => wc.WorkedAsName.Equals(creatorAlias.Alias, StringComparison.InvariantCultureIgnoreCase))
                .Select(wc => wc.Work)
                .Distinct();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private CreatorAliasEntity GetCreatorAliasOrThrowNotFound(string aliasName)
        {
            var result = db.CreatorAliases.SingleOrDefault(c => c.Alias.Equals(aliasName, StringComparison.InvariantCultureIgnoreCase));
            if (result == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return result;
        }
    }
}
