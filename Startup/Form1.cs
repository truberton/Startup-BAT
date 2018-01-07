using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Startup
{
    public partial class Lisamine : Form
    {
        public string BATPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Startup.bat");

        public Lisamine()
        {
            InitializeComponent();
            string[] lines = System.IO.File.ReadAllLines(BATPath);
            foreach (string line in lines)
            {
                string item = line.Split('"')[2];
                item = item.Replace(" ", "");
                comboBox1.Items.Add(item);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "EXE files (*.exe)|*.exe|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            comboBox1.Items.Add(Path.GetFileName(file));
                            using (StreamWriter sw = File.AppendText(BATPath))
                            {
                                sw.WriteLine("start /d \"" + Path.GetDirectoryName(file) + "\" " + Path.GetFileName(file));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //See on lihtsalt copy paste
            var oldLines = System.IO.File.ReadAllLines(BATPath);
            var newLines = oldLines.Where(line => !line.Contains(comboBox1.SelectedItem.ToString()));
            System.IO.File.WriteAllLines(BATPath, newLines);

            comboBox1.Items.Remove(comboBox1.SelectedItem);
        }
    }
}
