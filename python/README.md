# Sora-2 Python Samples

This folder contains Python samples demonstrating how to use Azure OpenAI's Sora-2 model to generate videos. Sora-2 is a state-of-the-art AI model that creates realistic and imaginative video content.

## 📋 Available Samples

### Core Generation Scripts

#### 1. Text-to-Video (`sora2-text-to-video-sample.py`)
Generate videos from text descriptions only. Simply describe what you want to see, and Sora-2 will create it.

**Example Use Cases:**
- "A red panda making latte art in a café"
- "A woman walking through a neon-lit cyberpunk city"
- "Ocean waves crashing on a beach at sunset"

**Usage:**
```powershell
python sora2-text-to-video-sample.py
```

#### 2. Image-to-Video (`sora2-image-to-video-sample.py`)
Animate a reference image based on your text description. Upload a photo and describe how you want it to move.

**Example Use Cases:**
- Make horses in a photo jump over a fence
- Animate a portrait to smile and wave
- Make a still landscape scene come to life with moving clouds and water

**Usage:**
```powershell
python sora2-image-to-video-sample.py
```

### Utility Scripts

#### 3. Video Creation Status (`sora2-video-creation-status.py`)
Check the status of a video generation job. This script polls the Azure OpenAI API every 20 seconds until the video is completed, failed, or cancelled.

**Usage:**
```powershell
python sora2-video-creation-status.py <video_id>
```

**Example:**
```powershell
python sora2-video-creation-status.py video_6901017960dc8190a6f7a19cd8f48976
```

**What it does:**
- Retrieves the current status of your video generation job
- Automatically polls every 20 seconds until completion
- Displays final video details when processing is complete
- Shows if the job failed or was cancelled

#### 4. Get Generated Videos (`sora2-get-generated-videos.py`)
Retrieve a list of all your generated videos from Azure OpenAI's Sora-2 model.

**Usage:**
```powershell
python sora2-get-generated-videos.py
```

**What it does:**
- Lists all videos associated with your Azure OpenAI account
- Shows video IDs, status, and creation timestamps
- Displays a summary count of total videos
- Helps you find video IDs for status checks or downloads

**Example Output:**
```json
{
  "data": [
    {
      "id": "video_abc123...",
      "status": "completed",
      "created_at": 1698508800,
      "object": "video"
    }
  ]
}
Total videos: 1
```

#### 5. Delete Generated Video (`sora2-delete-generated-video.py`)
Delete a specific video from your Azure OpenAI account by video ID.

**Usage:**
```powershell
python sora2-delete-generated-video.py <video_id>
```

**Example:**
```powershell
python sora2-delete-generated-video.py video_6901017960dc8190a6f7a19cd8f48976
```

**What it does:**
- Permanently deletes a video from Azure OpenAI storage
- Confirms successful deletion
- Helps manage your storage and video inventory

**Important:** Deletion is permanent and cannot be undone. Make sure to download any videos you want to keep before deleting them.

## 🚀 Getting Started (For First-Time Python Users)

### Step 1: Install Python

If you don't have Python installed:

1. Go to [python.org/downloads](https://www.python.org/downloads/)
2. Download Python 3.7 or newer (recommended: Python 3.11+)
3. **Important**: During installation, check the box that says "Add Python to PATH"
4. Complete the installation

To verify Python is installed, open PowerShell and type:
```powershell
python --version
```

You should see something like `Python 3.11.x`

### Step 2: Download This Project

1. Download this repository as a ZIP file, or
2. If you have Git installed, clone it:
   ```powershell
   git clone <repository-url>
   cd sora-2-model-startup-samples\python
   ```

### Step 3: Install Required Packages

Python needs some additional libraries to run this script. Open PowerShell in the `python` folder and run:

```powershell
pip install requests python-dotenv openai --upgrade
```

This installs:
- **requests**: For making HTTP requests to Azure
- **python-dotenv**: For loading configuration from a `.env` file
- **openai**: For working with Azure OpenAI API (upgraded to the latest version to support video features)

### Step 4: Set Up Your Azure Credentials

1. **Get Azure OpenAI Access**:
   - You need an Azure subscription with access to Azure OpenAI Service
   - You need access to the Sora-2 model specifically
   - Get your API key and resource name from the Azure Portal

2. **Create Your Configuration File**:
   - In the `python` folder, you'll find a file named `.env.sample`
   - Make a copy of this file and rename it to `.env`
   
   On Windows PowerShell (make sure you're in the `python` folder):
   ```powershell
   Copy-Item .env.sample .env
   ```

3. **Edit the `.env` File**:
   - Open `.env` in any text editor (Notepad, VS Code, etc.)
   - Replace the placeholder values with your actual Azure credentials:
   
   ```
   AZURE_API_KEY=your-actual-api-key-here
   AZURE_RESOURCE_NAME=your-actual-resource-name-here
   ```

   **Example**:
   ```
   AZURE_API_KEY=abc123def456ghi789jkl012mno345pqr678
   AZURE_RESOURCE_NAME=aoai-test-swedencentral-001
   ```

### Step 5: Prepare Your Reference Image (Image-to-Video Only)

**If you're using `sora2-image-to-video-sample.py`**, you'll need a reference image to animate. By default, it looks for a file named `horses-1280x720.jpg`.

**Important Requirements**:
- The image must be in JPEG, PNG, or WebP format
- The image dimensions must match the `size` parameter in the script (default: 1280x720 pixels)
- Supported sizes: `720x1280`, `1280x720`, `1024x1792`, `1792x1024`

**To use your own image**:
1. Place your image in the `python` folder
2. Open `sora2-image-to-video-sample.py` in a text editor
3. Find the line that says `with open("horses-1280x720.jpg", "rb") as image_file:`
4. Replace `horses-1280x720.jpg` with your image filename

**If you're using `sora2-text-to-video-sample.py`**, you can skip this step as it doesn't require a reference image.

### Step 6: Run a Script

Open PowerShell in the `python` folder and run one of the following:

**For Text-to-Video:**
```powershell
python sora2-text-to-video-sample.py
```

**For Image-to-Video:**
```powershell
python sora2-image-to-video-sample.py
```

## 🔄 How It Works

Video generation is an asynchronous process. You create a job request with your text prompt and video format specifications, and the model processes the request in the background. You can check the status of the video generation job and, once it finishes, retrieve the generated video through a download URL.

### Complete Workflow Example

Here's a typical workflow for generating and managing videos:

**Step 1: Generate a video**
```powershell
python sora2-text-to-video-sample.py
# Output: Video ID: video_abc123...
```

**Step 2: Check the status**
```powershell
python sora2-video-creation-status.py video_abc123...
# Polls every 20 seconds until complete
```

**Step 3: List all your videos**
```powershell
python sora2-get-generated-videos.py
# Shows all videos with their IDs and statuses
```

**Step 4: Download your video (optional)**
Once completed, you can use the download script to save the video locally.

**Step 5: Clean up (optional)**
```powershell
python sora2-delete-generated-video.py video_abc123...
# Removes the video from Azure storage
```

**Note:** Remember that jobs are available for up to 24 hours after creation. Download any videos you want to keep before they expire or before deleting them.

## � What to Expect

When you run either script, you'll see:

1. **Request Details**: Information about what's being sent to the API
2. **Status Code**: 200 or 201 means success!
3. **Response**: The API's response with video generation details

**Example Output (Text-to-Video)**:
```
Sending video generation request to Azure OpenAI Sora-2...
Prompt: Create a 4‑second, photoreal cinematic shot. In a sunlit, minimalist café...
Duration: 4 seconds
Resolution: 1280x720
------------------------------------------------------------
Status Code: 200
Response: {...}

✓ Video generation started successfully!

Video Generation Details:
  {'id': 'video-abc123...', 'status': 'processing', ...}

Note: Video generation is an asynchronous process.
The video may take several minutes to complete.
You can check the status using the video ID returned in the response.
```

**Example Output (Image-to-Video)**:
```
Sending image-to-video generation request to Azure OpenAI Sora-2...
Reference Image: horses-1280x720.jpg
Duration: 8 seconds
Resolution: 1280x720
Prompt: Reference image: the uploaded photo of three chestnut horses cantering...
------------------------------------------------------------
Status Code: 200
Response: {...}

✓ Video generation started successfully!

Video Generation Details:
  {'id': 'video-abc123...', 'status': 'processing', ...}

Note: Video generation is an asynchronous process.
The video may take several minutes to complete.
You can check the status using the video ID returned in the response.
```

## ⚙️ Customization Options

You can customize the video generation by editing the parameters in either script.

### Text-to-Video Parameters (`sora2-text-to-video-sample.py`)

```python
data = {
    "prompt": "Your detailed description here...",  # Describe what you want to see
    "model": "sora-2",                              # AI model to use
    "size": "1280x720",                             # Video resolution
    "seconds": "4",                                 # Video duration (4, 8, or 12)
}
```

### Image-to-Video Parameters (`sora2-image-to-video-sample.py`)

```python
form_data = {
    "prompt": "Your detailed description here...",  # Describe the animation
    "model": "sora-2",                              # AI model to use
    "size": "1280x720",                             # Video resolution
    "seconds": "8",                                 # Video duration (4, 8, or 12)
}
```

### Writing Good Prompts

**Best Practices:**
- Write text prompts in English or other Latin script languages for the best video generation performance.

**For Text-to-Video**, include:
- Shot type (close-up, wide shot, aerial view)
- Subject description (a tabby cat, a young woman)
- Action (riding, walking, dancing)
- Scene setting (cyberpunk city, beach at sunset)
- Lighting (golden hour, neon lights, soft morning light)
- Camera motion (pans left, slow zoom in)

**For Image-to-Video**, include:
- Reference to the uploaded image
- Desired action and motion
- Timing details (what happens when)
- Camera movement description
- Lighting and environmental details
- Physics constraints for realism
- What NOT to do (to avoid artifacts)

## ⚠️ Limitations

### Content Quality Limitations
Sora might have difficulty with:
- Complex physics
- Causal relationships (for example, bite marks on a cookie)
- Spatial reasoning (for example, knowing left from right)
- Precise time-based event sequencing such as camera movement

### Sora 2 Technical Limitations
- Jobs are available for up to 24 hours after they're created. After that, you must create a new job to generate the video again.
- You can have two video creation jobs running at the same time. You must wait for one of the jobs to finish before you can create another.
- Supported output resolution dimensions: `720x1280`, `1280x720`, `1024x1792`, `1792x1024`
- Video duration options: 4, 8, or 12 seconds

### Sora 1 Technical Limitations
- Sora supports the following output resolution dimensions: `480x480`, `480x854`, `854x480`, `720x720`, `720x1280`, `1280x720`, `1080x1080`, `1080x1920`, `1920x1080`
- Sora can produce videos between 1 and 20 seconds long
- You can request multiple video variants in a single job:
  - For 1080p resolutions: this feature is disabled
  - For 720p: maximum of two variants
  - For other resolutions: maximum of four variants
- You can have two video creation jobs running at the same time. You must wait for one of the jobs to finish before you can create another.
- Jobs are available for up to 24 hours after they're created. After that, you must create a new job to generate the video again.
- You can use up to two images as input (the generated video interpolates content between them)
- You can use one video up to five seconds as input

### Responsible AI
Sora has a robust safety stack that includes content filtering, abuse monitoring, sensitive content blocking, and safety classifiers.

- Sora doesn't generate scenes with acts of violence but can generate adjacent content, such as realistic war-like footage.

## 🐛 Troubleshooting

### "Python is not recognized..."
- You need to install Python or add it to your PATH
- Restart PowerShell after installing Python

### "pip is not recognized..."
- Try using `python -m pip install requests python-dotenv` instead

### "Missing required environment variables"
- Make sure you created the `.env` file (not `.env.sample`)
- Check that your API key and resource name are correct

### "FileNotFoundError: [Errno 2] No such file or directory: 'horses-1280x720.jpg'"
- This error is specific to `sora2-image-to-video-sample.py`
- Make sure your reference image is in the `python` folder
- Check the filename matches exactly (including the extension)

### "Error: 401" or "Error: 403"
- Your API key may be incorrect
- Your Azure resource name may be wrong
- You may not have access to Sora-2 on your subscription

### "Error: 400"
- Check that your image dimensions match the `size` parameter exactly (for Image-to-Video)
- Verify your image is in a supported format: JPEG, PNG, WebP (for Image-to-Video)
- Make sure your prompt and parameters are valid
- Check that the video duration is one of the supported values: 4, 8, or 12 seconds

## 📝 Requirements Summary

- **Python**: 3.7 or newer
- **Python Packages**: 
  - `requests`
  - `python-dotenv`
- **Azure**: 
  - Azure OpenAI subscription
  - Access to Sora-2 model
  - API key and resource name
- **Reference Image** (for Image-to-Video only): 
  - JPEG, PNG, or WebP format
  - Dimensions matching the `size` parameter

## 📁 Files in This Folder

**Core Generation Scripts:**
- `sora2-text-to-video-sample.py` - Generate videos from text prompts only
- `sora2-image-to-video-sample.py` - Generate videos from reference images with text prompts

**Utility Scripts:**
- `sora2-video-creation-status.py` - Check the status of video generation jobs (polls until complete)
- `sora2-get-generated-videos.py` - List all generated videos in your Azure account
- `sora2-download-generated-video.py` - Download completed videos to your local machine
- `sora2-delete-generated-video.py` - Delete videos from Azure storage

**Configuration:**
- `.env.sample` - Template for environment variables
- `.env` - Your actual credentials (create from .env.sample, not tracked in Git)
- `.gitignore` - Git ignore rules

**Documentation:**
- `README.md` - This file (comprehensive guide to all samples)

## 📚 Additional Resources

- [Azure OpenAI Service Documentation](https://learn.microsoft.com/en-us/azure/ai-services/openai/)
- [Python Official Documentation](https://docs.python.org/)
- [Python for Beginners](https://www.python.org/about/gettingstarted/)

## 🤝 Support

If you encounter issues:
1. Check the troubleshooting section above
2. Verify all requirements are met
3. Review the Azure OpenAI service status
4. Check the console output for specific error messages

## 📄 License

Copyright © Microsoft Corporation. All rights reserved.

---

**Happy Video Generating! 🎬✨**
