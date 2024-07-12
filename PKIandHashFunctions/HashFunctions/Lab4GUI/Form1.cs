using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        [DllImport("D:\\C#\\Lab4\\Lab4.1\\x64\\Debug\\Lab4.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "hash_data")]
        public static extern IntPtr hash_data(
            [MarshalAs(UnmanagedType.LPStr)] string data, 
            int data_len,
            [MarshalAs(UnmanagedType.LPStr)] string algorithm, 
            int digest_length
        );

        [DllImport("D:\\C#\\Lab4\\Lab4.1\\x64\\Debug\\Lab4.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "read_file")]
        public static extern IntPtr read_file([MarshalAs(UnmanagedType.LPStr)] string file_path);

        private bool checkCondition()
        {
            string selectedItem = comboBox1.SelectedItem?.ToString(); // Use ?.ToString() to avoid null reference exception

            if ((selectedItem == "SHAKE128" || selectedItem == "SHAKE256") && !string.IsNullOrEmpty(textBox1.Text))
                return true;
            else if (!string.IsNullOrEmpty(selectedItem))
                return true;
            return false;
        }

        private string convert2hash()
        {
            try
            {
                int digest_length = string.IsNullOrEmpty(textBox1.Text) ? 0 : int.Parse(textBox1.Text);
                IntPtr resultPtr = hash_data(richTextBox2.Text, richTextBox2.Text.Length, comboBox1.SelectedItem.ToString(), digest_length);
                return Marshal.PtrToStringAnsi(resultPtr);
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = comboBox1.SelectedItem?.ToString();
            if(selectedItem == "SHAKE128" || selectedItem == "SHAKE256")
            {
                textBox1.Enabled = true;
                if (checkCondition())
                {
                    richTextBox1.Clear();
                    richTextBox1.AppendText(convert2hash());
                }
                    
            }
            else if(checkCondition())
            {
                textBox1.Enabled = false;
                richTextBox1.Clear();
                richTextBox1.AppendText(convert2hash());
            }
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            try
            {
                if (checkCondition())
                {
                    richTextBox1.AppendText(convert2hash());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Handle or log the exception appropriately
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            try
            {
                if (checkCondition())
                {
                   richTextBox1.AppendText(convert2hash());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // Handle or log the exception appropriately
            }
        }
        private double MeasureExecutionTime(Action action, int runs)
        {
            var stopwatch = new Stopwatch();
            double totalTime = 0;

            for (int i = 0; i < runs; i++)
            {
                stopwatch.Reset();
                stopwatch.Start();
                action.Invoke();
                stopwatch.Stop();
                totalTime += stopwatch.Elapsed.TotalMilliseconds;
            }

            return totalTime / runs; // Return average time
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string testFilesFolder = "D:\\C#\\Lab4\\test_files"; // Adjust to your actual folder path
            int averageRuns = 10;

            StringBuilder results = new StringBuilder();

            string[] algorithms = { "SHA224", "SHA256", "SHA384", "SHA512",
            "SHA3-224", "SHA3-256", "SHA3-384", "SHA3-512",
            "SHAKE128", "SHAKE256" };
            int[] digestLengths = { 0, 0, 0, 0, // SHA224, SHA256, SHA384, SHA512
            0, 0, 0, 0, // SHA3-224, SHA3-256, SHA3-384, SHA3-512
            128, 256 };
            try
            {
                // Iterate through all files in the test_file folder
                string[] files = Directory.GetFiles(testFilesFolder);
                foreach (string file in files)
                {
                    richTextBox2.AppendText($"File: {file}\n");
                    string fileContent = File.ReadAllText(file);
                    for (int i = 0; i < algorithms.Length; i++)
                    {
                        string algorithm = algorithms[i];
                        int digestLength = digestLengths[i];

                        // Measure hash operation performance
                        double hashAverageTime = MeasureExecutionTime(() =>
                        {
                            IntPtr hashResult = hash_data(fileContent, fileContent.Length, algorithm, digestLength);
                            // Ensure to free any allocated memory if necessary
                        }, averageRuns);

                        // Append results for the current file and algorithm 

                        richTextBox2.AppendText($"Hash Algorithm: {algorithm}\n");
                        richTextBox2.AppendText($"Average Hash Time: {hashAverageTime:F2} ms\n");
                        richTextBox2.AppendText("\n");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

    }
}
