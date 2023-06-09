﻿namespace SMS_Services.Model
{
    public class Message_Receive : IAuditableEntity
    {
        public string phone_receive { get; set; } = "";
        public string phone_send { get; set; } = "";
        public string message { get; set; } = "";
        public string status { get; set; } = "";
        public string date_receive  { get; set; } = "";
        public string port_name { get; set; } = "";
        public string computer_name { get; set; } = "";
        public long? android_sms_id { get; set; } 
    }
}
