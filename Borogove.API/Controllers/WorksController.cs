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
        WorkContext db = new WorkContext();

        [EnableQuery]
        public IQueryable<WorkEntity> Get()
        {
            return db.Works;
        }

        [EnableQuery]
        public SingleResult<WorkEntity> Get([FromODataUri] Guid key)
        {
            var result = db.Works.Where(w => w.Identifier.Equals(key));
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
            var result = db.Works.Where(w => w.Identifier.Equals(key)).Select(w => w.Content);
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
            db.Dispose();
            base.Dispose(disposing);
        }

        private WorkEntity GetWorkOrThrowNotFound(Guid key)
        {
            var result = db.Works.SingleOrDefault(w => w.Identifier.Equals(key));
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