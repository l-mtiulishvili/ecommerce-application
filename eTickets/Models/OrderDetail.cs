using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int MovieId { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual Order Order { get; set; }
    }
}
