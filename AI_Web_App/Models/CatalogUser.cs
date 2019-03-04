using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_Web_App.Models
{
    public class CatalogUser
    {
        private int id;
        private string userName;
        private string lastBookRead;
        private int hours;

        public int Id { get => id; set => id = value; }
        public string UserName { get => userName; set => userName = value; }
        public string LastBookRead { get => lastBookRead; set => lastBookRead = value; }
        public int Hours { get => hours; set => hours = value; }
    }
}