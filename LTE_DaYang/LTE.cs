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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LTE_DaYang
{
    public partial class LTE : Form
    {
        public LTE()
        {
            InitializeComponent();
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
                    data.min_z = fromdata["range"]["min_zhu"];
                    data.max_z = fromdata["range"]["max_zhu"];
                    data.min_f = fromdata["range"]["min_fu"];
                    data.max_f = fromdata["range"]["max_fu"];
                    data.min_zf = Convert.ToSingle(data.min_z);
                    data.max_zf = Convert.ToSingle(data.max_z);
                    data.min_ff = Convert.ToSingle(data.min_f);
                    data.max_ff = Convert.ToSingle(data.max_f);
                    data.station = fromdata["station"]["name"];
                    //if (data.station.Length > 2)
                    //{
                    //    button2.Visible = true;
                    //}
                    //else
                    //{
                    //    button1.Visible = true;
                    //}
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
                    label1.Text = "";
                    label10.Text = "";
                    label11.Text = "";
                    label12.Text = "";
                    button1.Enabled = false;
                }
                catch
                {
                    MessageBox.Show("SerialPort Close Fail", "Error Message");
                }
            }
        }

        class data
        {
            public static string status;
            public static string min_z;
            public static string max_z;
            public static string min_f;
            public static string max_f;
            public static float min_zf;
            public static float max_zf;
            public static float min_ff;
            public static float max_ff;
            public static string db;
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

        private void Form1_Load(object sender, EventArgs e)
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
                    if(data.SN.Length > 10)
                    {
                        Invoke((new Action(() =>
                        {
                            label1.Text = data.SN;
                        })));
                    }
                }
                else if (datav.IndexOf("$FTM:AT:+QRXFTM") == 0)
                {
                    string[] zhu = dataq.Split(',');
                    Invoke((new Action(() =>
                    {
                        label9.Text += zhu[1] + ",";
                    })));
                }
                Invoke((new Action(() => //C# 3.0以后代替委托的新方法
                {
                    textBox1.AppendText("---" + time + "---" + "\r\n");
                    textBox1.AppendText(datav);//对话框追加显示数据
                })));
            }
            catch
            {
                
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            serialPort1.Write("$LTE:SN\r\n");
            //serialPort1.Write("$LTE:CEREG\r\n");
            //serialPort1.Write("$LTE:CSQ\r\n");
            serialPort1.Write("$FTM:AT:AT+QRFTESTMODE=1\r\n");
            if (label1.Text.Length > 10)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            if (data.station.Length > 2)
            {
                if(CheckStation(label1.Text,data.station) == "true")
                {
                    timer1.Stop();
                    button1.Enabled = false;
                    label10.Text = "";
                    label11.Text = "";
                    label12.Text = "";
                    bool result_zhu = false;
                    bool result_fu = false;
                    timer2.Interval = 1000;
                    timer2.Start();
                    Delay(6000);
                    timer2.Stop();
                    Delay(10000);
                    try
                    {
                        string[] zhu = label9.Text.Split(',');
                        label10.Text = zhu[0] + "," + zhu[2] + "," + zhu[4] + "," + zhu[6] + "," + zhu[8];// + "," + zhu[10] + "," + zhu[12] + "," + zhu[14] + "," + zhu[16] + "," + zhu[18];
                        label11.Text = zhu[1] + "," + zhu[3] + "," + zhu[5] + "," + zhu[7] + "," + zhu[9];// + "," + zhu[11] + "," + zhu[13] + "," + zhu[15] + "," + zhu[17] + "," + zhu[19];
                        if (Convert.ToSingle(label11.Text.Split(',').Min()) > data.min_ff && Convert.ToSingle(label11.Text.Split(',').Min()) < data.max_ff)
                        {
                            result_fu = true;
                        }
                        if (Convert.ToSingle(label10.Text.Split(',').Min()) > data.min_zf && Convert.ToSingle(label10.Text.Split(',').Min()) < data.max_zf)
                        {
                            result_zhu = true;
                        }

                        if (data.post == "YES")
                        {
                            if (result_fu == true && result_zhu == true)
                            {
                                if (label1.Text.Length > 10 && UpdataSQL(label1.Text, "LTE", "pass") == "ok")
                                {
                                    UpdataSQL(label1.Text, "LTEDT", time);
                                    label12.Text = "PASS";
                                    label12.ForeColor = Color.Green;
                                    button1.Enabled = true;
                                }
                            }
                            else if (result_fu == false || result_zhu == false)
                            {
                                if (label1.Text.Length > 10 && UpdataSQL(label1.Text, "LTE", "fail") == "ok")
                                {
                                    UpdataSQL(label1.Text, "LTEDT", time);
                                    label12.Text = "FAIL";
                                    label12.ForeColor = Color.Red;
                                    button1.Enabled = true;
                                }
                            }
                        }
                        else if (data.post == "NO")
                        {
                            if (result_fu == false || result_zhu == false)
                            {
                                label12.Text = "FAIL";
                                label12.ForeColor = Color.Red;
                                button1.Enabled = true;
                            }
                            else if (result_fu == true && result_zhu == true)
                            {
                                label12.Text = "PASS";
                                label12.ForeColor = Color.Green;
                                button1.Enabled = true;
                            }
                        }
                    }
                    catch
                    {
                        label12.Text = "请重新测试";
                        button1.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("配置站位" + data.station + ": 未测试通过！", "Error");
                }
            }
            else
            {
                timer1.Stop();
                button1.Enabled = false;
                label10.Text = "";
                label11.Text = "";
                label12.Text = "";
                bool result_zhu = false;
                bool result_fu = false;
                timer2.Interval = 1000;
                timer2.Start();
                Delay(6000);
                timer2.Stop();
                Delay(10000);
                try
                {
                    string[] zhu = label9.Text.Split(',');
                    label10.Text = zhu[0] + "," + zhu[2] + "," + zhu[4] + "," + zhu[6] + "," + zhu[8];// + "," + zhu[10] + "," + zhu[12] + "," + zhu[14] + "," + zhu[16] + "," + zhu[18];
                    label11.Text = zhu[1] + "," + zhu[3] + "," + zhu[5] + "," + zhu[7] + "," + zhu[9];// + "," + zhu[11] + "," + zhu[13] + "," + zhu[15] + "," + zhu[17] + "," + zhu[19];
                    if (Convert.ToSingle(label11.Text.Split(',').Min()) > data.min_ff && Convert.ToSingle(label11.Text.Split(',').Min()) < data.max_ff)
                    {
                        result_fu = true;
                    }
                    if (Convert.ToSingle(label10.Text.Split(',').Min()) > data.min_zf && Convert.ToSingle(label10.Text.Split(',').Min()) < data.max_zf)
                    {
                        result_zhu = true;
                    }

                    if (data.post == "YES")
                    {
                        if (result_fu == true && result_zhu == true)
                        {
                            if (label1.Text.Length > 10 && UpdataSQL(label1.Text, "LTE", "pass") == "ok")
                            {
                                UpdataSQL(label1.Text, "LTEDT", time);
                                label12.Text = "PASS";
                                label12.ForeColor = Color.Green;
                                button1.Enabled = true;
                            }
                        }
                        else if (result_fu == false || result_zhu == false)
                        {
                            if (label1.Text.Length > 10 && UpdataSQL(label1.Text, "LTE", "fail") == "ok")
                            {
                                UpdataSQL(label1.Text, "LTEDT", time);
                                label12.Text = "FAIL";
                                label12.ForeColor = Color.Red;
                                button1.Enabled = true;
                            }
                        }
                    }
                    else if (data.post == "NO")
                    {
                        if (result_fu == false || result_zhu == false)
                        {
                            label12.Text = "FAIL";
                            label12.ForeColor = Color.Red;
                            button1.Enabled = true;
                        }
                        else if (result_fu == true && result_zhu == true)
                        {
                            label12.Text = "PASS";
                            label12.ForeColor = Color.Green;
                            button1.Enabled = true;
                        }
                    }
                }
                catch
                {
                    label12.Text = "请重新测试";
                    button1.Enabled = true;
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            serialPort1.Write("$FTM:AT:AT+QRXFTM=1,7,3625,0,0,3\r\n");
            serialPort1.Write("$FTM:AT:AT+QRXFTM=1,7,3625,1,0,3\r\n");
        }

        public string UpdataSQL(string sn, string name, string reslut)
        {
            string ok;
            MySqlConnection conn = new MySqlConnection(data.connstr);
            try
            {
                conn.Open();
                string update = "update " + data.tablename + " set " + name + "='" + reslut + "' where SN='" + sn + "';";
                MySqlCommand update1 = new MySqlCommand(update, conn);
                if (update1.ExecuteNonQuery() > 0)
                {
                    Console.WriteLine("设备：" + sn + "  - - - - " + name + " - -上传成功");
                    return ok ="ok";
                }
                else
                {
                    MessageBox.Show("上传数据库失败", "Error");
                    return ok = "fail";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return ok = "fail";
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
    }
}
