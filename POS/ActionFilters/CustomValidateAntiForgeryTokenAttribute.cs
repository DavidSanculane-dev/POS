using System;
using NLog.Filters;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace POS.ActionFilters
{
    
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class CustomValidateAntiForgeryTokenAttribute : System.Web.Mvc.FilterAttribute, System.Web.Mvc.IAuthorizationFilter
    {
        /// <summary>
        /// The ValidateAntiForgeryTokenAttribute.
        /// </summary>
        private System.Web.Mvc.ValidateAntiForgeryTokenAttribute Validator { get; set; }

        /// <summary>
        /// The Accept Verbs Attribute.
        /// </summary>
        private System.Web.Mvc.AcceptVerbsAttribute AcceptVerbsAttribute { get; set; }

        /// <summary>
        /// Gets the salt.
        /// </summary>
        public string Salt { get; private set; }

        /// <summary>
        /// Gets the verbs.
        /// </summary>
        public HttpVerbs Verbs { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomValidateAntiForgeryTokenAttribute"/> class.
        /// </summary>
        /// <param name="verbs">The verbs.</param>
        public CustomValidateAntiForgeryTokenAttribute(HttpVerbs verbs)
            : this(verbs, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomValidateAntiForgeryTokenAttribute"/> class.
        /// </summary>
        /// <param name="verbs">The verbs.</param>
        /// <param name="salt">The salt.</param>
        public CustomValidateAntiForgeryTokenAttribute(HttpVerbs verbs, string salt)
        {
            Verbs = verbs;
            Salt = salt;

            AcceptVerbsAttribute = new System.Web.Mvc.AcceptVerbsAttribute(Verbs);
            Validator = new System.Web.Mvc.ValidateAntiForgeryTokenAttribute
            {
                //Salt = salt
            };
        }

        /// <summary>
        /// Called when authorization is required.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var httpMethodOverride = filterContext.HttpContext.Request.GetHttpMethodOverride();

            var found = false;
            foreach (var verb in AcceptVerbsAttribute.Verbs)
            {
                if (verb.Equals(httpMethodOverride, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                }
            }

            if (found && !filterContext.RequestContext.RouteData.Values["action"].ToString().StartsWith("Json"))
            {
                Validator.OnAuthorization(filterContext);
            }
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}