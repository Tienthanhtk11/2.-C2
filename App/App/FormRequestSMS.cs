﻿using App.Model;
using ExcelDataReader;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace App
{
    public partial class FormRequestSMS : Form
    {
        public static DataSet list_phone;
        public static DataSet list_message;
        public static long customer_id;
        public FormRequestSMS(long current_user_id)
        {
            InitializeComponent();
            customer_id = current_user_id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ServiceUrl = "http://103.120.242.146:8088/api/";
            try
            {

                Data_Upload data = new Data_Upload();
                foreach (DataRow row in list_phone.Tables[0].Rows)
                {
                    Phone_Number phone = new Phone_Number
                    {
                        phone_number = row[1].ToString(),
                        telco = row[2].ToString(),
                        customer_id = customer_id
                    };
                    data.list_phone_number.Add(phone);
                }
                foreach (DataRow row in list_message.Tables[0].Rows)
                {
                    SMS_Template sms_Template = new SMS_Template
                    {
                        customer_id = customer_id,
                        message = row[1].ToString()
                    };
                    data.list_sms_template.Add(sms_Template);
                }

                string resourcePath = "SMS/request";
                var body = JsonConvert.SerializeObject(data);
                var client = new RestClient(ServiceUrl);
                var request = new RestRequest(resourcePath, Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "application/json");
                request.AddJsonBody(body);
                var response = client.Execute(request);
                var response_token = JsonConvert.DeserializeObject<ResponseSingleContentModel<string>>(response.Content ?? "");
                if (response_token.StatusCode == 200)
                    MessageBox.Show("Success upload data to server!");
                else
                    MessageBox.Show("Send data to server false!, Please try again ");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Send data to server false!, Please try again ");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel Workbook|*.xlsx|Excel Workbook 97-2003|*.xls", ValidateNames = true })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))
                    {
                        IExcelDataReader reader;
                        if (ofd.FilterIndex == 2)
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }

                        list_phone = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        });
                        reader.Close();
                    }
                }
            }
            dataGridView1.DataSource = list_phone.Tables[0];
        }


        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel Workbook|*.xlsx|Excel Workbook 97-2003|*.xls", ValidateNames = true })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))
                    {
                        IExcelDataReader reader;
                        if (ofd.FilterIndex == 2)
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        }
                        else
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }

                        list_message = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        });
                        reader.Close();
                    }
                }
            }
            dataGridView2.DataSource = list_message.Tables[0];
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FormAutoSendSMS form = new FormAutoSendSMS(customer_id);
            form.ShowDialog();
        }

        private void FormRequestSMS_Load(object sender, EventArgs e)
        {

        }
    }

}

