using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor
{
    public partial class TextDocumentView : Form
    {
        public TextDocumentView()
        {
            InitializeComponent();
        }

        private void TextDocumentView_Load(object sender, EventArgs e)
        {
            DocumentManager.Current.RegisterDocumentView(this);
        }

        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewWindow();
        }

        public void CreateNewWindow()
        {
            //create and show a new window..
            TextDocumentView view = new TextDocumentView();
            view.Show();
        }
    }
}
