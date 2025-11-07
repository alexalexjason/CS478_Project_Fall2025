document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('chatForm');
    const input = document.getElementById('messageInput');
    const chatWindow = document.getElementById('chatWindow');

    function appendMessage(sender, text) {
        const wrapper = document.createElement('div');
        wrapper.style.marginBottom = '10px';

        const who = document.createElement('div');
        who.style.fontSize = '0.85rem';
        who.style.fontWeight = '600';
        who.textContent = sender;

        const msg = document.createElement('div');
        msg.style.padding = '8px';
        msg.style.borderRadius = '6px';
        msg.style.marginTop = '4px';
        msg.style.background = sender === 'You' ? '#e1f5fe' : '#fff9c4';
        msg.textContent = text;

        wrapper.appendChild(who);
        wrapper.appendChild(msg);
        chatWindow.appendChild(wrapper);
        chatWindow.scrollTop = chatWindow.scrollHeight;
    }

    form.addEventListener('submit', function (e) {
        e.preventDefault();
        const text = input.value.trim();
        if (!text) return;

        appendMessage('You', text);
        input.value = '';
        input.focus();

        // grab antiforgery token from hidden input
        const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
        const token = tokenInput ? tokenInput.value : '';

        const formData = new FormData();
        formData.append('message', text);
        formData.append('__RequestVerificationToken', token);

        // POST to the page handler (Razor Pages): /Chat?handler=Send
        fetch(window.location.pathname + '?handler=Send', {
            method: 'POST',
            body: formData
        })
            .then(response => {
                if (!response.ok) return response.json().then(err => Promise.reject(err));
                return response.json();
            })
            .then(json => {
                appendMessage('Bot', json.bot);
            })
            .catch(err => {
                console.error(err);
                appendMessage('Bot', 'Sorry, an error occurred sending your message.');
            });
    });
});