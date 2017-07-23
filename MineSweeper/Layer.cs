using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BDash
{
    public class Layer
    {
        [XmlElement("TileMap")]
        public TileMap Tile;
        public Image Image;
        List<Tile> tiles;

        public Layer()
        {
            Image = new Image();
            tiles = new List<Tile>();
        }

        public void LoadContent(Vector2 tileDimensions)
        {

            Vector2 position = tileDimensions - new Vector2(32, 32);

            foreach(string row in Tile.Row)
            {
                var split = row.Split(']');
                position.Y += tileDimensions.Y;
                foreach(string s in split){
                    if (s != string.Empty)
                    {
                        position.X += tileDimensions.X;
                        tiles.Add(new Tile());
                        var str = s.Replace("[", "");
                        var xAndYAsStrings = str.Split(':');
                        int x = int.Parse(xAndYAsStrings[0]);
                        int y = int.Parse(xAndYAsStrings[1]);

                        tiles[tiles.Count - 1].LoadContent(position, new Rectangle(x * (int)tileDimensions.X, y * (int)tileDimensions.Y, (int)tileDimensions.X, (int)tileDimensions.Y));
                    }
                }
            }
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tile in tiles)
            {
                Image.Position = tile.Position;
                Image.SourceRect = tile.SourceRect;
                Image.Draw(spriteBatch);
            }
        }
    }
}
