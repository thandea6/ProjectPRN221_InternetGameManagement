using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_InternetGameManagement.Models;

namespace ProjectPRN221_InternetGameManagement.Pages.Manager
{
    public class AddUserModel : PageModel
    {
        private readonly InternetGameManagementContext _context;

        public AddUserModel(InternetGameManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public int Time { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public IActionResult OnPost()
        {
            // Kiểm tra nếu Username đã tồn tại
            var existingUser = _context.Accounts.FirstOrDefault(u => u.Username == Username);
            if (existingUser != null)
            {
                ErrorMessage = "Username already exists. Please choose a different one.";
                return Page();
            }

            // Tạo đối tượng người dùng mới
            var newUser = new Account
            {
                Username = Username,
                Password = Password,
                Role = "user", // Đặt vai trò là "user" cho người dùng mới
                Time = Time
            };

            // Thêm người dùng mới vào cơ sở dữ liệu
            _context.Accounts.Add(newUser);
            _context.SaveChanges();

            SuccessMessage = "User added successfully!";
            return Page(); // Giữ lại trang để hiển thị thông báo thành công
        }
    }
}
