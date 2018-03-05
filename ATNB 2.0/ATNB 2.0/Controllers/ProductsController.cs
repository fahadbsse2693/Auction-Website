using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ATNB_2._0.Models;
using System.IO;
using System.Security.Claims;

namespace ATNB_2._0.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {

        private AuctionTenderNoticeBoardEntities db = new AuctionTenderNoticeBoardEntities();

        // GET: Products
        [AllowAnonymous]
        public ActionResult Index()
        {

            var pro = db.Products.Include(p => p.AspNetUser);
            List<Product> products = pro.ToList();
            products.Reverse();
            return View(products);
        }

        //Get: All product
        public ActionResult UserProducts()
        {

            var products = db.Products.Include(p => p.AspNetUser);
            return View(products.ToList().Where(i => i.UserId == getLogedInUserId()));
        }

        public ActionResult MakeBid(int? id)
        {
            return RedirectToAction("MakeBid", "BidsOnProducts", new { id = id });
        }


        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Product product = db.Products.Find(id);
            Product product = db.Products.Find(id);
            if (product.UserId == getLogedInUserId())
            {
                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);
            }
            else
                return RedirectToAction("UserProducts", "Products");
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            string[] cat = { "electronics", "home", "vehicle", "property", "photo", "other" };
            ViewBag.catagory = cat;
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "FullName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Type,OfferPrice,Description, productId,image")]  Product product)
        {
            product.UserId = getLogedInUserId();

            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        string path = Path.Combine(Server.MapPath("~/images/"), fileName);
                        file.SaveAs(path);
                        product.image = fileName;

                    }
                }
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("UserProducts");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "FullName", product.UserId);
            return View(product);
        }
       

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);


            if (product.UserId == getLogedInUserId())
            {
                if (product == null)
                {
                    return HttpNotFound();
                }
                ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "FullName", product.UserId);
                return View(product);
            }
            else
                return RedirectToAction("UserProducts", "Products");
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name,Type,OfferPrice,Description,image,UserId,productId")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        string path = Path.Combine(Server.MapPath("~/images/"), fileName);
                        file.SaveAs(path);
                        product.image = fileName;
                    }
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("UserProducts");
                }
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "FullName", product.UserId);
            return View(product);
        }
        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product.UserId == getLogedInUserId())
            {
                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);
            }
            return RedirectToAction("UserProducts", "Products");
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("UserProducts");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
