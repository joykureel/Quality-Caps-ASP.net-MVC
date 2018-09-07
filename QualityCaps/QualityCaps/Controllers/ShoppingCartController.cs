using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QualityCaps.Data;
using QualityCaps.Models;
using Microsoft.AspNetCore.Authorization;
namespace QualityCaps.Controllers
{
    [AllowAnonymous]
    [Authorize(Roles = "Member")]

    public class ShoppingCartController : Controller
    {
        ApplicationDbContext _context;

        public ShoppingCartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Return the view
            return View(cart);
        }
    public ActionResult CartEmpty(int id)
        {
            ShoppingCart.GetCart(this.HttpContext).EmptyCart(_context);
            return RedirectToAction("Index","MemberCaps");
        }

        //
        // GET: /Store/AddToCart/5
        public ActionResult AddToCart(int id)
        {
            // Retrieve the album from the database
            var addedCap = _context.Cap
                .Single(cap => cap.ID == id);
            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddToCart(addedCap, _context);
            // Go back to the main store page for more shopping
            return RedirectToAction("Index", "MemberCaps");
        }

        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            int itemCount = cart.RemoveFromCart(id, _context);
            return Redirect(Request.Headers["Referer"].ToString());
        }
        

    }
}