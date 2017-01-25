using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOCOM_Archives
{
    public partial class frmNewFolder : Form
    {
        public string FolderName;

        public frmNewFolder()
        {
            InitializeComponent();
        }

        private void txtFolder_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            char[] badChars = { '/', '\\', '?', '%', ':', '|', '<', '>', ' ' };
            
            if (txtFolder.Text.Length < 1) { MessageBox.Show("Invalid directory name!"); return; }

            char[] dirChars = txtFolder.Text.ToCharArray();
            for (int i = 0; i < dirChars.Length; i++)
            {
                for (int i2 = 0; i2 < badChars.Length; i2++)
                {
                    if (dirChars[i] == badChars[i2])
                    {
                        MessageBox.Show("Invalid directory name!");
                        return;
                    }
                }
            }

            FolderName = txtFolder.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
