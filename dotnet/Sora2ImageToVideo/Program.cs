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
var modelName = Environment.GetEnvironmentVariable("AZURE_MODEL_NAME");

if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(resourceName) || string.IsNullOrEmpty(modelName))
{
    throw new InvalidOperationException(
        "Missing required environment variables. " +
        "Please copy .env.sample to .env and populate with your values.");
}

// ============================================================================
// Video Generation Parameters
// ============================================================================
var prompt = @"Create a cinematic video starting from the uploaded image of three horses 
running along a beach. Gradually transform the scene: the calm ocean begins to split open, 
forming a massive circular opening in the sea. Water cascades dramatically into the deep abyss
 below, creating a waterfall effect inside the ocean. The sandy ground near the horses cracks
  open, revealing the edge of the abyss. The horses leap gracefully over the crack in slow 
  motion, their manes flowing in the wind, droplets of water sparkling in the sunlight. 
  The sky remains partly cloudy with soft natural light, adding a sense of epic wonder and 
  surreal beauty to the scene. Maintain realistic physics and fluid motion for water and 
  horses, with high cinematic detail and smooth transitions.";

var imageFilename = "horses-1280x720.jpg";
var size = "1280x720";
var seconds = "8";

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

    var baseUrl = $"https://{resourceName}/openai/v1";
    
    Console.WriteLine($"Endpoint URL: {baseUrl}/videos");
    Console.WriteLine(new string('-', 60));

    // Create multipart form data
    using var formData = new MultipartFormDataContent();
    
    // Add image file
    var imageBytes = await File.ReadAllBytesAsync(imageFilename);
    var imageContent = new ByteArrayContent(imageBytes);
    
    // Determine content type based on file extension
    var extension = Path.GetExtension(imageFilename).ToLower();
    var contentType = extension switch
    {
        ".jpg" or ".jpeg" => "image/jpeg",
        ".png" => "image/png",
        ".webp" => "image/webp",
        _ => "image/jpeg"
    };
    imageContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
    formData.Add(imageContent, "input_reference", imageFilename);
    
    // Add other parameters
    formData.Add(new StringContent(modelName), "model");
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
