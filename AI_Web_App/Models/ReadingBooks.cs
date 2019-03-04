using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_Web_App.Models
{
    public class ReadingBooks
    {
        private int id;
        private string user;
        private string bookName;
        private DateTime beginTime;
        private DateTime endTime;

        public int Id { get => id; set => id = value; }
        public string User { get => user; set => user = value; }
        public string BookName { get => bookName; set => bookName = value; }
        public DateTime BeginTime { get => beginTime; set => beginTime = value; }
        public DateTime EndTime { get => endTime; set => endTime = value; }
    }
}