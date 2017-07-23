using System.Collections.Generic;
using System.Xml.Serialization;

namespace BDash
{
    public class TileMap
    {
        [XmlElement("Row")]
        public List<string> Row;

        public TileMap()
        {
            Row = new List<string>();
        }
    }
}
