using System;

namespace App.Entity
{
    public class Customer 
    {
        public string user_name { get; set; }
        public string password { get; set; }
        public string passcode { get; set; } = "";
        public string name { get; set; } = "";
        public string email { get; set; } = "";
        public int cash { get; set; } = 0;
        public bool active { get; set; } = true;
        public DateTime license_exp { get; set; } = DateTime.Now.AddMonths(3);
        public string license_key { get; set; } = string.Empty;
        public long id { set; get; }
        public long userAdded { set; get; } = 0;
        public long? userUpdated { set; get; }
        public DateTime dateAdded { get; set; } = DateTime.Now;
        public DateTime? dateUpdated { get; set; } = DateTime.Now;
        public bool is_delete { get; set; } = false;
    }
}
