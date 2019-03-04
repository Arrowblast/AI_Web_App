using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_Web_App.Models
{
    public enum Loan
    {
        InsideSystem,
        OutsideSystem,
        None
    }
    public class BookCatalog
    {
        private int id;
        private string name;
        private string owner;
        private string artist;
        private string wydawnictwo;
        private string link;
        private string trueOwner;
        private Loan loan;
        private Boolean reading;

        
        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Owner { get => owner; set => owner = value; }
        public string Artist { get => artist; set => artist = value; }
        public string Wydawnictwo { get => wydawnictwo; set => wydawnictwo = value; }
        public string Link { get => link; set => link = value; }
        public string TrueOwner { get => trueOwner; set => trueOwner = value; }
        public Loan Loan { get => loan; set => loan = value; }
        public bool Reading { get => reading; set => reading = value; }
    }
}