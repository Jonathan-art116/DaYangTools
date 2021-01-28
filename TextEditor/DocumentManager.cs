using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;


namespace TextEditor
{
    public class DocumentManager
    {
        //fields..
        private static DocumentManager _current;
        private ArrayList _documents = new ArrayList();

        public static DocumentManager Current
        {
            get
            {
                return _current;
            }
        }

        public ArrayList Documents
        {
            get
            {
                return _documents;
            }
        }

        [STAThread()] static void Main()
        {
            //Store the current manger...
            _current = new DocumentManager();

            //Create the first view...
            TextDocumentView view = new TextDocumentView();
            view.Show();

            //Run the application...
            Application.Run();
        }

        public void RegisterDocumentView(TextDocumentView view)
        {
            //Store the view in the list of views...
            Documents.Add(view);

            //Hook into the "closed" event...
            view.Closed += new EventHandler(ViewClosed);
        }

        private void ViewClosed(object sender, EventArgs e)
        {
            //Remove the sender from out document list...
            Documents.Remove(sender);

            //Have we closed the final window?
            if(Documents.Count == 0)
            {
                Application.Exit();
            }
        }
   
    }
}
