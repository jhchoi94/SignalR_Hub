using Microsoft.AspNet.SignalR;
using Plc_Comm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR_Server.Hubs
{
    public class MyHub : Hub
    {
        static log4net.ILog Logger = log4net.LogManager.GetLogger("MyHub");
        public void PlcModeChange(bool plcMode)
        {
            Clients.All.PlcModeChange(plcMode);
            Logger.Info($"{Context.Headers["UserName"]} PlcModeChange : {plcMode}  (UserId : {Context.ConnectionId})");
        }

        public void PlcDataChange(EqpInfo eqpInfo)
        {
            Clients.All.PlcDataChange(eqpInfo);
            Logger.Info($"{Context.Headers["UserName"]} PlcDataChange    (UserId : {Context.ConnectionId})");
        }

        public void BroadCastMyData(string myDatas)
        {
            Clients.All.BroadCastMyData(myDatas);
        }

        public override Task OnConnected()
        {
            Logger.Info($"{Context.Headers["UserName"]} OnConnected(UserId : {Context.ConnectionId})");
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Logger.Info($"{Context.Headers["UserName"]} Disconnected(UserId : {Context.ConnectionId})");
            return base.OnDisconnected(stopCalled);
        }
    }
}
