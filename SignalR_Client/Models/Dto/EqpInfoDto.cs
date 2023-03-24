using SignalR_Client.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR_Client.Models.Dto
{
    public class EqpInfoDto : BindableBase
    {
        public string Name { get; set; }

        private bool _isSensor1; public bool IsSensor1 { get { return _isSensor1; } set{Set(ref _isSensor1, value);}}
        private bool _isSensor2; public bool IsSensor2 { get { return _isSensor2; } set { Set(ref _isSensor2, value); } }
        private bool _isSensor3; public bool IsSensor3 { get { return _isSensor3; } set { Set(ref _isSensor3, value); } }
        private bool _isSensor4; public bool IsSensor4 { get { return _isSensor4; } set { Set(ref _isSensor4, value); } }
        private bool _isSensor5; public bool IsSensor5 { get { return _isSensor5; } set { Set(ref _isSensor5, value); } }
        private bool _isSensor6; public bool IsSensor6 { get { return _isSensor6; } set { Set(ref _isSensor6, value); } }
        private bool _isSensor7; public bool IsSensor7 { get { return _isSensor7; } set { Set(ref _isSensor7, value); } }
        private bool _isSensor8; public bool IsSensor8 { get { return _isSensor8; } set { Set(ref _isSensor8, value); } }
        private bool _isSensor9; public bool IsSensor9 { get { return _isSensor9; } set { Set(ref _isSensor9, value); } }
        private bool _isSensor10; public bool IsSensor10 { get { return _isSensor10; } set { Set(ref _isSensor10, value); } }


        private int _value1; public int Value1 { get { return _value1; } set { Set(ref _value1, value); } }
        private int _value2; public int Value2 { get { return _value2; } set { Set(ref _value2, value); } }
        private int _value3; public int Value3 { get { return _value3; } set { Set(ref _value3, value); } }
        private int _value4; public int Value4 { get { return _value4; } set { Set(ref _value4, value); } }
        private int _value5; public int Value5 { get { return _value5; } set { Set(ref _value5, value); } }
        private int _value6; public int Value6 { get { return _value6; } set { Set(ref _value6, value); } }
        private int _value7; public int Value7 { get { return _value7; } set { Set(ref _value7, value); } }
        private int _value8; public int Value8 { get { return _value8; } set { Set(ref _value8, value); } }
        private int _value9; public int Value9 { get { return _value9; } set { Set(ref _value9, value); } }
        private int _value10; public int Value10 { get { return _value10; } set { Set(ref _value10, value); } }

        public string Description { get; set; }
    }
}
