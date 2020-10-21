using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FiscalizaDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace FiscalizaDashboard.Pages
{
    [BindProperties(SupportsGet = true)]
    public class RequireDetailsModel : PageModel
    {
        public int Status { get; set; }
        public void OnGet(int id, [FromServices] IConfiguration config)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                string baseURL =
                    config.GetSection("FirebaseAPI:BaseURL").Value;
                string url = baseURL + "/request/1/" + id;

                HttpResponseMessage response = client.GetAsync(url).Result;

                response.EnsureSuccessStatusCode();
                string conteudo =
                    response.Content.ReadAsStringAsync().Result;

                SingleRequire resultado = JsonConvert.DeserializeObject<SingleRequire>(conteudo);

                Datum require = resultado.data;
                require.type = GetType(require.type);

                TempData["Requires"] = require;
            }
        }

        [BindProperty]
        public Datum Input { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var a = Status;
            return RedirectToPage("/Index");
        }

        private string GetType(string type)
        {
            switch (type)
            {
                case "illumination":
                    return "Iluminação";

                case "water":
                    return "Água/Esgoto";

                case "traffic-light":
                    return "Semáforo";

                case "road":
                    return "Asfalto";

                case "terreno":
                    return "Terreno";

                case "aedes":
                    return "Dengue";

                default:
                    return "";
            }
        }

        public class CreateStatus
        {
            public IEnumerable<SelectListItem> Status { set; get; }
            public int SelectedStatus { set; get; }
        }
    }
}
