namespace VoiceToText
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.TextBox finishedVoiceInputBox;
        private System.Windows.Forms.TextBox hypothesizedVoiceInputBox;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.finishedVoiceInputBox = new System.Windows.Forms.TextBox();
            this.hypothesizedVoiceInputBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonUpload = new System.Windows.Forms.Button();
            this.conversionDisplayBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(404, 61);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(147, 92);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Start Listening";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(404, 159);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(147, 105);
            this.buttonStop.TabIndex = 1;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // finishedVoiceInputBox
            // 
            this.finishedVoiceInputBox.Location = new System.Drawing.Point(557, 159);
            this.finishedVoiceInputBox.Multiline = true;
            this.finishedVoiceInputBox.Name = "finishedVoiceInputBox";
            this.finishedVoiceInputBox.ReadOnly = true;
            this.finishedVoiceInputBox.Size = new System.Drawing.Size(140, 105);
            this.finishedVoiceInputBox.TabIndex = 2;
            // 
            // hypothesizedVoiceInputBox
            // 
            this.hypothesizedVoiceInputBox.Location = new System.Drawing.Point(557, 61);
            this.hypothesizedVoiceInputBox.Multiline = true;
            this.hypothesizedVoiceInputBox.Name = "hypothesizedVoiceInputBox";
            this.hypothesizedVoiceInputBox.ReadOnly = true;
            this.hypothesizedVoiceInputBox.Size = new System.Drawing.Size(140, 92);
            this.hypothesizedVoiceInputBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(483, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Live Voice Recorder";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(27, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(360, 32);
            this.label2.TabIndex = 6;
            this.label2.Text = "Audio-to-Text Conversion";
            // 
            // buttonUpload
            // 
            this.buttonUpload.Location = new System.Drawing.Point(116, 61);
            this.buttonUpload.Name = "buttonUpload";
            this.buttonUpload.Size = new System.Drawing.Size(129, 74);
            this.buttonUpload.TabIndex = 7;
            this.buttonUpload.Text = "File Upload";
            this.buttonUpload.UseVisualStyleBackColor = true;
            this.buttonUpload.Click += new System.EventHandler(this.buttonUpload_Click);
            // 
            // conversionDisplayBox
            // 
            this.conversionDisplayBox.Location = new System.Drawing.Point(48, 141);
            this.conversionDisplayBox.Multiline = true;
            this.conversionDisplayBox.Name = "conversionDisplayBox";
            this.conversionDisplayBox.ReadOnly = true;
            this.conversionDisplayBox.Size = new System.Drawing.Size(291, 297);
            this.conversionDisplayBox.TabIndex = 8;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(705, 450);
            this.Controls.Add(this.conversionDisplayBox);
            this.Controls.Add(this.buttonUpload);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hypothesizedVoiceInputBox);
            this.Controls.Add(this.finishedVoiceInputBox);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStart);
            this.Name = "Form1";
            this.Text = "Voice to Text";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonUpload;
        private System.Windows.Forms.TextBox conversionDisplayBox;
    }
}
