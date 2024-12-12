using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_InternetGameManagement.Models;

namespace ProjectPRN221_InternetGameManagement.Pages.Manager
{
    public class AddTimeModel : PageModel
    {
        private readonly InternetGameManagementContext _context;

        public AddTimeModel(InternetGameManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        public int Amount { get; set; } // Số tiền nạp

        public string Username { get; set; }
        public int? CurrentTime { get; set; } // Thời gian hiện tại của người dùng
        public string SuccessMessage { get; set; }

        public IActionResult OnGet(int id)
        {
            // Tìm người dùng theo ID
            var user = _context.Accounts.FirstOrDefault(u => u.Id == id && u.Role == "user");
            if (user == null)
            {
                return RedirectToPage("/Manager/List");
            }

            // Đổ dữ liệu người dùng vào model để hiển thị
            Id = user.Id;
            Username = user.Username;
            CurrentTime = user.Time;

            return Page();
        }

        public IActionResult OnPost()
        {
            // Tìm người dùng theo ID
            var user = _context.Accounts.FirstOrDefault(u => u.Id == Id && u.Role == "user");
            if (user == null)
            {
                return RedirectToPage("/Manager/List");
            }

            // Chuyển đổi số tiền thành thời gian không làm tròn
            double additionalTime = ((Amount * 60.0) / 3000.0) * 60;

            // Cộng thêm thời gian vào tài khoản người dùng
            user.Time = (user.Time ?? 0) + (int)additionalTime;
            _context.SaveChanges();

            // Cập nhật thông báo thành công, bao gồm cả phút lẻ
            SuccessMessage = $"Deposited {Amount} VND, added {additionalTime:F2} minutes to {Username}'s account.";
            CurrentTime = user.Time;

            return Page(); // Giữ lại trang để hiển thị thông báo thành công
        }
    }
}
