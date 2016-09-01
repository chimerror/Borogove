using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using Auth0.ManagementApi;
using Borogove.DataAccess;
using Auth0.Core;
using System.Collections.Generic;

namespace Borogove.API.Controllers
{
    [Authorize]
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
            return _client.Users.GetAsync(key).Result;
        }
    }
}