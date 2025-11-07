"""
Sora-2 Image-to-Video Generation Sample

This script demonstrates how to use the Azure OpenAI Sora-2 model to generate videos
from a reference image and text prompt. Sora-2 can take a still image and animate it
based on your description, creating realistic and imaginative video content.

Requirements:
    - Python 3.7+
    - openai library (install via: pip install openai)
    - python-dotenv library (install via: pip install python-dotenv)
    - Valid Azure OpenAI API key and endpoint
    - Reference image file (JPEG, PNG, or WebP format)
    
Usage:
    1. Copy .env.sample to .env and populate with your Azure credentials
    2. Place your reference image in the same directory as this script
    3. Update the image filename and parameters as needed
    4. Run the script: python sora2-image-to-video-sample.py

Author: Microsoft Corporation
Date: October 2025
"""

import os
from openai import OpenAI
from dotenv import load_dotenv

# Load environment variables from .env file
load_dotenv()

# ============================================================================
# API Configuration
# ============================================================================
# Load credentials from environment variables
api_key = os.getenv("AZURE_API_KEY")
resource_name = os.getenv("AZURE_RESOURCE_NAME")

# Validate required environment variables
if not api_key or not resource_name:
    raise ValueError(
        "Missing required environment variables. "
        "Please copy .env.sample to .env and populate with your values."
    )

# Initialize OpenAI client for Azure
base_url = f"https://{resource_name}/openai/v1/"
client = OpenAI(
    api_key=api_key,
    base_url=base_url,
)

# ============================================================================
# Video Generation Parameters
# ============================================================================
# Configure the parameters for your image-to-video generation request
#
# Available Parameters:
# ---------------------
# prompt (String, REQUIRED):
#     Natural-language description that guides how the reference image should be animated.
#     Best practices for image-to-video prompts:
#     - Reference the uploaded image explicitly (e.g., "Reference image: the uploaded photo...")
#     - Describe the desired action and motion clearly
#     - Include timing details for different phases of motion
#     - Specify camera movement (e.g., "tracking shot", "static camera", "pan left to right")
#     - Mention lighting and environmental continuity with the reference image
#     - Add physics and realism constraints (e.g., "realistic water splashes", "correct anatomy")
#     - List what NOT to do to avoid unwanted artifacts
#     - Keep the final frame description for smooth looping if needed
#
# model (String, OPTIONAL):
#     The AI model to use for video generation.
#     Default: "sora-2"
#     Currently supported: "sora-2"
#
# size (String, OPTIONAL):
#     Output video resolution in width × height format.
#     IMPORTANT: Must exactly match the dimensions of your input reference image.
#     Supported values:
#     - '720x1280' - Portrait orientation (9:16 aspect ratio)
#     - '1280x720' - Landscape orientation (16:9 aspect ratio)
#     - '1024x1792' - Portrait orientation (taller format)
#     - '1792x1024' - Landscape orientation (wider format)
#     Default: '720x1280'
#
# seconds (Integer, OPTIONAL):
#     Duration of the generated video in seconds.
#     Supported values: 4, 8, 12
#     Default: 4
#     Note: Longer videos may take more time to generate
#
# input_reference (File, REQUIRED for this script):
#     A reference image file that will be used as the starting frame for the video.
#     Requirements:
#     - Accepted MIME types: image/jpeg, image/png, image/webp
#     - Image dimensions must exactly match the specified 'size' parameter
#     - The first frame of the generated video will closely match this image

prompt = '''Reference image: the uploaded photo of three chestnut horses cantering in shallow surf under an overcast sky (lead horse with a white blaze in the foreground, two horses trailing left-of-frame).
Goal: generate a 4-second photorealistic clip where the lead horse performs a natural show-jumping motion over an imaginary fence—no object should appear—while the setting and other horses remain consistent with the still image.
Action & timing:
• 0-1 s - approach canter toward camera left→right; subtle water splashes.
• 1-2 s - the lead horse gathers, lifts, tucks forelegs, arches the back (bascule), clearing an invisible low fence slightly ahead of its path; ears forward.
• 2-3 s - landing into shallow water: realistic hoof sequence, sand/spray fans outward; ripples propagate.
• 3-4 s - resumes canter for two strides past landing.
Camera & look: tracking shot, chest-height, ~50 mm lens, gentle left-to-right pan with slight parallax; horizon level, no cuts; natural overcast lighting; crisp detail; cinematic but photorealistic.
Continuity constraints: preserve exact coat colors, white blaze, and proportions of the lead horse; keep the two background horses cantering normally and not jumping; waves, wet sand reflections, and cloud field match the reference.
Physics & realism: correct equine gait, weight shift, fetlock compression, mane and tail motion; believable water droplets and splash arcs; no warping or melting.
Do NOT: show a visible fence or any added props; no extra legs/heads; no object duplication; no dramatic lighting changes; no camera shake; no slow-shutter ghosting.
Ending frame: lead horse fully landed, mid-canter, suitable to loop with a subtle cross-fade.'''

image_filename = "horses-1280x720.jpg"
size = "1280x720"  # Video resolution - MUST match input image dimensions
seconds = 8  # Video duration - 8 seconds

# ============================================================================
# Send Image-to-Video Generation Request
# ============================================================================
# Open the reference image file and send it along with the parameters
# The image will be used as the first frame of the generated video
print("Sending image-to-video generation request to Azure OpenAI Sora-2...")
print(f"Endpoint URL: {base_url}videos")
print(f"Reference Image: {image_filename}")
print(f"Duration: {seconds} seconds")
print(f"Resolution: {size}")
print(f"Prompt: {prompt[:100]}...")  # Print first 100 chars of prompt
print("-" * 60)

try:
    # Open and read the reference image file
    with open(image_filename, "rb") as image_file:
        # Create video generation with image reference using OpenAI SDK
        video = client.videos.create(
            model="sora-2",
            prompt=prompt,
            size=size,
            seconds=seconds,
            input_reference=image_file,
        )
    
    # ============================================================================
    # Process API Response
    # ============================================================================
    print("\n✓ Video generation started successfully!")
    print(f"\nVideo Generation Details:")
    print(f"  Video ID: {video.id}")
    print(f"  Status: {video.status}")
    print(f"  Model: {video.model}")
    print(f"  Created At: {video.created_at}")
    print(f"\nNote: Video generation is an asynchronous process.")
    print("The video may take several minutes to complete.")
    print(f"You can check the status using: python sora2-video-creation-status.py {video.id}")
    
except FileNotFoundError:
    print(f"\n✗ Error: Image file '{image_filename}' not found!")
    print("Please ensure the reference image exists in the same directory as this script.")
except Exception as e:
    print(f"\n✗ Error: {e}")
    print(f"Error type: {type(e).__name__}")
    print("\nTroubleshooting:")
    print("  - Verify your API key is correct")
    print("  - Check that your Azure resource name is correct")
    print("  - Ensure your Azure subscription has access to Sora-2")
    print("  - Verify the reference image file exists and matches the size parameter")
    print("  - Check that the image dimensions match the 'size' parameter exactly")