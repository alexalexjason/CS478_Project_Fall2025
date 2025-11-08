from transformers import AutoTokenizer, AutoModelForCausalLM
import torch
from fastapi import FastAPI
from pydantic import BaseModel

# Model configuration
model_name = "ministral/Ministral-4b-instruct"  
tokenizer = AutoTokenizer.from_pretrained(model_name)
model = AutoModelForCausalLM.from_pretrained(model_name, device_map="auto")

app = FastAPI()

class Request(BaseModel):
    prompt: str

@app.post("/generate")
async def generate(request: Request):
    # Tokenize and move inputs to the model 
    inputs = tokenizer(request.prompt, return_tensors="pt").to(model.device)

    # Generate output 
    outputs = model.generate(
        **inputs,
        max_new_tokens=150,  
        pad_token_id=tokenizer.eos_token_id
    )

    text = tokenizer.decode(outputs[0], skip_special_tokens=True)
    return {"response": text}
