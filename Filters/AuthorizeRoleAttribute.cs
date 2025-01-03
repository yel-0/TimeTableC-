using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TimeTable.Filters
{
    public class AuthorizeRoleAttribute : ActionFilterAttribute
    {
        private readonly int _roleId;

        // Constructor accepts a role ID that you want to check against (e.g., 0 for Admin, 1 for User)
        public AuthorizeRoleAttribute(int roleId)
        {
            _roleId = roleId; // Store the role ID (e.g., 0 for Admin, 1 for User)
        }

        // Override the OnActionExecuting method to check the user's role before the action is executed
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Get the user's role from the session
            var userRole = context.HttpContext.Session.GetInt32("UserRole");

            // If the user is not logged in or their role does not match the required role
            if (!userRole.HasValue || userRole.Value != _roleId)
            {
                // Redirect to the AccessDenied action (create this action in your AccountController)
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
            }

            // Call base method to ensure other filters or logic are applied
            base.OnActionExecuting(context);
        }
    }
}
