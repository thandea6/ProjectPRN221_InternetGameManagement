using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectPRN221_InternetGameManagement.Models;

namespace ProjectPRN221_InternetGameManagement.Pages.Product
{
    public class AddProductModel : PageModel
    {
        private readonly InternetGameManagementContext _context;

        public AddProductModel(InternetGameManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Product Product { get; set; }

        public SelectList Categories { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            // Lấy danh sách các danh mục từ bảng Product để hiển thị trong combo box
            var categoryList = _context.Products.Select(p => p.Category).Distinct().ToList();
            Categories = new SelectList(categoryList);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please fill in all fields correctly.";
                return Page();
            }

            // Kiểm tra xem sản phẩm đã tồn tại với cùng Tên và Category hay chưa
            bool productExists = _context.Products.Any(p => p.Name == Product.Name && p.Category == Product.Category);
            if (productExists)
            {
                ErrorMessage = "A product with the same name and category already exists.";
                return Page();
            }

            // Thêm sản phẩm vào cơ sở dữ liệu nếu không bị trùng lặp
            _context.Products.Add(Product);
            _context.SaveChanges();

            // Điều hướng người dùng về trang danh sách sản phẩm sau khi thêm thành công
            return RedirectToPage("/Product/List");
        }
    }

}
