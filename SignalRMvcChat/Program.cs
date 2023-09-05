using HTTPChat_Application;
using SignalRMvcChat.Controllers;
using SignalRMvcChat.Hubs;
using static HTTPChat_Application.EventsClass;
using System.IO.Pipelines;
using Microsoft.Extensions.DependencyInjection;
using System.Buffers;

namespace SignalRMvcChat;

public class Program
{
    public static ExternClass ExternClass = new ExternClass();

    public static event EventHandler<messageEventArgs> MessageEvent;

    public static ChatHub chatHub;

    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddSignalR();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment()) {
            app.UseExceptionHandler("/Home/Error");
        }
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapHub<ChatHub>("/chatHub");

        MessageEvent += HomeController.eventDistributer;

        app.MapPost("/input", context => {
            //ExternClass.receiveMutex.ReleaseMutex();
            var a = context.Request.Query;
            var reader = context.Request.BodyReader;
            ReadResult b = new ReadResult();
            reader.TryRead(out b);
            string s = System.Text.Encoding.Default.GetString(b.Buffer.ToArray());
            EventHandler<messageEventArgs> handler = MessageEvent;
            messageEventArgs eventArgs = new messageEventArgs();
            eventArgs.text = s;
            eventArgs.id = 1;
            context.Request.BodyReader.AdvanceTo(b.Buffer.Start, b.Buffer.End);// si no hi ha aixo peta...
            ExternClass.ReceivingQueue.Add(s);
            handler?.Invoke(null, eventArgs);
            return Task.CompletedTask;
        });

        app.Run();
    }
}