using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qiu.Utils.Json
{
    public enum JsonType
    {
        MessagePack = 1,//https://github.com/neuecc/MessagePack-CSharp
        ProtobufNet = 2,
        SimdJsonSharp = 3, //net core 3.0实现 https://github.com/EgorBo/SimdJsonSharp
        SpanJson = 4, // 不支持.net Standard 2.0 https://github.com/Tornhoof/SpanJson
        SwifterJson = 5,//https://github.com/Dogwei/Swifter.Json
        Utf8Json = 6, //https://github.com/neuecc/Utf8Json
        Jil = 7, //https://github.com/kevin-montrose/Jil
        ServiceStackText = 8, //https://github.com/ServiceStack/ServiceStack.Text
        Newtonsoft = 9, //https://github.com/JamesNK/Newtonsoft.Json
        TextJson = 10
    }
}
