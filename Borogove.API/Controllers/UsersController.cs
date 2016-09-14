using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using Auth0.Core;
using Auth0.Core.Exceptions;
using Auth0.ManagementApi;

namespace Borogove.API.Controllers
{
    [ODataRoutePrefix("Users")]
    public class UsersController : ODataController
    {
        private readonly ManagementApiClient _client;

        public UsersController()
        {
            var apiUri = new Uri($"https://{WebConfigurationManager.AppSettings["auth0:Domain"]}/api/v2");
            _client = new ManagementApiClient(WebConfigurationManager.AppSettings["auth0:ManagementToken"], apiUri);
        }

        [EnableQuery]
        public IQueryable<User> Get()
        {
            return _client.Users.GetAllAsync().Result.AsQueryable();
        }

        [EnableQuery]
        public User Get([FromODataUri] string key)
        {
            try
            {
                return _client.Users
                    .GetAsync(key)
                    .Result;
            }
            catch (AggregateException ex) when (ex.InnerException is ApiException)
            {
                var apiException = ex.InnerException as ApiException;
                if (apiException.ApiError.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                else
                {
                    throw;
                }
            }
        }

        [EnableQuery]
        [HttpGet]
        public User LoggedInUser()
        {
            var principal = Thread.CurrentPrincipal as ClaimsPrincipal;
            if (principal == null)
            {
                throw new HttpResponseException(HttpStatusCode.NoContent);
            }

            var userId = principal.Claims.ToList().FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new HttpResponseException(HttpStatusCode.NoContent);
            }
            else
            {
                return Get(userId);
            }
        }
    }
}