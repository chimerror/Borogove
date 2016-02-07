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
            var result = GetTagOrAlias(key);
            return SingleResult.Create(result);
        }

        [EnableQuery]
        public IQueryable<TagAliasEntity> GetAliases([FromODataUri] string key)
        {
            return GetTagOrAliasOrThrowNotFound(key).Aliases.AsQueryable();
        }

        [EnableQuery]
        public IQueryable<TagEntity> GetImplications([FromODataUri] string key)
        {
            return GetTagOrAliasOrThrowNotFound(key).Implications.AsQueryable();
        }

        [EnableQuery]
        public IQueryable<WorkEntity> GetWorks([FromODataUri] string key)
        {
            var tag = GetTagOrAliasOrThrowNotFound(key);
            return db.Works
                .Where(w => w.Tags.Select(t => t.TagName).Contains(tag.TagName));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private IQueryable<TagEntity> GetTagOrAlias(string tagName)
        {
            var result = ExactTagExists(tagName) ?
                db.Tags
                    .Where(t => t.TagName.Equals(tagName, StringComparison.InvariantCultureIgnoreCase)) :
                db.TagAliases
                    .Where(ta => ta.Alias.Equals(tagName, StringComparison.InvariantCultureIgnoreCase))
                    .Join(db.Tags, ta => ta.TagName, t => t.TagName, (ta, t) => t);
            return result;
        }

        private bool ExactTagExists(string tagName)
        {
            return db.Tags.Any(t => t.TagName.Equals(tagName, StringComparison.InvariantCultureIgnoreCase));
        }

        private TagEntity GetTagOrAliasOrThrowNotFound(string tagName)
        {
            var result = GetTagOrAlias(tagName).SingleOrDefault();
            if (result == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return result;
        }
    }
}
