using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using Plc_Comm.Models;
using SignalR_Client.Common;
using SignalR_Client.Models.Dto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Threading;

namespace SignalR_Client.ViewModel
{
    public class MyDataCustomViewModel : ViewModelBase
    {
        #region [Prop] MyEqpInfos 수정할때 정보
        private List<EqpInfoDto> myEqpDtos;

        public List<EqpInfoDto> MyEqpDtos
        {
            get { return myEqpDtos; }
            set
            {
                myEqpDtos = value;
                RaisePropertyChanged("MyEqpDtos");
            }
        }
        #endregion

        #region [Prop] PlcMode
        private bool plcMode;

        public bool PlcMode
        {
            get { return plcMode; }
            set
            {
                plcMode = value;
                RaisePropertyChanged("PlcMode");
            }
        }
        #endregion

        List<PropertyInfo> eqpDtoProps = typeof(EqpInfoDto).GetProperties().ToList();

        #region [Creaotr] MyDataCustomViewModel
        public MyDataCustomViewModel()
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
                var dtos = ShareData.Instance().mapper.Map<List<EqpInfoDto>>(datainfos.EqpInfos);
                MyEqpDtos = dtos;
            }

            

            Messenger.Default.Register<MyDataInfo>(this, (myDatas) =>
            {
                PlcMode = myDatas.IsAuto;
                //if (PlcMode)
                {
                    var dtoInfos = ShareData.Instance().mapper.Map<List<EqpInfoDto>>(myDatas.EqpInfos);
                    var jsonDtoInfos = JsonConvert.SerializeObject(dtoInfos);
                    var jsonMyEqps = JsonConvert.SerializeObject(MyEqpDtos);
                    if (!string.Equals(jsonMyEqps, jsonDtoInfos))
                    {
                        MyEqpDtos.ToList().ForEach(g => 
                        {
                            var info = dtoInfos.FirstOrDefault(f => f.Name == g.Name);
                            g.Value1 = info.Value1;
                            g.Value2 = info.Value2;
                            g.Value3 = info.Value3;
                            g.Value4 = info.Value4;
                            g.Value5 = info.Value5;
                            g.Value6 = info.Value6;
                            g.Value7 = info.Value7;
                            g.Value8 = info.Value8;
                            g.Value9 = info.Value9;
                            g.Value10 = info.Value10;

                            g.IsSensor1 = info.IsSensor1;
                            g.IsSensor2 = info.IsSensor2;
                            g.IsSensor3 = info.IsSensor3;
                            g.IsSensor4 = info.IsSensor4;
                            g.IsSensor5 = info.IsSensor5;
                            g.IsSensor6 = info.IsSensor6;
                            g.IsSensor7 = info.IsSensor7;
                            g.IsSensor8 = info.IsSensor8;
                            g.IsSensor9 = info.IsSensor9;
                            g.IsSensor10 = info.IsSensor10;
                        });
                    }
                }
            });
        }
        #endregion

        #region [Cmd] ModeChangeCommand
        public RelayCommand ModeChangeCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Messenger.Default.Send(!PlcMode);
                });
            }
        }
        #endregion

        #region [Cmd] SensorChangeCommand
        public RelayCommand<EqpInfoDto> SensorChangeCommand
        {
            get
            {
                return new RelayCommand<EqpInfoDto>((dto) =>
                {
                    if (!plcMode)
                    {
                        var eqpInfo = ShareData.Instance().mapper.Map<EqpInfo>(dto);
                        Messenger.Default.Send(eqpInfo);
                    }
                });
            }
        }
        #endregion

        #region [Cmd] ValueChangeCommand
        public RelayCommand<TextBox> ValueChangeCommand
        {
            get
            {
                return new RelayCommand<TextBox>((txtBox) =>
                {
                    if (!plcMode)
                    {
                        var value = string.Empty == txtBox.Text ? 0 : Convert.ToInt32(txtBox.Text);
                        var eqpDto = ShareData.Instance().mapper.Map<EqpInfoDto>(txtBox.DataContext);
                        var prop = eqpDtoProps.FirstOrDefault(g => g.Name == txtBox.Tag.ToString());
                        prop.SetValue(eqpDto, value);

                        var eqpInfo = ShareData.Instance().mapper.Map<EqpInfo>(eqpDto);
                        Messenger.Default.Send(eqpInfo);
                    }
                });
            }
        }
        #endregion
    }
}
