using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectPRN221_InternetGameManagement.Hubs;
using ProjectPRN221_InternetGameManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectPRN221_InternetGameManagement.Pages.Product
{
    public class BuyModel : PageModel
    {
        private readonly InternetGameManagementContext _context;
        private readonly IHubContext<OrderHub> _hubContext;

        public BuyModel(InternetGameManagementContext context, IHubContext<OrderHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [BindProperty]
        public string SelectedCategory { get; set; }

        public List<Models.Product> FilteredProducts { get; set; }
        public SelectList Categories { get; set; }
        public List<BillDetail> BillDetails { get; set; } = new List<BillDetail>();
        public List<Bill> OrderHistory { get; set; } = new List<Bill>();
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
                OrderHistory = _context.Bills
                    .Where(b => b.AccountId == userId.Value)
                    .OrderByDescending(b => b.OrderTime)
                    .ToList();

                foreach (var order in OrderHistory)
                {
                    order.TotalAmount = _context.BillDetails
                        .Where(bd => bd.BillId == order.Id)
                        .Sum(bd => bd.TotalPrice ?? 0);
                }

                LoadCart();
            }
        }

        public IActionResult OnPostAddToCart(int productId)
        {
            var cart = GetCart();
            var existingCartDetail = cart.FirstOrDefault(cd => cd.ProductId == productId);

            if (existingCartDetail != null)
            {
                existingCartDetail.Quantity++;
                existingCartDetail.TotalPrice = existingCartDetail.Quantity * existingCartDetail.Price;
            }
            else
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == productId);
                if (product != null)
                {
                    cart.Add(new BillDetail
                    {
                        ProductId = product.Id,
                        Quantity = 1,
                        Price = product.Price,
                        TotalPrice = product.Price
                    });
                }
            }

            SaveCart(cart);
            return RedirectToPage(new { category = SelectedCategory });
        }

        public IActionResult OnPostSubmitCart()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Login");

            var bill = new Bill
            {
                AccountId = userId.Value,
                OrderTime = DateTime.Now,
                TotalAmount = GetCart().Sum(bd => bd.TotalPrice ?? 0)
            };
            _context.Bills.Add(bill);
            _context.SaveChanges();

            var cart = GetCart();
            var productList = new List<object>();

            foreach (var item in cart)
            {
                item.BillId = bill.Id;
                _context.BillDetails.Add(item);
                var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null)
                {
                    productList.Add(new { name = product.Name, quantity = item.Quantity });
                }
            }
            _context.SaveChanges();
            _hubContext.Clients.All.SendAsync("ReceiveOrderNotification",
                 HttpContext.Session.GetString("Username"), // Tên người dùng
                productList                                // Danh sách sản phẩm
    );
            ClearCart();

            return RedirectToPage("/Product/Buy", new { category = SelectedCategory });
        }

        private List<BillDetail> GetCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            return string.IsNullOrEmpty(cartJson) ? new List<BillDetail>() : JsonConvert.DeserializeObject<List<BillDetail>>(cartJson);
        }

        private void SaveCart(List<BillDetail> cart)
        {
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        }

        private void ClearCart()
        {
            HttpContext.Session.Remove("Cart");
        }

        private void LoadCart()
        {
            BillDetails = GetCart();
            TotalAmount = BillDetails.Sum(bd => bd.TotalPrice ?? 0);
        }
    }
}
