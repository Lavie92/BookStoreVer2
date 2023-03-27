using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookStoreVer2.Models;
using PagedList;

namespace BookStoreVer2.Controllers
{
    public class BooksController : Controller
    {
        public BookDBContext db = new BookDBContext();

        // GET: Books
        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageIndex = page == null? 1 : page.Value;
            var result = db.Books.ToList().ToPagedList(pageIndex, pageSize);
            return View(result);
        }
        public ActionResult GetBookByCategory(int id, int? page)
        {
            int pageSize = 2;
            int pageIndex = page == null ? 1 : page.Value;
            var books = db.Books.Where(x => x.CategoryId == id);
            return View("Index", books.ToList().ToPagedList(pageIndex, pageSize));
        }
        public ActionResult Search(string searchKey = "") 
        {
            var books = db.Books.Where(x => x.Title.ToLower().Contains(searchKey.ToLower())).ToList();
            return Json(books, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCategory()
        {
            var listCategory = db.Categories.ToList().Select(x => new Category
            {
                CategoryName = x.CategoryName,
                CategoryId = x.CategoryId,
                BookCount = x.Books.Count()
            });
            return PartialView(listCategory);
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.FirstOrDefault(x => x.Id == id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        [Authorize(Roles ="Admin")]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Author,Price,Description,Image,CategoryId")] Book book)
        {
            book.ImageFile = Request.Files["ImageFile"];
            if (book.ImageFile != null && book.ImageFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(book.ImageFile.FileName);
                var filePath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                book.ImageFile.SaveAs(filePath);
                book.Image = "/Content/Images/" + fileName;

            }
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", book.CategoryId);
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", book.CategoryId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Title,Author,Price,Description,Image,CategoryId")] Book book)
        {
            book.ImageFile = Request.Files["ImageFile"];
            if (book.ImageFile != null && book.ImageFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(book.ImageFile.FileName);
                var filePath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                book.ImageFile.SaveAs(filePath);
                book.Image = "/Content/Images/" + fileName;
            }
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", book.CategoryId);
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
