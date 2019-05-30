using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PdfToImage
{
    /// <summary>
    /// A simple Graphical interface for the library
    /// </summary>
    public partial class MainForm : Form
    {
        //This is the object that perform the real conversion!
        PDFConvert converter = new PDFConvert();
        #region Init
        public MainForm()
        {
            InitializeComponent();
            lblInfo.Text = string.Format("{0}:Waiting Orders", DateTime.Now.ToShortTimeString());
            this.Text += " " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        #endregion
        #region general GUI stuff (not interesting for what i have to do)
        /// <summary>When press browse if i choose something display it!</summary>
        /// <param name="sender"></param><param name="e"></param>
        private void FindFile(object sender, EventArgs e)
        {
            OpenFileDialog newDialog = new OpenFileDialog();
            newDialog.DefaultExt = "*.pdf";
            if (newDialog.ShowDialog() == DialogResult.OK)
            {
                txtSingleFile.Text = newDialog.FileName;
                radioSingleFile.Checked = true;
                lblInfo.Text = "";
            }
        }
        private void FindDirectory(object sender, EventArgs e)
        {
            FolderBrowserDialog newDialog = new FolderBrowserDialog();
            if (newDialog.ShowDialog() == DialogResult.OK)
            {
                txtDirectory.Text = newDialog.SelectedPath;
                radioDirectory.Checked = true;
                lblInfo.Text = "";
            }
        }
        /// <summary>Choose an appropiate extension for the file format</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormatChanged(object sender, EventArgs e)
        {
            switch (comboFormat.Text)
            {
                case "tifflzw":
                    txtExtension.Text = ".tif";
                    break;
                case "pnggray":
                    goto case "png16m";
                case "png16":
                    goto case "png16m";
                case "png256":
                    goto case "png16m";
                case "png16m":
                    txtExtension.Text = ".png";
                    break;
                case "jpeg":
                    txtExtension.Text = ".jpg";
                    break;
                default:
                    txtExtension.Text = "." + comboFormat.Text;
                    break;
            }
        }

        /// <summary>Try to avoid impossible first and last page parameters</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericValueChanged(object sender, EventArgs e)
        {
            if (numericLastPage.Value >= 0)
            {
                if ((numericFirstPage.Value >= 0) && (numericLastPage.Value < numericFirstPage.Value))
                {
                    if (numericFirstPage.Value != 1)
                        numericFirstPage.Value = numericFirstPage.Value - 1;
                    else
                        numericFirstPage.Value = -1;
                }
            }
        }
        #endregion

        #region Folder Monitoring
        /// <summary>A new PDF was created in the directory transform it!</summary>
        /// <param name="sender"></param><param name="e"></param>
        private void NewPDFCreated(object sender, System.IO.FileSystemEventArgs e)
        {
            System.IO.FileInfo input = new System.IO.FileInfo(e.FullPath);
            if (!input.Exists)
            {
                MessageBox.Show("The file \"{0}\" can't be founded", txtSingleFile.Text);
                return;
            }
            //You should do this on a separate thread
            //to be sure that the program don't freeze while converting
            //just for the sake of demontration i'm doing it here in this thread
            ConvertSingleImage(input.FullName);
            lblInfo.Text = string.Format("{0}:File converted! Continue to Monitor!",DateTime.Now.ToShortTimeString());
        }
        #endregion

        #region Button
        /// <summary>If is a single file convert it otherwise start the folder watcher</summary>
        /// <param name="sender"></param><param name="e"></param>
        private void ConvertToImage(object sender, EventArgs e)
        {
            #region Check if DLL is present
            //First check if the dll that i use is present!
            if (!System.IO.File.Exists(Application.StartupPath + "\\gsdll32.dll"))
            {
                lblDllInfo.Font = new Font(lblDllInfo.Font.FontFamily, 10, FontStyle.Bold);
                lblDllInfo.ForeColor = Color.Red;
                txtArguments.Text = "Download: http://mirror.cs.wisc.edu/pub/mirrors/ghost/GPL/gs863/gs863w32.exe";
                MessageBox.Show("The library 'gsdll32.dll' required to run this program is not present! download GhostScript and copy \"gsdll32.dll\" to this program directory");
                return;
            }
            //Ok now check what version is!
            GhostScriptRevision version = converter.GetRevision();
            lblVersion.Text = version.intRevision.ToString() + " " + version.intRevisionDate;
            #endregion
            if (radioSingleFile.Checked)
            {

                #region Check input of the user
                if (string.IsNullOrEmpty(txtSingleFile.Text))
                {
                    MessageBox.Show("Insert a filename!");
                    txtSingleFile.Focus();
                    return;
                }
                if (!File.Exists(txtSingleFile.Text))
                {
                    MessageBox.Show("The file \"{0}\" can't be founded", txtSingleFile.Text);
                    txtSingleFile.Focus();
                    return;
                }
                #endregion
                //Convert the file
                ConvertSingleImage(txtSingleFile.Text);
            }
            else
            {
                //If disabled check the parameter
                #region Check Input
                if (!fileSystemWatcher.EnableRaisingEvents)
                {
                    if ((txtDirectory.TextLength <= 0) || (System.IO.Directory.Exists(txtDirectory.Text)))
                    {
                        MessageBox.Show(string.Format("The directory '{0}' doesn't exist", txtDirectory.Text));
                        txtDirectory.Focus();
                        return;
                    }
                    fileSystemWatcher.Path = txtDirectory.Text;
                }
                #endregion
                //Start to monitoring or stop it!
                fileSystemWatcher.EnableRaisingEvents = !fileSystemWatcher.EnableRaisingEvents;
                if (fileSystemWatcher.EnableRaisingEvents)
                    lblInfo.Text = "I'm monitoring the directory";
                else
                    lblInfo.Text = "I stop monitorig the directory";
            }
        }
        #endregion
        /// <summary>Convert a single file</summary>
        /// <remarks>this function PRETEND that the filename is right!</remarks>
        private void ConvertSingleImage(string filename)
        {
            bool Converted = false;
            //Setup the converter
            if (numericThreads.Value > 0)
                converter.RenderingThreads = (int)numericThreads.Value;
            if (((int)numericTextSampling.Value > 0) && ((int)numericTextSampling.Value != 3))
                converter.TextAlphaBit = (int)numericTextSampling.Value;
            if (((int)numericGraphSampling.Value > 0) && ((int)numericGraphSampling.Value != 3))
                converter.TextAlphaBit = (int)numericGraphSampling.Value;
            converter.OutputToMultipleFile = checkSingleFile.Checked;
            converter.FirstPageToConvert = (int)numericFirstPage.Value;
            converter.LastPageToConvert = (int)numericLastPage.Value;
            converter.FitPage = checkFitTopage.Checked;
            converter.JPEGQuality = (int)numQuality.Value;
            converter.OutputFormat = comboFormat.Text;
            System.IO.FileInfo input = new FileInfo(filename);
            string output = string.Format("{0}\\{1}{2}",input.Directory,input.Name,txtExtension.Text);
            //If the output file exist alrady be sure to add a random name at the end until is unique!
            while (File.Exists(output))
            {
                output = output.Replace(txtExtension.Text, string.Format("{1}{0}", txtExtension.Text,DateTime.Now.Ticks));
            }
            //Just avoid this code, isn't working yet
            //if (checkRedirect.Checked)
            //{
            //    Image newImage = converter.Convert(input.FullName);
            //    Converted = (newImage != null);
            //    if (Converted)
            //        pictureOutput.Image = newImage;
            //}
            //else 
                Converted = converter.Convert(input.FullName, output);
            txtArguments.Text = converter.ParametersUsed;
            if (Converted)
            {
                lblInfo.Text = string.Format("{0}:File converted!", DateTime.Now.ToShortTimeString());
                txtArguments.ForeColor = Color.Black;
            }
            else
            {
                lblInfo.Text = string.Format("{0}:File NOT converted! Check Args!", DateTime.Now.ToShortTimeString());
                txtArguments.ForeColor = Color.Red;
            }
        }

        private void numQuality_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}