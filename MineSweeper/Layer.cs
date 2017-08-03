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
        public string TileTypes;
        public string SpriteSheetPosition;
        public string Solid;
        public string Consumable;
        public string Falling;
        public string Pushable;
        private Dictionary<string, string> tileTypesAndShortNames;
        private Dictionary<string, Vector2> spriteSheetPosition;
        private Dictionary<string, List<states>> tileStates;
        private Vector2 tileDimensions;
        public string SolidTiles;
        List<Tile> tiles;

        public Layer()
        {
            Image = new Image();
            tiles = new List<Tile>();
            tileTypesAndShortNames = new Dictionary<string, string>();
            spriteSheetPosition = new Dictionary<string, Vector2>();
            tileStates = new Dictionary<string, List<states>>();
            tileDimensions = Vector2.Zero;
        }

        public Vector2 LoadContent(Vector2 tileDimensions, Player player, List<Collectable> collectables, Spaceship spaceship)
        {
            Image.LoadContent();
            this.tileDimensions = tileDimensions;
            Vector2 position = -tileDimensions;
            Vector2 mapSize = Vector2.Zero;

            var types = TileTypes.Split(',');
            foreach (var item in types)
            {
                var splittedItem = item.Split(':');
                tileTypesAndShortNames.Add(splittedItem[1], splittedItem[0]);
            }

            var imagePosition = SpriteSheetPosition.Split(',');
            foreach (var item in imagePosition)
            {
                var splittedItem = item.Split(':');
                spriteSheetPosition.Add(splittedItem[0], new Vector2() { X = int.Parse(splittedItem[1]), Y = int.Parse(splittedItem[2]) });
            }
            
            foreach (var item in tileTypesAndShortNames)
            {
                tileStates.Add(item.Key, new List<states>());
                if (Solid.Contains(item.Key))
                {
                    tileStates[item.Key].Add(states.Solid);
                }

                if (Consumable.Contains(item.Key))
                {
                    tileStates[item.Key].Add(states.Consumable);
                }

                if (Falling.Contains(item.Key))
                {
                    tileStates[item.Key].Add(states.Falling);
                }

                if (Pushable.Contains(item.Key))
                {
                    tileStates[item.Key].Add(states.Pushable);
                }
            }
            
            foreach(string row in Tile.Row)
            {
                var split = row.Split(']');
                position.Y += tileDimensions.Y;
                foreach(string s in split){
                    if (s != string.Empty)
                    {
                        position.X += tileDimensions.X;
                        var type = s.Substring(1, 1);

                        if (type.Equals("e"))
                            continue;

                        else if (type.Equals("p"))
                            player.Image.Position = position;

                        else if (type.Equals("x"))
                            spaceship.Image.Position = position;

                        else if (type.Equals("g"))
                        {
                            collectables.Add(new Collectable());
                            collectables[collectables.Count - 1].Position = position;
                        }


                        else
                        {
                            tiles.Add(new Tile());
                            tiles[tiles.Count - 1].LoadContent(position, new Rectangle((int)spriteSheetPosition[type].X * (int)tileDimensions.X, (int)spriteSheetPosition[type].Y * (int)tileDimensions.Y, (int)tileDimensions.X, (int)tileDimensions.Y), tileStates[type]);
                        }
                    }
                }
                position.X = -tileDimensions.X;
                if (mapSize.X == 0)
                    mapSize.X = split.Length * tileDimensions.X;
            }
            mapSize.Y = Tile.Row.Count * tileDimensions.Y;

            return mapSize;
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime, ref Player player, List<Collectable> collectables)
        {
            var tilesInYOrder = tiles.OrderByDescending(t => t.Position.Y).ToList();
            foreach (var tile in tilesInYOrder)
            {
                tile.Update(gameTime, ref player, tiles, tileDimensions, collectables);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tile in tiles)
            {
                Image.Position = tile.Position - Camera.Instance.Offset;
                Image.SourceRect = tile.SourceRect;
                Image.Draw(spriteBatch);
            }
        }
    }
}
