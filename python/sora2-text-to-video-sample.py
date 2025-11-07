"""
Sora-2 Text-to-Video Generation Sample

This script demonstrates how to use the Azure OpenAI Sora-2 model to generate videos
from text prompts. Sora-2 is a state-of-the-art AI model that creates realistic and
imaginative video content based on natural language descriptions.

Requirements:
    - Python 3.7+
    - openai library (install via: pip install openai)
    - python-dotenv library (install via: pip install python-dotenv)
    - Valid Azure OpenAI API key and endpoint in .env file

Usage:
    python sora2-text-to-video-sample.py

Author: Microsoft Corporation
Date: October 2025
"""

import os
from openai import OpenAI
from dotenv import load_dotenv

# Load environment variables from .env file
load_dotenv()

# Get configuration from environment variables
api_key = os.getenv("AZURE_API_KEY")
resource_name = os.getenv("AZURE_RESOURCE_NAME")
model_name = os.getenv("AZURE_MODEL_NAME")

if not api_key or not resource_name or not model_name:
    raise ValueError(
        "Missing required environment variables. "
        "Please copy .env.sample to .env and populate with your values."
    )

base_url = f"https://{resource_name}/openai/v1/"
client = OpenAI(
    api_key=api_key,
    base_url=base_url,
)

# Video generation parameters
prompt = '''Create a 4-econd, photoreal cinematic shot. In a sunlit, minimalist café, a red panda stands on a sturdy stool behind a stainless-steel espresso bar and pours latte art into a ceramic cup. The animal's movement is precise and confident—unexpectedly expert.
Camera: 35 mm lens, close-medium framing, gentle push-in (dolly) from right to left; slight handheld micro-jitter for realism.
Action beat: In one smooth motion, the red panda tilts the pitcher and draws a crisp rosetta pattern; steam swirls rise; a micro-splash lands on the saucer (shallow depth of field catches droplets in bokeh).
Lighting & look: Late-morning natural light, soft shadows, warm color temperature; high-detail fur, whiskers, and reflective metal surfaces; subsurface scattering on ears; subtle condensation on the milk pitcher.
Environment cues: Background defocused café plants and wood grain; no logos, no text, no humans.
Tone: Wholesome, delightful, subtly humorous; no slapstick.
End frame (hold ~0.4s): The red panda sets the cup down, proud head tilt to camera as steam curls upward.
Safety & realism: Anatomically plausible proportions; stable footing; appropriate utensil sizes.'''

size = "1280x720"
seconds = "4"

print("Sending video generation request to Azure OpenAI Sora-2...")
print(f"Endpoint URL: {base_url}videos")
print(f"Prompt: {prompt}")
print(f"Duration: {seconds} seconds")
print(f"Resolution: {size}")
print("-" * 60)

try:
    video = client.videos.create(
        model=model_name,
        prompt=prompt,
        size=size,
        seconds=seconds,
    )
    
    print("\n✓ Video generation started successfully!")
    print(f"\nVideo Generation Details:")
    print(f"  Video ID: {video.id}")
    print(f"  Status: {video.status}")
    print(f"  Model: {video.model}")
    print(f"  Created At: {video.created_at}")
    print(f"\nNote: Video generation is an asynchronous process.")
    print("The video may take several minutes to complete.")
    print(f"You can check the status using: python sora2-video-creation-status.py {video.id}")
    
except Exception as e:
    print(f"\n✗ Error: {e}")
    print(f"Error type: {type(e).__name__}")
    print("\nTroubleshooting:")
    print("  - Verify your API key is correct")
    print("  - Check that your Azure resource name is correct")
    print("  - Ensure your Azure subscription has access to Sora-2")
    print("  - Verify the prompt and parameters are valid")
