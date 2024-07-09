using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClient;

        public RegionsController(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            List<RegionDTO> response = new List<RegionDTO>();
            try
            {
                //To get all Regions from web API
                var client = httpClient.CreateClient();
                // Call GetRegion action method from NZWalksWebAPI
                var httpResponseMessage = await client.GetAsync("https://localhost:7131/api/regions");
                httpResponseMessage.EnsureSuccessStatusCode();
                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>());
                
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
        public async Task<IActionResult> Add(AddRegionViewModel model) 
        {
            var client = httpClient.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7131/api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
            };
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();
            if (response is not null) 
            {
                return RedirectToAction("Index", "Regions");
            }
            return View();        
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id) 
        { 
            var client = httpClient.CreateClient();
            var response = await client.GetFromJsonAsync<RegionDTO>($"https://localhost:7131/api/regions/{id.ToString()}");
            if (response is not null) 
            {
                return View(response);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionDTO request) 
        {
            var client = httpClient.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7131/api/regions/{request.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();

            if (response is not null)
            {
                return RedirectToAction("Index", "Regions");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(RegionDTO request) 
        {
            try
            {
                var client = httpClient.CreateClient();

                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7131/api/regions/{request.Id}");

                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Regions");
            }
            catch (Exception ex)
            {
                // Console
            }

            return View("Edit");
        }
    }
}
