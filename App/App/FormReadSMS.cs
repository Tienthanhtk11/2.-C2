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
    public partial class FormReadSMS : Form
    {
        public FormReadSMS(List<Config_Port> model, long current_user_id, string config_mode1)
        {
            InitializeComponent();
            list_port = model;
            config_mode = config_mode1;
            customer_id = current_user_id;
        }
        public static long customer_id = 0;
        public static string config_mode = "";
        public static string ServiceUrl = "http://103.120.242.146:8088/api/";
        //public static string ServiceUrl = "https://localhost:7067/api/";
        public static string computer_name = Environment.MachineName;
        public static List<Config_Port> list_port = new List<Config_Port>();
        public static ExtractSMS extractSMS = new ExtractSMS();
        public static string log = "";
        static void ReadSMS(object str)
        {
            while (true)
            {
                Console.WriteLine("thread {0} str start", str);
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
                                sms.computer_name = computer_name;
                                sms.port_name = str.ToString();
                                sms.userAdded = customer_id;
                                log = "new SMS from: " + sms.phone_receive + ", content: " + sms.message + " ,phone send: " + sms.phone_send + " , time: " + sms.date_receive + "\r\n" + log;
                            }
                            string resourcePath = "SMS/create-list-sms-receive";
                            var body = JsonConvert.SerializeObject(list_sms);
                            var client = new RestClient(ServiceUrl);
                            var request = new RestRequest(resourcePath, Method.Post);
                            request.AddHeader("Content-Type", "application/json");
                            request.AddHeader("Accept", "application/json");
                            request.AddJsonBody(body);
                            var response = client.Execute(request);
                        }
                        extractSMS.ClosePort(serialPort);
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

        private void FromReadSMS_Load(object sender, EventArgs e)
        {

            if (config_mode != "true")
            {
                manual.Text = "false";
                load_port_config();
            }
            for (int i = 0; i < 100; i++)
            {
                string port = "COM" + i;
                Thread thread = new Thread(ReadSMS)
                {
                    IsBackground = true
                };
                thread.Start(port);
            }
            timer1.Interval = 2000;
            timer1.Start();
            timer2.Interval = 60000;
            timer2.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string input = log;
            Console.WriteLine(input);
            textBox1.Text = input;
            timer1.Interval = 5000;
            timer1.Start();
        }
        private void FormReadSMS_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
        private void timer2_Tick_1(object sender, EventArgs e)
        {
            string resourcePath = "customer/ping?customer_id=" + customer_id;
            var client = new RestClient(ServiceUrl);
            var request = new RestRequest(resourcePath, Method.Get);
            client.Execute(request);
            timer2.Interval = 60000;
            timer2.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormConfigPort formConfigPort = new FormConfigPort(list_port, customer_id, config_mode);
            this.Hide();
            formConfigPort.ShowDialog();
            this.Close();
        }

    }
}