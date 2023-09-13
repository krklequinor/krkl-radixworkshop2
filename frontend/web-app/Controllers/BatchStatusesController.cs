using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.OpenAPITools.Model;

namespace web_app.Controllers
{
    [Host("*:8060")]
    public class BatchStatusesController : Controller
    {
        [HttpPost]
        public IActionResult BatchStatuses([FromBody] BatchStatus batchStatus)
        {
            try
            {
                Console.Out.WriteLine($"Name: {batchStatus.Name}, Created: {batchStatus.Created}");
                InMemoryStore.BatchStatuses[batchStatus.Name] = batchStatus;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
            return Ok();
        }
    }

    public class BatchStatus
    {
        public string Name { get; set; } = "";
        public string Created { get; set; } = "";
        public string? BatchName { get; set; }
        public string? Ended { get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; }
    }
}