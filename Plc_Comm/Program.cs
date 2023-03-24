using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Plc_Comm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Plc_Comm
{
    class Program
    {
        static log4net.ILog Logger = log4net.LogManager.GetLogger("Program");
        static List<PropertyInfo> propEqpInfo;
        static void Main(string[] args)
        {
            MyDataInfo myDataInfo = new MyDataInfo();
            propEqpInfo = typeof(EqpInfo).GetProperties().ToList();

            // Log4Net 설정
            log4net.Config.XmlConfigurator.Configure();
            Logger.Info("프로그램 실행");

            var eqpInfoProps = typeof(EqpInfo).GetProperties();
            for (int i = 0; i < 20; i++)
            {
                myDataInfo.EqpInfos.Add(new EqpInfo
                {
                    Name = $"TEST{i+1}",
                });
            }

            #region SerialR_Server 연결
            var connection = new HubConnection("http://localhost:8080/");
            var myHub = connection.CreateHubProxy("MyHub");
            connection.Headers.Add("UserName", "PlcComm");

            try
            {
                connection.Start().Wait();
                Logger.Info("Server 연결 성공");
            }
            catch 
            {
                Logger.Error("Server 연결 실패");
                Logger.Info("Server 재연결 시도");
            }
            #endregion

            myHub.On<bool>("PlcModeChange", (plcMode) =>
            {
                Logger.Info($"PLC 모드 변경 {myDataInfo.IsAuto} -> {plcMode}");
                myDataInfo.IsAuto = plcMode;
            });

            myHub.On<EqpInfo>("PlcDataChange", (eqpData) =>
            {
                var findEqp = myDataInfo.EqpInfos.FirstOrDefault(g => g.Name == eqpData.Name);
                Logger.Info($"Eqp Data 변경 {JsonConvert.SerializeObject(findEqp)} -> {JsonConvert.SerializeObject(eqpData)}");

                propEqpInfo.ForEach(g => 
                {
                    g.SetValue(findEqp, g.GetValue(eqpData));
                });
                
                var jsonData = JsonConvert.SerializeObject(myDataInfo);
                //Logger.Debug(jsonData);
                myHub.Invoke("BroadCastMyData", jsonData);
            });

            Task.Run(async () => 
            {
                string oldJsonPlcData = string.Empty;
                string oldJsonCellData = string.Empty;
                DateTime oldTime = DateTime.Now;
                Random rnd = new Random();
                while (true)
                {
                    if (connection.State != ConnectionState.Connected)
                    {
                        try
                        {
                            await connection.Start();
                            if (connection.State != ConnectionState.Reconnecting)
                                Logger.Info("Server 연결 성공");
                        }
                        catch 
                        {
                            Logger.Error("Server 연결 실패");
                            Logger.Info("Server 재연결 시도");
                        }

                        await Task.Delay(5000);
                        continue;
                    }

                    var nowDate = DateTime.Now;
                    if ((nowDate - oldTime).TotalSeconds > 5)
                    {
                        oldTime = nowDate;

                        // data 임의로 가공
                        // Logger.Debug(myDataInfo.IsAuto);
                        if (myDataInfo.IsAuto == true)
                        {
                            myDataInfo.EqpInfos.ForEach(g =>
                            {
                                // bool 형
                                eqpInfoProps.Where(w => w.PropertyType == typeof(bool)).ToList().ForEach(prop =>
                                {
                                    prop.SetValue(g, rnd.Next(0, 100) % 2 == 0);
                                });

                                // int형
                                eqpInfoProps.Where(w => w.PropertyType == typeof(int)).ToList().ForEach(prop =>
                                {
                                    prop.SetValue(g, rnd.Next(0, 10000));
                                });
                            });
                        }
                    }

                    var jsonData = JsonConvert.SerializeObject(myDataInfo);
                    //Logger.Debug(jsonData);
                    await myHub.Invoke("BroadCastMyData", jsonData);

                    await Task.Delay(1000);
                }
            });

            Console.ReadLine();
        }
    }
}
