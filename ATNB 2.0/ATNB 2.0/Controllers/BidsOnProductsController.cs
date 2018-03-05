using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ATNB_2._0.Models;
using System.Security.Claims;

namespace ATNB_2._0.Controllers
{
    public class BidsOnProductsController : Controller
    {
        private static int pId;
        private AuctionTenderNoticeBoardEntities db = new AuctionTenderNoticeBoardEntities();
        //Get: action
        public ActionResult MakeBid(int? id) {
            var product = db.Products.Find(id);
            var productOwner = db.AspNetUsers.Find(product.UserId);
            var bidsList = db.BidsOnProducts.Where(i => i.productId == product.productId).ToList();

            var Users = db.AspNetUsers.ToList();
            pId = product.productId;

            ViewData["User"] = Users;
            ViewData["Product"] = product;
            ViewData["Bids"] = bidsList;
            ViewData["Owner"] = productOwner;
            
            return View();
        }
        //Post Action
        [HttpPost]
        public ActionResult MakeBid(BidsOnProduct bid) {
            if (ModelState.IsValid)
            {
                bid.productId = pId;
                bid.dateTime = DateTime.Now;
                bid.userId = getLogedInUserId();

                db.BidsOnProducts.Add(bid);
                db.SaveChanges();
                return RedirectToAction("MakeBid");

            }
            else
                return HttpNotFound();
        }
        //method for getting the user id
        public string getLogedInUserId()
        {
            string id = null;
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var userIdClaims = claimsIdentity.Claims.FirstOrDefault(X => X.Type == ClaimTypes.NameIdentifier);
                if (userIdClaims != null)
                {
                    var userIdValue = userIdClaims.Value;
                    id = userIdValue;
                }
            }
            return id;
        }
    }
}
