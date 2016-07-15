using System;
using System.IdentityModel.Services;
using System.Web;

namespace Borogove.API
{
    /// <summary>
    /// Handler to log out the current user.
    /// </summary>
    public class Logout : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            FederatedAuthentication.SessionAuthenticationModule.SignOut();

            var profileCookie = context.Request.Cookies["BorogoveProfile"];
            if (profileCookie != null)
            {
                // Release the cookie from its servitude. (cf. U.S. Const. amend. XII, § 1)
                profileCookie.Expires = new DateTime(1865, 12, 6);
                context.Response.Cookies.Add(profileCookie);
            }

            var returnUrl = context.Request.QueryString["ReturnUrl"];
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                context.Response.Redirect(returnUrl, true);
            }
            else
            {
                context.Response.Redirect("/", true);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}