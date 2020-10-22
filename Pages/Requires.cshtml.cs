using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FiscalizaDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CoreUI_Free_Bootstrap_Admin.Pages
{
    public class RequiresModel : PageModel
    {
        public void OnGet([FromServices] IConfiguration config)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                string baseURL =
                    config.GetSection("FirebaseAPI:BaseURL").Value;
                string url = baseURL + "/request/1";

                HttpResponseMessage response = client.GetAsync(url).Result;

                response.EnsureSuccessStatusCode();
                string conteudo =
                    response.Content.ReadAsStringAsync().Result;

                Require resultado = JsonConvert.DeserializeObject<Require>(conteudo);

                List<Datum> requires = new List<Datum>();

                foreach (var a in resultado.data)
                {
                    requires.Add(new Datum
                    {
                        description = a.description,
                        id = a.id,
                        image = a.image,
                        statusDescription = GetStatus(a.status),
                        statusColor = GetStatusColor(a.status),
                        status = a.status,
                        type = GetType(a.type),
                        user = a.user,
                    });
                }

                TempData["Requires"] = requires.OrderBy(x => x.status).ToList();
            }
        }

        private string GetStatusColor(int status)
        {
            switch (status)
            {
                case 1:
                    return "goldenrod";

                case 2:
                    return "blue";

                case 3:
                    return "green";

                case 4:
                    return "red";

                default:
                    return "black";
            }
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

        private string GetStatus(int status)
        {
            switch (status)
            {
                case 1:
                    return "Em Aberto";

                case 2:
                    return "Em Andamento";

                case 3:
                    return "Concluído";

                case 4:
                    return "Vencido";

                default:
                    return "";
            }
        }
    }
}
