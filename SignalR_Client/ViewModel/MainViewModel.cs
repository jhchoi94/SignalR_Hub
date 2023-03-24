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
            // Log4Net 설정
            log4net.Config.XmlConfigurator.Configure();
            Logger.Info("프로그램 시작");

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

            // Hub에 모드변경 요청(Hub는 -> Comm에 전송)
            Messenger.Default.Register<bool>(this, (plcMode) =>
            {
                myHub.Invoke("PlcModeChange", plcMode);
            });

            // Hub에 모드변경 요청(Hub는 -> Comm에 전송)
            Messenger.Default.Register<EqpInfo>(this, (dto) =>
            {
                myHub.Invoke("PlcDataChange", dto);
            });

            try
            {
                await connection.Start();
                if (connection.State == ConnectionState.Connected)
                    Logger.Info("Server 연결 성공");
                else
                {
                    Logger.Error("Server 연결 실패");
                    Logger.Info("Server 재연결 시도");
                }
            }
            catch
            {
                Logger.Error("Server 연결 실패");
                Logger.Info("Server 재연결 시도");
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
                                Logger.Info("재연결중. . .");
                                await connection.Start();
                                if (connection.State == ConnectionState.Connected)
                                    Logger.Info("Server 연결 성공");
                            }
                            catch
                            {
                                Logger.Error("Server 연결 실패");
                                Logger.Info("Server 재연결 시도");
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