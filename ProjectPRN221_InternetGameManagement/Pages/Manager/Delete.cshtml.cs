using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221_InternetGameManagement.Models;

namespace ProjectPRN221_InternetGameManagement.Pages.Manager
{
    public class DeleteModel : PageModel
    {
		private readonly InternetGameManagementContext _context;

		public DeleteModel(InternetGameManagementContext context)
		{
			_context = context;
		}

		public IActionResult OnGet(int id)
		{
			var user = _context.Accounts.FirstOrDefault(u => u.Id == id && u.Role == "user");
			if (user != null)
			{
				_context.Accounts.Remove(user);
				_context.SaveChanges();
			}
			return RedirectToPage("/Manager/List");
		}
	}
}
