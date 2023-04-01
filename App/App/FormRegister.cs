using App.Entity;
using App.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Windows.Forms;

namespace App
{
    public partial class FormRegister : Form
    {
        public FormRegister()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Customer customer_resign = new Customer
            {
                user_name = textBox1.Text,
                password = textBox2.Text,
                passcode = "",
                email = textBox3.Text,
                name = textBox4.Text,
                active = true,
                is_delete = true,
                cash = 0,
                license_exp = DateTime.Now,
                dateAdded = DateTime.Now,
                license_key ="",
                id = 0,
                userAdded = 0,
            };
            string resourcePath = "customer/register";
            string ServiceUrl = "http://localhost:8088/api/";
            var body = JsonConvert.SerializeObject(customer_resign);
            var client = new RestClient(ServiceUrl);
            var request = new RestRequest(resourcePath, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(body);
            try
            {
                var response = client.Execute(request);
                var response_token = JsonConvert.DeserializeObject<ResponseSingleContentModel<Customer>>(response.Content ?? "");
                MessageBox.Show("Registration Susscess, Your license exp: " + response_token.Data.license_exp );
            }
            catch (Exception)
            {
                MessageBox.Show("Registration False, please contact with Admin!");
            }
            this.Close();
        }
    }
}
