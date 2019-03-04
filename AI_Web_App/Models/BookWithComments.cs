using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AI_Web_App.Models
{
    public class BookWithComments
    {
        private BookCatalog book;
        private IEnumerable<Comments> db;
        private Comments comment;

        public BookCatalog Book { get => book; set => book = value; }
        public IEnumerable<Comments> Db { get => db; set => db = value; }
        public Comments Comment { get => comment; set => comment = value; }
    }
}