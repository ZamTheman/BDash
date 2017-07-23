using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BDash
{
    public class Map
    {
        [XmlElement("Layer")]
        public List<Layer> Layer;
        public Vector2 TileDimenstions;


        public Map()
        {
            Layer = new List<Layer>();
            TileDimenstions = Vector2.Zero;
        }

        public void LoadContent()
        {
            foreach (var l in Layer)
            {
                l.LoadContent(TileDimenstions);
            }
        }

        public void UnloadContent()
        {
            foreach (var l in Layer)
            {
                l.UnloadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var l in Layer)
            {
                l.Update(gameTime);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var l in Layer)
            {
                l.Draw(spriteBatch);
            }
        }


    }
}
