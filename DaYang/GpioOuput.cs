using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DaYang
{
    public partial class GpioOuput : Form
    {
        public GpioOuput()
        {
            InitializeComponent();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            main.button7.BackColor = Color.Green;
            if (main.label2.Text.Length > 10 && main.label3.Text == "YES")
            {
                main.UpdataSQL(main.label2.Text, "GpioOutput", "pass");
            }
            Closeoutput();
            this.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            main.button7.BackColor = Color.Red;
            if (main.label2.Text.Length > 10 && main.label3.Text == "YES")
            {
                main.UpdataSQL(main.label2.Text, "GpioOutput", "fail");
            }
            Closeoutput();
            this.Close();
        }

        public bool test2 = false;
        private void button2_Click(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            //main.serialPort1.Write("$GPIO:OUTPUT2,{0 1}\r\n");
            if (test2 == false)
            {
                main.serialPort1.Write("$GPIO:OUTPUT2,1\r\n");
                test2 = true;
            }
            else if (test2 == true)
            {
                main.serialPort1.Write("$GPIO:OUTPUT2,0\r\n");
                test2 = false;
            }
        }

        public bool test3 = false;
        private void button3_Click(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            //main.serialPort1.Write("$GPIO: OUTPUT3,{ 0 1}\r\n");
            if (test3 == false)
            {
                main.serialPort1.Write("$GPIO:OUTPUT3,1\r\n");
                test3 = true;
            }
            else if (test3 == true)
            {
                main.serialPort1.Write("$GPIO:OUTPUT3,0\r\n");
                test3 = false;
            }
        }

        public bool test4 = false;
        private void button4_Click(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            //main.serialPort1.Write("$GPIO: OUTPUT4,{ 0 1}\r\n");
            if (test4 == false)
            {
                main.serialPort1.Write("$GPIO:OUTPUT4,1\r\n");
                test4 = true;
            }
            else if (test4 == true)
            {
                main.serialPort1.Write("$GPIO:OUTPUT4,0\r\n");
                test4 = false;
            }
        }

        public bool test5 = false;
        private void button5_Click(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            //main.serialPort1.Write("$GPIO:LEFT_TRUN,{0 1}\r\n");
            if (test5 == false)
            {
                main.serialPort1.Write("$GPIO:LEFT_TRUN,1\r\n");
                test5 = true;
            }
            else if (test5 == true)
            {
                main.serialPort1.Write("$GPIO:LEFT_TRUN,0\r\n");
                test5 = false;
            }
        }

        public bool test6 = false;
        private void button6_Click(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            //main.serialPort1.Write("$GPIO:RIGHT_TRUN,{0 1}\r\n");
            if (test6 == false)
            {
                main.serialPort1.Write("$GPIO:RIGHT_TRUN,1\r\n");
                test6 = true;
            }
            else if (test6 == true)
            {
                main.serialPort1.Write("$GPIO:RIGHT_TRUN,0\r\n");
                test6 = false;
            }
        }

        public bool test7 = false;
        private void button7_Click(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            //main.serialPort1.Write("$GPIO:LOCK_BLUE,{0 1}\r\n");
            if (test7 == false)
            {
                main.serialPort1.Write("$GPIO:LOCK_BLUE,1\r\n");
                test7 = true;
            }
            else if (test7 == true)
            {
                main.serialPort1.Write("$GPIO:LOCK_BLUE,0\r\n");
                test7 = false;
            }
        }

        public bool test8 = false;
        private void button8_Click(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            //main.serialPort1.Write("$GPIO:LOCK_RED,{0 1}\r\n");
            if (test8 == false)
            {
                main.serialPort1.Write("$GPIO:LOCK_RED,1\r\n");
                test8 = true;
            }
            else if (test8 == true)
            {
                main.serialPort1.Write("$GPIO:LOCK_RED,0\r\n");
                test8 = false;
            }
        }

        public bool test9 = false;
        private void button9_Click(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            //main.serialPort1.Write("$GPIO:ELECT,{0 1}\r\n");
            if (test9 == false)
            {
                main.serialPort1.Write("$GPIO:ELECT,1\r\n");
                test9 = true;
            }
            else if (test9 == true)
            {
                main.serialPort1.Write("$GPIO:ELECT,0\r\n");
                test9 = false;
            }
        }

        public static void Delay(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)//毫秒
            {
                Application.DoEvents();//可执行某无聊的操作
            }
        }

        public bool test1 = false;
        private void button1_Click(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            if (test1 == false)
            {
                main.serialPort1.Write("$GPIO:OUTPUT1,1\r\n");
                test1 = true;
            }
            else if (test1 == true)
            {
                main.serialPort1.Write("$GPIO:OUTPUT1,0\r\n");
                test1 = false;
            }
        }

        public void Closeoutput()
        {
            Main main = (Main)this.Owner;
            main.serialPort1.Write("$GPIO:OUTPUT1,0\r\n");
            main.serialPort1.Write("$GPIO:OUTPUT2,0\r\n");
            main.serialPort1.Write("$GPIO:OUTPUT3,0\r\n");
            main.serialPort1.Write("$GPIO:OUTPUT4,0\r\n");
            main.serialPort1.Write("$GPIO:LEFT_TRUN,0\r\n");
            main.serialPort1.Write("$GPIO:RIGHT_TRUN,0\r\n");
            main.serialPort1.Write("$GPIO:LOCK_BLUE,0\r\n");
            main.serialPort1.Write("$GPIO:LOCK_RED,0\r\n");
            main.serialPort1.Write("$GPIO:ELECT,0\r\n");
        }
    }
}
