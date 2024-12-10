using System;
using System.Collections.Generic;

namespace ProjectPRN221_InternetGameManagement.Models
{
    public partial class BillDetail
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal? TotalPrice { get; set; }

        public virtual Bill Bill { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
