"""
Sora-2 Video Download Utility

This script downloads previously generated videos from Azure OpenAI Sora-2 using their
video IDs. After generating a video using the text-to-video or image-to-video APIs,
use this utility to retrieve and save the completed video file to your local system.

Requirements:
    - Python 3.7+
    - openai library (install via: pip install openai)
    - python-dotenv library (install via: pip install python-dotenv)
    - Valid Azure OpenAI API key and endpoint

Usage:
    python sora2-download-generated-video.py <video_id> <output_file_path>

    Parameters:
        video_id: The ID of the generated video (format: video_xxx)
                  This ID is returned when you create a video generation request
        
        output_file_path: Full path where the video should be saved
                         Can include directories; they will be created if needed
                         Recommended format: .mp4

Examples:
    # Download to current directory
    python sora2-download-generated-video.py video_68fa7f59f2588190a48ea2b0aa73d844 output.mp4
    
    # Download to specific directory
    python sora2-download-generated-video.py video_68fa7f59f2588190a48ea2b0aa73d844 videos/red_panda_latte.mp4
    
    # Download with full path
    python sora2-download-generated-video.py video_68fa7f59f2588190a48ea2b0aa73d844 C:/Videos/generated/output.mp4

Author: Microsoft Corporation
Date: October 2025
"""

import os
import sys
from openai import OpenAI
from dotenv import load_dotenv

# ============================================================================
# Load Environment Configuration
# ============================================================================
# Load environment variables from .env file
load_dotenv()

def download_video(video_id, output_file_path):
    """
    Download a generated video from Azure OpenAI Sora-2 using the video ID.
    
    This function:
    1. Authenticates with Azure OpenAI using credentials from .env
    2. Retrieves the video content using the provided video ID
    3. Creates output directories if they don't exist
    4. Saves the video to the specified file path
    
    Args:
        video_id (str): The ID of the video to download (format: video_xxx)
                       This is returned in the response when creating a video
        
        output_file_path (str): The full path where the video should be saved
                               Directories in the path will be created automatically
                               Example: "output/my_video.mp4"
    
    Raises:
        ValueError: If environment variables are missing
        Exception: If the video download fails or video ID is invalid
    """
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
            "Please ensure AZURE_API_KEY and AZURE_RESOURCE_NAME are set in your .env file."
        )
    
    # Construct the base URL for Azure OpenAI
    base_url = f"https://{resource_name}/openai/v1/"
    
    # ============================================================================
    # Initialize OpenAI Client
    # ============================================================================
    # Create the OpenAI client with Azure-specific configuration
    client = OpenAI(
        api_key=api_key,
        base_url=base_url,
    )

    # ============================================================================
    # Download Video Content
    # ============================================================================
    print("-" * 60)
    print(f"Downloading video...")
    print(f"  Endpoint URL: {base_url}videos/{video_id}/content")
    print(f"  Video ID: {video_id}")
    print(f"  Output Path: {output_file_path}")
    print("-" * 60)
    
    # Retrieve the video content from Azure OpenAI
    # The 'variant' parameter specifies we want the full video file
    content = client.videos.download_content(video_id, variant="video")
    
    # ============================================================================
    # Create Output Directory
    # ============================================================================
    # Extract directory path from the output file path
    output_dir = os.path.dirname(output_file_path)
    
    # Create directory structure if it doesn't exist
    if output_dir and not os.path.exists(output_dir):
        os.makedirs(output_dir)
        print(f"Created directory: {output_dir}")
    
    # ============================================================================
    # Save Video File
    # ============================================================================
    # Write the video content to the specified file
    content.write_to_file(output_file_path)
    
    print(f"\n✓ Video saved successfully!")
    print(f"  Location: {os.path.abspath(output_file_path)}")
    print(f"  File size: {os.path.getsize(output_file_path):,} bytes")

# ============================================================================
# Main Entry Point
# ============================================================================
if __name__ == "__main__":
    # Validate command-line arguments
    if len(sys.argv) != 3:
        print("\n" + "=" * 60)
        print("Sora-2 Video Download Utility")
        print("=" * 60)
        print("\nUsage:")
        print("  python sora2-download-generated-video.py <video_id> <output_file_path>")
        print("\nParameters:")
        print("  video_id          - The ID of the video to download (format: video_xxx)")
        print("  output_file_path  - Full path where the video will be saved")
        print("\nExamples:")
        print("  python sora2-download-generated-video.py video_68fa7f59f2588190a48ea2b0aa73d844 output.mp4")
        print("  python sora2-download-generated-video.py video_abc123 videos/my_video.mp4")
        print("=" * 60)
        sys.exit(1)
    
    # Extract command-line arguments
    video_id = sys.argv[1]
    output_file_path = sys.argv[2]
    
    # Execute video download
    try:
        download_video(video_id, output_file_path)
    except Exception as e:
        print(f"\n✗ Error downloading video: {str(e)}")
        print("\nTroubleshooting:")
        print("  - Verify the video ID is correct (format: video_xxx)")
        print("  - Check that your API key is valid in the .env file")
        print("  - Ensure your Azure resource name is correct")
        print("  - Confirm the video generation has completed")
        print("  - Check your network connection")
        sys.exit(1)
