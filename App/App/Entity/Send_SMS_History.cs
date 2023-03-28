using System;

namespace App.Entity
{
    public class Send_SMS_History 
    {
        public long message_request_id { get; set; } = 0;
        public string phone_receive { get; set; }
        public string phone_send { get; set; }
        public string message { get; set; }
        public string telco { get; set; }
        public int sum_sms { get; set; }
        public int status { get; set; } = 0;
        public string system_response { get; set; } = "";
        public long id { set; get; }
        public DateTime dateAdded { get; set; } = DateTime.Now;
        public bool is_delete { get; set; } = false;

    }
}
