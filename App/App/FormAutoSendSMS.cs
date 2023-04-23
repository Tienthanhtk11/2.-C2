using App.Common;
using App.Entity;
using App.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace App
{
    public partial class FormAutoSendSMS : Form
    {
        public FormAutoSendSMS(long current_customer_id)
        {
            InitializeComponent();
            customer_id = current_customer_id;
        }
        public static string config_mode = "";
        public static string ServiceUrl = "http://103.120.242.146:8088/api/";
        //public static string ServiceUrl = "https://localhost:7067/api/";
        public static long customer_id = 0;
        public static string computer_name = Environment.MachineName;
        public static ExtractSMS extractSMS = new ExtractSMS();
        public static string log = "";
        public static List<SMS_Request_Customer> list_sms = new List<SMS_Request_Customer>();
        public static List<Config_Port> list_port = new List<Config_Port>();
        private void FormAutoSendSMS_Load(object sender, EventArgs e)
        {
            load_port_config();
            for (int i = 0; i < 100; i++)
            {
                string port = "COM" + i;
                Thread thread = new Thread(SendSMS)
                {
                    IsBackground = true
                };
                thread.Start(port);
            }
            timer1.Interval = 2000;
            timer1.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            string input = log;
            Console.WriteLine(input);
            dataGridView1.DataSource = list_sms.ToList();
            timer1.Interval = 5000;
            timer1.Start();
        }
        static void SendSMS(object str)
        {
            while (true)
            {
                SerialPort serialPort = new SerialPort();

                try
                {
                    serialPort = extractSMS.OpenPort(str.ToString(), Convert.ToInt32(115200), Convert.ToInt32(8), Convert.ToInt32(100), Convert.ToInt32(100));
                    if (serialPort != null)
                    {
                        string resourcePath = "customer/get-sms-request?customer_id=" + customer_id;
                        var client = new RestClient(ServiceUrl);
                        var request = new RestRequest(resourcePath, Method.Get);
                        request.AddHeader("Content-Type", "application/json");
                        request.AddHeader("Accept", "application/json");
                        var response = client.Execute(request);
                        var response_content = JsonConvert.DeserializeObject<ResponseSingleContentModel<SMS_Request_Customer>>(response.Content ?? "");
                        var sms = response_content.Data;
                        if (sms != null)
                        {
                            bool smsSent = extractSMS.sendMsg2(serialPort, sms.phone_receive, sms.message, 500);
                            Config_Port port = list_port.FirstOrDefault(x => x.Port_Name == str.ToString());
                            if (port != null)
                            {
                                sms.phone_send = port.Phone_Number;
                            }
                            if (smsSent == false)
                            {
                                sms.system_response = "Gửi thành công";
                            }
                            else
                                sms.system_response = "Gửi thất bại";
                            list_sms.Add(sms);
                        }
                    }
                    else
                    {
                        Thread.Sleep(300000);
                    }
                }
                catch (Exception)
                {
                    extractSMS.ClosePort(serialPort);
                    Thread.Sleep(300000);
                }
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
    }
}
