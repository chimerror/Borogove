namespace Borogove.API
{
    using System;
    using System.Threading.Tasks;
    using Auth0.AuthenticationApi;
    using Auth0.AuthenticationApi.Models;
    using Auth0.AspNet;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IdentityModel.Services;
    using System.Linq;
    using System.Web;

    public class LoginCallback : HttpTaskAsyncHandler
    {
        public override async Task ProcessRequestAsync(HttpContext context)
        {
            AuthenticationApiClient client = new AuthenticationApiClient(
                new Uri(string.Format("https://{0}", ConfigurationManager.AppSettings["auth0:Domain"])));

            var token = await client.ExchangeCodeForAccessTokenAsync(new ExchangeCodeRequest
            {
                ClientId = ConfigurationManager.AppSettings["auth0:ClientId"],
                ClientSecret = ConfigurationManager.AppSettings["auth0:ClientSecret"],
                AuthorizationCode = context.Request.QueryString["code"],
                RedirectUri = context.Request.Url.ToString()
            });

            var profile = await client.GetUserInfoAsync(token.AccessToken);

            var user = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("name", profile.FullName),
                new KeyValuePair<string, object>("email", profile.Email),
                new KeyValuePair<string, object>("family_name", profile.LastName),
                new KeyValuePair<string, object>("given_name", profile.FirstName),
                new KeyValuePair<string, object>("nickname", profile.NickName),
                new KeyValuePair<string, object>("picture", profile.Picture),
                new KeyValuePair<string, object>("user_id", profile.UserId),
                new KeyValuePair<string, object>("id_token", token.IdToken),
                new KeyValuePair<string, object>("access_token", token.AccessToken),
                new KeyValuePair<string, object>("refresh_token", token.RefreshToken),
                new KeyValuePair<string, object>("connection", profile.Identities.First().Connection),
                new KeyValuePair<string, object>("provider", profile.Identities.First().Provider)
            };

            FederatedAuthentication.SessionAuthenticationModule.CreateSessionCookie(user);

            if (context.Request.QueryString["state"] != null && context.Request.QueryString["state"].StartsWith("ru="))
            {
                var state = HttpUtility.ParseQueryString(context.Request.QueryString["state"]);
                context.Response.Redirect(state["ru"], true);
            }

            context.Response.Redirect("/");
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}