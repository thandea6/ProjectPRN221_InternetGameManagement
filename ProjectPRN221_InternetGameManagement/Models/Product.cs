using System;
using System.Collections.Generic;

namespace ProjectPRN221_InternetGameManagement.Models
{
    public partial class Product
    {
        public Product()
        {
            BillDetails = new HashSet<BillDetail>();
        }

        public int Id { get; set; }
        public string Category { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }

        public virtual ICollection<BillDetail> BillDetails { get; set; }
    }
}
