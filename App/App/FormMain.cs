using App.Common;
using App.Entity;
using App.Model;
using ExcelDataReader.Log;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace App
{
    public partial class FormMain : Form
    {
        public static long customer_id = 0;
        public static string config_mode = "";
        public static string ServiceUrl = "http://103.120.242.146:8088/api/";
        public static List<Config_Port> list_port = new List<Config_Port>();
        HelperSMS helperSMS = new HelperSMS();
        public FormMain()
        {
            InitializeComponent();
        }
        public static Database db = new Database();
        public static ExtractSMS extractSMS = new ExtractSMS();
        SerialPort serialPort = new SerialPort();
        private void button2_Click(object sender, EventArgs e)
        {
            List<string> list_receive_phones = tb_receive_phone.Text.Split(',').ToList();
            string message = tb_message.Text;
            int count_sms = helperSMS.CountSms(message);
            List<Message_Request> list_message_request = new List<Message_Request>();
            foreach (var item in list_receive_phones)
            {
                Message_Request message_Request = new Message_Request()
                {
                    phone_receive = item,
                    message = message,
                    status = 0,
                    id = 0,
                    dateAdded = DateTime.Now,
                    is_delete = false,
                    telco = helperSMS.GetTelco(item),
                    sum_sms = count_sms
                };
                list_message_request.Add(message_Request);
            }
            db.Message_Request.AddRange(list_message_request);
            db.SaveChanges();
            var list_message_request_new = from p in db.Message_Request.Where(x => !x.is_delete).OrderByDescending(x => x.id).ToList()
                                           select new
                                           {
                                               phone_receive = p.phone_receive,
                                               message = p.message,
                                               telco = p.telco,
                                               sum_sms = p.sum_sms,
                                               status = p.status,
                                               dateAdded = p.dateAdded,
                                           }; ;
            dataGridView2.DataSource = list_message_request_new.ToList();
        }
        private void FormMain_Load(object sender, EventArgs e)
        {
            var list_port = from p in db.Port.Where(x => !x.is_delete).ToList()
                            select new
                            {
                                cash = p.cash,
                                telco = p.telco,
                                phone_number = p.phone_number,
                                name = p.name,
                            };
            dataGridView1.DataSource = list_port.ToList();
            var list_message_request = from p in db.Message_Request.Where(x => !x.is_delete).OrderByDescending(x => x.id).ToList()
                                       select new
                                       {
                                           phone_receive = p.phone_receive,
                                           message = p.message,
                                           telco = p.telco,
                                           sum_sms = p.sum_sms,
                                           status = p.status,
                                           dateAdded = p.dateAdded,
                                       }; ;
            dataGridView2.DataSource = list_message_request.ToList();
        }
        static void ReadSMS(object str)
        {
            try
            {
                SerialPort serialPort = new SerialPort();
                serialPort = extractSMS.OpenPort(str.ToString(), Convert.ToInt32(115200), Convert.ToInt32(8), Convert.ToInt32(100), Convert.ToInt32(100));
                //string network = extractSMS.GetSimNetWork(serialPort);
                //Port port = new Port()
                //{
                //    name = str.ToString(),
                //    telco = network,
                //};
                string raw_info = extractSMS.CheckMoney(serialPort);
                Console.WriteLine("PORT " + str);
                Console.WriteLine(raw_info);

                //if (raw_info != null && raw_info != "")
                //{
                //    Regex phone_number_rule = new Regex(@"(0|84)\d{9}");
                //    port.phone_number = phone_number_rule.Match(raw_info).Groups[0].Value;
                //    Regex balance_rule = new Regex(@"(\d+) ?(VND|d)"); //Regex balance_rule = new Regex(@"([\.\d]+) ?(VND|d)");
                //    port.cash = int.Parse(balance_rule.Match(raw_info.Replace(".", string.Empty)).Groups[1].Value);
                //}
                //else
                //    port.cash = 0;
                //extractSMS.ClosePort(serialPort);
                //db.Port.Add(port);
                //db.SaveChanges();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(str.ToString() + "false    " + ex.Message);
            }
        }
        private void bt_refresh_Click(object sender, EventArgs e)
        {

            string portname = "COM52";
            serialPort = extractSMS.OpenPort(portname, Convert.ToInt32(115200), Convert.ToInt32(8), Convert.ToInt32(100), Convert.ToInt32(100));
            string network = extractSMS.GetSimNetWork(serialPort);
            MessageBox.Show(network);

            Port port = new Port()
            {
                name = portname,
                telco = network,
            };
            string raw_info = extractSMS.CheckMoney(serialPort);
            MessageBox.Show(raw_info);
            if (raw_info != null && raw_info != "")
            {
                Regex phone_number_rule = new Regex(@"(0|84)\d{9}");
                port.phone_number = phone_number_rule.Match(raw_info).Groups[0].Value;
                Regex balance_rule = new Regex(@"(\d+) ?(VND|d)"); //Regex balance_rule = new Regex(@"([\.\d]+) ?(VND|d)");
                port.cash = int.Parse(balance_rule.Match(raw_info.Replace(".", string.Empty)).Groups[1].Value);
            }
            else
                port.cash = 0;
            extractSMS.ClosePort(serialPort);
            List<Port> list_port_db = db.Port.Where(x => !x.is_delete).ToList();
            db.Port.RemoveRange(list_port_db);
            db.Port.Add(port);
            db.SaveChanges();
            var list_port_new = from p in db.Port.Where(x => !x.is_delete).ToList()
                                select new
                                {
                                    cash = p.cash,
                                    telco = p.telco,
                                    phone_number = p.phone_number,
                                    name = p.name,
                                };
            dataGridView1.DataSource = list_port_new.ToList();
            MessageBox.Show("Quét thành công!");

        }
        //private void bt_refresh_Click(object sender, EventArgs e)
        //{

        //    for (int i = 0; i < 100; i++)
        //    {
        //        string port = "COM" + i;
        //        Thread thread = new Thread(ReadSMS)
        //        {
        //            IsBackground = true
        //        };
        //        thread.Start(port);
        //    }

        //    Form1 form1 = new Form1();
        //    form1.Show();
        //    string text = "";

        //    List<Port> list_port = new List<Port>();
        //    for (int i = 1; i < 100; i++)
        //    {
        //        text = "Bắt đầu quét cổng!" + i + "\n\r"+ text ;
        //        form1.textBox1.Text = text;

        //        try
        //        {
        //            SerialPort serialPort = new SerialPort();
        //            Console.WriteLine("COM" + i);
        //            serialPort = extractSMS.OpenPort("COM" + i, Convert.ToInt32(115200), Convert.ToInt32(8), Convert.ToInt32(100), Convert.ToInt32(100));
        //            string network = extractSMS.GetSimNetWork(serialPort);
        //            text = network + "\n\r" + text;
        //            form1.textBox1.Text = text;

        //            Port port = new Port()
        //            {
        //                name = "COM" + i,
        //                telco = network,
        //            };
        //            string raw_info = extractSMS.CheckMoney(serialPort);
        //            text = raw_info + "\n\r" + text;
        //            form1.textBox1.Text = text;

        //            if (raw_info != null && raw_info != "")
        //            {
        //                Regex phone_number_rule = new Regex(@"(0|84)\d{9}");
        //                port.phone_number = phone_number_rule.Match(raw_info).Groups[0].Value;
        //                Regex balance_rule = new Regex(@"(\d+) ?(VND|d)"); //Regex balance_rule = new Regex(@"([\.\d]+) ?(VND|d)");
        //                port.cash = int.Parse(balance_rule.Match(raw_info.Replace(".", string.Empty)).Groups[1].Value);
        //            }
        //            else
        //                port.cash = 0;
        //            list_port.Add(port);
        //            extractSMS.ClosePort(serialPort);
        //        }
        //        catch (Exception ex)
        //        {
        //            extractSMS.ClosePort(serialPort);
        //            text = ex.Message + "\n\r" + text;
        //            form1.textBox1.Text = text;

        //        }
        //        form1.textBox1.Text = text;
        //    }
        //    List<Port> list_port_db = db.Port.Where(x => !x.is_delete).ToList();
        //    db.Port.RemoveRange(list_port_db);
        //    db.Port.AddRange(list_port);
        //    db.SaveChanges();
        //    var list_port_new = from p in db.Port.Where(x => !x.is_delete).ToList()
        //                        select new
        //                        {
        //                            cash = p.cash,
        //                            telco = p.telco,
        //                            phone_number = p.phone_number,
        //                            name = p.name,
        //                        };
        //    dataGridView1.DataSource = list_port_new.ToList();
        //    MessageBox.Show("Quét thành công!");
        //}
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var curent_row = dataGridView1.CurrentRow;
            string phone_number = curent_row.Cells["phone_number"].Value.ToString();
            textBox1.Text = phone_number;
            string telco = curent_row.Cells["telco"].Value.ToString();
            textBox3.Text = telco;

            var list_sms_history = from p in db.Send_SMS_History.Where(x => x.phone_receive == phone_number).ToList()
                                   select new
                                   {
                                       phone_receive = p.phone_receive,
                                       phone_send = p.phone_send,
                                       message = p.message,
                                       telco = p.telco,
                                       sum_sms = p.sum_sms,
                                       status = p.status,
                                       dateAdded = p.dateAdded,
                                   };
            dataGridView3.DataSource = list_sms_history.ToList();

            var list_sms_receive = from p in db.Message_Receive.Where(x => x.phone_receive == phone_number).ToList()
                                   select new
                                   {
                                       phone_receive = p.phone_receive,
                                       phone_send = p.phone_send,
                                       message = p.message,
                                       status = p.status,
                                       date_receive = p.date_receive,
                                   };
            dataGridView4.DataSource = list_sms_receive.ToList();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            //timer1.Interval = (30 * 1000); // 45 mins
            //timer1.Tick += new EventHandler(MyTimer_Tick);
            //timer1.Start();
            Message_Request message_request_db = db.Message_Request.Where(x => !x.is_delete && x.status == 0).OrderByDescending(x => x.id).FirstOrDefault();
            Port current_port = new Port();
            bool connect_gsm = false;
            List<Port> ports = db.Port.Where(x => !x.is_delete).ToList();
            foreach (var item in ports)
            {
                if (item.cash > 300)
                {
                    connect_gsm = loadPort(item.name);
                    if (connect_gsm)
                    {
                        current_port = item;
                        break;
                    }
                }
            }
            bool smsSent = extractSMS.sendMsg2(this.serialPort, message_request_db.phone_receive, message_request_db.message, 500);
            Send_SMS_History sms_history = new Send_SMS_History()
            {
                phone_receive = message_request_db.phone_receive,
                message = message_request_db.message,
                telco = message_request_db.telco,
                sum_sms = message_request_db.sum_sms,
                status = 1,
                phone_send = current_port.phone_number
            };
            message_request_db.status = 2;
            db.Message_Request.AddOrUpdate(message_request_db);
            db.Send_SMS_History.Add(sms_history);
            db.SaveChanges();
            var list_message_request = from p in db.Message_Request.Where(x => !x.is_delete).OrderByDescending(x => x.id).ToList()
                                       select new
                                       {
                                           phone_receive = p.phone_receive,
                                           message = p.message,
                                           telco = p.telco,
                                           sum_sms = p.sum_sms,
                                           status = p.status,
                                           dateAdded = p.dateAdded,
                                       }; ;
            dataGridView2.DataSource = list_message_request.ToList();

        }
        private void MyTimer_Tick(object sender, EventArgs e)
        {
            Message_Request message_request_db = db.Message_Request.Where(x => !x.is_delete && x.status == 0).OrderByDescending(x => x.id).FirstOrDefault();
            Port current_port = new Port();
            bool connect_gsm = false;
            List<Port> ports = db.Port.Where(x => !x.is_delete).ToList();
            foreach (var item in ports)
            {
                if (item.cash > 300)
                {
                    connect_gsm = loadPort(item.name);
                    if (connect_gsm)
                    {
                        current_port = item;
                        break;
                    }
                }
            }
            bool smsSent = extractSMS.sendMsg(this.serialPort, message_request_db.phone_receive, message_request_db.message, 500);
            Send_SMS_History sms_history = new Send_SMS_History()
            {
                phone_receive = message_request_db.phone_receive,
                message = message_request_db.message,
                telco = message_request_db.telco,
                sum_sms = message_request_db.sum_sms,
                status = 1,
                phone_send = current_port.phone_number
            };
            db.Send_SMS_History.Add(sms_history);
            db.SaveChanges();
            timer1.Interval = (30 * 1000); // 45 mins
        }
        public bool loadPort(string com_port)
        {
            try
            {
                serialPort = extractSMS.OpenPort(com_port, Convert.ToInt32(115200), Convert.ToInt32(8), Convert.ToInt32(100), Convert.ToInt32(100));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormConfig formConfig = new FormConfig();
            formConfig.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Port port = db.Port.FirstOrDefault(x => !x.is_delete && x.phone_number == textBox1.Text);
            serialPort = extractSMS.OpenPort(port.name, Convert.ToInt32(115200), Convert.ToInt32(8), Convert.ToInt32(100), Convert.ToInt32(100));
            string response = extractSMS.NapTienVaoSim(serialPort, textBox2.Text);
            MessageBox.Show(response);
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
