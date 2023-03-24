using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using Plc_Comm.Models;
using SignalR_Client.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR_Client.ViewModel
{
    public class MyDataUnitViewModel : ViewModelBase
    {
        #region [Prop] MyEqpInfos
        private List<EqpInfo> myEqpInfos;

        public List<EqpInfo> MyEqpInfos
        {
            get { return myEqpInfos; }
            set
            {
                myEqpInfos = value;
                RaisePropertyChanged("MyEqpInfos");
            }
        }
        #endregion

        #region [Creator] MyDataUnitViewModel
        public MyDataUnitViewModel()
        {
            Init();
        }
        #endregion

        #region [Method] Init
        public void Init()
        {
            var datainfos = ShareData.Instance().MyDataInfos;
            if (datainfos != null)
            {
                MyEqpInfos = datainfos.EqpInfos;
            }

            Messenger.Default.Register<MyDataInfo>(this, (myDatas) =>
            {
                if (MyEqpInfos != null)
                {
                    var tt = JsonConvert.SerializeObject(myDatas.EqpInfos);
                    var ss = JsonConvert.SerializeObject(MyEqpInfos);
                    if (tt != ss)
                        MyEqpInfos = myDatas.EqpInfos;
                }
                else
                    MyEqpInfos = myDatas.EqpInfos;
            });
        }
        #endregion
    }
}
