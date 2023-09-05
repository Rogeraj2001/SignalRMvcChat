using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRMvcChat.Hubs;
using SignalRMvcChat.Models;
using System.Diagnostics;
using static HTTPChat_Application.EventsClass;

namespace SignalRMvcChat.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public readonly IHubContext<ChatHub> hubContext;
    public static event EventHandler<messageEventArgs> InternalMessageEvent;

    public HomeController(ILogger<HomeController> logger, IHubContext<ChatHub> hubContext) {
        _logger = logger;
        this.hubContext = hubContext;
        InternalMessageEvent += c_ThresholdReached;
    }

    public IActionResult Index() {
        return View();
    }

    public IActionResult Privacy() {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    public void c_ThresholdReached(object sender, messageEventArgs e) {
        Console.WriteLine("The threshold of {0} was reached at {1}.", e.id, e.text);
        //chatHub = new ChatHub();
        //chatHub.SendMessage(e.id.ToString(), e.text);
        //hubContext.Clients.All.SendAsync("SendMessage",e.text);
        object?[] objects = { e.id.ToString(), e.text };
        hubContext.Clients.All.SendCoreAsync("SendMessage", objects);
    }

    public static void eventDistributer(object sender, messageEventArgs e) {
        EventHandler<messageEventArgs> handler = InternalMessageEvent;
        messageEventArgs eventArgs = new messageEventArgs();
        eventArgs.text = e.text;
        eventArgs.id = e.id;
        handler?.Invoke(null, eventArgs);
    }
}
