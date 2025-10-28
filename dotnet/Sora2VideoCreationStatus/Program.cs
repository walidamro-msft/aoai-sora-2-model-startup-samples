/*
 * Sora-2 Video Creation Status Check
 * 
 * This application checks the status of a video generation job by video ID.
 * It polls the Azure OpenAI API every 20 seconds until the video is completed,
 * failed, or cancelled.
 * 
 * Requirements:
 *     - .NET 9.0 or later
 *     - Valid Azure OpenAI API key and endpoint in .env file
 * 
 * Usage:
 *     dotnet run --project Sora2VideoCreationStatus -- <video_id>
 * 
 * Example:
 *     dotnet run --project Sora2VideoCreationStatus -- video_6901017960dc8190a6f7a19cd8f48976
 * 
 * Author: Microsoft Corporation
 * Date: October 2025
 */

using System.Net.Http.Json;
using System.Text.Json;

if (args.Length < 1)
{
    Console.WriteLine("Usage: dotnet run --project Sora2VideoCreationStatus -- <video_id>");
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

var baseUrl = $"https://{resourceName}.openai.azure.com/openai/v1";

// Retrieve initial video status
var response = await httpClient.GetAsync($"{baseUrl}/videos/{videoId}");
response.EnsureSuccessStatusCode();

var video = await response.Content.ReadFromJsonAsync<JsonElement>();
Console.WriteLine($"Initial status for video {videoId}: {video.GetProperty("status").GetString()}");

// Poll every 20 seconds
while (video.GetProperty("status").GetString() is string status && 
       status != "completed" && status != "failed" && status != "cancelled")
{
    Console.WriteLine($"Status: {status}. Waiting 20 seconds...");
    await Task.Delay(TimeSpan.FromSeconds(20));
    
    // Retrieve the latest status
    response = await httpClient.GetAsync($"{baseUrl}/videos/{videoId}");
    response.EnsureSuccessStatusCode();
    video = await response.Content.ReadFromJsonAsync<JsonElement>();
}

// Final status
var finalStatus = video.GetProperty("status").GetString();
if (finalStatus == "completed")
{
    Console.WriteLine("Video successfully completed!");
    Console.WriteLine($"Video ID: {video.GetProperty("id").GetString()}");
    Console.WriteLine($"Status: {finalStatus}");
    Console.WriteLine($"Created At: {DateTimeOffset.FromUnixTimeSeconds(video.GetProperty("created_at").GetInt64()).DateTime}");
}
else
{
    Console.WriteLine($"Video creation ended with status: {finalStatus}");
    Console.WriteLine($"Video ID: {video.GetProperty("id").GetString()}");
}
