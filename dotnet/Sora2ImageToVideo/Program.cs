/*
 * Sora-2 Image-to-Video Generation Sample
 * 
 * This application demonstrates how to use the Azure OpenAI Sora-2 model to generate videos
 * from a reference image and text prompt. Sora-2 can take a still image and animate it
 * based on your description, creating realistic and imaginative video content.
 * 
 * Requirements:
 *     - .NET 9.0 or later
 *     - Valid Azure OpenAI API key and endpoint
 *     - Reference image file (JPEG, PNG, or WebP format)
 * 
 * Usage:
 *     1. Copy .env.sample to .env and populate with your Azure credentials
 *     2. Place your reference image in the same directory as this project
 *     3. Update the image filename and parameters as needed
 *     4. Run: dotnet run --project Sora2ImageToVideo
 * 
 * Author: Microsoft Corporation
 * Date: October 2025
 */

using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

// Load environment variables from .env file
var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
if (File.Exists(envPath))
{
    foreach (var line in File.ReadAllLines(envPath))
    {
        if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
            continue;

        var parts = line.Split('=', 2);
        if (parts.Length == 2)
        {
            Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
        }
    }
}

// ============================================================================
// API Configuration
// ============================================================================
var apiKey = Environment.GetEnvironmentVariable("AZURE_API_KEY");
var resourceName = Environment.GetEnvironmentVariable("AZURE_RESOURCE_NAME");

if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(resourceName))
{
    throw new InvalidOperationException(
        "Missing required environment variables. " +
        "Please copy .env.sample to .env and populate with your values.");
}

// ============================================================================
// Video Generation Parameters
// ============================================================================
var prompt = @"Reference image: the uploaded photo of three chestnut horses cantering in shallow surf under an overcast sky (lead horse with a white blaze in the foreground, two horses trailing left-of-frame).
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
Ending frame: lead horse fully landed, mid-canter, suitable to loop with a subtle cross-fade.";

var imageFilename = "horses-1280x720.jpg";
var size = "1280x720";
var seconds = 8;

// ============================================================================
// Send Image-to-Video Generation Request
// ============================================================================
Console.WriteLine("Sending image-to-video generation request to Azure OpenAI Sora-2...");
Console.WriteLine($"Reference Image: {imageFilename}");
Console.WriteLine($"Duration: {seconds} seconds");
Console.WriteLine($"Resolution: {size}");
Console.WriteLine($"Prompt: {prompt[..100]}...");
Console.WriteLine(new string('-', 60));

try
{
    // Read the reference image file
    if (!File.Exists(imageFilename))
    {
        throw new FileNotFoundException($"Image file '{imageFilename}' not found!");
    }

    using var httpClient = new HttpClient();
    httpClient.DefaultRequestHeaders.Add("api-key", apiKey);

    var baseUrl = $"https://{resourceName}.openai.azure.com/openai/v1";

    // Create multipart form data
    using var formData = new MultipartFormDataContent();
    
    // Add image file
    var imageBytes = await File.ReadAllBytesAsync(imageFilename);
    var imageContent = new ByteArrayContent(imageBytes);
    imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
    formData.Add(imageContent, "input_reference", imageFilename);
    
    // Add other parameters
    formData.Add(new StringContent("sora-2"), "model");
    formData.Add(new StringContent(prompt), "prompt");
    formData.Add(new StringContent(size), "size");
    formData.Add(new StringContent(seconds.ToString()), "seconds");

    var response = await httpClient.PostAsync($"{baseUrl}/videos", formData);
    response.EnsureSuccessStatusCode();

    var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>();

    // ============================================================================
    // Process API Response
    // ============================================================================
    Console.WriteLine("\n✓ Video generation started successfully!");
    Console.WriteLine("\nVideo Generation Details:");
    Console.WriteLine($"  Video ID: {jsonResponse.GetProperty("id").GetString()}");
    Console.WriteLine($"  Status: {jsonResponse.GetProperty("status").GetString()}");
    Console.WriteLine($"  Model: {jsonResponse.GetProperty("model").GetString()}");
    Console.WriteLine($"  Created At: {DateTimeOffset.FromUnixTimeSeconds(jsonResponse.GetProperty("created_at").GetInt64()).DateTime}");
    Console.WriteLine("\nNote: Video generation is an asynchronous process.");
    Console.WriteLine("The video may take several minutes to complete.");
    Console.WriteLine($"You can check the status using: dotnet run --project Sora2VideoCreationStatus -- {jsonResponse.GetProperty("id").GetString()}");
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"\n✗ Error: {ex.Message}");
    Console.WriteLine("Please ensure the reference image exists in the same directory as this project.");
}
catch (Exception e)
{
    Console.WriteLine($"\n✗ Error: {e.Message}");
    Console.WriteLine($"Error type: {e.GetType().Name}");
    Console.WriteLine("\nTroubleshooting:");
    Console.WriteLine("  - Verify your API key is correct");
    Console.WriteLine("  - Check that your Azure resource name is correct");
    Console.WriteLine("  - Ensure your Azure subscription has access to Sora-2");
    Console.WriteLine("  - Verify the reference image file exists and matches the size parameter");
    Console.WriteLine("  - Check that the image dimensions match the 'size' parameter exactly");
}
