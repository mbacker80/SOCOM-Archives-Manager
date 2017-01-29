using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace SOCOM_Archives
{
    public partial class Form1 : Form
    {
        private CA_Archive MyArchive = new CA_Archive();
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dlgRet;

            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "*.ZDB; *.ZAR|*.zdb;*.zar";
            dlgRet = openFileDialog1.ShowDialog();
            if (dlgRet.ToString() == "Cancel") { return; }

            int filesLoaded = MyArchive.OpenArchive(openFileDialog1.FileName);
            if (filesLoaded <= 0)
            {
                MessageBox.Show("Not a valid archive");
                return;
            }
            txtArchive.Text = openFileDialog1.FileName;

            string vBlF = Convert.ToChar(0x0a).ToString();
            string vBCrLf = Convert.ToChar(0x0d).ToString() + Convert.ToChar(0x0a).ToString();
            lstListings.Items.Clear();

            txtBrowsePath.Text = "z:/";
            string[] sp = MyArchive.GetDIRListing(txtBrowsePath.Text).Split(Convert.ToChar(0x0a));

            lblFileCount.Text = "File Count: " + MyArchive.GetFileCount().ToString();
            for (int i = 0; i < sp.Length-1; i++)
            {
                lstListings.Items.Add(sp[i]);
            }
            switch (MyArchive.GetArchiveType())
            {
                case 0:
                    optTypeC8.Checked = false;
                    optTypeFC.Checked = false;
                    break;
                case 1:
                    optTypeC8.Checked = true;
                    optTypeFC.Checked = false;
                    break;
                case 2:
                    optTypeC8.Checked = false;
                    optTypeFC.Checked = true;
                    break;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstListings.SelectedIndex < 0) { return; }

            if (lstListings.Items[lstListings.SelectedIndex].ToString().Substring(0,6) == "[FILE]")
            {
                string[] spc = lstListings.Items[lstListings.SelectedIndex].ToString().Split(' ');
                int i = Convert.ToInt32(spc[1], 16);
                txtDetails.Text = MyArchive.GetFileDetails(i);
            }
        }
        private void lstListings_DoubleClick(object sender, EventArgs e)
        {
            if (lstListings.SelectedIndex < 0) { return; }
            if (lstListings.Items[lstListings.SelectedIndex].ToString().Substring(0, 5) == "[DIR]")
            {
                string[] spc = lstListings.Items[lstListings.SelectedIndex].ToString().Split(' ');
                if (spc[1] == ".")
                {
                    //txtBrowsePath.Text
                }
                else if (spc[1] == "..")
                {
                    string tmp = txtBrowsePath.Text + "/";
                    tmp = tmp.Replace("//", "/");
                    spc = tmp.Split('/');
                    tmp = spc[0] + "/";
                    for (int i = 1; i < spc.Length - 2; i++)
                    {
                        tmp += spc[i] + "/";
                    }
                    txtBrowsePath.Text = tmp;
                }
                else
                {
                    txtBrowsePath.Text += spc[1] + "/";
                }

                lstListings.Items.Clear();
                spc = MyArchive.GetDIRListing(txtBrowsePath.Text).Split(Convert.ToChar(0x0a));
                for (int i = 0; i < spc.Length - 1; i++)
                {
                    lstListings.Items.Add(spc[i]);
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (lstListings.SelectedIndex < 0) { return; }

            string[] spc = lstListings.Items[lstListings.SelectedIndex].ToString().Split(' ');
            int fIndex = Convert.ToInt32(spc[1], 16);

            if (spc[0] != "[FILE]") { return; }

            DialogResult dlgRet;

            saveFileDialog1.FileName = spc[spc.Length-1];
            saveFileDialog1.Filter = "*.*|*.*";
            dlgRet = saveFileDialog1.ShowDialog();
            if (dlgRet.ToString() == "Cancel") { return; }
            
            if (MyArchive.ExtractFile(saveFileDialog1.FileName, fIndex))
            {
                MessageBox.Show("Extracted Successfully!");
            }
            else
            {
                MessageBox.Show("Extract Failed!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (txtBrowsePath.Text.Length > 1)
            {
                frmNewFolder nfolder = new frmNewFolder();
                DialogResult dlgRet = nfolder.ShowDialog();

                MyArchive.AddFolder(txtBrowsePath.Text + nfolder.FolderName);
                nfolder.Dispose();

                lstListings.Items.Clear();
                string[] spc;
                spc = MyArchive.GetDIRListing(txtBrowsePath.Text).Split(Convert.ToChar(0x0a));
                for (int i = 0; i < spc.Length - 1; i++)
                {
                    lstListings.Items.Add(spc[i]);
                }
                lblFileCount.Text = "File Count: " + MyArchive.GetFileCount().ToString();
            }
        }

        private void optTypeFC_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] startupArgs = Environment.GetCommandLineArgs();
            
            if (startupArgs.Length > 1)
            {
                int filesLoaded = MyArchive.OpenArchive(startupArgs[1]);
                if (filesLoaded <= 0)
                {
                    MessageBox.Show("Not a valid archive");
                    return;
                }
                txtArchive.Text = startupArgs[1];

                string vBlF = Convert.ToChar(0x0a).ToString();
                string vBCrLf = Convert.ToChar(0x0d).ToString() + Convert.ToChar(0x0a).ToString();
                lstListings.Items.Clear();

                txtBrowsePath.Text = "z:/";
                string[] sp = MyArchive.GetDIRListing(txtBrowsePath.Text).Split(Convert.ToChar(0x0a));

                lblFileCount.Text = "File Count: " + MyArchive.GetFileCount().ToString();
                for (int i = 0; i < sp.Length - 1; i++)
                {
                    lstListings.Items.Add(sp[i]);
                }
                switch (MyArchive.GetArchiveType())
                {
                    case 0:
                        optTypeC8.Checked = false;
                        optTypeFC.Checked = false;
                        break;
                    case 1:
                        optTypeC8.Checked = true;
                        optTypeFC.Checked = false;
                        break;
                    case 2:
                        optTypeC8.Checked = false;
                        optTypeFC.Checked = true;
                        break;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (lstListings.SelectedIndex < 0) { return; }

            string lstName = lstListings.Items[lstListings.SelectedIndex].ToString();
            if (lstName.Substring(0,6) != "[FILE]") { return; }
            string fname = lstName.Substring(12, lstName.Length - 12);
            
            MyArchive.DeleteFile(txtBrowsePath.Text + fname, fname);

            lstListings.Items.Clear();
            string[] spc;
            spc = MyArchive.GetDIRListing(txtBrowsePath.Text).Split(Convert.ToChar(0x0a));
            for (int i = 0; i < spc.Length - 1; i++)
            {
                lstListings.Items.Add(spc[i]);
            }
            lblFileCount.Text = "File Count: " + MyArchive.GetFileCount().ToString();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult dlgResult;

            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "All Files *.*|*.*";

            dlgResult = openFileDialog1.ShowDialog();
            if (dlgResult.ToString() == "Cancel") { return; }

            string[] nameSplit = openFileDialog1.FileName.Split('\\');
            string fName = nameSplit[nameSplit.Length - 1];

            MyArchive.AddFile(openFileDialog1.FileName, txtBrowsePath.Text + fName, 0);

            lstListings.Items.Clear();
            string[] spc;
            spc = MyArchive.GetDIRListing(txtBrowsePath.Text).Split(Convert.ToChar(0x0a));
            for (int i = 0; i < spc.Length - 1; i++)
            {
                lstListings.Items.Add(spc[i]);
            }
            lblFileCount.Text = "File Count: " + MyArchive.GetFileCount().ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult dlgResult;

            saveFileDialog1.FileName = "MyArchive";
            saveFileDialog1.Filter = ".ZDB;.ZAR|*.zdb;*.zar";
            dlgResult = saveFileDialog1.ShowDialog();
            if (dlgResult.ToString() == "Cancel") { return; }

            MyArchive.SaveArchive(saveFileDialog1.FileName);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            MyArchive.FixAllChecksums();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (optTypeC8.Checked)
                MyArchive.NewArchive(1);
            else if (optTypeFC.Checked)
                MyArchive.NewArchive(2);

            txtBrowsePath.Text = "z:/";
            txtArchive.Text = "<New Archive>";

            lstListings.Items.Clear();
            string[] spc;
            spc = MyArchive.GetDIRListing(txtBrowsePath.Text).Split(Convert.ToChar(0x0a));
            for (int i = 0; i < spc.Length - 1; i++)
            {
                lstListings.Items.Add(spc[i]);
            }
            lblFileCount.Text = "File Count: " + MyArchive.GetFileCount().ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string dump = "C:\\ISO\\Dumps\\CA\\Patch Load\\Take 2\\Dump.bin";
            byte[] raw = System.IO.File.ReadAllBytes(dump);
            string[] fileNames =
            {
                "S3CORE", "RTBASEv1.4", "RTCERTv1.4", "RTCOMMv1.4", "RTCRYPTv1.4", "RTINETCv1.4",
                "RTMEDIAv1.4", "RTMEDIASv1.4", "RTMCLv1.4", "RTMGCLv1.4", "RTMSGCLv1.4", "RTOBJECTv1.4",
                "RTP2Pv1.4", "RTSSLv1.4", "INETCV6v1.4"
            };

            int Table, Entry, Size, FullSize;
            Table = 0x000804c4;
            for (int i = 0; i < fileNames.Length; i++)
            {
                Entry = BitConverter.ToInt32(raw, Table);
                Table += 4;
                Size = BitConverter.ToInt32(raw, Table);
                Table += 4;
                FullSize = BitConverter.ToInt32(raw, Table);
                Table += 4;

                if (i == 0)
                {
                    byte[] fData = new byte[Size];
                    for (int i2 = 0; i2 < Size; i2++)
                    {
                        fData[i2] = raw[Entry];
                        Entry++;
                    }
                    string fSave = "C:\\ISO\\Dumps\\CA\\Patch Load\\Take 2\\Decrypted\\" + fileNames[i];
                    //System.IO.File.WriteAllBytes(fSave, fData);
                    System.IO.BinaryWriter bw = new System.IO.BinaryWriter(System.IO.File.Open(fSave, System.IO.FileMode.Create));
                    bw.Write(fData);
                    int tmpSize = Size;
                    while ((tmpSize & 3) != 0)
                    {
                        bw.Write(Convert.ToByte(0));
                        tmpSize++;
                    }
                    bw.Write(Size);
                    bw.Close();
                }
                else
                {
                    int entry2 = (Entry + FullSize);
                    do
                    {
                        entry2 -= 4;
                        FullSize -= 4;
                    } while (BitConverter.ToInt32(raw, entry2) != 0);

                    byte[] fData = new byte[FullSize];
                    for (int i2 = 0; i2 < FullSize; i2++)
                    {
                        fData[i2] = raw[Entry];
                        Entry++;
                    }

                    string fSave = "C:\\ISO\\Dumps\\CA\\Patch Load\\Take 2\\Decrypted\\" + fileNames[i];
                    System.IO.BinaryWriter bw = new System.IO.BinaryWriter(System.IO.File.Open(fSave, System.IO.FileMode.Create));
                    bw.Write(fData);
                    bw.Write(Size);
                    bw.Close();
                }
            }
            MessageBox.Show("Done.");

            MyArchive.NewArchive(2);

            for (int i = 0; i < fileNames.Length; i++)
            {
                string fAdd = "C:\\ISO\\Dumps\\CA\\Patch Load\\Take 2\\Decrypted\\" + fileNames[i];
                MyArchive.AddFile(fAdd, "z:/" + fileNames[i], 0);
            }

            txtBrowsePath.Text = "z:/";
            lstListings.Items.Clear();
            string[] spc;
            spc = MyArchive.GetDIRListing(txtBrowsePath.Text).Split(Convert.ToChar(0x0a));
            for (int i = 0; i < spc.Length - 1; i++)
            {
                lstListings.Items.Add(spc[i]);
            }
            lblFileCount.Text = "File Count: " + MyArchive.GetFileCount().ToString();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (lstListings.SelectedIndex < 0) { return; }

            string[] spc = lstListings.Items[lstListings.SelectedIndex].ToString().Split(' ');
            int fIndex = Convert.ToInt32(spc[1], 16);

            if (spc[0] != "[FILE]") { return; }

            byte[] compressed;
            MyArchive.GetFileBytes(fIndex, out compressed);

            byte[] uncompressed = new byte[0];
            
            try
            {
                DecompressData(compressed, out uncompressed);
            }
            catch
            {
                MessageBox.Show("Decompression Error");
                return;
            }
            
            MessageBox.Show("Decompressed to " + uncompressed.Length.ToString() + " bytes");

            MyArchive.SetFileBytes(fIndex, ref uncompressed);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (lstListings.SelectedIndex < 0) { return; }

            string[] spc = lstListings.Items[lstListings.SelectedIndex].ToString().Split(' ');
            int fIndex = Convert.ToInt32(spc[1], 16);

            if (spc[0] != "[FILE]") { return; }

            byte[] uncompressed;
            MyArchive.GetFileBytes(fIndex, out uncompressed);


            byte[] compressed = new byte[0];

            try
            {
                CompressData(uncompressed, out compressed);
            }
            catch
            {
                MessageBox.Show("Compression Error");
                return;
            }

            MessageBox.Show("Compressed to " + compressed.Length.ToString() + " bytes");

            MyArchive.SetFileBytes(fIndex, ref compressed);
        }



        //---------------------------------------------------------------------------------
        // http://stackoverflow.com/questions/6620655/compression-and-decompression-problem-with-zlib-net
        //---------------------------------------------------------------------------------

        public static void CompressData(byte[] inData, out byte[] outData)
        {
            using (MemoryStream outMemoryStream = new MemoryStream())
            using (zlib.ZOutputStream outZStream = new zlib.ZOutputStream(outMemoryStream, zlib.zlibConst.Z_DEFAULT_COMPRESSION))
            using (Stream inMemoryStream = new MemoryStream(inData))
            {
                CopyStream(inMemoryStream, outZStream);
                outZStream.finish();
                outData = outMemoryStream.ToArray();
            }
        }

        public static void DecompressData(byte[] inData, out byte[] outData)
        {
            using (MemoryStream outMemoryStream = new MemoryStream())
            using (zlib.ZOutputStream outZStream = new zlib.ZOutputStream(outMemoryStream))
            using (Stream inMemoryStream = new MemoryStream(inData))
            {
                CopyStream(inMemoryStream, outZStream);
                outZStream.finish();
                outData = outMemoryStream.ToArray();
            }
        }

        public static void CopyStream(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] buffer = new byte[2000];
            int len;
            while ((len = input.Read(buffer, 0, 2000)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Flush();
        }

    }
}
