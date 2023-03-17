namespace SMS_APP
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
                FormMain form = new FormMain();
                this.Hide();
                form.ShowDialog();
                this.Close();

                //string ServiceUrl = "";
                //Login Logindata = new()
                //{
                //    user_name = textBox1.Text,
                //    password = textBox2.Text,
                //};
                //string resourcePath = "/api/user/shop-list-follow";
                //var body = JsonConvert.SerializeObject(Logindata);
                //var client = new RestClient(ServiceUrl);
                //var request = new RestRequest(resourcePath, Method.Post);
                //request.AddHeader("Content-Type", "application/json");
                //request.AddHeader("Accept", "application/json");
                //request.AddParameter("application/json; charset=utf-8", body, ParameterType.RequestBody);
                //var response = client.Execute(request);
                //string content = response.Content ?? "";
                ////var data = JsonConvert.DeserializeObject<ResponseSingleContentModel<List<ShopViewModel>>>(content);

                //if (content==null)
                //{
                //    MessageBox.Show(content);
                //}
                //else
                //{
                //    FormMain form= new FormMain();
                //    form.Show();
                //    this.Close();   
                //}
            }
        }
    }
    public class Login
    {
        public string user_name { get; set; }
        public string password { get; set; }
    }
}
