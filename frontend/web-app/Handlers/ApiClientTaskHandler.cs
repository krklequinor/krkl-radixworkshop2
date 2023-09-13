using Org.OpenAPITools.Model;

namespace web_app.Handlers;

public class ApiClientTaskHandler
{
    private readonly Uri _taskApiEndpoint;
    public ApiClientTaskHandler(Uri taskApiEndpoint)
    {
        _taskApiEndpoint = taskApiEndpoint;
    }

    internal void RunTask(List<string> dataItems)
    {
        SubmitCreateJobRequestForDataItemProcessing(_taskApiEndpoint, dataItems);
    }

    private void SubmitCreateJobRequestForDataItemProcessing(Uri apiEndpoint, List<string> dataItems)
    {
        Console.Out.WriteLine($"Process {dataItems.Count} item(s) with API client");
        var processMultipleImages = dataItems.Count > 1;
        Console.Out.WriteLine("Sending POST request to " + apiEndpoint);
        try
        {
            if (processMultipleImages)
            {
                var client = new Org.OpenAPITools.Api.BatchApi(apiEndpoint.ToString());
                var batchDescription = GetBatchDescription(dataItems);
                var batchStatus = client.CreateBatchAsync(batchDescription).Result;
                Console.Out.WriteLine(batchStatus);
            }
            else
            {
                var client = new Org.OpenAPITools.Api.JobApi(apiEndpoint.ToString());
                var jobScheduleDescription = GetJobScheduleDescriptionFor(dataItems[0]);
                var jobStatus = client.CreateJobAsync(jobScheduleDescription).Result;
                Console.Out.WriteLine(jobStatus);
            }
            Console.Out.WriteLine($"Run a task job submitted");
        }
        catch (Exception e)
        {
            Console.Out.WriteLine($"Run a task failed {e}");
        }
    }

    private static BatchScheduleDescription GetBatchDescription(List<string> imageNames)
    {
        var jobDescriptions = new List<JobScheduleDescription>();
        foreach (var imageName in imageNames)
        {
            jobDescriptions.Add(GetJobScheduleDescriptionFor(imageName));
        }

        var radixJobComponentConfig = new RadixJobComponentConfig();
        return new BatchScheduleDescription(radixJobComponentConfig,jobDescriptions);
    }

    private static JobScheduleDescription GetJobScheduleDescriptionFor(string dataItem)
    {
        return new JobScheduleDescription { Payload = "{\"data\":\"" + dataItem + "\"}" };
    }

}