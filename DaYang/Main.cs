using Microsoft.Win32;
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
using WinFrmDemo;
using IniParser;
using IniParser.Model;
using MySql.Data.MySqlClient;

namespace DaYang
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            //CheckForIllegalCrossThreadCalls = false;
        }

        private void Main_Load(object sender, EventArgs e)
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
                    serialPort1.DataReceived += new SerialDataReceivedEventHandler(post_DataReceived);//串口接收处理函数
                    button1.Enabled = true;
                    var parser = new FileIniDataParser();
                    IniData fromdata = parser.ReadFile("config.ini");
                    data.ip = fromdata["database"]["ip"];
                    data.database = fromdata["database"]["database"];
                    data.username = fromdata["database"]["username"];
                    data.password = fromdata["database"]["password"];
                    data.port = fromdata["database"]["port"];
                    data.tablename = fromdata["database"]["tablename"];
                    data.post= fromdata["mysql"]["post"];
                    label3.Text = data.post;
                    data.connstr = "server=" + data.ip + ";user=" + data.username + ";database=" + data.database + ";port=" + data.port + ";password=" + data.password;
                    MySqlConnection conn = new MySqlConnection(data.connstr);
                    try
                    {
                        conn.Open();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "数据库连接失败,请检查数据库配置","错误提示");
                    }
                    conn.Close();
                    create();
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
                    serialPort1.Close();     //关闭串口
                    SerialButton.Text = "打开串口";
                    comboBox1.Enabled = true;//打开使能
                    isOpened = false;
                    button1.Enabled = false;
                    button1.BackColor = Color.Gray;
                    button2.Enabled = false;
                    button2.BackColor = Color.Gray;
                    button3.Enabled = false;
                    button3.BackColor = Color.Gray;
                    button4.Enabled = false;
                    button4.BackColor = Color.Gray;
                    button5.Enabled = false;
                    button5.BackColor = Color.Gray;
                    button6.Enabled = false;
                    button6.BackColor = Color.Gray;
                    button7.Enabled = false;
                    button7.BackColor = Color.Gray;
                    button8.Enabled = false;
                    button8.BackColor = Color.Gray;
                }
                catch
                {
                    MessageBox.Show("SerialPort Close Fail", "Error Message");
                }
            }
        }

        public void UpdataSQL(string sn, string name ,string reslut)
        {
            MySqlConnection conn = new MySqlConnection(data.connstr);
            try
            {
                conn.Open();
                string update = "update " + data.tablename + " set " + name + "='" + reslut + "' where SN='" + sn + "';";
                MySqlCommand update1 = new MySqlCommand(update, conn);
                if (update1.ExecuteNonQuery() > 0)
                {
                    Console.WriteLine("设备：" + sn  + "  - - - - " + name  + " - -上传成功");
                }
                else
                {
                    MessageBox.Show("上传数据库失败","Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"警告");
            }
            conn.Close();
        }


        private void create()
        {
            MySqlConnection conn = new MySqlConnection(data.connstr);
            try
            {
                conn.Open();
                string creatdatabase = "CREATE TABLE IF NOT EXISTS " + data.tablename + " ("
                                + "ID int NOT NULL AUTO_INCREMENT,PRIMARY KEY (ID),"
                                + "SN char(20) UNIQUE,"
                                + "IMEI char(24) UNIQUE,"
                                + "ICCID char(24) UNIQUE,"
                                + "VER char(20),"
                                + "DeviceInfo char(8),"
                                + "Flash char(8),"
                                + "Music char(8),"
                                + "PKE char(8),"
                                + "Battery char(8),"
                                + "Module char(8),"
                                + "GpioInput char(8),"
                                + "GpioOutput char(8),"
                                + "BLE char(8),"
                                + "BLEDT datetime,"
                                + "Gps char(8),"
                                + "LTE char(8))";
                MySqlCommand creat = new MySqlCommand(creatdatabase, conn);
                try
                {
                    creat.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn.Close();
        }

        public string Insert(string sn)
        {
            string res;
            MySqlConnection conn = new MySqlConnection(data.connstr);
            try
            {
                conn.Open();
                string select = "select * from " + data.tablename + " where SN='" + sn + "';";
                MySqlCommand cmd = new MySqlCommand(select, conn);
                Object result = cmd.ExecuteScalar();
                //Console.WriteLine(result);
                if (result != null)
                {
                    MessageBox.Show("序列号已存在数据库中！","警告信息");
                    return res = "selectfalse";
                }
                else
                {
                    string insert = "insert into " + data.tablename + " (SN) values ('" + sn + "');";
                    MySqlCommand inserta = new MySqlCommand(insert, conn);
                    if (inserta.ExecuteNonQuery() > 0)
                    {
                        //MessageBox.Show("数据更新成功sn");
                        return res = "inserttrue";
                    }
                    else
                    {
                        MessageBox.Show("序列号数据更新失败");
                        return res = "updatefalse";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return res = "dberror";
            }
            conn.Close();
        }

        public string UpdateImei(string sn,string imei)
        {
            string res;
            MySqlConnection conn = new MySqlConnection(data.connstr);
            try
            {
                conn.Open();
                string select = "select * from " + data.tablename + " where IMEI='" + imei + "';";
                MySqlCommand cmd = new MySqlCommand(select, conn);
                Object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    MessageBox.Show("IMEI已存在数据库中","警告信息");
                    return res = "selectimei";
                }
                else
                {
                    string update = "update " + data.tablename + " set IMEI='" + imei + "' where SN='" + sn + "';";
                    MySqlCommand update1 = new MySqlCommand(update, conn);
                    if (update1.ExecuteNonQuery() > 0)
                    {
                        //MessageBox.Show("更新IMEI成功");
                        return res = "updateok";
                    }
                    else
                    {
                        MessageBox.Show("更新IMEI失败");
                        return res = "updatefail";
                    }
                }
            }
            catch
            {
                return res = "get error";
            }
        }

        public string UpdateIccid(string sn, string iccid)
        {
            string res;
            MySqlConnection conn = new MySqlConnection(data.connstr);
            try
            {
                conn.Open();
                string select = "select * from " + data.tablename + " where ICCID='" + iccid + "';";
                MySqlCommand cmd = new MySqlCommand(select, conn);
                Object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    MessageBox.Show("ICCID已存在数据库中", "警告信息");
                    return res = "selectimei";
                }
                else
                {
                    string update = "update " + data.tablename + " set ICCID='" + iccid + "' where SN='" + sn + "';";
                    MySqlCommand update1 = new MySqlCommand(update, conn);
                    if (update1.ExecuteNonQuery() > 0)
                    {
                        //MessageBox.Show("更新IMEI成功");
                        return res = "updateok";
                    }
                    else
                    {
                        MessageBox.Show("更新IMEI失败");
                        return res = "updatefail";
                    }
                }
            }
            catch
            {
                return res = "get error";
            }
        }

        class data
        {
            public static string truedatag;
            public static string volt;
            public static string ver;
            public static string flash;
            public static string writer;
            public static string SN;
            public static string IMEI;
            public static string MODEM;
            public static string SIM;
            public static string ICCID;
            public static string IMSI;
            public static string ip;
            public static string database;
            public static string username;
            public static string password;
            public static string port;
            public static string tablename;
            public static string connstr;
            public static string post;
        }

        public void post_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                string datav = serialPort1.ReadLine() + "\r\n";
                string dataq = datav.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                string[] t_data = dataq.Split(':');
                if (datav.IndexOf("$VER:RSP") == 0)
                {
                    data.ver = t_data[2];
                }
                else if (datav.IndexOf("$FLASH:j") == 0)
                {
                    data.flash = t_data[1];
                    Invoke((new Action(() =>
                    {
                        SendMsgEvent(this, new MyEventArg() { Flash = data.flash });
                    })));
                }
                else if (datav.IndexOf("$FLASH:OK") == 0)
                {
                    data.writer = t_data[1];
                }
                else if (datav.IndexOf("$LTE:SN") == 0)
                {
                    data.SN = t_data[2];
                    Invoke((new Action(() =>
                    {
                        SendMsgEvent(this, new MyEventArg() { Ver = data.ver, SN = data.SN });
                    })));
                }
                else if (datav.IndexOf("$GPIO") == 0)
                {
                    data.truedatag = t_data[1].Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                    Invoke((new Action(() =>
                    {
                        SendMsgEvent(this, new MyEventArg() { Text = this.label1.Text });
                        label1.Text = data.truedatag;
                    })));
                }
                else if (datav.IndexOf("$ADC:vo") == 0)
                {
                    data.volt = t_data[1];
                    Invoke((new Action(() =>
                    {
                        SendMsgEvent(this, new MyEventArg() { Battery = data.volt });
                    })));
                }
                else if (datav.IndexOf("$LTE:IMEI") == 0)
                {
                    data.IMEI = t_data[2];
                    Invoke((new Action(() =>
                    {
                        SendMsgEventBat(this, new MyEventArg() { MODEM = data.MODEM, IMEI = data.IMEI, SIM = data.SIM, ICCID = data.ICCID, IMSI = data.IMSI });
                    })));
                }
                else if (datav.IndexOf("$LTE:MODEM") == 0)
                {
                    data.MODEM = t_data[2];
                }
                else if (datav.IndexOf("$LTE:SIM") == 0)
                {
                    data.SIM = t_data[2];
                }
                else if (datav.IndexOf("$LTE:ICCID") == 0)
                {
                    data.ICCID = t_data[2];
                }
                else if (datav.IndexOf("$LTE:IMSI") == 0)
                {
                    data.IMSI = t_data[2];
                }
                Invoke((new Action(() => //C# 3.0以后代替委托的新方法
                {
                    textBox1.AppendText("---" + time + "---" + "\r\n");
                    textBox1.AppendText(datav);//对话框追加显示数据
                    label2.Text = data.SN;
                })));
            }
            catch
            {
                Invoke((new Action(() => //C# 3.0以后代替委托的新方法
                {
                    textBox1.AppendText("获取数据异常");//对话框追加显示数据
                })));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        { 
        
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            DeviceInfo deviceInfo = new DeviceInfo
            {
                Owner = this
            };
            SendMsgEvent += deviceInfo.MainFormTxtChaned;
            deviceInfo.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serialPort1.Write("$MUSIC:START\r\n");
            Thread.Sleep(1000);
            Music music = new Music
            {
                Owner = this
            };
            music.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            serialPort1.Write("$PKE:START\r\n");
            Thread.Sleep(1000);
            PKE pKE = new PKE
            {
                Owner = this
            };
            pKE.ShowDialog();
        }

        public event EventHandler SendMsgEvent;
        public event EventHandler SendMsgEventBat;
        private void button6_Click(object sender, EventArgs e)
        {
            serialPort1.Write("$GPIO:INPUT\r\n");
            GpioInput gpioInput = new GpioInput
            {
                Owner = this
            };
            SendMsgEvent += gpioInput.MainFormTxtChaned;
            gpioInput.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            serialPort1.Write("$ADC:VOLTAGE\r\n\r\n");
            Battery battery = new Battery
            {
                Owner = this
            };
            SendMsgEvent += battery.MainFormTxtChaned;
            battery.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Module module = new Module
            {
                Owner = this
            };
            SendMsgEventBat += module.MainFormTxtChaned;
            module.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            GpioOuput gpioOuput = new GpioOuput
            {
                Owner = this
            };
            gpioOuput.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            button8.Enabled = false;
            progressBar1.Value = 1;
            serialPort1.Write("$FLASH:WRITE\r\n");
            Flash flash = new Flash()
            {
                Owner = this
            };
            SendMsgEvent += flash.MainFormTxtChaned;
            progressBar1.Visible = true;
            for (int x = 0; x <= 11; x++)
            {
                Thread.Sleep(1000);
                progressBar1.PerformStep();
            }
            if (data.writer == "OK")
            {
                flash.ShowDialog();
                progressBar1.Visible = false;
            }
            else
            {
                MessageBox.Show("写入音频数据失败", "Error");
                UpdataSQL(label2.Text, "Flash", "fail");
                button8.BackColor = Color.Red;
                progressBar1.Visible = false;
            }
            button8.Enabled = true;
        }
    }
}
