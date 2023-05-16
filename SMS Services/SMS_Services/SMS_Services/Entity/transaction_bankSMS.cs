using System.ComponentModel.DataAnnotations;

namespace SMS_Services.Entity
{
    public class sms
    {
        public long id { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public Nullable<System.DateTime> updated_date { get; set; }
        public Nullable<int> status { get; set; }
        public string text { get; set; }

    }
}
