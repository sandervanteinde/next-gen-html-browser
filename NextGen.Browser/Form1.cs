using NextGen.HTMLParser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NextGen.Browser
{
    public partial class Form1 : Form
    {
        const string BROWSER_NAME = "Awesome Browser0";
        public Form1()
        {
            InitializeComponent();
            Text = BROWSER_NAME;
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                string text = textBox1.Text;
                if(text.StartsWith("wwww") || text.StartsWith("http"))
                    HandleWebRequest(text);
                else
                    HandleFile(text);

            }
        }
        private void HandleFile(string file)
        {
            if(File.Exists(file))
            {
                var stream = File.OpenRead(file);
                HTMLDocument document = new HTMLDocument(stream);
                document.Parse();
                SetDocument(document, file);
            }
        }
        private void SetDocument(HTMLDocument document, string location)
        {
            browserPanel1.SetDocument(document, location);
            var titleEl = document.HeadElement.FindFirst("title");
            if (titleEl != null)
                Text = $"{titleEl.Content} - {BROWSER_NAME}";
            else
                Text = BROWSER_NAME;
        }
        private void HandleWebRequest(string url)
        {

        }
    }
}
