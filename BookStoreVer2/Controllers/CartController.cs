using BookStoreVer2.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStoreVer2.Controllers
{
    public class CartController : Controller
    {
        public List<Cart> GetCartFromSesstion ()
        {
            var listCart = Session["cart"] as List<Cart>;
            if (listCart == null)
            {
                listCart = new List<Cart>();
                Session["cart"] = listCart;
            }
            return listCart;
        }
        // GET: Cart
        public ActionResult Index()
        {
            List<Cart> listCart = GetCartFromSesstion();
            if (listCart.Count == 0)
            {
                RedirectToAction("Index", "Books");
            }
            ViewBag.Quantity = listCart.Sum(x => x.Quantity);
            ViewBag.Total = listCart.Sum(x => x.Money);
            return View(listCart);
        }
        public ActionResult AddToCart(int id)
        {
            List<Cart> listCart = GetCartFromSesstion();
            var findcartItem = listCart.FirstOrDefault(x => x.Id == id);
            if (findcartItem == null)
            {
                BookDBContext db = new BookDBContext();
                Book findBook = db.Books.FirstOrDefault(x => x.Id == id);
                Cart cartItem = new Cart();
                {
                    cartItem.Id = id;
                    cartItem.Price = findBook.Price.Value;
                    cartItem.Quantity = 1;
                    cartItem.Title = findBook.Title;
                    cartItem.Description = findBook.Description;
                    cartItem.Image = findBook.Image;
                }
                listCart.Add(cartItem);
            }
            else
            {
                findcartItem.Quantity++;
            }
            return RedirectToAction("Index", "Cart");
        }
        public RedirectToRouteResult UpdateCart(int id, int txtQuantity)
        {
            List<Cart> listCart = GetCartFromSesstion();
            var findItem = listCart.FirstOrDefault(x => x.Id == id);
            findItem.Quantity = txtQuantity;
            return RedirectToAction("Index", "Cart");
        }
        public ActionResult CartSummary()
        {
            List<Cart> listCart = GetCartFromSesstion();
            ViewBag.CartSummary = listCart.Count();
            return PartialView("CartSummary");
        }
        public ActionResult Order()
        {
            List<Cart> listCart = GetCartFromSesstion();
            string currentCustomerId = User.Identity.GetUserId();
            BookDBContext db = new BookDBContext();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var newOrder = new Order()
                    {
                        CustomerId = currentCustomerId,
                        OrderDate = DateTime.Now,
                        DeliveryDate = null,
                        IsPaid = false,
                        Status = "Đã đặt",
                        Total = listCart.Sum(x => x.Money)
                    };
                    db.Orders.Add(newOrder);
                    db.SaveChanges();
                    int idNo = newOrder.OrderNo;
                    foreach (var item in listCart)
                    {
                        var orderDetail = new OrderDetail()
                        {
                            OrderNo = idNo,
                            BookId = item.Id,
                            Price = item.Price,
                            Quantity = item.Quantity
                        };
                        db.OrderDetails.Add(orderDetail);
                    }
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return Content("Gặp lỗi khi mua hàng");
                    throw;
                }
            }
            Session["cart"] = null;
            return RedirectToAction("Index", "Books");
        }
    }
}