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
    [ODataRoutePrefix("Works")]
    public class WorksController : ODataController
    {
        public const string SearchInputQueryString = "searchInput";

        private WorkContext _db = new WorkContext("name=WorkContext");

        [EnableQuery]
        public IQueryable<WorkEntity> Get()
        {
            return _db.Works;
        }

        [EnableQuery()]
        [HttpGet]
        public IQueryable<WorkEntity> Search([FromODataUri] string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                input = Request.GetQueryNameValuePairs()
                    .FirstOrDefault(kvp => kvp.Key.Equals(SearchInputQueryString))
                    .Value ?? string.Empty;
            }

            var matchingTagsByAlias = _db.TagAliases
                .Where(ta => ta.Alias.Contains(input))
                .Join(_db.Tags, ta => ta.TagName, t => t.TagName, (ta, t) => t);
            var matchingTags = _db.Tags
                .Where(t => t.TagName.Contains(input))
                .Concat(matchingTagsByAlias);

            return _db.Works
                .Where(w =>
                    w.Content.Contains(input) ||
                    w.Description.Contains(input) ||
                    w.Title.Contains(input) ||
                    w.Tags.Any(t => matchingTags.Contains(t)) ||
                    w.WorkCreators.Any(wc => wc.CreatorName.Contains(input) || wc.WorkedAsName.Contains(input)));
        }

        [EnableQuery]
        public SingleResult<WorkEntity> Get([FromODataUri] Guid key)
        {
            var result = _db.Works.Where(w => w.Identifier.Equals(key));
            return SingleResult.Create(result);
        }

        [EnableQuery]
        public IQueryable<WorkCreatorEntity> GetWorkCreators([FromODataUri] Guid key)
        {
            return GetWorkOrThrowNotFound(key).WorkCreators.AsQueryable();
        }

        [EnableQuery()]
        public IQueryable<TagEntity> GetTags([FromODataUri] Guid key)
        {
            return GetWorkOrThrowNotFound(key).Tags.AsQueryable();
        }

        [EnableQuery]
        public SingleResult<string> GetContent([FromODataUri] Guid key)
        {
            var result = _db.Works.Where(w => w.Identifier.Equals(key)).Select(w => w.Content);
            return SingleResult.Create(result);
        }

        [EnableQuery]
        public WorkEntity GetParent([FromODataUri] Guid key)
        {
            return GetWorkPropertyOrThrowNoContent(key, w => w.Parent);
        }

        [EnableQuery()]
        public IQueryable<WorkEntity> GetChildren([FromODataUri] Guid key)
        {
            return GetWorkOrThrowNotFound(key).Children.AsQueryable();
        }

        [EnableQuery()]
        public IQueryable<WorkEntity> GetPreviousWorks([FromODataUri] Guid key)
        {
            return GetWorkOrThrowNotFound(key).PreviousWorks.AsQueryable();
        }

        [EnableQuery()]
        public IQueryable<WorkEntity> GetNextWorks([FromODataUri] Guid key)
        {
            return GetWorkOrThrowNotFound(key).NextWorks.AsQueryable();
        }

        [EnableQuery]
        public WorkEntity GetDraftOf([FromODataUri] Guid key)
        {
            return GetWorkPropertyOrThrowNoContent(key, w => w.DraftOf);
        }

        [EnableQuery()]
        public IQueryable<WorkEntity> GetDrafts([FromODataUri] Guid key)
        {
            return GetWorkOrThrowNotFound(key).Drafts.AsQueryable();
        }

        [EnableQuery]
        public WorkEntity GetArtifactOf([FromODataUri] Guid key)
        {
            return GetWorkPropertyOrThrowNoContent(key, w => w.ArtifactOf);
        }

        [EnableQuery()]
        public IQueryable<WorkEntity> GetArtifacts([FromODataUri] Guid key)
        {
            return GetWorkOrThrowNotFound(key).Artifacts.AsQueryable();
        }

        [EnableQuery]
        public WorkEntity GetCommentsOn([FromODataUri] Guid key)
        {
            return GetWorkPropertyOrThrowNoContent(key, w => w.CommentsOn);
        }

        [EnableQuery()]
        public IQueryable<WorkEntity> GetComments([FromODataUri] Guid key)
        {
            return GetWorkOrThrowNotFound(key).Comments.AsQueryable();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }

        private WorkEntity GetWorkOrThrowNotFound(Guid key)
        {
            var result = _db.Works.SingleOrDefault(w => w.Identifier.Equals(key));
            if (result == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return result;
        }

        private T GetWorkPropertyOrThrowNoContent<T>(Guid key, Func<WorkEntity, T> propertyFunction)
        {
            T result = propertyFunction(GetWorkOrThrowNotFound(key));
            if (result == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NoContent));
            }
            return result;
        }
    }
}