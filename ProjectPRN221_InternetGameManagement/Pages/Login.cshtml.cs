using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_InternetGameManagement.Models;



namespace ProjectPRN221_InternetGameManagement.Pages
{
    public class LoginModel : PageModel
    {
        private readonly InternetGameManagementContext _context;

        public LoginModel(InternetGameManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public string ErrorMessage { get; set; }

        public IActionResult OnPost()
        {
            var user = _context.Accounts.FirstOrDefault(u => u.Username == Username && u.Password == Password);

            if (user != null && user.Role == "user")
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserRole", user.Role);
                HttpContext.Session.SetString("Username", user.Username);

                return RedirectToPage("/User/UserScreen");
            }
            else if (user != null && user.Role == "manager")
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserRole", user.Role);
                HttpContext.Session.SetString("Username", user.Username);

                return RedirectToPage("/Manager/List");
            }
            else
            {
                ErrorMessage = "Invalid username or password.";
                return Page();
            }
        }
    }
}