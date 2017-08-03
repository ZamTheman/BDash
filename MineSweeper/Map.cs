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
        public Vector2 TileDimensions;


        public Map()
        {
            Layer = new List<Layer>();
            TileDimensions = Vector2.Zero;
        }

        public Vector2 LoadContent(Player player, List<Collectable> collectables, Spaceship spaceship)
        {
            Vector2 mapSize = Vector2.Zero;
            foreach (var l in Layer)
            {
                mapSize = l.LoadContent(TileDimensions, player, collectables, spaceship);
            }
            return mapSize;
        }

        public void UnloadContent()
        {
            foreach (var l in Layer)
            {
                l.UnloadContent();
            }
        }

        public void Update(GameTime gameTime, ref Player player, List<Collectable> collectables)
        {
            foreach (var l in Layer)
            {
                l.Update(gameTime, ref player, collectables);
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
