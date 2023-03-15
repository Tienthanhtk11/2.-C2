using SMS_Services.Model;
using System.ComponentModel;

namespace SMS_Services
{
    public class OrderDetails : IAuditableEntity
    {
        public long order_id { get; set; }
        public string order_code { get; set; }
        public string phone_receive { get; set; }
        public string message { get; set; }
        public string telco { get; set; }
        public int sum_sms { get; set; }
        public int status { get; set; } = 0; // 0 la moi khoi tao, 1 la da gui
    }
    public class SMS_EXPORT
    {
        [Description("Số điện thoại")]
        public string phone { get; set; }
        [Description("Tin nhắn")]
        public string message { get; set; }

    }
}
