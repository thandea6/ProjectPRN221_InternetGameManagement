using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_InternetGameManagement.Models;

namespace ProjectPRN221_InternetGameManagement.Pages.Product
{
    public class DeleteModel : PageModel
    {
        private readonly InternetGameManagementContext _context;

        public DeleteModel(InternetGameManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Product Product { get; set; }

        public IActionResult OnGet(int id)
        {
            Product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (Product == null)
            {
                return RedirectToPage("/Product/List");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (Product == null || Product.Id == 0)
            {
                return RedirectToPage("/Product/List");
            }

            var productToDelete = _context.Products.FirstOrDefault(p => p.Id == Product.Id);
            if (productToDelete != null)
            {
                _context.Products.Remove(productToDelete);
                _context.SaveChanges();
            }

            return RedirectToPage("/Product/List");
        }
    }
}
