# Sora-2 .NET Samples

This folder contains .NET samples demonstrating how to use Azure OpenAI's Sora-2 model to generate videos. Sora-2 is a state-of-the-art AI model that creates realistic and imaginative video content.

> **✨ Recent Updates (November 2025):** The API endpoint structure and authentication have been verified to work with Azure OpenAI. The samples use the correct endpoint format (`/openai/v1/videos`) and proper JSON serialization with snake_case naming conventions.

## 📋 Available Samples

### Core Generation Projects

#### 1. Text-to-Video (`Sora2TextToVideo`)
Generate videos from text descriptions only. Simply describe what you want to see, and Sora-2 will create it.

**Example Use Cases:**
- "A red panda making latte art in a café"
- "A woman walking through a neon-lit cyberpunk city"
- "Ocean waves crashing on a beach at sunset"

**Usage:**
```bash
dotnet run --project Sora2TextToVideo
```

#### 2. Image-to-Video (`Sora2ImageToVideo`)
Animate a reference image based on your text description. Upload a photo and describe how you want it to move.

**Example Use Cases:**
- Make horses in a photo jump over a fence
- Animate a portrait to smile and wave
- Make a still landscape scene come to life with moving clouds and water

**Usage:**
```bash
dotnet run --project Sora2ImageToVideo
```

### Utility Projects

#### 3. Video Creation Status (`Sora2VideoCreationStatus`)
Check the status of a video generation job. This application polls the Azure OpenAI API every 20 seconds until the video is completed, failed, or cancelled.

**Usage:**
```bash
dotnet run --project Sora2VideoCreationStatus -- <video_id>
```

**Example:**
```bash
dotnet run --project Sora2VideoCreationStatus -- video_6901017960dc8190a6f7a19cd8f48976
```

**What it does:**
- Retrieves the current status of your video generation job
- Automatically polls every 20 seconds until completion
- Displays final video details when processing is complete
- Shows if the job failed or was cancelled

#### 4. Get Generated Videos (`Sora2GetGeneratedVideos`)
Retrieve a list of all your generated videos from Azure OpenAI's Sora-2 model.

**Usage:**
```bashx
dotnet run --project Sora2GetGeneratedVideos
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

#### 5. Download Generated Video (`Sora2DownloadGeneratedVideo`)
Download a completed video to your local machine.

**Usage:**
```bash
dotnet run --project Sora2DownloadGeneratedVideo -- <video_id> <output_file_path>
```

**Examples:**
```bash
dotnet run --project Sora2DownloadGeneratedVideo -- video_68fa7f59f2588190a48ea2b0aa73d844 output.mp4
dotnet run --project Sora2DownloadGeneratedVideo -- video_68fa7f59f2588190a48ea2b0aa73d844 videos/red_panda_latte.mp4
```

**What it does:**
- Downloads the video file from Azure OpenAI
- Creates output directories if they don't exist
- Saves the video to the specified location
- Reports file size and location

#### 6. Delete Generated Video (`Sora2DeleteGeneratedVideo`)
Delete a specific video from your Azure OpenAI account by video ID.

**Usage:**
```bash
dotnet run --project Sora2DeleteGeneratedVideo -- <video_id>
```

**Example:**
```bash
dotnet run --project Sora2DeleteGeneratedVideo -- video_6901017960dc8190a6f7a19cd8f48976
```

**What it does:**
- Permanently deletes a video from Azure OpenAI storage
- Confirms successful deletion
- Helps manage your storage and video inventory

**Important:** Deletion is permanent and cannot be undone. Make sure to download any videos you want to keep before deleting them.

## 🚀 Getting Started (For First-Time .NET Users)

### Step 1: Install .NET

If you don't have .NET installed:

1. Go to [dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
2. Download .NET 9.0 SDK or newer (recommended: .NET 9.0+)
3. Complete the installation
4. Restart your terminal/command prompt after installation

To verify .NET is installed, open your terminal and type:
```bash
dotnet --version
```

You should see something like `9.0.xxx`

### Step 2: Download This Project

1. Download this repository as a ZIP file, or
2. If you have Git installed, clone it:
   ```bash
   git clone <repository-url>
   cd aoai-sora-2-model-startup-samples/dotnet
   ```

### Step 3: Restore NuGet Packages

The projects reference the Azure.AI.OpenAI NuGet package (though the samples use direct HTTP calls). Restore dependencies by running:

```bash
dotnet restore
```

This will download all required packages and dependencies.

### Step 4: Set Up Your Azure Credentials

1. **Get Azure OpenAI Access**:
   - You need an Azure subscription with access to Azure OpenAI Service
   - You need access to the Sora-2 model specifically
   - Get your API key and resource name from the Azure Portal

2. **Create Your Configuration File**:
   - In the `dotnet` folder, you'll find a file named `.env.sample`
   - Make a copy of this file and rename it to `.env`
   
   On Windows PowerShell (make sure you're in the `dotnet` folder):
   ```powershell
   Copy-Item .env.sample .env
   ```
   
   On Linux/macOS:
   ```bash
   cp .env.sample .env
   ```

3. **Edit the `.env` File**:
   - Open `.env` in any text editor (Notepad, VS Code, etc.)
   - Replace the placeholder values with your actual Azure credentials:
   
   ```
   AZURE_API_KEY=your-actual-api-key-here
   AZURE_RESOURCE_NAME=your-actual-resource-name-here
   ```

   **For Azure OpenAI Example**:
   ```
   AZURE_API_KEY=abc123def456ghi789jkl012mno345pqr678
   AZURE_RESOURCE_NAME=aoai-test-swedencentral-001.openai.azure.com
   ```
   
   **For AI Foundry Example**:
   ```
   AZURE_API_KEY=abc123def456ghi789jkl012mno345pqr678
   AZURE_RESOURCE_NAME=my-project.region.models.ai.azure.com
   ```
   
   **Note**: The applications automatically construct the appropriate endpoint URL and will display it when you run them, making it easy to verify that you're connecting to the correct service (AI Foundry or Azure OpenAI).

### Step 5: Prepare Your Reference Image (Image-to-Video Only)

**If you're using `Sora2ImageToVideo`**, you'll need a reference image to animate. A sample image `horses-1280x720.jpg` is included in the `Sora2ImageToVideo` project folder and will be automatically copied to the output directory when you build the project.

**Important Requirements**:
- The image must be in JPEG, PNG, or WebP format
- The image dimensions must match the `size` parameter in the code (default: 1280x720 pixels)
- Supported sizes: `720x1280`, `1280x720`, `1024x1792`, `1792x1024`

**To use your own image**:
1. Place your image in the `Sora2ImageToVideo` folder (next to `Program.cs`)
2. Update the `.csproj` file to include your image:
   ```xml
   <ItemGroup>
     <None Update="your-image.jpg">
       <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
     </None>
   </ItemGroup>
   ```
3. Open `Sora2ImageToVideo/Program.cs` in a text editor
4. Find the line that says `var imageFilename = "horses-1280x720.jpg";`
5. Replace `horses-1280x720.jpg` with your image filename

**Note:** The image will be automatically copied to the output directory (`bin/Debug/net9.0/`) when you build the project, so the program can find it at runtime.

**If you're using `Sora2TextToVideo`**, you can skip this step as it doesn't require a reference image.

### Step 6: Build the Solution

Before running any project, build the solution to ensure everything compiles correctly:

```bash
dotnet build
```

### Step 7: Run a Project

From the `dotnet` folder, run one of the following:

**For Text-to-Video:**
```bash
dotnet run --project Sora2TextToVideo
```

**For Image-to-Video:**
```bash
dotnet run --project Sora2ImageToVideo
```

## 🔄 How It Works

Video generation is an asynchronous process. You create a job request with your text prompt and video format specifications, and the model processes the request in the background. You can check the status of the video generation job and, once it finishes, retrieve the generated video through a download.

**Endpoint Visibility**: All applications now display the full endpoint URL before making API calls, making it easy to verify connections to either Azure OpenAI or AI Foundry services.

### Complete Workflow Example

Here's a typical workflow for generating and managing videos:

**Step 1: Generate a video**
```bash
dotnet run --project Sora2TextToVideo
# Output: Video ID: video_abc123...
```

**Step 2: Check the status**
```bash
dotnet run --project Sora2VideoCreationStatus -- video_abc123...
# Polls every 20 seconds until complete
```

**Step 3: List all your videos**
```bash
dotnet run --project Sora2GetGeneratedVideos
# Shows all videos with their IDs and statuses
```

**Step 4: Download your video**
```bash
dotnet run --project Sora2DownloadGeneratedVideo -- video_abc123... output.mp4
# Downloads the completed video to your local machine
```

**Step 5: Clean up (optional)**
```bash
dotnet run --project Sora2DeleteGeneratedVideo -- video_abc123...
# Removes the video from Azure storage
```

**Note:** Remember that jobs are available for up to 24 hours after creation. Download any videos you want to keep before they expire or before deleting them.

## 📊 What to Expect

When you run either generation project, you'll see:

1. **Request Details**: Information about what's being sent to the API
2. **Success Message**: Confirmation that the request was received
3. **Video Details**: The video ID, status, and creation timestamp

**Example Output (Text-to-Video)**:
```
Sending video generation request to Azure OpenAI Sora-2...
Prompt: Create a 4-second, photoreal cinematic shot. In a sunlit, minimalist café...
Duration: 4 seconds
Resolution: 1280x720
------------------------------------------------------------
Endpoint URL: https://aoai-test-swedencentral-001.openai.azure.com/openai/v1/videos
------------------------------------------------------------

✓ Video generation started successfully!

Video Generation Details:
  Video ID: video_abc123...
  Status: Queued
  Created At: 10/28/2025 10:30:00 AM

Note: Video generation is an asynchronous process.
The video may take several minutes to complete.
You can check the status using: dotnet run --project Sora2VideoCreationStatus -- video_abc123...
```

**Example Output (Image-to-Video)**:
```
Sending image-to-video generation request to Azure OpenAI Sora-2...
Reference Image: horses-1280x720.jpg
Duration: 8 seconds
Resolution: 1280x720
Prompt: Reference image: the uploaded photo of three chestnut horses cantering...
------------------------------------------------------------
Endpoint URL: https://aoai-test-swedencentral-001.openai.azure.com/openai/v1/videos
------------------------------------------------------------

✓ Video generation started successfully!

Video Generation Details:
  Video ID: video_abc123...
  Status: Queued
  Created At: 10/28/2025 10:30:00 AM

Note: Video generation is an asynchronous process.
The video may take several minutes to complete.
You can check the status using: dotnet run --project Sora2VideoCreationStatus -- video_abc123...
```

## ⚙️ Customization Options

You can customize the video generation by editing the code in the respective Program.cs files.

### Text-to-Video Parameters (`Sora2TextToVideo/Program.cs`)

```csharp
var prompt = "Your detailed description here...";  // Describe what you want to see
var size = "1280x720";                             // Video resolution
var seconds = "4";                                 // Video duration as string: "4", "8", or "12"
```

### Image-to-Video Parameters (`Sora2ImageToVideo/Program.cs`)

```csharp
var prompt = "Your detailed description here...";  // Describe the animation
var imageFilename = "horses-1280x720.jpg";         // Reference image filename
var size = "1280x720";                             // Video resolution
var seconds = "8";                                 // Video duration as string: "4", "8", or "12"
```

### Available Video Sizes

Choose one of the following size strings:
- `"720x1280"` - Portrait orientation (9:16 aspect ratio)
- `"1280x720"` - Landscape orientation (16:9 aspect ratio)
- `"1024x1792"` - Portrait orientation (taller format)
- `"1792x1024"` - Landscape orientation (wider format)

### Available Video Durations

⚠️ **Important:** Duration must be a **string value**, not an integer.

Choose one of the following duration strings:
- `"4"` - 4 seconds
- `"8"` - 8 seconds
- `"12"` - 12 seconds

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

### Responsible AI
Sora has a robust safety stack that includes content filtering, abuse monitoring, sensitive content blocking, and safety classifiers.

- Sora doesn't generate scenes with acts of violence but can generate adjacent content, such as realistic war-like footage.

## 🐛 Troubleshooting

### ".NET SDK is not found" or "dotnet is not recognized"
- You need to install the .NET SDK
- Restart your terminal after installing .NET
- Make sure .NET is added to your PATH

### "Missing required environment variables"
- Make sure you created the `.env` file (not `.env.sample`)
- Check that your API key and resource name are correct
- Ensure the `.env` file is in the `dotnet` folder

### "FileNotFoundException" for horses-1280x720.jpg
- This error is specific to `Sora2ImageToVideo`
- The sample image `horses-1280x720.jpg` should be automatically copied to the output directory when you build the project
- If you still get this error, try rebuilding the project: `dotnet build Sora2ImageToVideo/Sora2ImageToVideo.csproj`
- Check the filename matches exactly (including the extension)
- Verify the image file exists in the `Sora2ImageToVideo` project folder
- Make sure the `.csproj` file includes the `<None Update>` element for your image file

### "Error: 401" or "Error: 403"
- Your API key may be incorrect
- Your Azure resource name may be wrong
- You may not have access to Sora-2 on your subscription

### "Error: 400"
- **Most common issue:** Make sure `seconds` is a **string** (e.g., `"4"`) not an integer (e.g., `4`)
- **For Image-to-Video:** The multipart form field name must be `"input_reference"`, not `"image"`
- Check that your image dimensions match the `size` parameter exactly (for Image-to-Video)
- Verify your image is in a supported format: JPEG, PNG, WebP (for Image-to-Video)
- Make sure your prompt and parameters are valid
- Check that the video duration is one of the supported string values: `"4"`, `"8"`, or `"12"`
- Verify the `size` parameter uses one of the exact supported resolutions
- **Important:** Input images with human faces are currently rejected by the API

### "Error: 404" or "Not Found"
- The Azure OpenAI endpoint should be: `https://{resourceName}.openai.azure.com/openai/v1/videos`
- Verify your `AZURE_RESOURCE_NAME` is correct (e.g., `aoai-test-swedencentral-001`)
- Check that the Sora-2 model is deployed in your Azure region

### HTTP Request Failures or Timeouts
- Video generation can take several minutes; use `Sora2VideoCreationStatus` to poll for completion
- Ensure you're not hitting rate limits (max 2 concurrent video jobs)
- Check your network connection and firewall settings
- Verify the Azure OpenAI service is available in your region

### Build Errors
If you get compilation errors:
- Make sure you ran `dotnet restore` to download all dependencies
- Verify you're using .NET 9.0 or later
- Check that the Azure.AI.OpenAI package version 2.1.0-beta.2 was installed correctly

## 🔧 Technical Implementation Notes

### API Endpoint Structure
The samples use the Azure OpenAI REST API directly with `HttpClient`. Key implementation details:

**Endpoint Format:**
```
https://{resourceName}.openai.azure.com/openai/v1/videos
```

**Authentication:**
- Header: `api-key: {your-api-key}`
- The samples use Azure OpenAI authentication (not OpenAI's `Authorization: Bearer` format)

**JSON Serialization:**
- Uses `System.Text.Json` with `JsonNamingPolicy.SnakeCaseLower` for proper snake_case conversion
- This ensures API parameters like `created_at` are correctly formatted

**Multipart Form Data (Image-to-Video):**
- The image field name is `"input_reference"` (as per OpenAI Sora 2 API specification)
- Supports JPEG, PNG, and WebP formats with automatic content-type detection
- Image dimensions must exactly match the `size` parameter

### Async Video Generation Flow
1. **Create Request**: POST to `/videos` returns a job ID immediately
2. **Poll Status**: GET `/videos/{id}` to check status (queued → processing → completed)
3. **Download**: GET `/videos/{id}/content?variant=video` retrieves the final video
4. **Cleanup**: DELETE `/videos/{id}` removes the video from Azure storage

## 📝 Requirements Summary

- **.NET SDK**: 9.0 or newer
- **NuGet Packages**: 
  - `Azure.AI.OpenAI` (version 2.1.0-beta.2) - Referenced but not required; samples use direct HTTP calls
  - Standard .NET libraries: `System.Net.Http`, `System.Text.Json`
- **Azure**: 
  - Azure OpenAI subscription
  - Access to Sora-2 model
  - API key and resource name
- **Reference Image** (for Image-to-Video only): 
  - JPEG, PNG, or WebP format
  - Dimensions matching the `size` parameter

**Note:** These samples use direct `HttpClient` calls to the Azure OpenAI REST API rather than the SDK, providing more control and transparency over the API interactions.

## 📁 Files in This Folder

**Core Generation Projects:**
- `Sora2TextToVideo/` - Generate videos from text prompts only
- `Sora2ImageToVideo/` - Generate videos from reference images with text prompts

**Utility Projects:**
- `Sora2VideoCreationStatus/` - Check the status of video generation jobs (polls until complete)
- `Sora2GetGeneratedVideos/` - List all generated videos in your Azure account
- `Sora2DownloadGeneratedVideo/` - Download completed videos to your local machine
- `Sora2DeleteGeneratedVideo/` - Delete videos from Azure storage

**Configuration:**
- `.env.sample` - Template for environment variables
- `.env` - Your actual credentials (create from .env.sample, not tracked in Git)
- `.gitignore` - Git ignore rules
- `Sora2Samples.sln` - Visual Studio solution file

**Documentation:**
- `README.md` - This file (comprehensive guide to all samples)

## 📚 Additional Resources

- [Azure OpenAI Service Documentation](https://learn.microsoft.com/en-us/azure/ai-services/openai/)
- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Azure SDK for .NET](https://azure.github.io/azure-sdk-for-net/)

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
