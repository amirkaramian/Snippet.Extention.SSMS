namespace Snippet.Extention
{
    partial class SnippetListForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lstSnippet = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lstSnippet
            // 
            this.lstSnippet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSnippet.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lstSnippet.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lstSnippet.FormattingEnabled = true;
            this.lstSnippet.ItemHeight = 20;
            this.lstSnippet.Location = new System.Drawing.Point(0, 0);
            this.lstSnippet.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.lstSnippet.Name = "lstSnippet";
            this.lstSnippet.Size = new System.Drawing.Size(391, 554);
            this.lstSnippet.TabIndex = 0;
            this.lstSnippet.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstSnippet_KeyDown);
            this.lstSnippet.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lstSnippet_KeyPress);
            this.lstSnippet.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstSnippet_MouseDoubleClick);
            // 
            // SnippetListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(391, 554);
            this.Controls.Add(this.lstSnippet);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SnippetListForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select custom snippet";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SnippetListForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SnippetListForm_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstSnippet;
    }
}