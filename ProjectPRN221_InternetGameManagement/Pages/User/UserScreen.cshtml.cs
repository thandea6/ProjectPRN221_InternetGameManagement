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
        public int RemainingMinutes { get; set; }

        // Phương thức OnGet để lấy thông tin người dùng và thời gian còn lại
        //public IActionResult OnGet()
        //{
        //    var userId = HttpContext.Session.GetInt32("UserId");
        //    var userRole = HttpContext.Session.GetString("UserRole");

        //    // Kiểm tra nếu không phải người dùng hợp lệ hoặc không phải người dùng thì chuyển đến trang đăng nhập
        //    if (userId == null || userRole != "user")
        //    {
        //        return RedirectToPage("/Login");
        //    }

        //    // Lấy thông tin người dùng từ cơ sở dữ liệu
        //    var user = _context.Accounts.FirstOrDefault(u => u.Id == userId.Value);

        //    if (user != null)
        //    {
        //        // Lấy thời gian còn lại từ cơ sở dữ liệu
        //        RemainingMinutes = user.Time ?? 0; // Sử dụng `??` để đảm bảo giá trị mặc định là 0 nếu không có dữ liệu
        //        RemainingTime = RemainingMinutes * 60; // Chuyển từ phút sang giây
        //    }

        //    return Page();
        //}
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
                RemainingTime = RemainingMinutes * 60;  // Tính lại thời gian còn lại bằng giây
            }

            return Page();
        }


        // Phương thức OnPost để cập nhật thời gian còn lại trên server
        //public IActionResult OnPost(int remainingTime)
        //{
        //    var userId = HttpContext.Session.GetInt32("UserId");

        //    if (userId == null)
        //    {
        //        return RedirectToPage("/Login");
        //    }

        //    var user = _context.Accounts.FirstOrDefault(u => u.Id == userId.Value);

        //    if (user != null)
        //    {
        //        // Cập nhật thời gian còn lại vào cơ sở dữ liệu
        //        user.Time = remainingTime / 60; // Chuyển từ giây về phút
        //        _context.SaveChanges(); // Lưu lại thay đổi
        //    }

        //    return new JsonResult(new { success = true });
        //}
        [HttpPost]
        public IActionResult OnPostUpdateTime(int remainingTime)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            var user = _context.Accounts.FirstOrDefault(u => u.Id == userId.Value);

            if (user != null)
            {
                // Cập nhật giá trị RemainingMinutes
                user.Time = remainingTime / 60;  // Chuyển thời gian còn lại từ giây sang phút
                _context.SaveChanges();  // Lưu thay đổi vào cơ sở dữ liệu
            }

            return new JsonResult(new { success = true });
        }
    }
}
