namespace Borogove.API
{
	using Auth0.AspNet;
	using System.Collections.Generic;
	using System.Configuration;
	using System.IdentityModel.Services;
	using System.Linq;
	using System.Web;

    public class LoginCallback : IHttpHandler
    {
        private readonly Auth0.Client client = new Auth0.Client(
                                ConfigurationManager.AppSettings["auth0:ClientId"],
                                ConfigurationManager.AppSettings["auth0:ClientSecret"],
                                ConfigurationManager.AppSettings["auth0:Domain"]);

        public void ProcessRequest(HttpContext context)
        {
            var token = client.ExchangeAuthorizationCodePerAccessToken(context.Request.QueryString["code"], context.Request.Url.ToString());
            var profile = client.GetUserInfo(token);

            var user = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("name", profile.Name),
                new KeyValuePair<string, object>("email", profile.Email),
                new KeyValuePair<string, object>("family_name", profile.FamilyName),
                new KeyValuePair<string, object>("gender", profile.Gender),
                new KeyValuePair<string, object>("given_name", profile.GivenName),
                new KeyValuePair<string, object>("nickname", profile.Nickname),
                new KeyValuePair<string, object>("picture", profile.Picture),
                new KeyValuePair<string, object>("user_id", profile.UserId),
                new KeyValuePair<string, object>("id_token", token.IdToken),
                new KeyValuePair<string, object>("access_token", token.AccessToken),
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

        public bool IsReusable { get { return false; } }
    }
}