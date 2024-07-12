using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ECC_DigitalSignature
{
    public partial class Form1 : Form
    {
        // Import DLL functions
        [DllImport("D:\\C#\\Lab5\\x64\\Debug\\Dll1.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "signFile")]
        public static extern bool signFile(
            [MarshalAs(UnmanagedType.LPStr)] string privateKeyPath,
            [MarshalAs(UnmanagedType.LPStr)] string filePath,
            [MarshalAs(UnmanagedType.LPStr)] string signaturePath);

        [DllImport("D:\\C#\\Lab5\\x64\\Debug\\Dll1.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "verifySignature")]
        public static extern bool verifySignature(
            [MarshalAs(UnmanagedType.LPStr)] string publicKeyPath,
            [MarshalAs(UnmanagedType.LPStr)] string filePath,
            [MarshalAs(UnmanagedType.LPStr)] string signaturePath);

        private string keyPath = string.Empty;
        private string filePath = string.Empty;
        private string signaturePath = string.Empty;
        private string signature = "../../../signature.bin";

        public Form1()
        {
            InitializeComponent();
            label2.Visible = false;
            textBox2.Visible = false;
            button2.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "../../",
                Filter = "All files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                keyPath = openFileDialog.FileName;
                textBox1.Text = keyPath;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "../../",
                Filter = "All files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                signaturePath = openFileDialog.FileName;
                textBox2.Text = keyPath;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "../../",
                Filter = "All files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
                textBox3.Text = filePath;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = ""; 
            textBox3.Text = "";
            keyPath = "";
            signaturePath = "";
            filePath = "";
            if (checkBox1.Checked)
            {
                checkBox2.Checked = false;
                label1.Text = "Private key";
                button4.Text = "Sign";
                label2.Visible = false;
                textBox2.Visible = false;
                button2.Visible = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox3.Text = "";
            keyPath = "";
            signaturePath = "";
            filePath = "";
            if (checkBox2.Checked)
            {
                checkBox1.Checked = false;
                label1.Text = "Public key";
                button4.Text = "Verify";
                label2.Visible = true;
                textBox2.Visible = true;
                button2.Visible = true;
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            try
            {
                richTextBox1.Clear();

                if (button4.Text == "Sign")
                {
                    if (File.Exists(keyPath) && File.Exists(filePath))
                    {
                        bool success = signFile(keyPath, filePath, signature);
                        if (success)
                        {
                            richTextBox1.AppendText("File successfully signed.\n");
                            richTextBox1.AppendText("The signature has been saved at the "+ signature + "\n");
                        }
                        else
                        {
                            richTextBox1.AppendText("Failed to sign the PDF");
                        }
                    }
                    else
                    {
                        richTextBox1.AppendText("One or more files do not exist.\n");
                    }
                }


                if (button4.Text == "Verify")
                {
                    bool success = verifySignature(keyPath, filePath, signaturePath);
                    if (success)
                    {
                        richTextBox1.AppendText("File verification was successful.\n");
                    }
                    else
                    {
                        richTextBox1.AppendText("File verification failed.\n");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Get the current working directory
            string currentDirectory = Environment.CurrentDirectory;

            textBox4.Text = currentDirectory;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string testFilesFolder = "D:\\C#\\Lab5\\test_file"; // Adjust to your actual folder path
            int averageRuns = 1000;

            StringBuilder results = new StringBuilder();

            try
            {
                // Iterate through all files in the test_file folder
                string[] files = Directory.GetFiles(testFilesFolder);
                foreach (string file in files)
                {
                    // Measure sign operation performance
                    double signAverageTime = MeasureExecutionTime(() =>
                    {
                        for (int i = 0; i < averageRuns; i++)
                        {
                            string signaturePath = file.Split('.')[0] + ".bin";
                            signFile("D:\\C#\\Lab5\\private-key.pem", file, signaturePath);
                        }
                    });

                    // Measure verify operation performance
                    double verifyAverageTime = MeasureExecutionTime(() =>
                    {
                        for (int i = 0; i < averageRuns; i++)
                        {
                            string signaturePath = file.Split('.')[0] + ".bin";
                            verifySignature("D:\\C#\\Lab5\\public-key.pem", file, signaturePath);
                        }
                    });

                    // Append results for the current file
                    results.AppendLine($"File: {Path.GetFileName(file)}");
                    results.AppendLine($"Average Sign Time: {signAverageTime:F2} ms");
                    results.AppendLine($"Average Verify Time: {verifyAverageTime:F2} ms");
                    results.AppendLine();
                }

                // Display results in RichTextBox
                richTextBox1.Text = results.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        // Function to measure execution time
        private double MeasureExecutionTime(Action action)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            action.Invoke();
            stopwatch.Stop();
            return (double)stopwatch.ElapsedMilliseconds / 1000.0; 
        }
    }
}
