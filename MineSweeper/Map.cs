using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MineSweeper
{
    public class Map
    {
        List<List<int>> tiles;

        public void LoadMap()
        {
            tiles = new List<List<int>>();
            XDocument xdoc = XDocument.Load("map.xml");

        
        }
    }
}
