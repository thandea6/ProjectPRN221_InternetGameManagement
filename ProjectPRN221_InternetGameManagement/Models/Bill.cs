using System;
using System.Collections.Generic;

namespace ProjectPRN221_InternetGameManagement.Models
{
    public partial class Bill
    {
        public Bill()
        {
            BillDetails = new HashSet<BillDetail>();
        }

        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTime OrderTime { get; set; }
        public decimal TotalAmount { get; set;}
        public virtual Account Account { get; set; } = null!;
        public virtual ICollection<BillDetail> BillDetails { get; set; }
    }
}
