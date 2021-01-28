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
    public partial class PKE : Form
    {
        public PKE()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            main.button3.BackColor = Color.Green;
            main.serialPort1.Write("$PKE:STOP\r\n");
            if (main.label2.Text.Length > 10 && main.label3.Text == "YES")
            {
                main.UpdataSQL(main.label2.Text, "PKE", "pass");
            }
            this.Close();
        }

        private void PKE_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Main main = (Main)this.Owner;
            main.button3.BackColor = Color.Red;
            main.serialPort1.Write("$PKE:STOP\r\n");
            if (main.label2.Text.Length > 10 && main.label3.Text == "YES")
            {
                main.UpdataSQL(main.label2.Text, "PKE", "fail");
            }
            this.Close();
        }
    }
}
