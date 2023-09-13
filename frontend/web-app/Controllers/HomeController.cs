using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using web_app.Handlers;
using web_app.Models;

namespace web_app.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClientTaskHandler _httpClientTaskHandler;
        private readonly ApiClientTaskHandler _apiClientTaskHandler;

        public HomeController()
        {
            var taskApiEndpoint = new Uri("http://task:8071/api/v1/");
            _httpClientTaskHandler = new HttpClientTaskHandler(taskApiEndpoint);
            _apiClientTaskHandler = new ApiClientTaskHandler(taskApiEndpoint);
        }

        public IActionResult Index()
        {
            var viewModel = new ItemViewModel
            {
                Items = new List<ItemModel>
                {
                    new ItemModel { Data = "", IsSelected = false },
                    new ItemModel { Data = "", IsSelected = false },
                    new ItemModel { Data = "", IsSelected = false }
                }
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ProcessDataItems(ItemViewModel model)
        {
            var selectedItems = model.Items.Where(item => item.IsSelected).Select(m => m.Data).ToList();
            if (selectedItems.Count == 0)
            {
                Console.Out.WriteLine("No items selected");
                return RedirectToAction("Index");
            }

            if (model.UseApiClient)
            {
                _apiClientTaskHandler.RunTask(selectedItems);
            }
            else
            {
                _httpClientTaskHandler.RunTask(selectedItems);
            }

            return RedirectToAction("Index");
        }
        
        public IActionResult BatchStatuses()
        {
            return View(InMemoryStore.BatchStatuses.Values.ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}