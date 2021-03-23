using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFrmDemo
{
    public class MyEventArg : EventArgs
    {
        //传递主窗体的数据信息
        public string Text { get; set; }
        public string Ver { get; set; }
        public string Flash { get; set; }
        public string  SN { get; set; }
        public string IMEI { get; set; }
        public string  MODEM { get; set; }
        public string SIM { get; set; }
        public string ICCID { get; set; }
        public string IMSI { get; set; }
        public string Battery { get; set; }
        public string Slope { get; set; }
        public string Can { get; set; }
        public string Gettime { get; set; }
    }
}
