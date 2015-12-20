using StorageSampleDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace azure_testing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        OpenFileDialog ofd = new OpenFileDialog();

        private void button1_Click(object sender, EventArgs e)
        {
            ofd.ShowDialog();
            MessageBox.Show(ofd.FileName);
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (ofd.FileName != String.Empty)
                {
                    BlobHelper bh = new BlobHelper(ConfigurationManager.ConnectionStrings["Storage"].ConnectionString);
                    //bh.PutBlob_File("fei", System.IO.Path.GetFileName(ofd.FileName), ofd.FileName);
                    //System.IO.FileStream  fs =System.IO.File.OpenRead(ofd.FileName);
                    byte[] file = System.IO.File.ReadAllBytes(ofd.FileName);
                    bh.PutBlob_bitarray("fei", System.IO.Path.GetFileName(ofd.FileName), file);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
