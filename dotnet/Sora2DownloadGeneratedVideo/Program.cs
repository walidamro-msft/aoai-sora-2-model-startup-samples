/*
 * Sora-2 Video Download Utility
 * 
 * This application downloads previously generated videos from Azure OpenAI Sora-2 using their
 * video IDs. After generating a video using the text-to-video or image-to-video APIs,
 * use this utility to retrieve and save the completed video file to your local system.
 * 
 * Requirements:
 *     - .NET 9.0 or later
 *     - Valid Azure OpenAI API key and endpoint
 * 
 * Usage:
 *     dotnet run --project Sora2DownloadGeneratedVideo -- <video_id> <output_file_path>
 * 
 * Parameters:
 *     video_id: The ID of the generated video (format: video_xxx)
 *               This ID is returned when you create a video generation request
 *     
 *     output_file_path: Full path where the video should be saved
 *                      Can include directories; they will be created if needed
 *                      Recommended format: .mp4
 * 
 * Examples:
 *     dotnet run --project Sora2DownloadGeneratedVideo -- video_68fa7f59f2588190a48ea2b0aa73d844 output.mp4
 *     dotnet run --project Sora2DownloadGeneratedVideo -- video_68fa7f59f2588190a48ea2b0aa73d844 videos/red_panda_latte.mp4
 * 
 * Author: Microsoft Corporation
 * Date: October 2025
 */

using System.Net.Http.Json;
using System.Text.Json;

if (args.Length != 2)
{
    Console.WriteLine("\n" + new string('=', 60));
    Console.WriteLine("Sora-2 Video Download Utility");
    Console.WriteLine(new string('=', 60));
    Console.WriteLine("\nUsage:");
    Console.WriteLine("  dotnet run --project Sora2DownloadGeneratedVideo -- <video_id> <output_file_path>");
    Console.WriteLine("\nParameters:");
    Console.WriteLine("  video_id          - The ID of the video to download (format: video_xxx)");
    Console.WriteLine("  output_file_path  - Full path where the video will be saved");
    Console.WriteLine("\nExamples:");
    Console.WriteLine("  dotnet run --project Sora2DownloadGeneratedVideo -- video_68fa7f59f2588190a48ea2b0aa73d844 output.mp4");
    Console.WriteLine("  dotnet run --project Sora2DownloadGeneratedVideo -- video_abc123 videos/my_video.mp4");
    Console.WriteLine(new string('=', 60));
    return;
}

var videoId = args[0];
var outputFilePath = args[1];

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
    throw new InvalidOperationException(
        "Missing required environment variables. " +
        "Please ensure AZURE_API_KEY and AZURE_RESOURCE_NAME are set in your .env file.");
}

using var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("api-key", apiKey);

var baseUrl = $"https://{resourceName}/openai/v1";

Console.WriteLine(new string('-', 60));
Console.WriteLine("Downloading video...");
Console.WriteLine($"  Endpoint URL: {baseUrl}/videos/{videoId}/content");
Console.WriteLine($"  Video ID: {videoId}");
Console.WriteLine($"  Output Path: {outputFilePath}");
Console.WriteLine(new string('-', 60));

try
{
    // Retrieve the video content URL
    var response = await httpClient.GetAsync($"{baseUrl}/videos/{videoId}/content?variant=video");
    response.EnsureSuccessStatusCode();

    // Create output directory if it doesn't exist
    var outputDir = Path.GetDirectoryName(outputFilePath);
    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
    {
        Directory.CreateDirectory(outputDir);
        Console.WriteLine($"Created directory: {outputDir}");
    }

    // Download and save the video content
    var videoBytes = await response.Content.ReadAsByteArrayAsync();
    await File.WriteAllBytesAsync(outputFilePath, videoBytes);

    Console.WriteLine("\n✓ Video saved successfully!");
    Console.WriteLine($"  Location: {Path.GetFullPath(outputFilePath)}");
    Console.WriteLine($"  File size: {new FileInfo(outputFilePath).Length:N0} bytes");
}
catch (Exception e)
{
    Console.WriteLine($"\n✗ Error downloading video: {e.Message}");
    Console.WriteLine("\nTroubleshooting:");
    Console.WriteLine("  - Verify the video ID is correct (format: video_xxx)");
    Console.WriteLine("  - Check that your API key is valid in the .env file");
    Console.WriteLine("  - Ensure your Azure resource name is correct");
    Console.WriteLine("  - Confirm the video generation has completed");
    Console.WriteLine("  - Check your network connection");
}
