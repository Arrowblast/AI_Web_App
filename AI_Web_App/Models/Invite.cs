using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_Web_App.Models
{
    public class Invite
    {
        private int id;
        private string owner;
        private string invited;
        

        public string Owner { get => owner; set => owner = value; }
        public string Invited { get => invited; set => invited = value; }
        public int Id { get => id; set => id = value; }
    }
}