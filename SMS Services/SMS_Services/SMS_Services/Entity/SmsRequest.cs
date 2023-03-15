using SMS_Services.Model;

namespace SMS_Services
{
    public class SmsRequest
    {
        public string brandName { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string type { get; set; } // 1:cskh; 2: qc
        public DateTime timeSend { get; set; }
        public Order contract { get; set; }
        public List<OrderDetails> list_sms { get; set; }
    }
}
