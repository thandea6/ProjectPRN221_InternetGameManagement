using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_InternetGameManagement.Models;

namespace ProjectPRN221_InternetGameManagement.Pages.User
{
    public class UserScreenModel : PageModel
    {
        private readonly InternetGameManagementContext _context;

        public UserScreenModel(InternetGameManagementContext context)
        {
            _context = context;
        }

        public string RemainingTime { get; set; }
        public int RemainingMinutes { get; set; }

        public IActionResult OnGet()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userId == null || userRole != "user")
            {
                return RedirectToPage("/Login");
            }

            var user = _context.Accounts.FirstOrDefault(u => u.Id == userId.Value);

            if (user != null)
            {
                RemainingMinutes = user.Time ?? 0;
                RemainingTime = $"{RemainingMinutes / 60:D2}h{RemainingMinutes % 60:D2}m";
            }
            else
            {
                RemainingMinutes = 0;
                RemainingTime = "00h00m";
            }

            return Page();
        }

        public IActionResult OnPostUpdateTime([FromBody] int remainingMinutes)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                var user = _context.Accounts.FirstOrDefault(u => u.Id == userId.Value);

                if (user != null)
                {
                    user.Time = remainingMinutes;
                    _context.SaveChanges();
                    Console.WriteLine($"Updated remaining time for user {userId}: {remainingMinutes} minutes");
                }
            }

            return new JsonResult(new { success = true });
        }
    }
}
