using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_InternetGameManagement.Models;

namespace ProjectPRN221_InternetGameManagement.Pages.Product
{
    public class UpdateProductModel : PageModel
    {
        private readonly InternetGameManagementContext _context;

        public UpdateProductModel(InternetGameManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Product Product { get; set; }

        public IActionResult OnGet(int id)
        {
            // Lấy thông tin sản phẩm theo ID
            Product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (Product == null)
            {
                Console.WriteLine("Không tìm thấy sản phẩm với ID: " + id);
                return RedirectToPage("/Product/List");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            // Kiểm tra trạng thái của ModelState
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model state không hợp lệ. Chi tiết lỗi:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("Error: " + error.ErrorMessage);
                }
                return Page();
            }

            // Tìm sản phẩm cần cập nhật
            var productToUpdate = _context.Products.FirstOrDefault(p => p.Id == Product.Id);
            if (productToUpdate != null)
            {
                Console.WriteLine("Sản phẩm trước khi cập nhật, Giá hiện tại: " + productToUpdate.Price);

                // Cập nhật giá sản phẩm
                productToUpdate.Price = Product.Price;
                _context.SaveChanges();

                Console.WriteLine("Giá sản phẩm đã được cập nhật thành công trong cơ sở dữ liệu.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy sản phẩm với ID: " + Product.Id);
                ModelState.AddModelError("", "Không tìm thấy sản phẩm.");
                return Page();
            }

            return RedirectToPage("/Product/List");
        }
    }
}
