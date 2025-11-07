"""
Sora-2 Video Creation Status Check

This script checks the status of a video generation job by video ID.
It polls the Azure OpenAI API every 20 seconds until the video is completed,
failed, or cancelled.

Requirements:
    - Python 3.7+
    - openai library (install via: pip install openai)
    - python-dotenv library (install via: pip install python-dotenv)
    - Valid Azure OpenAI API key and endpoint in .env file

Usage:
    python sora2-video-creation-status.py <video_id>

Example:
    python sora2-video-creation-status.py video_6901017960dc8190a6f7a19cd8f48976

Author: Microsoft Corporation
Date: October 2025
"""

import os
import sys
import time
from openai import OpenAI
from dotenv import load_dotenv

# Load environment variables from .env file
load_dotenv()

def check_video_status(video_id):
    """
    Check the status of a video creation by video ID.
    Polls every 20 seconds until the video is completed, failed, or cancelled.
    
    Args:
        video_id: The ID of the video to check
    """
    # Get configuration from environment variables
    api_key = os.getenv("AZURE_API_KEY")
    resource_name = os.getenv("AZURE_RESOURCE_NAME")
    
    if not api_key or not resource_name:
        print("Error: AZURE_API_KEY and AZURE_RESOURCE_NAME must be set in .env file")
        sys.exit(1)
    
    base_url = f"https://{resource_name}/openai/v1/"
    client = OpenAI(
        api_key=api_key,
        base_url=base_url,
    )
    
    print(f"Endpoint URL: {base_url}videos/{video_id}")
    
    # Retrieve initial video status
    video = client.videos.retrieve(video_id)
    print(f"Initial status for video {video_id}: {video.status}")
    
    # Poll every 20 seconds
    while video.status not in ["completed", "failed", "cancelled"]:
        print(f"Status: {video.status}. Waiting 20 seconds...")
        time.sleep(20)
        
        # Retrieve the latest status
        video = client.videos.retrieve(video_id)
    
    # Final status
    if video.status == "completed":
        print("Video successfully completed!")
        print(video)
    else:
        print(f"Video creation ended with status: {video.status}")
        print(video)
    
    return video

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python sora2-video-creation-status.py <video_id>")
        sys.exit(1)
    
    video_id = sys.argv[1]
    check_video_status(video_id)
