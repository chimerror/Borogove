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
    [ODataRoutePrefix("Languages")]
    public class LanguagesController : ODataController
    {
        WorkContext db = new WorkContext();

        [EnableQuery]
        public IQueryable<LanguageEntity> Get()
        {
            return db.Languages;
        }

        [EnableQuery]
        public SingleResult<LanguageEntity> Get([FromODataUri] string key)
        {
            var result = db.Languages.Where(l => l.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase));
            return SingleResult.Create(result);
        }

        [EnableQuery]
        public IQueryable<WorkEntity> GetWorks([FromODataUri] string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return db.Works
                    .Where(w => w.Language == null);
            }

            var language = GetLanguageOrThrowNotFound(key);
            return db.Works
                .Where(w => w.Language != null && w.Language.Name.Equals(language.Name));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private LanguageEntity GetLanguageOrThrowNotFound(string languageName)
        {
            var result = db.Languages.SingleOrDefault(l => l.Name.Equals(languageName, StringComparison.InvariantCultureIgnoreCase));
            if (result == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return result;
        }
    }
}
