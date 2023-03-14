using Microsoft.VisualStudio.OLE.Interop;
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
    public partial class PromptForm : Form
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public PromptForm()
        {
            InitializeComponent();
        }

        private void PromptForm_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Put valid name", "Snippet", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            Name = txtName.Text;
            Description = txtDescription.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
