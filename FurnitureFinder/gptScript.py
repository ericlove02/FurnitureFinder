import openai
import json

# Your API key
openai.api_key = "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"

response = openai.ChatCompletion.create(
  model="gpt-3.5-turbo",
  messages=[
    {
      "role": "system",
      "content": "You will be provided with statements, and your task is to convert them to standard English."
    },
    {
      "role": "user",
      "content": "She no went to the market."
    }
  ],
  temperature=0,
  max_tokens=256,
  top_p=1,
  frequency_penalty=0,
  presence_penalty=0
)