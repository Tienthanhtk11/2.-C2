using App.Common;
using App.Entity;
using App.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.IO.Ports;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace App
{
    public partial class FormMain2 : Form
    {
        public static List<Config_Port> list_port = new List<Config_Port>();
        public static ExtractSMS extractSMS = new ExtractSMS();
        public static long customer_id = 0;
        public static string ServiceUrl = "http://103.120.242.146:8088/api/";
        public static string config_mode = "";
        HelperSMS helperSMS = new HelperSMS();
        public static bool auto_send = false;
        public static List<Message_Receive> message_Receives = new List<Message_Receive>();
        public static List<SMS_Request_Customer> list_message_request = new List<SMS_Request_Customer>();
        public static List<Send_SMS_History> list_message_sent = new List<Send_SMS_History>();
        public FormMain2(List<Config_Port> model, long current_user_id, string config_mode1)
        {
            InitializeComponent();
            list_port = model;
            config_mode = config_mode1;
            customer_id = current_user_id;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            List<string> list_receive_phones = tb_receive_phone.Text.Split(',').ToList();
            string message = tb_message.Text;
            int count_sms = helperSMS.CountSms(message);
            List<SMS_Request_Customer> list_message_request = new List<SMS_Request_Customer>();
            foreach (var item in list_receive_phones)
            {
                SMS_Request_Customer message_Request = new SMS_Request_Customer()
                {
                    phone_receive = item,
                    customer_id = customer_id,
                    message = message,
                    status = 0,
                    id = 0,
                    dateAdded = DateTime.Now,
                    telco = helperSMS.GetTelco(item),
                    sum_sms = count_sms
                };
                list_message_request.Add(message_Request);
            }
            string resourcePath = "customer/create-sms-request";
            var body = JsonConvert.SerializeObject(list_message_request);
            var client = new RestClient(ServiceUrl);
            var request = new RestRequest(resourcePath, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(body);
            var x = client.Execute(request);
            Console.WriteLine("Send sms to server {0}, Response: {1}", DateTime.Now, x);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (button4.Text != "Stop")
            {
                auto_send = true;
                button4.Text = "Stop";
                foreach (var item in list_port)
                {
                    Thread thread = new Thread(SendSMS)
                    {
                        Name = item.Port_Name,
                        IsBackground = true
                    };
                    thread.Start(item.Port_Name);
                }
            }
            else
            {
                auto_send = false;
                button4.Text = "Start Auto Send SMS";
            }
        }
        static void SendSMS(object str)
        {
            while (true)
            {

                if (auto_send == false)
                {
                    Thread current = Thread.CurrentThread;
                    Console.WriteLine("Abort thread  {0} {1} ", str, DateTime.Now);
                    current.Abort();
                }
                else
                {
                    SerialPort serialPort = new SerialPort();
                    try
                    {
                        Console.WriteLine("start thread  {0} {1} ", str, DateTime.Now);
                        serialPort = extractSMS.OpenPort(str.ToString(), Convert.ToInt32(115200), Convert.ToInt32(8), Convert.ToInt32(100), Convert.ToInt32(100));
                        if (serialPort != null)
                        {
                            string resourcePath = "customer/get-sms-request?customer_id=" + customer_id;
                            var client = new RestClient(ServiceUrl);
                            var request = new RestRequest(resourcePath, Method.Get);
                            var response = client.Execute(request);
                            var response_content = JsonConvert.DeserializeObject<ResponseSingleContentModel<SMS_Request_Customer>>(response.Content ?? "");
                            var sms = response_content.Data;
                            if (sms != null)
                            {
                                Send_SMS_History sms_sent = new Send_SMS_History();
                                sms_sent.phone_receive = sms.phone_receive;
                                sms_sent.message = sms.message;
                                sms_sent.dateAdded = DateTime.Now;
                                bool smsSent = extractSMS.sendMsg2(serialPort, sms.phone_receive, sms.message, 500);
                                Config_Port port = list_port.FirstOrDefault(x => x.Port_Name == str.ToString());
                                if (port != null)
                                {
                                    sms_sent.phone_send = port.Phone_Number;
                                }
                                if (smsSent == true)
                                    sms_sent.system_response = "Gửi thành công";
                                else
                                    sms_sent.system_response = "Gửi thất bại";
                                list_message_sent.Add(sms_sent);
                            }
                        }
                        else
                        {
                            Thread.Sleep(30000);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine( "exeption {0} {1} {2}", str, DateTime.Now, ex.Message);
                        extractSMS.ClosePort(serialPort);
                        Thread.Sleep(60000);
                    }
                }
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                string resourcePath = "customer/list-sms-request?customer_id=" + customer_id;
                var client = new RestClient(ServiceUrl);
                var request = new RestRequest(resourcePath, Method.Get);
                var response = client.Execute(request);
                var response_content = JsonConvert.DeserializeObject<ResponseSingleContentModel<List<SMS_Request_Customer>>>(response.Content ?? "");
                if (response_content != null)
                {
                    list_message_request = response_content.Data;
                }
                Console.WriteLine("Update data timmer 2 tick {0} , Response {1}", DateTime.Now, response_content);
                dataGridView2.DataSource = list_message_request.ToList();
                dataGridView3.DataSource = list_message_sent.ToList();
                dataGridView4.DataSource = message_Receives.ToList();
                timer2.Interval = (5000);
            }
            catch (Exception)
            {
            }
        }
        public void load_port_config()
        {
            try
            {
                string resourcePath = "customer/list-config-port?customer_id=" + customer_id;
                var client = new RestClient(ServiceUrl);
                var request = new RestRequest(resourcePath, Method.Get);
                var response = client.Execute(request);
                var response_token = JsonConvert.DeserializeObject<ResponseSingleContentModel<List<Config_Port>>>(response.Content ?? "");
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
        private void FormMain2_Load(object sender, EventArgs e)
        {
            if (config_mode != "true")
            {
                load_port_config();
            }
            timer2.Start();
        }
        static void ReadSMS(object str)
        {
            while (true)
            {
                Console.WriteLine("Read sms from {0} {1}", str, DateTime.Now);
                SerialPort serialPort = new SerialPort();
                try
                {
                    serialPort = extractSMS.OpenPort(str.ToString(), Convert.ToInt32(115200), Convert.ToInt32(8), Convert.ToInt32(100), Convert.ToInt32(100));
                    if (serialPort != null)
                    {
                        var list_sms = extractSMS.ReadUnReadMesss(serialPort);
                        if (list_sms.Count > 0)
                        {
                            foreach (var sms in list_sms)
                            {
                                Config_Port port = list_port.FirstOrDefault(x => x.Port_Name == str.ToString());
                                if (port != null)
                                {
                                    sms.phone_receive = port.Phone_Number;
                                }
                                else
                                    sms.phone_receive = str.ToString();
                                sms.port_name = str.ToString();
                            }
                            string resourcePath = "SMS/create-list-sms-receive";
                            var body = JsonConvert.SerializeObject(list_sms);
                            var client = new RestClient(ServiceUrl);
                            var request = new RestRequest(resourcePath, Method.Post);
                            request.AddHeader("Content-Type", "application/json");
                            request.AddHeader("Accept", "application/json");
                            request.AddJsonBody(body);
                            var response = client.Execute(request);
                            Console.WriteLine("Send sms receive to server {0}, Response: {1}", DateTime.Now, response);
                            message_Receives.AddRange(list_sms);
                        }
                        extractSMS.ClosePort(serialPort);
                        Thread.Sleep(30000);
                    }
                    else
                    {
                        Thread.Sleep(60000);
                    }
                }
                catch (Exception ex)
                {
                    extractSMS.ClosePort(serialPort);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FormConfigPort formConfigPort = new FormConfigPort(list_port, customer_id, config_mode);
            this.Hide();
            formConfigPort.ShowDialog();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormConfig formConfig = new FormConfig();
            formConfig.ShowDialog();
        }

        private void FormMain2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FormRequestSMS formRequest = new FormRequestSMS(customer_id);
            formRequest.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < 100; i++)
            {
                try
                {
                    SerialPort serialPort = new SerialPort();
                    Console.WriteLine("COM" + i);
                    serialPort = extractSMS.OpenPort("COM" + i, Convert.ToInt32(115200), Convert.ToInt32(8), Convert.ToInt32(100), Convert.ToInt32(100));
                    if (serialPort!=null)
                    {
                        MessageBox.Show("COM" + i);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }
    }
}
