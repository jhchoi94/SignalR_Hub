using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using SignalR_Server;
using SignalR_Server.Hubs;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(Program.Startup))]
namespace SignalR_Server
{
    class Program
    {
        static log4net.ILog Logger = log4net.LogManager.GetLogger("Program");
        static void Main(string[] args)
        {
            // Log4Net 설정
            log4net.Config.XmlConfigurator.Configure();
            Logger.Info("프로그램 실행");

            string url = "http://localhost:8080";

            using (WebApp.Start<Startup>(url))
            {
                Logger.Info($"Server running on {url}");
                Console.ReadLine();
            }
        }

        public class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                app.UseCors(CorsOptions.AllowAll);
                app.MapSignalR();
            }
        }
    }
}
