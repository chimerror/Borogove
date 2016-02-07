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
    [ODataRoutePrefix("Tags")]
    public class TagsController : ODataController
    {
        WorkContext db = new WorkContext();

        [EnableQuery]
        public IQueryable<TagEntity> Get()
        {
            return db.Tags;
        }

        [EnableQuery]
        public SingleResult<TagEntity> Get([FromODataUri] string key)
        {
            var result = db.Tags.Where(t => t.TagName.Equals(key, StringComparison.InvariantCultureIgnoreCase));
            return SingleResult.Create(result);
        }

        [EnableQuery]
        public IQueryable<TagAliasEntity> GetAliases([FromODataUri] string key)
        {
            return GetTagOrThrowNotFound(key).Aliases.AsQueryable();
        }

        [EnableQuery]
        public IQueryable<TagEntity> GetImplications([FromODataUri] string key)
        {
            return GetTagOrThrowNotFound(key).Implications.AsQueryable();
        }

        [EnableQuery]
        public IQueryable<WorkEntity> GetWorks([FromODataUri] string key)
        {
            var tag = GetTagOrThrowNotFound(key);
            return db.Works
                .Where(w => w.Tags.Select(t => t.TagName).Contains(tag.TagName));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private TagEntity GetTagOrThrowNotFound(string tagName)
        {
            var result = db.Tags.SingleOrDefault(l => l.TagName.Equals(tagName, StringComparison.InvariantCultureIgnoreCase));
            if (result == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return result;
        }
    }
}
