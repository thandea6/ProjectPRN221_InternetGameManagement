using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace ProjectPRN221_InternetGameManagement.Pages.User
{
    [AllowAnonymous]
    public class ChatModel : PageModel
    {
        public string Username { get; private set; }
        public string UserRole { get; private set; }


        public void OnGet()
        {
            Username = HttpContext.Session.GetString("Username") ?? "Anonymous";
            UserRole = HttpContext.Session.GetString("UserRole");
        }
    }
}
