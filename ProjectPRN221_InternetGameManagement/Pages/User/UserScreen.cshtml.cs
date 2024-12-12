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
        public int UserId { get; set; }
        public int RemainingTime { get; set; }

        // Phương thức OnGet để lấy thông tin người dùng và thời gian còn lại
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
                RemainingTime = user.Time ?? 0; // Lấy thời gian từ cơ sở dữ liệu
                                                // Kiểm tra nếu thời gian còn lại là 0, yêu cầu người dùng nạp tiền
                if (RemainingTime == 0)
                {
                    TempData["ErrorMOfTime"] = "Bạn đã hết tiền, cần nạp thêm để sử dụng dịch vụ.";
                    return RedirectToPage("/Login"); // Chuyển hướng về trang đăng nhập
                }
                HttpContext.Session.SetInt32("RemainingTime", RemainingTime); // Lưu vào Session
            }

            return Page();
        }

        
        [HttpPost]
        public IActionResult OnPost()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                // Xóa session
                HttpContext.Session.Clear();

                return RedirectToPage("/Login");
            }

            return RedirectToPage("/Login"); // Nếu không có UserId trong session, chuyển hướng về Login
        }



    }


}

