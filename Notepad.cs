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
//hä börjar det<!
namespace Notepad
{
    public partial class Notepad : Form
    {

        private string filename = null;
        private bool isUnsaved = false;
        private bool ignoreTextChangedEvent = false;

        public Notepad()
        {
            InitializeComponent();
            updateTitle();
        }

        private void updateTitle()
        {
            string file;
            if (string.IsNullOrEmpty(filename))
            {
                file = "Unnamed";
            }
            else
            
                file = Path.GetFileName(filename);

            if (isUnsaved)
            {
                Text = file + "* - Notepad";
            }
            else
                Text = file + " - Notepad";
        }

        private void saveFile()
        {
            if (string.IsNullOrEmpty(filename))
            {
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    filename = saveFileDialog.FileName;
                else
                    return;
            }

            File.WriteAllText(filename, textBox.Text);
            isUnsaved = false;
            updateTitle();
        }

        private void nyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var eventArgs = new FormClosingEventArgs(CloseReason.None, false);
            Notepad_FormClosing(null, eventArgs);

            if (eventArgs.Cancel)
            {
                return;
            }

            textBox.Text = String.Empty;
            filename = null;
            isUnsaved = false;
            updateTitle();
        }

        private void öppnaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var eventArgs = new FormClosingEventArgs(CloseReason.None, false);
            Notepad_FormClosing(null, eventArgs);

            if (eventArgs.Cancel)
            {
                return;
            }
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ignoreTextChangedEvent = true;
                textBox.Text = File.ReadAllText(openFileDialog.FileName);
                filename = openFileDialog.FileName;
                updateTitle();
            }
        }

        private void sparaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile();
        }
        private void stängToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (ignoreTextChangedEvent)
            {
                ignoreTextChangedEvent = false;
                return;
            }
            isUnsaved = true;
            updateTitle();
        }

        private void Notepad_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isUnsaved)
            {
               var dialogResult = MessageBox.Show(this,"Vill du spara?", "Notepad", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);

                if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                {
                    saveFile();
                }
                else if (dialogResult == System.Windows.Forms.DialogResult.No)
                {
                    //do nothing
                }
                else if (dialogResult == System.Windows.Forms.DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
