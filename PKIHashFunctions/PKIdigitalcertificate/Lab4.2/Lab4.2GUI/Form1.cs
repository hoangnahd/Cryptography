using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Lab4._2GUI
{
    public partial class Form1 : Form
    {
        // Import the verify_certificate function from Lab4.2DLL.dll
        [DllImport("D:\\C#\\Lab4\\Lab4.2\\Lab4.2\\x64\\Debug\\Lab4.2DLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "verify_certificate")]
        public static extern bool verify_certificate(
            [MarshalAs(UnmanagedType.LPStr)] string cert_file,
            bool isPem
        );

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Open file dialog to select a .pem or .der file
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "PEM Files (*.pem)|*.pem|DER Files (*.der)|*.der|All Files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.Multiselect = false;

                DialogResult result = openFileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string selectedFile = openFileDialog.FileName;

                    // Validate selected file extension
                    string extension = Path.GetExtension(selectedFile);
                    if (extension != ".pem" && extension != ".der")
                    {
                        MessageBox.Show("Please select a .pem or .der file.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Display selected file path in textBox1
                    textBox1.Text = selectedFile;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Check if the selected file is a .pem or .der file
            bool isPem = textBox1.Text.EndsWith(".pem", StringComparison.OrdinalIgnoreCase);

            // Call the DLL function to verify the certificate
            if (verify_certificate(textBox1.Text, isPem))
            {
                textBox2.Text = "Signature is valid";
                // Retrieve and use public key if needed
            }
            else
            {
                textBox2.Text = "Signature is not valid";
            }
        }
    }
}
