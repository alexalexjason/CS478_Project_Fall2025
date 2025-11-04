using System;
using System.Windows.Forms;

namespace VoiceToText
{
    public partial class Form1 : Form
    {
        private readonly VoiceRecognizer _recognizer;

        public Form1() // Basic throwaway UI for testing purposes 
        {
            InitializeComponent();

            _recognizer = new VoiceRecognizer();

            _recognizer.SpeechRecognized += (s, text) =>
            {
                conversionDisplayBox.Text += text + Environment.NewLine;
            };
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            try
            {
                _recognizer.Start();
                conversionDisplayBox.Text = "Listening...";
            }
            catch (Exception ex)
            {
                conversionDisplayBox.Text = "Error: " + ex.Message;
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            _recognizer.Stop();
            conversionDisplayBox.Text += Environment.NewLine + "[Stopped]";
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            conversionDisplayBox.Text = _recognizer.UploadAndRecognize();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _recognizer.Dispose();
            base.OnFormClosing(e);
        }
    }
}
