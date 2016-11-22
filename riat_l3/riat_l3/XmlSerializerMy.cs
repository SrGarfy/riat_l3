using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;


namespace riat_l2
{
    class XmlSerializerMy
    {
        private static readonly XmlWriterSettings settings;
        private static readonly XmlSerializerNamespaces namespaces;
        private static readonly ConcurrentDictionary<Type, XmlSerializer> dictionary;

        static XmlSerializerMy()
        {
            settings = new XmlWriterSettings
            {
                Indent = false,
                OmitXmlDeclaration = true
            };
            namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            dictionary = new ConcurrentDictionary<Type, XmlSerializer>();
        }

        public byte[] Serialize<T>(T obj)
        {
            var serializer = dictionary.GetOrAdd(typeof(T), type => new XmlSerializer(type));
            var output = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(output, settings))
            {
                serializer.Serialize(xmlWriter, obj, namespaces);
                return Encoding.UTF8.GetBytes(output.ToString());
            }
        }

        public T Deserialize<T>(byte[] data)
        {
            var serializer = dictionary.GetOrAdd(typeof(T), type => new XmlSerializer(type));
            return (T)serializer.Deserialize(new MemoryStream(data));
        }
    }
}
//<Input><K>10</K><Sums><decimal>1.01</decimal><decimal>2.02</decimal></Sums><Muls><int>1</int><int>4</int></Muls></Input>
