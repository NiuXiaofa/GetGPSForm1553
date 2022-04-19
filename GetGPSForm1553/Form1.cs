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

namespace GetGPSForm1553
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string path;
        public string readText;
        public int substringlength = 0;
        public int count = 0;
        public int proc_count = 0;
        public int total_count = 0;
        public string keystring = "5-T-22-0";

        private void button1_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            textBox1.Text = openFileDialog1.FileName;
                            path = System.IO.Path.GetDirectoryName(openFileDialog1.FileName);
                            StreamReader m_streamReader = new StreamReader(openFileDialog1.FileName, System.Text.Encoding.Default);

                            string filePath = openFileDialog1.FileName;

                            readText = File.ReadAllText(filePath);
                            total_count = readText.Length;
                            richTextBox1.Text ="数据文件加载完毕，共"+ total_count.ToString()+"字节！\n";
                            m_streamReader.Close();
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
            StreamWriter sw = new StreamWriter(path + "\\" + "提取结果" + System.DateTime.Now.Ticks.ToString() + ".txt", false, Encoding.Default);

            richTextBox1.Text += "开始提取PVT数据...\n";
            progressBar1.Maximum = total_count;
            string s_time;
            string s_pv;
            string s_px;
            string s_py;
            string s_pz;
            string s_vx;
            string s_vy;
            string s_vz;
            substringlength = readText.IndexOf(keystring);
            //string tmmss = "5-T-22-0     DATA 0655 C9B5 0039 FEFF 0617 3717 A5F5 0437 6425 3C31 82F0 82F6 FB01 5EEB 83FE B0C5 C2FB DEEE 7F00 0000 042D BA6C 0218 9ABE 22CD 78C0 04AE 0337 2101 8B7C 26BB 32C5      STA2 2800";

            //substringlength = tmmss.IndexOf(keystring);
            //s_pv = tmmss.Substring(substringlength + keystring.Length + 32, 10);
            //richTextBox1.Text += s_pv.ToString() + "\n";

            //CMD2 2EC0-->5-T-22-0     DATA 0655 C9B5 0039 FEFF 0617 3717 A5F5 0437 6425 3C31 82F0 82F6 FB01 5EEB 83FE B0C5 
            //                                           C2FB DEEE 7F00 0000 042D BA6C 0218 9ABE 22CD 78C0 04AE 0337 2101 8B7C 26BB 32C5      STA2 2800

            while (readText.Length != 0)
            {
                substringlength = readText.IndexOf(keystring);
                if (substringlength != -1)
                {
                    count++;
                    //s_time = readText.Substring(substringlength + keystring.Length + 184, 10);
                    //新
                    s_time = readText.Substring(substringlength + keystring.Length + 32, 10);
                    s_time = s_time.Replace(" ", "");
                    s_pv = readText.Substring(substringlength + keystring.Length + 42, 60);
                    //新
                    s_pv = s_pv.Replace(" ", "");
                    s_px = s_pv.Substring(0, 8);
                    s_py = s_pv.Substring(8, 8);
                    s_pz = s_pv.Substring(16, 8);
                    s_vx = s_pv.Substring(24, 8);
                    s_vy = s_pv.Substring(32, 8);
                    s_vz = s_pv.Substring(40, 8);
                    sw.WriteLine(s_time + "\t" + s_px + "\t" + s_py + "\t" + s_pz + "\t" + s_vx + "\t" + s_vy + "\t" + s_vz);
                    readText = readText.Remove(0, substringlength + keystring.Length - 1);
                    progressBar1.Value = total_count - readText.Length;
                }
                else
                    break;
            }
            progressBar1.Value = progressBar1.Maximum;
            sw.Flush();
            sw.Close();
            richTextBox1.Text += count.ToString();

            richTextBox1.Text += "数据提取完毕！";
        }
    }
}
