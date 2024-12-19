using ApiConsume.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiConsume.Controllers
{
    public class CountryController : Controller
    {
        private readonly HttpClient _httpClient;
        Uri baseAddress = new Uri("http://localhost:41349/api");

        public CountryController()
        {
            _httpClient=new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GETCountryList()
        {
            List<CountryModel> countries = new List<CountryModel>();
            HttpResponseMessage response=_httpClient.GetAsync($"{_httpClient.BaseAddress}/Country").Result;
            if (response.IsSuccessStatusCode) { 
                string data=response.Content.ReadAsStringAsync().Result;
                dynamic jsonObject=JsonConvert.DeserializeObject<dynamic>(data);
                var extractedDataJson = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

                countries=JsonConvert.DeserializeObject<List<CountryModel>>(extractedDataJson);
            }
            return View("CountryList",countries);
        }
    }
}
