using App.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Windows.Forms;

namespace App
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Không được bỏ trống Tài khoản & mật khẩu");
            }
            else
            {
                Login Logindata = new Login()
                {
                    user_name = textBox1.Text,
                    password = textBox2.Text,
                };
                string ServiceUrl = "https://localhost:7067/api/";
                try
                {
                    string resourcePath = "customer/login";
                    var body = JsonConvert.SerializeObject(Logindata);
                    var client = new RestClient(ServiceUrl);
                    var request = new RestRequest(resourcePath, Method.Post);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Accept", "application/json");
                    request.AddJsonBody(body);
                    var response = client.Execute(request);
                    var response_token = JsonConvert.DeserializeObject<ResponseSingleContentModel<CustomerTokenModel>>(response.Content ?? "");
                    if (DateTime.Now >= response_token.Data.license_exp)
                    {
                        MessageBox.Show("License của bạn đã hết hạn, vui lòng liên hệ admin để được gia hạn!");
                    }
                    else
                    {
                        MessageBox.Show("Đăng nhập thành công, license của bạn còn: " + (int)((response_token.Data.license_exp - DateTime.Now).TotalDays) + " ngày!");
                        FormMain form = new FormMain();
                        this.Hide();
                        form.ShowDialog();
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("License của bạn chưa được kích hoạt hoặc đã hết hạn, vui lòng liên hệ admin để được gia hạn!");
                }


            }
        }
    }
    public class Login
    {
        public string user_name { get; set; }
        public string password { get; set; }
    }
}
