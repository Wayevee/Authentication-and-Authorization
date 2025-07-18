using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthenticationAPI.Pages
{
    [Authorize(Policy = "HRAdminOnly")]
    public class HumanResourceMangerModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;

        [BindProperty]
        public List<WeatherForcastDto> WeatherForcastDtos { get; set; } = new List<WeatherForcastDto>();
        public HumanResourceMangerModel(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task OnGet()
        {
            var httpClient = httpClientFactory.CreateClient("WebAPI");
            WeatherForcastDtos = await httpClient.GetFromJsonAsync<List<WeatherForcastDto>>("WeatherForecast") ??  new List<WeatherForcastDto>();
        }
    }
}
