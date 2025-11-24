// Lightweight chatbot module - toggle + simple message flow to /api/chat
window.Chatbot = (function () {
  let visible = false;
  let initialized = false;
  const rootId = 'chatbot-root';

  function init() {
    if (initialized) return;
    initialized = true;

    const root = document.getElementById(rootId);
    if (!root) return;

    const close = root.querySelector('#chatbot-close');
    const send = root.querySelector('#chatbot-send');
    const input = root.querySelector('#chatbot-input');
    const messages = root.querySelector('#chatbot-messages');

    close.addEventListener('click', () => toggle(false));
    send.addEventListener('click', sendMessage);
    input.addEventListener('keydown', (e) => { if (e.key === 'Enter') sendMessage(); });

    // expose init marker for layout partial
    if (window._chatbotLazyReady) window._chatbotLazyReady();

    function appendMessage(text, cls) {
      const el = document.createElement('div');
      el.className = cls + ' msg p-2';
      el.textContent = text;
      messages.appendChild(el);
      messages.scrollTop = messages.scrollHeight;
    }

    async function sendMessage() {
      const text = input.value.trim();
      if (!text) return;
      appendMessage(text, 'user'); // user bubble
      input.value = '';
      const thinking = document.createElement('div');
      thinking.className = 'bot msg p-2 text-muted';
      thinking.textContent = '…';
      messages.appendChild(thinking);
      messages.scrollTop = messages.scrollHeight;

      try {
        const res = await fetch('/api/chat', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ message: text })
        });

        let replyText;
        if (!res.ok) {
          replyText = 'Server error — see console.';
          console.error('Chat API error', await res.text());
        } else {
          const data = await res.json();
          replyText = data.reply ?? 'No reply';
        }

        thinking.remove();
        appendMessage(replyText, 'bot');
      } catch (err) {
        thinking.remove();
        appendMessage('Network error', 'bot');
        console.error(err);
      }
    }
  }

  function toggle(force) {
    const root = document.getElementById(rootId);
    if (!root) return;
    if (!initialized) init();
    visible = typeof force === 'boolean' ? force : !visible;
    root.classList.toggle('hidden', !visible);
    root.setAttribute('aria-hidden', (!visible).toString());
  }

  return { init, toggle };
})();