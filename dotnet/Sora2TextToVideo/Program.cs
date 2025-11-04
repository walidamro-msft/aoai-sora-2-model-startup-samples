/*
 * Sora-2 Text-to-Video Generation Sample
 * 
 * This application demonstrates how to use the Azure OpenAI Sora-2 model to generate videos
 * from text prompts. Sora-2 is a state-of-the-art AI model that creates realistic and
 * imaginative video content based on natural language descriptions.
 * 
 * Requirements:
 *     - .NET 9.0 or later
 *     - Valid Azure OpenAI API key and endpoint in .env file
 * 
 * Usage:
 *     dotnet run --project Sora2TextToVideo
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

// Get configuration from environment variables
var apiKey = Environment.GetEnvironmentVariable("AZURE_API_KEY");
var resourceName = Environment.GetEnvironmentVariable("AZURE_RESOURCE_NAME");

if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(resourceName))
{
    throw new InvalidOperationException(
        "Missing required environment variables. " +
        "Please copy .env.sample to .env and populate with your values.");
}

// Video generation parameters
var prompt = @"Create a 4-second, photoreal cinematic shot. In a sunlit, minimalist café, a red panda stands on a sturdy stool behind a stainless-steel espresso bar and pours latte art into a ceramic cup. The animal's movement is precise and confident—unexpectedly expert.
Camera: 35 mm lens, close-medium framing, gentle push-in (dolly) from right to left; slight handheld micro-jitter for realism.
Action beat: In one smooth motion, the red panda tilts the pitcher and draws a crisp rosetta pattern; steam swirls rise; a micro-splash lands on the saucer (shallow depth of field catches droplets in bokeh).
Lighting & look: Late-morning natural light, soft shadows, warm color temperature; high-detail fur, whiskers, and reflective metal surfaces; subsurface scattering on ears; subtle condensation on the milk pitcher.
Environment cues: Background defocused café plants and wood grain; no logos, no text, no humans.
Tone: Wholesome, delightful, subtly humorous; no slapstick.
End frame (hold ~0.4s): The red panda sets the cup down, proud head tilt to camera as steam curls upward.
Safety & realism: Anatomically plausible proportions; stable footing; appropriate utensil sizes.";

var size = "1280x720";
var seconds = "4";

Console.WriteLine("Sending video generation request to Azure OpenAI Sora-2...");
Console.WriteLine($"Prompt: {prompt}");
Console.WriteLine($"Duration: {seconds} seconds");
Console.WriteLine($"Resolution: {size}");
Console.WriteLine(new string('-', 60));

try
{
    using var httpClient = new HttpClient();
    httpClient.DefaultRequestHeaders.Add("api-key", apiKey);

    var baseUrl = $"https://{resourceName}.openai.azure.com/openai/v1";
    
    var jsonOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        WriteIndented = false
    };
    
    var requestBody = new
    {
        model = "sora-2",
        prompt = prompt,
        size = size,
        seconds = seconds
    };

    var requestContent = new StringContent(
        JsonSerializer.Serialize(requestBody, jsonOptions),
        System.Text.Encoding.UTF8,
        "application/json"
    );

    var response = await httpClient.PostAsync($"{baseUrl}/videos", requestContent);
    response.EnsureSuccessStatusCode();

    var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>();
    
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
catch (Exception e)
{
    Console.WriteLine($"\n✗ Error: {e.Message}");
    Console.WriteLine($"Error type: {e.GetType().Name}");
    Console.WriteLine("\nTroubleshooting:");
    Console.WriteLine("  - Verify your API key is correct");
    Console.WriteLine("  - Check that your Azure resource name is correct");
    Console.WriteLine("  - Ensure your Azure subscription has access to Sora-2");
    Console.WriteLine("  - Verify the prompt and parameters are valid");
}
