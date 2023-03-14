using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snippet.Extention
{
    public partial class SnippetListForm : Form
    {
        public string CurrentSnippet { get; set; }
        public SnippetListForm()
        {
            InitializeComponent();
        }

        private void SnippetListForm_Load(object sender, EventArgs e)
        {
            lstSnippet.SelectedItem = "Item2";
            lstSnippet.ValueMember = "Item1";
            var snippets = Snippethelper.GetAllCustomeSnippets();
            snippets.ForEach((item) =>
           {
               lstSnippet.Items.Add(new Tuple<string, string>(item.Name.Replace(".snippet", ""), item.FullName));
           });
        }

        private void lstSnippet_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstSnippet.SelectedItem == null)
                return;
            backtoParent();
        }
        private void backtoParent()
        {
            CurrentSnippet = ((Tuple<string, string>)lstSnippet.SelectedItem).Item2;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void SnippetListForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void lstSnippet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void lstSnippet_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && lstSnippet.SelectedItem != null)
            {
                backtoParent();
            }
        }
    }
}
