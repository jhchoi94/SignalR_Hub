using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR_ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new HubConnection("http://localhost:8080/");
            var myHub = connection.CreateHubProxy("MyHub");
            myHub.On<string>("addMessage", (message) => Console.WriteLine(message));
            connection.Start().Wait();
            while (true)
            {
                var message = Console.ReadLine();
                myHub.Invoke("Send", message);
            }
        }
    }
}
