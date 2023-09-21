import openai

# Your API key
openai.api_key = "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"

product_description = input("Enter a product description: ") # TODO - pull from Wayfair API
vibe = input("Enter a vibe: ") # TODO - add a list of vibes to choose from

response = openai.ChatCompletion.create(
  model="gpt-3.5-turbo",
  messages=[
    {
      "role": "system",
      "content": """Given a furniture/decoration product description and a vibe/theme word, 
                    please rate it on a scale of 1-10 with 1 being the total opposite of the vibe/theme and
                    10 being a spot on match of the vibe/theme. Return just the number rating."""
    },
    {
      "role": "user",
      "content": f"""Product description: {product_description} 
                     Vibe: {vibe}"""
    }
  ],
  temperature=0,
  max_tokens=256,
  top_p=1,
  frequency_penalty=0,
  presence_penalty=0
)

response_text = response['choices'][0]['message']['content']

print(response_text)