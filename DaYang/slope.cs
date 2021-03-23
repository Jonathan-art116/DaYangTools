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
    public partial class slope : Form
    {
        public slope()
        {
            InitializeComponent();
        }

        private void slope_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            Main main = (Main)this.Owner;
            main.button8.BackColor = Color.Green;
            if(main.label2.Text.Length > 10 && main.label3.Text == "YES")
            {
                main.UpdataSQL(main.label2.Text, "Slope", "pass");
            }
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            Main main = (Main)this.Owner;
            main.button8.BackColor = Color.Red;
            if (main.label2.Text.Length > 10 && main.label3.Text == "YES")
            {
                main.UpdataSQL(main.label2.Text, "Slope", "fail");
            }
            this.Close();
        }
        internal void MainFormTxtChaned(object sender, EventArgs e)
        {
            //取到主窗体的传来的文本
            MyEventArg arg = e as MyEventArg;
            this.Setlab(arg.Slope);
        }

        public void Setlab(string Batt)
        {
            this.label1.Text = Batt;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(label1.Text == "震动告警已触发")
            {
                button1.Enabled = true;
            }
        }
    }
}
