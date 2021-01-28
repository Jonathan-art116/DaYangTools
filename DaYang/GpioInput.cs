using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFrmDemo;

namespace DaYang
{
    public partial class GpioInput : Form
    {
        public GpioInput()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            main.button6.BackColor = Color.Green;
            timer1.Stop();
            if (main.label2.Text.Length > 10 && main.label3.Text == "YES")
            {
                main.UpdataSQL(main.label2.Text, "GpioInput", "pass");
            }
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            main.button6.BackColor = Color.Red;
            timer1.Stop();
            if (main.label2.Text.Length > 10 && main.label3.Text == "YES")
            {
                main.UpdataSQL(main.label2.Text, "GpioInput", "fail");
            }
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            main.serialPort1.Write("$GPIO:INPUT\r\n");
            label11.Text = label1.Text.Substring(0, 1);
            label12.Text = label1.Text.Substring(1, 1);
            label13.Text = label1.Text.Substring(2, 1);
            label14.Text = label1.Text.Substring(3, 1);
            label15.Text = label1.Text.Substring(4, 1);
            label16.Text = label1.Text.Substring(5, 1);
            label17.Text = label1.Text.Substring(6, 1);
            label18.Text = label1.Text.Substring(7, 1);
        }

        private void GpioInput_Load(object sender, EventArgs e)
        {
            timer1.Interval = 500;
            timer1.Start();
        }

        public void Setlab(string txt)
        {
            this.label1.Text = txt;
        }

        //public void AfterParentFrmTextChange(object sender, EventArgs e)
        //{
        //    //拿到父窗体的传来的文本
        //    MyEventArg arg = e as MyEventArg;
        //    this.Setlab(arg.Text);
        //}

        internal void MainFormTxtChaned(object sender, EventArgs e)
        {
            //取到主窗体的传来的文本
            MyEventArg arg = e as MyEventArg;
            this.Setlab(arg.Text);
        }
    }
}
