using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AI_Web_App.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;


using SendGrid;
using HtmlAgilityPack;

namespace AI_Web_App.Controllers
{
    public class BookCatalogsController : Controller
    {
        private BooksCatalogDbContext db = new BooksCatalogDbContext();
        private InviteDbContext inviteDb = new InviteDbContext();
        private ApplicationDbContext usersDb = new ApplicationDbContext();
        private LoanMonitorDbContext loanDb = new LoanMonitorDbContext();
        private CommentDbContext commentDb = new CommentDbContext();
        private List<Comments> comments = new List<Comments>();
        public BookCatalog bookComment = new BookCatalog();
        private ReadingBooksDbContext readingDb = new ReadingBooksDbContext();
        private CatalogUserDbContext catalogUsersDb = new CatalogUserDbContext();
        // GET: BookCatalogs
        public ActionResult Index(string searchString)
        {

            var Books = from b in db.Catalogs
                        select b;
            var currUser = System.Web.HttpContext.Current.User.Identity.Name;
            List<string> availableBooksUsers = new List<string>();
            availableBooksUsers.Add(currUser);
            foreach (Invite i in inviteDb.Invites)
            {
                if (i.Invited.Contains(currUser))
                {
                    availableBooksUsers.Add(i.Owner);
                }
            }
            var availableBooks = Books.Where(s => availableBooksUsers.Contains(s.Owner));
            if (!String.IsNullOrEmpty(searchString))
            {
                availableBooks = availableBooks.Where(s => s.Name.Contains(searchString));
            }
            return View(availableBooks.ToList());
        }

        //GET: BookCatalogs/Invite
        public ActionResult Invite()
        {
            List<String> li = new List<String>();
            List<ApplicationUser> uli = new List<ApplicationUser>();
            li = usersDb.Users.Where(x=> x.UserName != System.Web.HttpContext.Current.User.Identity.Name).Select(x => x.UserName).ToList();
            uli = usersDb.Users.Where(x => x.UserName != System.Web.HttpContext.Current.User.Identity.Name).ToList();
            foreach (Invite i in inviteDb.Invites)
            {
                if (li.Contains(i.Invited) && i.Owner==System.Web.HttpContext.Current.User.Identity.Name)
                {
                    uli.Remove(uli.Where(x => x.UserName == i.Invited).ToList()[0]);
                }
            }
            ViewBag.Users = uli;
            return View();
        }
        //POST: BookCatalogs/Invite
        [HttpPost]
        public async Task<ActionResult> Invite([Bind(Include = "UserName")] ApplicationUser invite)
        {
            List<String> li = new List<String>();
            List<ApplicationUser> uli = new List<ApplicationUser>();
            li = usersDb.Users.Where(x => x.UserName != System.Web.HttpContext.Current.User.Identity.Name).Select(x => x.UserName).ToList();
            uli = usersDb.Users.Where(x => x.UserName != System.Web.HttpContext.Current.User.Identity.Name).ToList();

            foreach (Invite i in inviteDb.Invites)
            {
                if (li.Contains(i.Invited) && i.Owner == System.Web.HttpContext.Current.User.Identity.Name)
                {
                    uli.Remove(uli.Where(x => x.UserName == i.Invited).ToList()[0]);
                }
            }
            ViewBag.Users = uli;
            if (ModelState.IsValid)
            {

                var inviteMessage = new SendGridMessage();
                inviteMessage.AddTo(invite.UserName);

                inviteMessage.From = new System.Net.Mail.MailAddress(
                          System.Web.HttpContext.Current.User.Identity.Name, System.Web.HttpContext.Current.User.Identity.Name);
                inviteMessage.Subject = "Invitation to catalog";
                var urlCallback = Url.Action("InviteConfirm", "BookCatalogs", new { userEmail = System.Web.HttpContext.Current.User.Identity.Name, addEmail = invite.UserName }, protocol: Request.Url.Scheme);
                inviteMessage.Text = "User " + System.Web.HttpContext.Current.User.Identity.Name + "added you to his catalog. Click" + "<a href=\"" + urlCallback + "\">here</a>";
                var credentials = new NetworkCredential(
                       ConfigurationManager.AppSettings["mailAccount"],
                       ConfigurationManager.AppSettings["mailPassword"]
                       );
                var transportWeb = new Web(credentials);
                await transportWeb.DeliverAsync(inviteMessage);

                return RedirectToAction("Index");

            }
            return View(invite);
        }

        // GET: BookCatalogs/InviteConfirm      
        public async Task<ActionResult> InviteConfirm(String userEmail, String addEmail)
        {
            if (userEmail == null)
            {
                return View("Error");
            }

            Invite invite = new Invite();

            invite.Owner = userEmail;
            invite.Invited = addEmail;
            inviteDb.Invites.Add(invite);
            inviteDb.SaveChanges();
            return View("InviteConfirm");

        }
        // GET: BookCatalogs/Read/5
        public ActionResult Read(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookCatalog bookCatalog = db.Catalogs.Find(id);
            if (bookCatalog == null)
            {
                return HttpNotFound();
            }
            if (!bookCatalog.Reading)
            {
                ReadingBooks reading = new ReadingBooks();
                reading.User = System.Web.HttpContext.Current.User.Identity.Name;
                reading.BookName = bookCatalog.Name;
                reading.BeginTime = DateTime.Now;
                reading.EndTime = DateTime.Now;
                readingDb.ReadingBooks.Add(reading);
                // Is bookCatalog equals 'b' inside 'if'?
                bookCatalog.Reading = true;
 //               foreach (BookCatalog b in db.Catalogs)
 //               {
 //                   if (b.Id.Equals(bookCatalog.Id))
 //                   {
 //                       b.Reading = true;
 //                       break;
 //                   }
 //               }
            }
            else
            {
                // Is it the same like commented? All conditions are in db
                ReadingBooks r = readingDb.ReadingBooks.Where(book => book.User == bookCatalog.Owner && book.BookName == bookCatalog.Name).First();
                if (r != null)
                {
                    r.EndTime = DateTime.Now;
                    TimeSpan time = r.EndTime.Subtract(r.BeginTime);
                    CatalogUser c = catalogUsersDb.CatalogUsers.Where(user => user.UserName == r.User).First();
                    if (c != null)
                    {
                        c.LastBookRead = r.BookName;
                        c.Hours += time.Hours;
                    }
                    readingDb.ReadingBooks.Remove(r);
                }
                //foreach(ReadingBooks r in readingDb.ReadingBooks)
                //{
                //    if(r.User==bookCatalog.Owner && r.BookName==bookCatalog.Name)
                //    {
                //        r.EndTime = DateTime.Now;
                //        TimeSpan time = r.EndTime.Subtract(r.BeginTime);
                //        foreach(CatalogUser c in catalogUsersDb.CatalogUsers)
                //        {
                //            if(c.UserName==r.User)
                //            {
                //                c.LastBookRead = r.BookName;
                //                c.Hours += time.Hours;
                //            }
                //        }
                //        readingDb.ReadingBooks.Remove(r);
                //        break;
                //    }
                //}
                // Is bookCatalog equals 'b' inside 'if'?
                bookCatalog.Reading = false;
//               foreach (BookCatalog b in db.Catalogs)
//               {
//                   if (b.Id.Equals(bookCatalog.Id))
//                   {
//                       b.Reading = false;
//                       break;
//                   }
//               }
            }
            catalogUsersDb.SaveChanges();
            readingDb.SaveChanges();
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: BookCatalogs/Return/5
        public ActionResult Return(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookCatalog bookCatalog = db.Catalogs.Find(id);
            if (bookCatalog == null)
            {
                return HttpNotFound();
            }
            
            foreach (BookCatalog b in db.Catalogs)
            {
                if(b.Id.Equals(id))
                {
                    b.Owner = b.TrueOwner;
                    b.Loan = Loan.None;
                    break;
                }
            }
            IEnumerable<LoansMonitor> loans = loanDb.Monitors.Where(x => x.Name == bookCatalog.Name);
            loanDb.Monitors.RemoveRange(loans);
            loanDb.SaveChanges();
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: BookCatalogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookCatalog bookCatalog = db.Catalogs.Find(id);
            if (bookCatalog == null)
            {
                return HttpNotFound();
            }
            BookWithComments bookWithComments = new BookWithComments();
            bookWithComments.Book = bookCatalog;
            bookWithComments.Comment = new Comments();
            
            bookWithComments.Db = commentDb.Comments.Where(x => x.Bookname.Equals(bookCatalog.Name)).ToList();
            comments = commentDb.Comments.Where(x => x.Bookname.Equals(bookCatalog.Name)).ToList();
            string url = Url.Content(Request.Url.PathAndQuery);
            ViewBag.Id = int.Parse(url.Split('/')[3]);
            ViewBag.Comments=commentDb.Comments.Where(x => x.Bookname.Equals(bookCatalog.Name)).ToList();


            return View(bookWithComments);
        }
        
        // POST: BookCatalogs/Details/5
        [HttpPost]
        public ActionResult Details([Bind(Include = "Comment")]BookWithComments bookWithComments)
        {
            string url = Url.Content(Request.Url.PathAndQuery);
            ViewBag.Id = int.Parse(url.Split('/')[3]);
            if (ModelState.IsValid)
            {
                Comments comment = new Comments();
                comment.User= System.Web.HttpContext.Current.User.Identity.Name;
                comment.Comment = bookWithComments.Comment.Comment;
                foreach (BookCatalog b in db.Catalogs)
                {
                    if (b.Id == ViewBag.Id)
                    {
                        comment.Bookname = b.Name;
                        break;

                    }
                }
                
                commentDb.Comments.Add(comment);
                commentDb.SaveChanges();
                
                return RedirectToAction("Details", new { id = ViewBag.Id });
            }

            return View("Error");
            
        }
        public async Task<BookCatalog> BookFromWebsite(string url)
        {
            BookCatalog book = new BookCatalog();
            HtmlWeb web = new HtmlWeb();
            var doc = await Task.Factory.StartNew(() => web.Load(url));
            book.Name = doc.DocumentNode.SelectNodes("//*[@id=\"maincontent\"]/div[1]/div[2]/h1/text()").Select(node => node.InnerText).ToList()[0];
            var Artists = doc.DocumentNode.SelectNodes("//*[@id=\"maincontent\"]/div[1]/div[2]/p[4]");
            book.Artist = Artists.Select(node => node.InnerText).ToList()[0];
            int i = 1;
            book.Wydawnictwo = "";
            var Wydawnictwa = doc.DocumentNode.SelectNodes("//*[@id=\"maincontent\"]/div[1]/div[2]/p[" + i.ToString() + "]");
            while (!book.Wydawnictwo.Contains("Publisher"))
            {
                ++i;
                Wydawnictwa = doc.DocumentNode.SelectNodes("//*[@id=\"maincontent\"]/div[1]/div[2]/p["+i.ToString()+"]");
                book.Wydawnictwo = Wydawnictwa.Select(node => node.InnerText).ToList()[0];
                
            }
            Wydawnictwa = doc.DocumentNode.SelectNodes("//*[@id=\"maincontent\"]/div[1]/div[2]/p[" + i.ToString() + "]/a");
            book.Wydawnictwo= Wydawnictwa.Select(node => node.InnerText).ToList()[0];
            book.Owner= System.Web.HttpContext.Current.User.Identity.Name;
            book.TrueOwner = System.Web.HttpContext.Current.User.Identity.Name;
            book.Loan = Loan.None;

            return book;
        }
        // GET: BookCatalogs/Lend/5
        public ActionResult Lend(int? id)
        {
            ViewBag.Users= usersDb.Users.Where(x => x.UserName!= System.Web.HttpContext.Current.User.Identity.Name).ToList();
            
            ViewBag.Loans = Enum.GetValues(typeof(Loan)).Cast<Loan>().Where(x => x != Loan.None);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookCatalog bookCatalog = db.Catalogs.Find(id);
            if (bookCatalog == null)
            {
                return HttpNotFound();
            }
            return View(bookCatalog);
        }

        // POST: BookCatalogs/Lend/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public ActionResult Lend([Bind(Include = "Id,Name,Loan,Owner")]BookCatalog book)
        {
            ViewBag.Users = usersDb.Users.Where(x => x.UserName != System.Web.HttpContext.Current.User.Identity.Name).ToList();
            ViewBag.Loans = Enum.GetValues(typeof(Loan)).Cast<Loan>().Where(x => x!=Loan.None);
            if (ModelState.IsValid)
            {
                
                
                LoansMonitor loan = new LoansMonitor();
                loan.LoanDate = DateTime.Now;
                loan.ReturnDate = DateTime.Now.AddDays(30);
                
                foreach (BookCatalog b in db.Catalogs)
                {
                    if (b.Id == book.Id)
                    {
                        if (book.Loan.Equals(Loan.InsideSystem))
                        {
                            b.Owner = book.Owner;
                            b.Loan = book.Loan;
                            loan.Name = b.Name;
                            break;
                        }
                        else
                        {
                            b.Loan = book.Loan;
                            loan.Name = b.Name;
                            break;
                        }

                    }
                }
                loanDb.Monitors.Add(loan);
                loanDb.SaveChanges();
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }

            return View("Error");
        }
        // GET: BookCatalogs/AddFromWebsite
        public ActionResult AddFromWebsite()
        {
            
            return View();
        }

        // POST: BookCatalogs/AddFromWebsite
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task< ActionResult >AddFromWebsite([Bind(Include = "Link")]BookCatalog book)
        {
            
            if (ModelState.IsValid)
            {
                BookCatalog newBook = await BookFromWebsite(book.Link);

                newBook.Link = book.Link;
                db.Catalogs.Add(newBook);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Error");
        }
        
        // GET: BookCatalogs/AddFromDatabase
        public ActionResult AddFromDatabase()
        {
            ViewBag.Books = db.Catalogs.Where(x => x.Owner==null).ToList();
            return View();
        }

        // POST: BookCatalogs/AddFromDatabase
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult AddFromDatabase([Bind(Include = "Name")] BookCatalog bookCatalog)
        {
            ViewBag.Books = db.Catalogs.Where(x => x.Owner == null).ToList();
            if (ModelState.IsValid)
            {
                BookCatalog b = db.Catalogs.Where(book => book.Name == bookCatalog.Name).First();
                if (b != null)
                {
                    b.Owner = System.Web.HttpContext.Current.User.Identity.Name;
                    b.TrueOwner = b.Owner;
                    b.Loan = Loan.None;
                }
                //foreach(BookCatalog b in db.Catalogs)
                //{
                //    if(b.Name==bookCatalog.Name)
                //    {
                //        b.Owner = System.Web.HttpContext.Current.User.Identity.Name;
                //        b.TrueOwner = b.Owner;
                //        b.Loan = Loan.None;
                //        break;
                //    }
                //}
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bookCatalog);
        }

        // GET: BookCatalogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BookCatalogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Artist,Wydawnictwo")] BookCatalog bookCatalog)
        {
            if (ModelState.IsValid)
            {
                bookCatalog.Owner = System.Web.HttpContext.Current.User.Identity.Name;
                bookCatalog.TrueOwner = System.Web.HttpContext.Current.User.Identity.Name;
                bookCatalog.Loan = Loan.None;
                db.Catalogs.Add(bookCatalog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bookCatalog);
        }

        // GET: BookCatalogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookCatalog bookCatalog = db.Catalogs.Find(id);
            if (bookCatalog == null)
            {
                return HttpNotFound();
            }
            return View(bookCatalog);
        }

        // POST: BookCatalogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Owner,Artist,Wydawnictwo")] BookCatalog bookCatalog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookCatalog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bookCatalog);
        }

        // GET: BookCatalogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookCatalog bookCatalog = db.Catalogs.Find(id);
            if (bookCatalog == null)
            {
                return HttpNotFound();
            }
            return View(bookCatalog);
        }

        // POST: BookCatalogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookCatalog bookCatalog = db.Catalogs.Find(id);
            bookCatalog.Owner = null;
            bookCatalog.TrueOwner = null;
            bookCatalog.Loan = Loan.None;
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
