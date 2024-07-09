using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Diagnostics;

namespace NZWalks.UI.Controllers
{
    public class WalksController : Controller
    {
        private readonly IHttpClientFactory httpClient;

        public WalksController(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            List<WalkDTO> response = new List<WalkDTO>();
            try
            {
                //To get all Regions from web API
                var client = httpClient.CreateClient();
                // Call GetRegion action method from NZWalksWebAPI
                var httpResponseMessage = await client.GetAsync("https://localhost:7131/api/walks");
                httpResponseMessage.EnsureSuccessStatusCode();
                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<WalkDTO>>());

            }
            catch (Exception ex)
            {
                //log the exception

            }

            return View(response);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddWalkViewModel model)
        {
            var client = httpClient.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7131/api/walks"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
            };
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();
            if (response is not null)
            {
                return RedirectToAction("Index", "Walks");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id) // Use Guid if Id is of type Guid
        {
            try
            {
                var client = httpClient.CreateClient();

                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7131/api/walks/{Id}");

                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Walks");
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                // Redirect to a simple error view
                return RedirectToAction("Error", new { message = ex.Message });
            }
        }


    }
}
