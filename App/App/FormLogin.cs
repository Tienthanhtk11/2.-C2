﻿using App.Model;
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Please fill user name & password!");
            }
            else
            {
                Login Logindata = new Login()
                {
                    user_name = textBox1.Text,
                    password = textBox2.Text,
                };
                string ServiceUrl = "http://103.120.242.146:8088/api/";
                //string ServiceUrl = "https://localhost:7067/api/";
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
                        MessageBox.Show("Your license has expired, please contact admin to renew it!");
                    }
                    else
                    {
                        FormConfigWarning formConfigWarning = new FormConfigWarning(response_token.Data.id);
                        this.Hide();
                        formConfigWarning.ShowDialog();
                        this.Close();
                        //FormMain form  = new FormMain();
                        //this.Hide();
                        //form.ShowDialog();
                        //this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Your license has not been activated or has expired, please contact admin to renew it!");
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            FormRegister form = new FormRegister();
            form.ShowDialog();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }
    }
    public class Login
    {
        public string user_name { get; set; }
        public string password { get; set; }
    }
}
