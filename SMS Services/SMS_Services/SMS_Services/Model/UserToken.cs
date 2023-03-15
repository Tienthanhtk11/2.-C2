namespace SMS_Services.Model
{
    namespace Point_of_Sale.Model.User
    {
        public class UserTokenModel
        {
            public long id { get; set; }
            public string user_name { get; set; }
            public string token { get; set; }
            public string full_name { get; set; }

        }
        public class LoginModel
        {
            public string user_name { set; get; }
            public string password { set; get; }
        }

    }
}
