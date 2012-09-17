using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Common
{
    [DataContract]
    public abstract class MessageBase
    {
        [DataContract(Name = "MessageContainer")]
        [KnownType("KnownTypes")]
        private class Container
        {
            [DataMember]
            public MessageBase Message;

            private static readonly Type[] _types;

            static Container()
            {
                var mt = typeof(MessageBase);
                _types = mt.Assembly.GetTypes().Where(mt.IsAssignableFrom).ToArray();
            }

            private static Type[] KnownTypes()
            {
                return _types;
            }

        }

        public static string Serialize<T>(T message)
            where T : MessageBase
        {
            var serializer = new DataContractSerializer(typeof(Container));
            using (var ms = new MemoryStream())
            using (var xw = XmlWriter.Create(ms))
            {
                serializer.WriteObject(xw, new Container()
                {
                    Message = message
                });
                xw.Flush();

                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        public static MessageBase Deserialize(string message)
        {
            var serializer = new DataContractSerializer(typeof(Container));
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(message)))
            using (var xr = XmlReader.Create(ms))
            {
                return ((Container)serializer.ReadObject(xr)).Message;
            }
        }
    }
}
