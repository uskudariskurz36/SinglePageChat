using Microsoft.AspNetCore.Mvc;
using SinglePageChat.Models;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace SinglePageChat.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // null model var.
            List<ChatMessage> model = null;

            // session kontrol edilir. Eğer session a daha önce list atılmamış ise.
            if (HttpContext.Session.Keys.Contains("data") == false)
            {
                // yeni bir list new edilerek session a atılır.
                model = new List<ChatMessage>();
                // session a sadece string ya da int atıldığı için büyük nesneler serialize 
                // edilerek string  dönüştürülerek atılır.
                HttpContext.Session.SetString("data", JsonSerializer.Serialize(model));
            }
            else
            {
                // Eğer session a daha önceden list atılmışsa,
                // o zaman sessiondan liste deserialize edilerek alınır ve model e verilir.
                string json = HttpContext.Session.GetString("data");
                model = JsonSerializer.Deserialize<List<ChatMessage>>(json);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string username, string message, string usercode)
        {
            List<ChatMessage> model = null;

            // sessiondan daha önce new leniş liste okunur. (Deserialize edilir)
            string json = HttpContext.Session.GetString("data");
            model = JsonSerializer.Deserialize<List<ChatMessage>>(json);

            // Message listeye eklenir.
            model.Add(new ChatMessage
            {
                Username = username,
                Message = message,
                IsUser1 = usercode == "user1" ? true : false
            });

            // Liste tekrar session a yazdırılır ki son eklenen veri uçmasın
            // session ı veri tabanı gibi kullandık.
            HttpContext.Session.SetString("data", JsonSerializer.Serialize(model));

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}