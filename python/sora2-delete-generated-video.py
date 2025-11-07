"""
Sora-2 Delete Generated Video

This script deletes a specific video from Azure OpenAI's Sora-2 model by video ID.

Requirements:
    - Python 3.7+
    - openai library (install via: pip install openai)
    - python-dotenv library (install via: pip install python-dotenv)
    - Valid Azure OpenAI API key and endpoint in .env file

Usage:
    python sora2-delete-generated-videos.py <video_id>

Example:
    python sora2-delete-generated-videos.py video_6901017960dc8190a6f7a19cd8f48976

Author: Microsoft Corporation
Date: October 2025
"""

import os
import sys
from openai import OpenAI
from dotenv import load_dotenv

# Load environment variables from .env file
load_dotenv()

def delete_video(video_id):
    """
    Delete a video by its ID.
    
    Args:
        video_id: The ID of the video to delete
    """
    # Get configuration from environment variables
    api_key = os.getenv("AZURE_API_KEY")
    resource_name = os.getenv("AZURE_RESOURCE_NAME")
    
    if not api_key or not resource_name:
        print("Error: AZURE_API_KEY and AZURE_RESOURCE_NAME must be set in .env file")
        sys.exit(1)
    
    # Initialize OpenAI client for Azure
    base_url = f"https://{resource_name}/openai/v1/"
    client = OpenAI(
        api_key=api_key,
        base_url=base_url,
    )
    
    print(f"Deleting video: {video_id}")
    print(f"Endpoint URL: {base_url}videos/{video_id}")
    print("=" * 60)
    
    try:
        # Delete the video
        response = client.videos.delete(video_id)
        
        print(f"\n✓ Successfully deleted video: {video_id}")
        print(f"Deletion confirmed: {response.deleted}")
        print(f"Video ID: {response.id}")
        
        return response
        
    except Exception as e:
        print(f"\n✗ Error deleting video: {e}")
        print(f"Error type: {type(e).__name__}")
        sys.exit(1)

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python sora2-delete-generated-videos.py <video_id>")
        print("\nExample:")
        print("  python sora2-delete-generated-videos.py video_6901017960dc8190a6f7a19cd8f48976")
        sys.exit(1)
    
    video_id = sys.argv[1]
    delete_video(video_id)
