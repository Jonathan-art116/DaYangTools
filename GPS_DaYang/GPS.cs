using IniParser;
using IniParser.Model;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
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

namespace GPS_DaYang
{
    public partial class GPS : Form
    {
        public GPS()
        {
            InitializeComponent();
        }

        private void GPS_Load(object sender, EventArgs e)
        {
            RegistryKey keyCom = Registry.LocalMachine.OpenSubKey("Hardware\\DeviceMap\\SerialComm");
            if (keyCom != null)
            {
                string[] sSubKeys = keyCom.GetValueNames();
                comboBox1.Items.Clear();
                foreach (string sName in sSubKeys)
                {
                    string sValue = (string)keyCom.GetValue(sName);
                    comboBox1.Items.Add(sValue);
                }
                if (comboBox1.Items.Count > 0)
                    comboBox1.SelectedIndex = 0;
            }
        }
        class data
        {
            public static string min;
            public static string max;
            public static string SN;
            public static string ip;
            public static string database;
            public static string username;
            public static string password;
            public static string port;
            public static string tablename;
            public static string connstr;
            public static string post;
            public static string station;
        }

        bool isOpened = false;
        private void SerialButton_Click(object sender, EventArgs e)
        {
            if (!isOpened)
            {
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = 115200;
                try
                {
                    serialPort1.Open();     //打开串口
                    SerialButton.Text = "关闭串口";
                    comboBox1.Enabled = false;//关闭使能
                    isOpened = true;
                    timer1.Interval = 1000;
                    timer1.Start();
                    Delay(3000);
                    //button1.Enabled = true;
                    serialPort1.DataReceived += new SerialDataReceivedEventHandler(post_DataReceived);//串口接收处理函数
                    var parser = new FileIniDataParser();
                    IniData fromdata = parser.ReadFile("config.ini");
                    data.ip = fromdata["database"]["ip"];
                    data.database = fromdata["database"]["database"];
                    data.username = fromdata["database"]["username"];
                    data.password = fromdata["database"]["password"];
                    data.port = fromdata["database"]["port"];
                    data.tablename = fromdata["database"]["tablename"];
                    data.post = fromdata["mysql"]["post"];
                    data.min = fromdata["range"]["min"];
                    data.max = fromdata["range"]["max"];
                    data.station = fromdata["station"]["name"];
                    if(data.station.Length > 2 )
                    {
                        button2.Visible = true;
                    }
                    else
                    {
                        button1.Visible = true;
                    }
                    data.connstr = "server=" + data.ip + ";user=" + data.username + ";database=" + data.database + ";port=" + data.port + ";password=" + data.password;
                    MySqlConnection conn = new MySqlConnection(data.connstr);
                    try
                    {
                        conn.Open();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "数据库连接失败,请检查数据库配置", "错误提示");
                    }
                    conn.Close();
                }
                catch
                {
                    MessageBox.Show("SerialPort Open Fail", "Error Message");
                }
            }
            else
            {
                try
                {
                    timer1.Stop();
                    serialPort1.Close();     //关闭串口
                    SerialButton.Text = "打开串口";
                    comboBox1.Enabled = true;//打开使能
                    isOpened = false;
                    label2.Text = "";
                    label5.Text = "";
                    label3.Text = "";
                    label4.Visible = false;
                    button1.Enabled = false;
                    button2.Enabled = false;
                }
                catch
                {
                    MessageBox.Show("SerialPort Close Fail", "Error Message");
                }
            }
        }

        public void post_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            try
            {
                string datav = serialPort1.ReadLine() + "\r\n";
                string dataq = datav.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                string[] t_data = dataq.Split(':');
                if (datav.IndexOf("$LTE:SN") == 0)
                {
                    data.SN = t_data[2];
                    Invoke((new Action(() =>
                    {
                        if(data.SN.Length > 10)
                        {
                            label2.Text = data.SN;
                            button1.Enabled = true;
                            button2.Enabled = true;
                        }
                        else
                        {
                            label2.Text = "获取SN号异常";
                        }
                    })));
                }
                else if(datav.IndexOf("$GPS:AT") == 0)
                {
                    string[] t = t_data[3].Split(',');
                    if(t[4] == "01")
                    {
                        if(t[7].Length > 2)
                        {
                            string[] g = t[7].Split('*');
                            Invoke((new Action(() =>
                            {
                                label3.Text += g[0] + ",";
                                label4.Visible = false;
                            })));
                        }
                        else
                        {
                            Invoke((new Action(() =>
                            {
                                label3.Text += t[7] + ",";
                                label4.Visible = false;
                            })));
                        }
                    }
                    else
                    {
                        Invoke((new Action(() =>
                        {
                            label4.Visible = true;
                        })));
                    }
       
                }
                Invoke((new Action(() => //C# 3.0以后代替委托的新方法
                {
                    textBox1.AppendText("---" + time + "---" + "\r\n");
                    textBox1.AppendText(datav);//对话框追加显示数据
                })));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            serialPort1.Write("$LTE:SN\r\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            bool result = false;
            label3.Text = "";
            label5.Text = "";
            timer1.Stop();
            timer2.Interval = 1000;
            timer2.Start();
            Delay(6000);
            timer2.Stop();
            if (string.Compare(label3.Text.Split(',').Max(), data.min, true) == 1 && string.Compare(data.max, label3.Text.Split(',').Max(), true) == 1)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            if (result == true && data.post == "YES")
            {
                UpdataSQL(label2.Text, "Gps", "pass");
                label5.Text = "PASS";
                label5.ForeColor = Color.Green;
            }
            else if (result == false && data.post == "YES")
            {
                UpdataSQL(label2.Text, "Gps","fail");
                label5.Text = "Fail";
                label5.ForeColor = Color.Red;
            }
            else if (result == true && data.post == "NO")
            {
                label5.Text = "PASS";
                label5.ForeColor = Color.Green;
            }
            else if (result == false && data.post == "NO")
            {
                label5.Text = "FAIL";
                label5.ForeColor = Color.Red;
            }
            button1.Enabled = true;
        }
        public static void Delay(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)//毫秒
            {
                Application.DoEvents();//可执行某无聊的操作
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            serialPort1.Write("$GPS:AT:AT+QGPS=1\r\n");
            serialPort1.Write("$GPS:AT:AT+QGPSGNMEA=\"GSV\"\r\n");
        }

        public void UpdataSQL(string sn, string name, string reslut)
        {
            MySqlConnection conn = new MySqlConnection(data.connstr);
            try
            {
                conn.Open();
                string update = "update " + data.tablename + " set " + name + "='" + reslut + "' where SN='" + sn + "';";
                MySqlCommand update1 = new MySqlCommand(update, conn);
                if (update1.ExecuteNonQuery() > 0)
                {
                    Console.WriteLine("设备：" + sn + "  - - - - " + name + " - -上传成功");
                }
                else
                {
                    MessageBox.Show("上传数据库失败", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn.Close();
        }

        public string CheckStation(string sn, string station)
        {
            string ui;
            MySqlConnection conn = new MySqlConnection(data.connstr);
            try
            {
                conn.Open();
                string select = "select * from " + data.tablename + " where SN='" + sn + "' and " + station + "='pass';";
                MySqlCommand cmd = new MySqlCommand(select, conn);
                Object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return ui = "true";
                }
                else
                {
                    return ui = "false";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return ui = "sqlerror";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(CheckStation(label2.Text,data.station) == "true")
            {
                button2.Enabled = false;
                bool result = false;
                label3.Text = "";
                label5.Text = "";
                timer1.Stop();
                timer2.Interval = 1000;
                timer2.Start();
                Delay(6000);
                timer2.Stop();
                if (string.Compare(label3.Text.Split(',').Max(), data.min, true) == 1 && string.Compare(data.max, label3.Text.Split(',').Max(), true) == 1)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

                if (result == true && data.post == "YES")
                {
                    UpdataSQL(label2.Text, "Gps", "pass");
                    label5.Text = "PASS";
                    label5.ForeColor = Color.Green;
                }
                else if (result == false && data.post == "YES")
                {
                    UpdataSQL(label2.Text, "Gps", "fail");
                    label5.Text = "Fail";
                    label5.ForeColor = Color.Red;
                }
                else if (result == true && data.post == "NO")
                {
                    label5.Text = "PASS";
                    label5.ForeColor = Color.Green;
                }
                else if (result == false && data.post == "NO")
                {
                    label5.Text = "FAIL";
                    label5.ForeColor = Color.Red;
                }
                button2.Enabled = true;
            }
            else
            {
                MessageBox.Show("配置站位:" + data.station + ",未测试通过！","Error");
            }
        }
    }
}
