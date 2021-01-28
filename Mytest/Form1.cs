using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mytest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You selected option 1");
        }

        public void Undo()
        {
            textBox1.Undo();
        }

        public void Cut()
        {
            textBox1.Cut();
        }
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paste();
        }

        public void Paste()
        {
            textBox1.Paste();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(textBox1.Text);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void standToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleStandardToolbar();
        }

        public void ToggleStandardToolbar()
        {
            standToolStripMenuItemVisible = !(standToolStripMenuItemVisible);
        }

        public bool standToolStripMenuItemVisible
        {
            get

            {
                return toolStrip1.Visible;
            }

            set

            {
                toolStrip1.Visible = value;
                standToolStripMenuItem.Checked = value;
            }
        }

        private void optionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleOptionToolbar();
        }

        public void ToggleOptionToolbar()
        {
            optionToolStripMenuItemVisible = !(optionToolStripMenuItemVisible);
        }

        public bool optionToolStripMenuItemVisible
        {
            get
            {
                return toolStrip2.Visible;
            }

            set
            {
                toolStrip2.Visible = value;
                optionToolStripMenuItem.Checked = value;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Connstr = "server=192.168.1.252;user=gts;database=gts;port=3306;password=123456";
            MySqlConnection conn = new MySqlConnection(Connstr);
            try
            {
                conn.Open();
                string sql = "select * from DaYang_Test202101;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    int index = this.dataGridView1.Rows.Add();

                    this.dataGridView1.Rows[index].Cells[0].Value = reader.GetString("sn");
                    this.dataGridView1.Rows[index].Cells[1].Value = reader.GetString("IMEI");
                    this.dataGridView1.Rows[index].Cells[2].Value = reader.GetString("ICCID");
                    this.dataGridView1.Rows[index].Cells[3].Value = reader.GetString("VER");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
