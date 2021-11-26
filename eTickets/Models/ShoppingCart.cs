using eTickets.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Models
{
    public class ShoppingCart
    {
        private readonly AppDbContext _appDbContext;

        public ShoppingCart(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public string ShoppingCartId { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            var context = services.GetService<AppDbContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public void AddToCart(Movie movie, int amount)
        {
            var shoppingCartItem = _appDbContext.ShoppingcartItems.SingleOrDefault(
                s => s.movie.id == movie.id && s.ShoppingCartId == ShoppingCartId && s.amount == amount);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    movie = movie,
                    amount = 1
                };
                _appDbContext.ShoppingcartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.amount++;
            }
            _appDbContext.SaveChanges();
        }

        public int RemoveFromCart(Movie movie)
        {
            var shoppingCartItem =
                _appDbContext.ShoppingcartItems.SingleOrDefault(
                    s => s.movie.id == movie.id && s.ShoppingCartId == ShoppingCartId);
            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.amount > 1)
                {
                    shoppingCartItem.amount--;
                    localAmount = shoppingCartItem.amount;
                }
                else
                {
                    _appDbContext.ShoppingcartItems.Remove(shoppingCartItem);
                }
            }

            _appDbContext.SaveChanges();
            return localAmount;
        }

        public List<ShoppingCartItem> GetShoppingCartItem()
        {
            return ShoppingCartItems ??
                (ShoppingCartItems =
                _appDbContext.ShoppingcartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                .Include(s => s.movie)
                .ToList());
        }

        public void ClearCart()
        {
            var cartItems = _appDbContext
                .ShoppingcartItems
                .Where(cart => cart.ShoppingCartId == ShoppingCartId);
            _appDbContext.ShoppingcartItems.RemoveRange(cartItems);
            _appDbContext.SaveChanges();

        }

        public double GetShoppingCartTotal()
        {
            var total = _appDbContext.ShoppingcartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.movie.Price * c.amount).Sum();
            return total;
        }
    }

}

