using App.Entity;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace App
{
    public partial class FormConfigWarning : Form
    {
        public FormConfigWarning(long current_user_id)
        {
            customer_id = current_user_id;
            InitializeComponent();
        }
         List<Config_Port> list_port = new List<Config_Port>();
        public static long customer_id = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            FormMain2 readSMS = new FormMain2(list_port, customer_id, "false");
            this.Hide();
            readSMS.ShowDialog();
            this.Close();
            //FormReadSMS readSMS = new FormReadSMS(list_port, customer_id, "false");
            //this.Hide();
            //readSMS.ShowDialog();
            //this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormConfigPort formConfigPort = new FormConfigPort(list_port, customer_id, "true");
            this.Hide();
            formConfigPort.ShowDialog();
            this.Close();
            //FormConfigPort formConfigPort = new FormConfigPort(list_port, customer_id, "true");
            //this.Hide();
            //formConfigPort.ShowDialog();
            //this.Close();
        }
    }
}
