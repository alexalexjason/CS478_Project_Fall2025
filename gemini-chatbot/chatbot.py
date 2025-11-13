import os
from google import genai
from dotenv import load_dotenv

# Load API key from .env
load_dotenv()
api_key = os.getenv("GOOGLE_API_KEY")

# Initialize the client
client = genai.Client(api_key=api_key)

print("Gemini Chatbot — type 'exit' or 'quit' to stop.\n")

# Start a chat session 
chat_session = client.chats.create(model="gemini-2.5-flash")

while True:
    user_input = input("You: ")
    
    # Check for exit commands
    if user_input.lower() in ["exit", "quit"]:
        print("Exiting...")
        break

    # Send user message
    try:
        response = chat_session.send_message(user_input)

        # Print reply
        print("Bot:", response.text)
        
    except Exception as e:
        # Basic error handling 
        print(f"An error occurred: {e}")
        print("Please verify your internet connection, API key, and model name.")