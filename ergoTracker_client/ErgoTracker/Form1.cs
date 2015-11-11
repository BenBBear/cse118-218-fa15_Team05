using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using mshtml;

namespace ErgoTracker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void document_completed(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.Maxubi.Document.Body.MouseDown += new HtmlElementEventHandler(document_mouseDown);
        }

        private void document_mouseDown(object sender, HtmlElementEventArgs e)
        {

        }
    }
}
