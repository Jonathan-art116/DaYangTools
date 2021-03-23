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
    public partial class Battery : Form
    {
        public Battery()
        {
            InitializeComponent();
            var parser = new FileIniDataParser();
            IniData fromdata = parser.ReadFile("config.ini");
            testdata.max = fromdata["battery"]["voltage_max"];
            testdata.min = fromdata["battery"]["voltage_min"];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            main.button4.BackColor = Color.Green;
            timer1.Stop();
            if (main.label2.Text.Length > 10 && main.label3.Text == "YES")
            {
                main.UpdataSQL(main.label2.Text, "Battery", "pass");
            }
            this.Close();
        }

        class testdata
        {
            public static string max;
            public static string min;
        }



        private void button3_Click(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            main.button4.BackColor = Color.Red;
            timer1.Stop();
            if (main.label2.Text.Length > 10 && main.label3.Text == "YES")
            {
                main.UpdataSQL(main.label2.Text, "Battery", "fail");
            }
            this.Close();
        }

        private void Battery_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Start();
        }

        public void Setlab(string Batt)
        {
            this.label1.Text = Batt;
        }

        internal void MainFormTxtChaned(object sender, EventArgs e)
        {
            //取到主窗体的传来的文本
            MyEventArg arg = e as MyEventArg;
            this.Setlab(arg.Battery);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            main.serialPort1.Write("$ADC:VOLTAGE\r\n");
            if(label1.Text.Length > 2)
            {
                string[] test = label1.Text.Split('=');
                if (Convert.ToSingle(testdata.max) > Convert.ToSingle(test[1]) && Convert.ToSingle(testdata.min) < Convert.ToSingle(test[1]))
                {
                    label1.ForeColor = Color.Green;
                    button1.Enabled = true;
                }
                else
                {
                    label1.ForeColor = Color.Red;
                }
            }
        }
    }
}
