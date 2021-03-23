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
    public partial class Time : Form
    {
        public Time()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            string time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            Main main = (Main)this.Owner;
            main.button10.BackColor = Color.Green;
            if (main.label2.Text.Length > 10 && main.label3.Text == "YES")
            {
                main.UpdataSQL(main.label2.Text, "Time", "pass");
                //main.UpdataSQL(main.label2.Text, "PCBDT", time);
            }
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            string time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            Main main = (Main)this.Owner;
            main.button10.BackColor = Color.Red;
            if (main.label2.Text.Length > 10 && main.label3.Text == "YES")
            {
                main.UpdataSQL(main.label2.Text, "Time", "fail");
                //main.UpdataSQL(main.label2.Text, "PCBDT", time);
            }
            this.Close();
        }

        internal void MainFormTxtChaned(object sender, EventArgs e)
        {
            //取到主窗体的传来的文本
            MyEventArg arg = e as MyEventArg;
            this.Setlab(arg.Gettime);
        }

        public void Setlab(string Batt)
        {
            this.label5.Text = Batt;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            main.serialPort1.Write("$LTE:TIME\r\n");
            string time = DateTime.Now.ToString("yyyy/MM/dd/HH/mm/ss");
            label3.Text = time;
            if(label3.Text.Length > 13 && label5.Text.Length > 13)
            {
                string Pcym = label3.Text.Replace("/", "").Substring(0, 6);
                string Deviceym = label5.Text.Replace("/", "").Substring(0, 6);
                string Pctime = label3.Text.Replace("/", "").Substring(6,6);
                string Deivcetime = label5.Text.Replace("/", "").Substring(6,6);
                if (Pcym == Deviceym)
                {
                    if((Convert.ToSingle(Pctime)+2) > Convert.ToSingle(Deivcetime) && (Convert.ToSingle(Pctime) - 2) < Convert.ToSingle(Deivcetime))
                    {
                        button1.Enabled = true;
                    }
                }
            }
        }

        private void Time_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Start();
        }
    }
}
