using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_InternetGameManagement.Models;

namespace ProjectPRN221_InternetGameManagement.Pages.Manager
{
    public class ListModel : PageModel
    {
        private readonly InternetGameManagementContext _context;

        public ListModel(InternetGameManagementContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; } // Từ khóa tìm kiếm

        public List<UserViewModel> Users { get; set; }

        public IActionResult OnGet()
        {
            // Kiểm tra quyền truy cập
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "manager")
            {
                return RedirectToPage("/Login");
            }

            Console.WriteLine($"Search Query Received (inside OnGet): {SearchQuery}");

            var query = _context.Accounts
                .Where(u => u.Role == "user");

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                Console.WriteLine("Applying Search Filter with Query: " + SearchQuery);
                query = query.Where(u => u.Username.Contains(SearchQuery));
            }

            Users = query.Select(u => new UserViewModel
            {
                Id = u.Id,
                Username = u.Username,
                Role = u.Role,
                FormattedTime = $"{u.Time / 3600:D2}h{(u.Time % 3600) / 60:D2}m{u.Time % 60:D2}s"
        }).ToList();

            return Page();
        }

        public IActionResult OnPostLogout()
        {
            // Xóa dữ liệu phiên và chuyển hướng đến trang Login
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }

        public class UserViewModel
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Role { get; set; }
            public string FormattedTime { get; set; } // Thời gian định dạng hhmm
        }
    }
}
