using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AI_Web_App.Models;

namespace AI_Web_App.Controllers
{
    public class CatalogUsersController : Controller
    {
        private CatalogUserDbContext db = new CatalogUserDbContext();
        private BooksCatalogDbContext booksDb = new BooksCatalogDbContext();
        // GET: CatalogUsers
        public ActionResult Index()
        {
            Boolean ok = false;
            
            foreach (CatalogUser u in db.CatalogUsers)
            {
                if (u.UserName.Equals(System.Web.HttpContext.Current.User.Identity.Name))
                {
                    ok = true;
                    break;
                }
            }
            if (!ok)
            {
                CatalogUser newUser = new CatalogUser();
                newUser.UserName = System.Web.HttpContext.Current.User.Identity.Name;
                
                db.CatalogUsers.Add(newUser);
                db.SaveChanges();
            }
            CatalogUser user = new CatalogUser();
            foreach(CatalogUser u in db.CatalogUsers)
            {
                if(u.UserName.Equals(System.Web.HttpContext.Current.User.Identity.Name))
                {
                    user = u;
                    break;
                }
            }
            BigCatalogUser bigUser = new BigCatalogUser();
            bigUser.User = user;
            bigUser.Time = new TimeSpan(user.Hours,0,0);
            bigUser.BookReadingNow = booksDb.Catalogs.Where(x => x.Owner == user.UserName && x.Reading).ToList();
            bigUser.LendBooks = booksDb.Catalogs.Where(x => x.TrueOwner == user.UserName && x.TrueOwner!=x.Owner).ToList();
            bigUser.ReturnBooks = booksDb.Catalogs.Where(x => x.Owner == user.UserName && x.TrueOwner != x.Owner).ToList();
            return View(bigUser);
        }

        // GET: CatalogUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CatalogUser catalogUser = db.CatalogUsers.Find(id);
            if (catalogUser == null)
            {
                return HttpNotFound();
            }
            return View(catalogUser);
        }

        // GET: CatalogUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CatalogUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Create([Bind(Include = "Id,UserName,LastBookRead")] CatalogUser catalogUser)
        {
            if (ModelState.IsValid)
            {
                db.CatalogUsers.Add(catalogUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(catalogUser);
        }

        // GET: CatalogUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CatalogUser catalogUser = db.CatalogUsers.Find(id);
            if (catalogUser == null)
            {
                return HttpNotFound();
            }
            return View(catalogUser);
        }

        // POST: CatalogUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Edit([Bind(Include = "Id,UserName,LastBookRead")] CatalogUser catalogUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(catalogUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(catalogUser);
        }

        // GET: CatalogUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CatalogUser catalogUser = db.CatalogUsers.Find(id);
            if (catalogUser == null)
            {
                return HttpNotFound();
            }
            return View(catalogUser);
        }

        // POST: CatalogUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        
        public ActionResult DeleteConfirmed(int id)
        {
            CatalogUser catalogUser = db.CatalogUsers.Find(id);
            db.CatalogUsers.Remove(catalogUser);
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
