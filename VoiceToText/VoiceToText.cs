using NAudio.Wave;
using System;
using System.IO;
using System.Speech.Recognition;
using System.Windows.Forms;

// Installed NAudio (so you aren't limited to .wav files) and System.Speech (to do anything here) from NuGet

namespace VoiceToText
{
    public class VoiceRecognizer : IDisposable // IDisposable to clean up audio files
    {
        private readonly SpeechRecognitionEngine _engine;
        private readonly double _confidenceThreshold;
        private bool _disposed;

        public event EventHandler<string> SpeechRecognized; // Final recognized speech
        public event EventHandler<string> SpeechHypothesized; // In process of recognition

        public VoiceRecognizer(double confidenceThreshold = 0.3) // Requires >=30% confidence 
        {
            _confidenceThreshold = confidenceThreshold;

            _engine = new SpeechRecognitionEngine(); // Performs voice recognition
            _engine.LoadGrammar(new DictationGrammar());
            _engine.SetInputToDefaultAudioDevice();

            _engine.SpeechRecognized += OnSpeechRecognizedInternal;
            _engine.SpeechHypothesized += OnSpeechHypothesizedInternal;
        }
        
        private void OnSpeechRecognizedInternal(object sender, SpeechRecognizedEventArgs e) // Pass along recognized speech
        {
            if (e.Result.Confidence < _confidenceThreshold) return;
            SpeechRecognized?.Invoke(this, e.Result.Text);
        }

        private void OnSpeechHypothesizedInternal(object sender, SpeechHypothesizedEventArgs e) // Pass along hypothesized speech
        {
            SpeechHypothesized?.Invoke(this, e.Result.Text);
        }

        public void Start() // Begin asynchronous voice recognition
        {
            if (_disposed) throw new ObjectDisposedException(nameof(VoiceRecognizer));
            _engine.RecognizeAsync(RecognizeMode.Multiple);
        }

        public void Stop() // Terminate voice recognition process
        {
            if (_disposed) throw new ObjectDisposedException(nameof(VoiceRecognizer));
            _engine.RecognizeAsyncStop();
        }

        public string RecognizeFromFile(string filePath) // Allows for the recognition of audio/video files
        {
            if (_disposed) throw new ObjectDisposedException(nameof(VoiceRecognizer));
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));
            if (!File.Exists(filePath)) throw new FileNotFoundException("File not found.", filePath); // Verifies that the file is valid (can still do more error handling)

            string wavPath = ConvertToWav(filePath); // Convert non-WAV to WAV

            using (var engine = new SpeechRecognitionEngine()) // Temporary engine instance 
            {
                engine.LoadGrammar(new DictationGrammar());
                engine.SetInputToWaveFile(wavPath);
                var result = engine.Recognize();
                return result?.Text ?? string.Empty;
            }
        }

        // File upload and recognition
        public string UploadAndRecognize()
        {
            using (var dialog = new OpenFileDialog()) // File selection and making sure that it is valid
            {
                dialog.Filter = "All Files|*.*";
                dialog.Title = "Select an audio or video file";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string path = dialog.FileName;
                    if (!File.Exists(path))
                        return "Error: File not found.";

                    try
                    {
                        string text = RecognizeFromFile(path);
                        return string.IsNullOrEmpty(text)
                            ? "No speech recognized."
                            : text;
                    }
                    catch (InvalidOperationException ex)
                    {
                        return "Invalid file: " + ex.Message;
                    }
                    catch (Exception ex)
                    {
                        return "Error: " + ex.Message;
                    }
                }

                return "No file selected.";
            }
        }

        private string ConvertToWav(string inputPath)
        {
            // If already WAV then return
            if (Path.GetExtension(inputPath).Equals(".wav", StringComparison.OrdinalIgnoreCase))
                return inputPath;

            string tempWav = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".wav"); // Create temporary file 

            try
            {
                using (var reader = new MediaFoundationReader(inputPath)) // Reads the audio or video file
                using (var pcm = WaveFormatConversionStream.CreatePcmStream(reader)) // Convert to raw PCM audio - is this where quality suffers?
                {
                    WaveFileWriter.CreateWaveFile(tempWav, pcm); // Creates temporary WAV file
                }
            }
            catch (FormatException)
            {
                throw new InvalidOperationException("This file is not a supported audio or video format.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to process file: " + ex.Message);
            }

            return tempWav;
        }

        public void Dispose() // Release resources
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) // Cleanup temporary files
        {
            if (_disposed) return;
            if (disposing)
            {
                _engine.RecognizeAsyncCancel();
                _engine.Dispose();
            }
            _disposed = true;
        }
    }
}

