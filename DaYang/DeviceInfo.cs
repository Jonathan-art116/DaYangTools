using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFrmDemo;

namespace DaYang
{
    public partial class DeviceInfo : Form
    {
        public DeviceInfo()
        {
            InitializeComponent();
        }

        private void DeviceInfo_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Start();
            var parser = new FileIniDataParser();
            IniData fromdata = parser.ReadFile("config.ini");
            data.ver = fromdata["VersionInformation"]["VER"];
        }

        class data
        {
            public static string ver;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            main.button1.BackColor = Color.Green;
            timer1.Stop();
            if(label7.Text.Length > 10 && label3.Text.Length > 7 && main.label3.Text == "YES")
            {
                main.Insert(label7.Text);
                main.UpdataSQL(label7.Text, "VER", label3.Text);
                main.UpdataSQL(main.label2.Text, "DeviceInfo", "pass");
            }
            main.button2.Enabled = true;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            main.button1.BackColor = Color.Red;
            timer1.Stop();
            if (label7.Text.Length > 10 && main.label3.Text == "YES")
            {
                main.UpdataSQL(label7.Text, "DeviceInfo", "fail");
            }
            this.Close();
        }

        public void Setlab(string ver)
        {
            this.label3.Text = ver;
        }

        //public void Setlab2(string flash)
        //{
        //    this.label4.Text = flash;
        //}
        public void Setlab3(string sn)
        {
            this.label7.Text = sn;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            main.serialPort1.Write("$VER:GET\r\n");
            //main.serialPort1.Write("$FLASH:INFO\r\n");
            main.serialPort1.Write("$LTE:SN\r\n");
            //Console.WriteLine(label7.Text.Length);
            if(label7.Text.Length > 7)
            {
                if (label3.Text == data.ver)
                {
                    button1.Enabled = true;
                }
            }
        }

        internal void MainFormTxtChaned(object sender, EventArgs e)
        {
            //取到主窗体的传来的文本
            MyEventArg arg = e as MyEventArg;
            this.Setlab(arg.Ver);
            //this.Setlab2(arg.Flash);
            this.Setlab3(arg.SN);
        }
    }
}
