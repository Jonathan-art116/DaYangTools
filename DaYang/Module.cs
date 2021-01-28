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
    public partial class Module : Form
    {
        public Module()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            main.button5.BackColor = Color.Green;
            timer1.Stop();
            if (main.label2.Text.Length > 10 && label9.Text.Length > 14 && label12.Text.Length > 10 && main.label3.Text == "YES")
            {
                main.UpdataSQL(main.label2.Text, "Module", "pass");
                //main.UpdataSQL(main.label2.Text, "ICCID", label12.Text);
                //main.UpdataSQL(main.label2.Text, "IMEI", label9.Text);
                main.UpdateIccid(main.label2.Text, label12.Text);
                main.UpdateImei(main.label2.Text, label9.Text);
            }
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            main.button5.BackColor = Color.Red;
            timer1.Stop();
            if (main.label2.Text.Length > 10 && main.label3.Text == "YES")
            {
                main.UpdataSQL(main.label2.Text, "Module", "fail");
            }
            this.Close();
        }

        public void Setlab(string ver)
        {
            this.label9.Text = ver;
        }

        public void Setlab3(string Modem)
        {
            this.label10.Text = Modem;
        }

        public void Setlab4(string SIM)
        {
            this.label11.Text = SIM;
        }
        public void Setlab5(string ICCID)
        {
            this.label12.Text = ICCID;
        }
        public void Setlab6(string IMSI)
        {
            this.label13.Text = IMSI;
        }
        internal void MainFormTxtChaned(object sender, EventArgs e)
        {
            //取到主窗体的传来的文本
            MyEventArg arg = e as MyEventArg;
            this.Setlab(arg.IMEI);
            this.Setlab3(arg.MODEM);
            this.Setlab4(arg.SIM);
            this.Setlab5(arg.ICCID);
            this.Setlab6(arg.IMSI);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            main.serialPort1.Write("$LTE:IMEI\r\n");
            main.serialPort1.Write("$LTE:MODEM\r\n");
            main.serialPort1.Write("$LTE:SIM\r\n");
            main.serialPort1.Write("$LTE:ICCID\r\n");
            main.serialPort1.Write("$LTE:IMSI\r\n");
        }

        private void Module_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Start();
        }
    }
}
