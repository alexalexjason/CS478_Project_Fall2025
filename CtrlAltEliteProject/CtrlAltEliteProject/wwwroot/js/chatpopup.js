// Lightweight chat popup client (simulated bot). Replace sendMessageToBackend with real API.
(function () {
    try {
        // support either id used previously or the new partial id
        const chatBtn = document.getElementById('globalChatBtn') || document.getElementById('chatBtn');
        const chatPopup = document.getElementById('chatPopup');
        if (!chatBtn || !chatPopup) return; // nothing to wire on this page

        const chatCloseBtn = document.getElementById('chatCloseBtn');
        const chatSendBtn = document.getElementById('chatSendBtn');
        const chatClearBtn = document.getElementById('chatClearBtn');
        const chatInput = document.getElementById('chatInput');
        const chatMessages = document.getElementById('chatMessages');
        const includeTranscript = document.getElementById('includeTranscript');
        const finalTextEl = document.getElementById('finalText');

        let messages = [];

        function openChat() {
            // ensure the popup can be transitioned (partial may have inlined display:none)
            chatPopup.style.display = chatPopup.style.display === 'flex' ? 'flex' : 'flex';
            // allow CSS transition
            requestAnimationFrame(() => {
                chatPopup.classList.add('open');
                chatPopup.setAttribute('aria-hidden', 'false');
                if (chatInput) chatInput.focus();
                renderMessages();
            });
        }

        function closeChat() {
            chatPopup.classList.remove('open');
            chatPopup.setAttribute('aria-hidden', 'true');
            // wait for CSS transition to finish then hide to prevent tabbing into it
            setTimeout(() => {
                // only hide if still closed
                if (!chatPopup.classList.contains('open')) {
                    chatPopup.style.display = 'none';
                }
            }, 220);
            chatBtn.focus();
        }

        function addMessage(sender, text) {
            messages.push({ sender, text, time: new Date().toISOString() });
            renderMessages();
        }

        function renderMessages() {
            if (!chatMessages) return;
            chatMessages.innerHTML = '';
            for (const m of messages) {
                const el = document.createElement('div');
                el.className = 'chat-message ' + (m.sender === 'user' ? 'user' : 'bot');
                const textEl = document.createElement('div');
                textEl.className = 'chat-message-text';
                textEl.textContent = m.text;
                el.appendChild(textEl);
                chatMessages.appendChild(el);
            }
            chatMessages.scrollTop = chatMessages.scrollHeight;
        }

        async function sendMessage() {
            if (!chatInput) return;
            const text = (chatInput.value || '').trim();
            if (!text) return;
            addMessage('user', text);
            chatInput.value = '';
            let payload = { message: text };
            if (includeTranscript && includeTranscript.checked) {
                payload.transcript = finalTextEl ? finalTextEl.value : '';
            }

            addMessage('bot', '…'); // typing indicator
            const typingIndex = messages.length - 1;
            setTimeout(async () => {
                const botReply = await simulatedBotReply(payload);
                messages[typingIndex].text = botReply;
                renderMessages();
            }, 600);
        }

        function simulatedBotReply(payload) {
            return new Promise(resolve => {
                const hasContext = payload.transcript && payload.transcript.length > 0;
                const reply = hasContext
                    ? "Simulated assistant reply. Received transcript (first 120 chars): " + payload.transcript.slice(0, 120)
                    : "Simulated assistant reply. (No transcript included)";
                setTimeout(() => resolve(reply), 600);
            });
        }

        // Event wiring (guard each element)
        chatBtn.addEventListener('click', openChat);
        if (chatCloseBtn) chatCloseBtn.addEventListener('click', closeChat);
        if (chatSendBtn) chatSendBtn.addEventListener('click', sendMessage);
        if (chatClearBtn) chatClearBtn.addEventListener('click', () => { messages = []; renderMessages(); });
        if (chatInput) {
            chatInput.addEventListener('keydown', (e) => {
                if (e.key === 'Enter' && !e.shiftKey) {
                    e.preventDefault();
                    sendMessage();
                }
            });
        }

        // accessible close on Escape
        document.addEventListener('keydown', (e) => {
            if (e.key === 'Escape' && chatPopup.classList.contains('open')) {
                closeChat();
            }
        });
    } catch (err) {
        // fail silently but keep console info for debugging
        console.error('Chat popup init error', err);
    }
})();