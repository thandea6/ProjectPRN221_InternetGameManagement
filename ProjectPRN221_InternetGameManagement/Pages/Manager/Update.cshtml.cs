using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_InternetGameManagement.Models;

namespace ProjectPRN221_InternetGameManagement.Pages.Manager
{
    public class UpdateModel : PageModel
    {
        private readonly InternetGameManagementContext _context;

        public UpdateModel(InternetGameManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Role { get; set; }
        [BindProperty]
        public int? Time { get; set; } // Sử dụng nullable int
        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public IActionResult OnGet(int id)
        {
            // Tìm người dùng theo ID
            var user = _context.Accounts.FirstOrDefault(u => u.Id == id && u.Role == "user");
            if (user == null)
            {
                ErrorMessage = "User not found.";
                return RedirectToPage("/Manager/List");
            }

            // Đổ dữ liệu người dùng vào model để hiển thị
            Id = user.Id;
            Username = user.Username;
            Role = user.Role;
            Time = user.Time; // Time có thể là null

            return Page();
        }

        public IActionResult OnPost()
        {
            // Tìm người dùng theo ID
            var user = _context.Accounts.FirstOrDefault(u => u.Id == Id && u.Role == "user");
            if (user == null)
            {
                ErrorMessage = "User not found.";
                return Page();
            }

            // Cập nhật mật khẩu mới
            user.Password = Password;
            _context.SaveChanges();

            SuccessMessage = "Password updated successfully!";
            return Page(); // Giữ lại trang để hiển thị thông báo thành công
        }
    }
}
