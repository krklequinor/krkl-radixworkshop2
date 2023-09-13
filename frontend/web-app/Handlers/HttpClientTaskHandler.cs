using System.Net;
using System.Text;
using System.Text.Json;

namespace web_app.Handlers;

public class HttpClientTaskHandler
{
    private readonly Uri _taskApiEndpoint;
    public HttpClientTaskHandler(Uri taskApiEndpoint)
    {
        _taskApiEndpoint = taskApiEndpoint;
    }

    internal void RunTask(List<string> dataItems)
    {
        SubmitCreateJobRequestForDataItemProcessing(_taskApiEndpoint, dataItems);
    }

    private void SubmitCreateJobRequestForDataItemProcessing(Uri apiEndpoint, List<string> dataItems)
    {
        Console.Out.WriteLine($"Process {dataItems.Count} item(s) with HTTP client");
        // create a http client to send POST request to create a job or a batch of jobs
        var client = new HttpClient();
        var processMultipleImages = dataItems.Count > 1;
        var fullApiEndpoint = new Uri(apiEndpoint, processMultipleImages ? "batches" : "jobs");
        var request = processMultipleImages
            ? GetRequestToProcessMultipleItems(dataItems, fullApiEndpoint)
            : GetRequestToProcessSingleItem(dataItems[0], fullApiEndpoint);
        Console.Out.WriteLine("Sending POST request to " + fullApiEndpoint);
        HttpResponseMessage response;
        try
        {
            response = client.SendAsync(request).Result;
        }
        catch (Exception e)
        {
            Console.Out.WriteLine($"Run a task failed {e}");
            return;
        }
        if (response.IsSuccessStatusCode)
        {
            Console.Out.WriteLine($"Run a task job submitted");
            var result = response.Content.ReadAsStringAsync().Result;
            Console.Out.WriteLine(result);
            return;
        }

        Console.Out.WriteLine($"Run a task failed");
        Console.Out.WriteLine(response.Content.ReadAsStringAsync().Result);
    }

    private HttpRequestMessage GetRequestToProcessMultipleItems(List<string> imageNames, Uri apiEndpoint)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, apiEndpoint);
        var jobDescriptions = new List<JobScheduleDescription>();
        foreach (var imageName in imageNames)
        {
            jobDescriptions.Add(GetJobScheduleDescriptionFor(imageName));
        }
        var batchDescription = new BatchDescription { JobScheduleDescriptions = jobDescriptions.ToArray() };
        var json = JsonSerializer.Serialize(batchDescription);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        return request;
    }

    private HttpRequestMessage GetRequestToProcessSingleItem(string dataItem, Uri apiEndpoint)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, apiEndpoint);
        var jobDescription = new JobScheduleDescription { Payload = "{\"data\":\"" + dataItem + "\"}" };
        var json = JsonSerializer.Serialize(jobDescription);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        return request;
    }

    private static JobScheduleDescription GetJobScheduleDescriptionFor(string dataItem)
    {
        return new JobScheduleDescription { Payload = "{\"data\":\"" + dataItem + "\"}" };
    }

    private class BatchDescription
    {
        public JobScheduleDescription[] JobScheduleDescriptions { get; set; } = Array.Empty<JobScheduleDescription>();
    }

    private class JobScheduleDescription
    {
        public string Payload { get; set; } = "";
    }
}