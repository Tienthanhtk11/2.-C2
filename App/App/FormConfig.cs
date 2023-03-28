using System;
using System.Configuration;
using System.Windows.Forms;

namespace App
{
    public partial class FormConfig : Form
    {
        public FormConfig()
        {
            InitializeComponent();
        }
        private void FormConfig_Load(object sender, EventArgs e)
        {
            //"Data Source=KRIZPHAM-PC;Persist Security Info=True;Integrated Security=True;Initial Catalog=SMS_Services_App;App=EntityFramework" providerName = "System.Data.SqlClient"
            string db = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            db = db.Replace("Data Source=", "");
            db = db.Replace("Persist Security Info=True;Integrated Security=True;Initial Catalog=", "");
            db.Replace("App=EntityFramework\" providerName = \"System.Data.SqlClient", "");
            db = db.Replace(" ", "");
            var db_info = db.Split(';');
            textBox1.Text = db_info[0];
            textBox4.Text = db_info[1];
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            Console.WriteLine(connectionStringsSection.ConnectionStrings["Database"].ConnectionString);
            connectionStringsSection.ConnectionStrings["Database"].ConnectionString =
                "Data Source=" + textBox1.Text + "; Persist Security Info=True;Integrated Security=True;Initial Catalog=" + textBox4.Text + "; App=EntityFramework\" providerName = \"System.Data.SqlClient\"";
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");
        }
    }
}
