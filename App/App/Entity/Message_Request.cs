using System;

namespace App.Entity
{
    public class Message_Request
    {
        public string phone_receive { get; set; }
        public string message { get; set; }
        public string telco { get; set; }
        public int sum_sms { get; set; }
        public int status { get; set; } = 0; // 0 la moi khoi tao, 1 la da gui, 2 la huy
        public long id { set; get; }
        public DateTime dateAdded { get; set; } = DateTime.Now;
        public bool is_delete { get; set; } = false;
    }
}
