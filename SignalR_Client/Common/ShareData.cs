using AutoMapper;
using Plc_Comm.Models;
using SignalR_Client.Models.Dto;
using System.Collections.Generic;

namespace SignalR_Client.Common
{
    public class ShareData
    {
        private static ShareData shareData;

        public IMapper mapper;

        public MyDataInfo MyDataInfos { get; set; }
        public ShareData()
        {
            // IMapper mapper = config.CreateMapper();

            //var config = new MapperConfiguration(
            //    cfg => cfg.CreateMap<EqpInfo, EqpInfoDto>());
            //mapper = new Mapper(config);









            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<EqpInfo, EqpInfoDto>();
                cfg.CreateMap<EqpInfoDto, EqpInfo>();
            });

            mapper = config.CreateMapper();
        }

        public static ShareData Instance()
        {
            if (shareData == null)
                shareData = new ShareData();

            return shareData;
        }
    }
}
