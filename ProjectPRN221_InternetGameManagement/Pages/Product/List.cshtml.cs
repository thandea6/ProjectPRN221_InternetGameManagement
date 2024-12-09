using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_InternetGameManagement.Models;

namespace ProjectPRN221_InternetGameManagement.Pages.Product
{
    public class ListModel : PageModel
    {
        private readonly InternetGameManagementContext _context;

        public ListModel(InternetGameManagementContext context)
        {
            _context = context;
        }

        public List<Models.Product> Products { get; set; }

        public void OnGet()
        {
            // Lấy danh sách sản phẩm từ cơ sở dữ liệu
            Products = _context.Products.ToList();
        }
    }
}
