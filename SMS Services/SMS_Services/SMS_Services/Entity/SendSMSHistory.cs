using System.ComponentModel;

namespace SMS_Services.Model
{
    public class SendSMSHistory : IAuditableEntity

    {
        public long order_id { get; set; } = 0;
        public string order_code { get; set; }
        public string phone_receive { get; set; }
        public string phone_send { get; set; }
        public string message { get; set; }
        public string telco { get; set; }
        public int sum_sms { get; set; }
        public int status { get; set; } = 0;
        public string system_response { get; set; } = "";

    }
}
