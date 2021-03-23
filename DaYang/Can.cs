using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFrmDemo;

namespace DaYang
{
    public partial class Can : Form
    {
        public Can()
        {
            InitializeComponent();
        }

        private void Can_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Start();
            var parser = new FileIniDataParser();
            IniData fromdata = parser.ReadFile("config.ini");
            cver.ver = fromdata["VersionInformation"]["Can_VER"];
        }

        class cver
        {
            public static string ver;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            string time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            Main main = (Main)this.Owner;
            main.button9.BackColor = Color.Green;
            if (main.label2.Text.Length > 10 && main.label3.Text == "YES")
            {
                main.UpdataSQL(main.label2.Text, "Can", "pass");
                main.UpdataSQL(main.label2.Text, "PCBDT", time);
            }
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            string time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            Main main = (Main)this.Owner;
            main.button9.BackColor = Color.Red;
            if (main.label2.Text.Length > 10 && main.label3.Text == "YES")
            {
                main.UpdataSQL(main.label2.Text, "Can", "fail");
                main.UpdataSQL(main.label2.Text, "PCBDT", time);
            }
            this.Close();
        }

        internal void MainFormTxtChaned(object sender, EventArgs e)
        {
            //取到主窗体的传来的文本
            MyEventArg arg = e as MyEventArg;
            this.Setlab(arg.Can);
        }

        public void Setlab(string Batt)
        {
            this.label2.Text = Batt;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            main.serialPort1.Write("$CAN:IPK\r\n");
            if(label2.Text.Length > 5)
            {
                string[] test = label2.Text.Split('m');
                label4.Text = test[0] + "m";
                label5.Text = test[1];
            }
            if(cver.ver == label5.Text)
            {
                button1.Enabled = true;
            }
        }
    }
}
