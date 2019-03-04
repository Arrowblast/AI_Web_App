using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_Web_App.Models
{
    public class BigCatalogUser
    {
        private CatalogUser user;
        private TimeSpan time;
        private List<BookCatalog> bookReadingNow;
        private List<BookCatalog> lendBooks;
        private List<BookCatalog> returnBooks;

        public CatalogUser User { get => user; set => user = value; }
        public TimeSpan Time { get => time; set => time = value; }
        public List<BookCatalog> BookReadingNow { get => bookReadingNow; set => bookReadingNow = value; }
        public List<BookCatalog> LendBooks { get => lendBooks; set => lendBooks = value; }
        public List<BookCatalog> ReturnBooks { get => returnBooks; set => returnBooks = value; }
    }
}