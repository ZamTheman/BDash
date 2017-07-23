using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using Windows.Storage;

namespace MineSweeper
{
    public class XmlManager<T>
    {
        public Type Type { get; set; }

        public XmlManager()
        {
            Type = typeof(T);
        }

        public T Load(string path)
        {
            T instance;
            var stream = File.OpenRead(path);
            using (TextReader reader = new StreamReader(stream))
            {
                XmlSerializer xml = new XmlSerializer(Type);
                instance = (T)xml.Deserialize(reader);
            }
            return instance;
        }

        public void Save(string path, object obj)
        {
            var stream = File.OpenWrite(path);
            using(TextWriter writer = new StreamWriter(stream))
            {
                XmlSerializer xml = new XmlSerializer(Type);
                xml.Serialize(writer, obj);
            }
        }
    }
}
