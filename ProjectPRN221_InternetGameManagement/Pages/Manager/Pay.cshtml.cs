using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_InternetGameManagement.Models;

namespace ProjectPRN221_InternetGameManagement.Pages.Manager
{
    public class PayModel : PageModel
    {
        private readonly InternetGameManagementContext _context;

        public PayModel(InternetGameManagementContext context)
        {
            _context = context;
        }

        public Account Account { get; set; }
        public List<BillDetail> BillDetails { get; set; } = new List<BillDetail>();
        public decimal TotalAmount { get; set; }

        public IActionResult OnGet(int accountId)
        {
            Account = _context.Accounts.FirstOrDefault(a => a.Id == accountId);
            if (Account == null)
            {
                return NotFound();
            }

            var bill = _context.Bills.FirstOrDefault(b => b.AccountId == accountId);
            if (bill != null)
            {
                BillDetails = _context.BillDetails
                    .Where(bd => bd.BillId == bill.Id)
                    .ToList();

                TotalAmount = BillDetails.Sum(bd => bd.TotalPrice ?? 0);
            }

            return Page();
        }

        public IActionResult OnPostConfirmPayment(int accountId)
        {
            var bill = _context.Bills.FirstOrDefault(b => b.AccountId == accountId);
            if (bill != null)
            {
                // Xóa các BillDetail liên quan
                var billDetails = _context.BillDetails.Where(bd => bd.BillId == bill.Id).ToList();
                _context.BillDetails.RemoveRange(billDetails);

                // Xóa Bill
                _context.Bills.Remove(bill);
                _context.SaveChanges();
            }

            return RedirectToPage("/Manager/List");
        }
    }
}
