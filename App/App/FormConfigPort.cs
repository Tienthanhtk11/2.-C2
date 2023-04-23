using App.Entity;
using App.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace App
{
    public partial class FormConfigPort : Form
    {
        public static List<Config_Port> list_port = new List<Config_Port>();

        public FormConfigPort(List<Config_Port> data, long curent_user_id, string config_mode1)
        {
            customer_id = curent_user_id;
            list_port = data;
            config_mode = config_mode1;
            InitializeComponent();
        }
        public static string config_mode = "true";
        public static string ServiceUrl = "http://103.120.242.146:8088/api/";
        //public static string ServiceUrl = "https://localhost:7067/api/";
        public static long customer_id = 0;
        //list_port = new List<Config_Port>();
        private void label4_TextChanged(object sender, EventArgs e)
        {
            if (label4.Text != "id")
            {
                button1.Text = "Update";
            }
            else
                button1.Text = "Insert";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var curent_row = dataGridView1.CurrentRow;
            label4.Text = curent_row.Cells["Id"].Value.ToString();
            textBox1.Text = curent_row.Cells["PortName"].Value.ToString();
            textBox2.Text = curent_row.Cells["PhoneNumber"].Value.ToString();
            textBox3.Text = curent_row.Cells["MobileCarrier"].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Config_Port port = new Config_Port
            {
                id = 0,
                Port_Name = textBox1.Text,
                Phone_Number = textBox2.Text,
                Mobile_Carrier = textBox3.Text,
                Customer_Id = customer_id,
            };
            var current_port_phone = list_port.FirstOrDefault(x => x.Phone_Number == port.Phone_Number);
            if (current_port_phone != null)
            {
                const string message = "Another PORT NAME using this phone number \r\nAre you sure that you would like to remove it?";
                const string caption = "Warning";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    list_port.Remove(current_port_phone);
                    var current_port_name = list_port.FirstOrDefault(x => x.Port_Name == port.Port_Name);
                    if (current_port_name != null)
                        list_port.Remove(current_port_name);
                    list_port.Add(port);
                    string resourcePath = "customer/config-port-create";
                    var body = JsonConvert.SerializeObject(port);
                    var client = new RestClient(ServiceUrl);
                    var request = new RestRequest(resourcePath, Method.Post);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Accept", "application/json");
                    request.AddJsonBody(body);
                    var response = client.Execute(request);
                }
            }
            else
            {
                var current_port_name = list_port.FirstOrDefault(x => x.Port_Name == port.Port_Name);
                if (current_port_name != null)
                    list_port.Remove(current_port_name);
                list_port.Add(port);
                string resourcePath = "customer/config-port-create";
                var body = JsonConvert.SerializeObject(port);
                var client = new RestClient(ServiceUrl);
                var request = new RestRequest(resourcePath, Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "application/json");
                request.AddJsonBody(body);
                var response = client.Execute(request);
            }    
            dataGridView1.DataSource = list_port.OrderByDescending(x => x.id).ToList();
            label4.Text = "id";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }
        private void FormConfigPort_Load(object sender, EventArgs e)
        {
            if (config_mode != "true")
            {
                string resourcePath = "customer/list-config-port?customer_id=" + customer_id;
                var client = new RestClient(ServiceUrl);
                var request = new RestRequest(resourcePath, Method.Get);
                var response = client.Execute(request);
                var response_token = JsonConvert.DeserializeObject<ResponseSingleContentModel<List<Config_Port>>>(response.Content ?? "");
                try
                {
                    if (response_token.Data.Count() > 0)
                    {
                        list_port.Clear();
                        list_port = response_token.Data;
                    }
                }
                catch (Exception)
                {
                }
            }
            dataGridView1.DataSource = list_port.OrderByDescending(x => x.id).ToList();

        }
        private void FormConfigPort_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormReadSMS readSMS = new FormReadSMS(list_port, customer_id, config_mode);
            this.Hide();
            readSMS.ShowDialog();
            this.Close();
        }
    }
}
