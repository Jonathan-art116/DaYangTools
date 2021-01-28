using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor
{
    public class TextDocumentView
    {
        private void TextDocumentView_Load(object sender, System.EventArgs e)
        {
            DocumentManager.Current.RegisterDocumentView(this);
        }
    }
}
