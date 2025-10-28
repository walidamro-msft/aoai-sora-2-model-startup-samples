/*
 * Sora-2 Get Generated Videos Sample
 * 
 * This application retrieves a list of all generated videos from Azure OpenAI's Sora-2 model.
 * 
 * Requirements:
 *     - .NET 8.0 or later
 *     - Valid Azure OpenAI API key and endpoint in .env file
 * 
 * Usage:
 *     dotnet run --project Sora2GetGeneratedVideos
 * 
 * Author: Microsoft Corporation
 * Date: October 2025
 */

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

var apiKey = Environment.GetEnvironmentVariable("AZURE_API_KEY");
var resourceName = Environment.GetEnvironmentVariable("AZURE_RESOURCE_NAME");

if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(resourceName))
{
    throw new InvalidOperationException("Missing required environment variables. Please check your .env file.");
}

using var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("api-key", apiKey);

var baseUrl = $"https://{resourceName}.openai.azure.com/openai/v1";

Console.WriteLine("Retrieving generated videos from Azure OpenAI Sora-2...");
Console.WriteLine(new string('=', 60));

try
{
    // List all videos
    var response = await httpClient.GetAsync($"{baseUrl}/videos");
    response.EnsureSuccessStatusCode();

    var videosResponse = await response.Content.ReadFromJsonAsync<JsonElement>();

    Console.WriteLine("\n✓ Successfully retrieved videos list!");
    Console.WriteLine("\n" + new string('=', 60));
    Console.WriteLine("Videos:");
    Console.WriteLine(new string('=', 60));

    // Pretty print the response
    var options = new JsonSerializerOptions { WriteIndented = true };
    Console.WriteLine(JsonSerializer.Serialize(videosResponse, options));

    // Display summary
    var data = videosResponse.GetProperty("data");
    var videoCount = data.GetArrayLength();
    
    Console.WriteLine("\n" + new string('=', 60));
    Console.WriteLine($"Total videos: {videoCount}");

    if (videoCount > 0)
    {
        Console.WriteLine("\nVideo IDs:");
        foreach (var video in data.EnumerateArray())
        {
            Console.WriteLine($"  - {video.GetProperty("id").GetString()} (Status: {video.GetProperty("status").GetString()})");
        }
    }
}
catch (Exception e)
{
    Console.WriteLine($"\n✗ An error occurred: {e.Message}");
    Console.WriteLine($"Error type: {e.GetType().Name}");
}
