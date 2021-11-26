using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Models
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; }
        public Movie movie { get; set; }
        public int amount { get; set; }
        public string ShoppingCartId { get; set; }
    }
}
