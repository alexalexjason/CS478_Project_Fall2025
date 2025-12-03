// Simple wrapper for the Web Speech API.
// Exposes window.VoiceRecognition with .start(interimCallback, finalCallback) and .stop().
// Notes: works in Chrome/Edge desktop. Must be served over HTTPS or localhost.

(function () {
    const SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;
    if (!SpeechRecognition) {
        window.VoiceRecognition = null;
        return;
    }

    let recog = null;
    let interimCallback = null;
    let finalCallback = null;

    function create() {
        const r = new SpeechRecognition();
        r.lang = 'en-US';
        r.interimResults = true;
        r.continuous = true; // keep recognizing until stopped
        r.maxAlternatives = 1;

        r.onresult = (ev) => {
            let interim = '';
            let finals = [];
            for (let i = ev.resultIndex; i < ev.results.length; i++) {
                const res = ev.results[i];
                if (res.isFinal) {
                    finals.push(res[0].transcript.trim());
                } else {
                    interim += res[0].transcript;
                }
            }

            if (interimCallback) interimCallback(interim || null);
            if (finals.length && finalCallback) {
                finals.forEach(f => finalCallback(f));
            }
        };

        r.onerror = (ev) => {
            // You may want to handle specific errors (no-speech, not-allowed, network, etc.)
            console.warn('SpeechRecognition error', ev);
        };

        r.onend = () => {
            // don't auto-restart here — control via start/stop
        };

        return r;
    }

    window.VoiceRecognition = {
        start: function (onInterim, onFinal) {
            if (!SpeechRecognition) return;
            interimCallback = onInterim || null;
            finalCallback = onFinal || null;
            if (!recog) recog = create();
            try {
                recog.start();
            } catch (e) {
                // sometimes start throws if already running; recreate and start
                try {
                    recog = create();
                    recog.start();
                } catch (ex) {
                    console.error('Failed to start recognition', ex);
                }
            }
        },
        stop: function () {
            if (recog) {
                try {
                    recog.stop();
                } catch { /* ignore */ }
            }
        }
    };
})();