using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ProjectPRN221_InternetGameManagement.Models;

namespace ProjectPRN221_InternetGameManagement.Pages.Product
{
    public class BuyModel : PageModel
    {
        private readonly InternetGameManagementContext _context;

        public BuyModel(InternetGameManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string SelectedCategory { get; set; }

        public List<Models.Product> FilteredProducts { get; set; }
        public SelectList Categories { get; set; }
        public List<BillDetail> BillDetails { get; set; } = new List<BillDetail>();
        public decimal TotalAmount { get; set; }

        public void OnGet(string category)
        {
            Categories = new SelectList(_context.Products.Select(p => p.Category).Distinct().ToList());
            SelectedCategory = category;
            FilteredProducts = string.IsNullOrEmpty(category)
                ? _context.Products.ToList()
                : _context.Products.Where(p => p.Category == category).ToList();

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                var bill = _context.Bills.FirstOrDefault(b => b.AccountId == userId);
                if (bill != null)
                {
                    BillDetails = _context.BillDetails
                        .Where(bd => bd.BillId == bill.Id)
                        .ToList();

                    TotalAmount = BillDetails.Sum(bd => bd.TotalPrice ?? 0);
                }
            }
        }

        public IActionResult OnPostAddToBill(int productId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Login");

            var bill = _context.Bills.FirstOrDefault(b => b.AccountId == userId);
            if (bill == null)
            {
                bill = new Bill { AccountId = userId.Value };
                _context.Bills.Add(bill);
                _context.SaveChanges();
            }

            var existingBillDetail = _context.BillDetails
                .FirstOrDefault(bd => bd.BillId == bill.Id && bd.ProductId == productId);

            if (existingBillDetail != null)
            {
                existingBillDetail.Quantity += 1;
                existingBillDetail.TotalPrice = existingBillDetail.Quantity * existingBillDetail.Price;
            }
            else
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == productId);
                if (product != null)
                {
                    var billDetail = new BillDetail
                    {
                        BillId = bill.Id,
                        ProductId = product.Id,
                        Quantity = 1,
                        Price = product.Price,
                        TotalPrice = product.Price
                    };
                    _context.BillDetails.Add(billDetail);
                }
            }

            _context.SaveChanges();

            return RedirectToPage("/Product/Buy", new { category = SelectedCategory });
        }

        public IActionResult OnPostDeleteBillDetail(int billDetailId)
        {
            var billDetail = _context.BillDetails.FirstOrDefault(bd => bd.Id == billDetailId);
            if (billDetail != null)
            {
                _context.BillDetails.Remove(billDetail);
                _context.SaveChanges();
            }

            return RedirectToPage("/Product/Buy", new { category = SelectedCategory });
        }
    }
}
