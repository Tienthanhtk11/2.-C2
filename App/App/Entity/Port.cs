using System;

namespace App.Entity
{
    public class Port
    {
        public string name { get; set; }
        public string phone_number { get; set; }
        public string telco { get; set; }
        public int cash { get; set; } = 0;
        public long id { set; get; }
        public DateTime dateAdded { get; set; } = DateTime.Now;
        public bool is_delete { get; set; } = false;

    }
}
