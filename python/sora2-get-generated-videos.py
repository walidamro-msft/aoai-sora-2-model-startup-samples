"""
Sora-2 Get Generated Videos Sample
This script retrieves a list of all generated videos from Azure OpenAI's Sora-2 model.

Requirements:
    - Python 3.7+
    - openai library (install via: pip install openai)
    - python-dotenv library (install via: pip install python-dotenv)
    - Valid Azure OpenAI API key and endpoint in .env file

Usage:
    python sora2-get-generated-videos.py

Author: Microsoft Corporation
Date: October 2025
"""

import os
import json
from openai import OpenAI
from dotenv import load_dotenv

# Load environment variables from .env file
load_dotenv()

# Configuration from environment variables
AZURE_API_KEY = os.getenv("AZURE_API_KEY")
AZURE_RESOURCE_NAME = os.getenv("AZURE_RESOURCE_NAME")

# Validate required environment variables
if not AZURE_API_KEY or not AZURE_RESOURCE_NAME:
    raise ValueError("Missing required environment variables. Please check your .env file.")

# Initialize OpenAI client for Azure
client = OpenAI(
    api_key=AZURE_API_KEY,
    base_url=f"https://{AZURE_RESOURCE_NAME}.openai.azure.com/openai/v1/",
)

print("Retrieving generated videos from Azure OpenAI Sora-2...")
print("=" * 60)

try:
    # List all videos
    videos = client.videos.list()
    
    print("\n✓ Successfully retrieved videos list!")
    print("\n" + "=" * 60)
    print("Videos:")
    print("=" * 60)
    
    # Convert to dict for pretty printing
    videos_dict = {
        "data": [
            {
                "id": video.id,
                "status": video.status,
                "created_at": video.created_at,
                "object": video.object
            }
            for video in videos.data
        ]
    }
    
    # Pretty print the response
    print(json.dumps(videos_dict, indent=2))
    
    # Display summary
    video_count = len(videos.data)
    print("\n" + "=" * 60)
    print(f"Total videos: {video_count}")
    
    if video_count > 0:
        print("\nVideo IDs:")
        for video in videos.data:
            print(f"  - {video.id} (Status: {video.status})")
        
except Exception as e:
    print(f"\n✗ An error occurred: {e}")
    print(f"Error type: {type(e).__name__}")
