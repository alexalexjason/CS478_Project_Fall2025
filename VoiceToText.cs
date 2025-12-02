using System;
using System.Speech.Recognition; // built-in converter

namespace VoiceToText
{
    public class VoiceRecognizer : IDisposable // allow temporary file cleanup
    {
        private readonly SpeechRecognitionEngine _engine;
        private readonly double _confidenceThreshold; // ignore voice input if not >= 30% confident
        private bool _disposed;

        public event EventHandler<string> SpeechRecognized;

        public event EventHandler<string> SpeechHypothesized;

        public VoiceRecognizer(double confidenceThreshold = 0.3) // not going to deal with any other languages - will also require error handling
        {
            _confidenceThreshold = confidenceThreshold;

            _engine = new SpeechRecognitionEngine(); // the built-in speech recognition

            _engine.LoadGrammar(new DictationGrammar()); // recognize user's speech
            _engine.SetInputToDefaultAudioDevice(); // connect to user's mic

            _engine.SpeechRecognized += OnSpeechRecognizedInternal;
            _engine.SpeechHypothesized += OnSpeechHypothesizedInternal;
        }

        private void OnSpeechRecognizedInternal(object sender, SpeechRecognizedEventArgs e) // show final recognition
        {
            if (e.Result.Confidence < _confidenceThreshold) return;

            SpeechRecognized?.Invoke(this, e.Result.Text);
        }

        private void OnSpeechHypothesizedInternal(object sender, SpeechHypothesizedEventArgs e) // show hypothesized recognition
        {
            SpeechHypothesized?.Invoke(this, e.Result.Text);
        }

        public void Start()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(VoiceRecognizer));

            try // continue until the user presses stop
            {
                _engine.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Speech recognition is already running.");
            }
        }

        public void Stop()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(VoiceRecognizer));

            try // stops voice recognition
            {
                _engine.RecognizeAsyncStop();
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Speech recognition is not currently running.");
            }
        }

        public void Dispose() // dispose of the user's audio input
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
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
