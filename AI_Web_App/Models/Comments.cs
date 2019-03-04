using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_Web_App.Models
{
    public class Comments
    {
        private int id;
        private string bookname;
        private string user;
        private string comment;

        public int Id { get => id; set => id = value; }
        public string User { get => user; set => user = value; }
        public string Comment { get => comment; set => comment = value; }
        public string Bookname { get => bookname; set => bookname = value; }
    }
}