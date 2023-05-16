using System;
using System.Collections.Generic;

namespace App.Model
{
    public class SMS_Template
    {
        public long customer_id { get; set; } = 0;
        public string message { get; set; }
        public long id { set; get; } = 0;
        public long userAdded { set; get; } = 0;
    }
    public class Phone_Number
    {
        public string telco { get; set; } = "";
        public string phone_number { get; set; } = "";
        public long customer_id { get; set; } = 0;
        public long id { set; get; } = 0;
        public long userAdded { set; get; } = 0;
    }
    public class Data_Upload
    {
        public List<Phone_Number> list_phone_number { get; set; } = new List<Phone_Number>();
        public List<SMS_Template> list_sms_template { get; set; } = new List<SMS_Template>();

    }
    public class SMS_Request_Customer 
    {
        public long id { get; set; }
        public long customer_id { get; set; }
        public long template_id { get; set; } = 0;
        public string phone_receive { get; set; }
        public string phone_send { get; set; }
        public string message { get; set; }
        public string telco { get; set; } = "";
        public int sum_sms { get; set; } = 1;
        public int status { get; set; } = 0;
        public string system_response { get; set; } = "";
        public DateTime dateAdded { get; set; } = DateTime.Now;
    }
}
