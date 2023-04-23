using System;

namespace App.Entity
{
    public class Message_Receive
    {
        public string phone_receive { get; set; } = "";
        public string phone_send { get; set; } = "";
        public string message { get; set; } = "";
        public string port_name { get; set; } = "";
        public string computer_name  { get; set; } = "";
        public string date_receive { get; set; } = "";
        public string status { get; set; } = "";
        public long id { set; get; }
        public DateTime dateAdded { get; set; } = DateTime.Now;
        public long userAdded { set; get; } = 0;
        public bool is_delete { get; set; } = false;
    }
}
