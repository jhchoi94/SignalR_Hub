using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plc_Comm.Models
{
    
    public class MyDataInfo
    {
        public bool IsAuto { get; set; }
        public List<EqpInfo> EqpInfos { get; set; }

        public MyDataInfo()
        {
            EqpInfos = new List<EqpInfo>();
        }
    }

    public class EqpInfo
    {
        public string Name { get; set; }
        public bool IsSensor1 { get; set; }
        public bool IsSensor2 { get; set; }
        public bool IsSensor3 { get; set; }
        public bool IsSensor4 { get; set; }
        public bool IsSensor5 { get; set; }
        public bool IsSensor6 { get; set; }
        public bool IsSensor7 { get; set; }
        public bool IsSensor8 { get; set; }
        public bool IsSensor9 { get; set; }
        public bool IsSensor10 { get; set; }

        public int Value1 { get; set; }
        public int Value2 { get; set; }
        public int Value3 { get; set; }
        public int Value4 { get; set; }
        public int Value5 { get; set; }
        public int Value6 { get; set; }
        public int Value7 { get; set; }
        public int Value8 { get; set; }
        public int Value9 { get; set; }
        public int Value10 { get; set; }
        public string Description { get; set; }
    }
}
