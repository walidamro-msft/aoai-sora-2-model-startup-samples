/*
 * Sora-2 Delete Generated Video
 * 
 * This application deletes a specific video from Azure OpenAI's Sora-2 model by video ID.
 * 
 * Requirements:
 *     - .NET 9.0 or later
 *     - Valid Azure OpenAI API key and endpoint in .env file
 * 
 * Usage:
 *     dotnet run --project Sora2DeleteGeneratedVideo -- <video_id>
 * 
 * Example:
 *     dotnet run --project Sora2DeleteGeneratedVideo -- video_6901017960dc8190a6f7a19cd8f48976
 * 
 * Author: Microsoft Corporation
 * Date: October 2025
 */

using System.Net.Http.Json;
using System.Text.Json;

if (args.Length < 1)
{
    Console.WriteLine("Usage: dotnet run --project Sora2DeleteGeneratedVideo -- <video_id>");
    Console.WriteLine("\nExample:");
    Console.WriteLine("  dotnet run --project Sora2DeleteGeneratedVideo -- video_6901017960dc8190a6f7a19cd8f48976");
    return;
}

var videoId = args[0];

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
    Console.WriteLine("Error: AZURE_API_KEY and AZURE_RESOURCE_NAME must be set in .env file");
    return;
}

using var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("api-key", apiKey);

var baseUrl = $"https://{resourceName}/openai/v1";

Console.WriteLine($"Deleting video: {videoId}");
Console.WriteLine($"Endpoint URL: {baseUrl}/videos/{videoId}");
Console.WriteLine(new string('=', 60));

try
{
    // Delete the video
    var response = await httpClient.DeleteAsync($"{baseUrl}/videos/{videoId}");
    response.EnsureSuccessStatusCode();

    var deleteResponse = await response.Content.ReadFromJsonAsync<JsonElement>();

    Console.WriteLine($"\n✓ Successfully deleted video: {videoId}");
    Console.WriteLine($"Deletion confirmed: {deleteResponse.GetProperty("deleted").GetBoolean()}");
    Console.WriteLine($"Video ID: {deleteResponse.GetProperty("id").GetString()}");
}
catch (Exception e)
{
    Console.WriteLine($"\n✗ Error deleting video: {e.Message}");
    Console.WriteLine($"Error type: {e.GetType().Name}");
}
