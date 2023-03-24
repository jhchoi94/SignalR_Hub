using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Plc_Comm.Models;
using SignalR_Client.Common;
using System;
using System.Threading.Tasks;

namespace SignalR_Client.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        log4net.ILog Logger = log4net.LogManager.GetLogger("MainViewModel");

        #region [prop] CurrentViewModel
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                if (_currentViewModel != value)
                {
                    Set(ref _currentViewModel, value);
                    RaisePropertyChanged("CurrentViewModel");
                }
            }
        }
        #endregion

        #region [prop] CurTime
        private string _curTime;

        public string CurTime
        {
            get { return _curTime; }
            set
            {
                _curTime = value;
                RaisePropertyChanged("curTime");
            }
        } 
        #endregion

        #region [Creator] MainViewModel
        public MainViewModel()
        {
            // Log4Net ����
            log4net.Config.XmlConfigurator.Configure();
            Logger.Info("���α׷� ����");

            Init();
        }
        #endregion

        #region [Init]
        private async void Init()
        {
            CurrentViewModel = ServiceLocator.Current.GetInstance<MyDataListViewModel>();

            var connection = new HubConnection("http://localhost:8080/");
            var myHub = connection.CreateHubProxy("MyHub");
            connection.Headers.Add("UserName", "SignalR_Client");

            myHub.On<string>("BroadCastMyData", (myData) =>
            {
                var info = JsonConvert.DeserializeObject<MyDataInfo>(myData);
                ShareData.Instance().MyDataInfos = info;

                Messenger.Default.Send(info);
            });

            // Hub�� ��庯�� ��û(Hub�� -> Comm�� ����)
            Messenger.Default.Register<bool>(this, (plcMode) =>
            {
                myHub.Invoke("PlcModeChange", plcMode);
            });

            // Hub�� ��庯�� ��û(Hub�� -> Comm�� ����)
            Messenger.Default.Register<EqpInfo>(this, (dto) =>
            {
                myHub.Invoke("PlcDataChange", dto);
            });

            try
            {
                await connection.Start();
                if (connection.State == ConnectionState.Connected)
                    Logger.Info("Server ���� ����");
                else
                {
                    Logger.Error("Server ���� ����");
                    Logger.Info("Server �翬�� �õ�");
                }
            }
            catch
            {
                Logger.Error("Server ���� ����");
                Logger.Info("Server �翬�� �õ�");
            }

            await Task.Run(async () =>
            {
                int timeCheck = 0;
                while (true)
                {
                    CurTime = DateTime.Now.ToString();

                    if (timeCheck % 5 == 0)
                    {
                        timeCheck = 0;
                        if (connection.State != ConnectionState.Connected)
                        {
                            try
                            {
                                Logger.Info("�翬����. . .");
                                await connection.Start();
                                if (connection.State == ConnectionState.Connected)
                                    Logger.Info("Server ���� ����");
                            }
                            catch
                            {
                                Logger.Error("Server ���� ����");
                                Logger.Info("Server �翬�� �õ�");
                            }
                        }
                    }
                    else
                        timeCheck++;

                    await Task.Delay(500);
                }
            });

            
        }
        #endregion

        public RelayCommand<string> MenuCommand
        {
            get
            {
                return new RelayCommand<string>((confType) =>
                {
                    if (confType == "MyDataList")
                    {
                        CurrentViewModel = ServiceLocator.Current.GetInstance<MyDataListViewModel>();
                    }
                    else if (confType == "MyDataSubList")
                    {
                        CurrentViewModel = ServiceLocator.Current.GetInstance<MyDataSubListViewModel>();
                    }
                    else if (confType == "MyDataUnit")
                    {
                        CurrentViewModel = ServiceLocator.Current.GetInstance<MyDataUnitViewModel>();
                    }
                    else if (confType == "MyDataCustom")
                    {
                        CurrentViewModel = ServiceLocator.Current.GetInstance<MyDataCustomViewModel>();
                    }
                });
            }
        }
    }
}