using System;

namespace App.Model
{
    public class CustomerTokenModel
    {
        public long id { get; set; }
        public string user_name { get; set; }
        public string token { get; set; }
        public string name { get; set; }
        public DateTime license_exp { get; set; }
        public string license_key { get; set; } = string.Empty;
    }
}
