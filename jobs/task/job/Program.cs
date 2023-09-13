using System.Text.Json;
using System.Text.Json.Serialization;

static class Program
{
    static async Task  Main(string[] args)
    {
        var payloadFile = "./input/payload";
        if (!Path.Exists(payloadFile)){
            payloadFile = Environment.GetEnvironmentVariable("PAYLOAD_FILE");
            if (!Path.Exists(payloadFile)){
                await Console.Out.WriteLineAsync("Payload does not exist");
                return;
            }
        }
        var payloadJson = await File.ReadAllTextAsync(payloadFile);
        var payload = JsonSerializer.Deserialize<Payload>(payloadJson, new JsonSerializerOptions{PropertyNameCaseInsensitive = false});
        if (payload != null)
        {
            await Console.Out.WriteLineAsync($"Payload data: '{payload.Data}'");
        }

        Console.WriteLine("Job started.");

        // Some work logic
        var task1 = DoWorkAsync("Task 1", 3);
        var task2 = DoWorkAsync("Task 2", 2);
        var task3 = DoWorkAsync("Task 3", 4);

        await Task.WhenAll(task1, task2, task3);

        Console.WriteLine("Job completed.");
    }

    private static async Task DoWorkAsync(string taskName, int seconds)
    {
        Console.WriteLine($"{taskName} started.");
        
        // Simulating work by delaying for a few seconds
        await Task.Delay(TimeSpan.FromSeconds(seconds));

        Console.WriteLine($"{taskName} completed after {seconds} seconds.");
    }
}

public class Payload
{
    [JsonPropertyName("data")]
    public string Data { get; set; } = "";
}