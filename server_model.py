from transformers import AutoTokenizer, AutoModelForCausalLM
import torch
from fastapi import FastAPI
from pydantic import BaseModel

model_name = "ministral/Ministral-4b-Instruct"
tokenizer = AutoTokenizer.from_pretrained(model_name)
model = AutoModelForCausalLM.from_pretrained(model_name, device_map="auto")

app = FastAPI()

class Request(BaseModel):
    prompt: str

@app.post("/generate")
async def generate(request: Request):
    inputs = tokenizer(request.prompt, return_tensors="pt").to(model.device)
    outputs = model.generate(**inputs, max_new_tokens=150)
    text = tokenizer.decode(outputs[0], skip_special_tokens=True)
    return {"response": text}