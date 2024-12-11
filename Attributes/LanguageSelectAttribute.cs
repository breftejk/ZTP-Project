using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ZTP_Project.Attributes
{
    /// <summary>
    /// Attribute used to ensure a language is selected in the session before executing the action.
    /// Redirects to the language selection page if no language is set.
    /// </summary>
    public class LanguageSelectedAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Executes before the action method to check if a language is selected in the session.
        /// </summary>
        /// <param name="context">The context for the action filter.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            var returnUrl = request.Path + request.QueryString;

            var languageId = context.HttpContext.Session.GetInt32("SelectedLanguage");
            if (languageId == null)
            {
                context.Result = new RedirectToActionResult("SelectLanguage", "LanguageSelection", new { returnUrl = returnUrl });
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}