using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BDash
{
    public enum states
    {
        Solid,
        Consumable,
        Falling,
        Expanding,
        Pushable,
        Player
    }

    public class Tile
    {
        Vector2 position;
        Rectangle sourceRect;
        List<states> tileStates;
        Double passedTime;
        Double pushedLeftTime;
        Double pushedRightTime;
        bool isFalling;
        int tileSize;

        public Rectangle SourceRect
        {
            get { return sourceRect;  }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public Tile()
        {
            isFalling = false;
            passedTime = 0;
            pushedLeftTime = 0;
            pushedRightTime = 0;
        }
        
        public void LoadContent(Vector2 position, Rectangle sourceRect, List<states> tileStates)
        {
            this.position = position;
            this.sourceRect = sourceRect;
            this.tileStates = tileStates;
        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime, ref Player player, List<Tile> tiles, Vector2 tileDimensions, List<Collectable> collectables)
        {
            tileSize = (int)tileDimensions.X;

            if(tileStates.Contains(states.Solid))
                SolidTileLogic(ref player);

            passedTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (tileStates.Contains(states.Falling) && passedTime > 125)
            {
                FallingTileLogic(ref player, tiles, collectables);
                passedTime = 0;
            }   

            if (tileStates.Contains(states.Pushable))
            {
                var wasPushed = false;

                if (InputManager.Instance.KeyDown(Keys.Right) && player.Image.Position.X == position.X - tileSize && player.Image.Position.Y == position.Y)
                {
                    wasPushed = PushableTileLogic("right", tiles, gameTime);
                    if (wasPushed)
                        player.Image.Position.X += tileSize;
                    pushedLeftTime = 0;
                }
                    

                else if (InputManager.Instance.KeyDown(Keys.Left) && player.Image.Position.X == position.X + tileSize && player.Image.Position.Y == position.Y)
                {
                    wasPushed = PushableTileLogic("left", tiles, gameTime);
                    if (wasPushed)
                        player.Image.Position.X -= tileSize;
                    pushedRightTime = 0;
                }

                else
                    pushedLeftTime = pushedRightTime = 0;
            }

            if (tileStates.Contains(states.Consumable))
            {
                if (position == player.Image.Position)
                    ConsumableTileLogic(tiles);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        private void SolidTileLogic(ref Player player)
        {
            
            Rectangle tileRect = new Rectangle((int)Position.X + 1, (int)Position.Y + 1, sourceRect.Width - 2, sourceRect.Height - 2);
            Rectangle playerRect = new Rectangle((int)player.Image.Position.X + 1, (int)player.Image.Position.Y + 1, player.Image.SourceRect.Width - 2, player.Image.SourceRect.Height - 2);

            if (playerRect.Intersects(tileRect))
            {
                player.Image.Position = player.OldPosition;
            }
        }

        private void FallingTileLogic(ref Player player, List<Tile> tiles, List<Collectable> collectables)
        {
            // Add the player to the tiles to check
            var tilesIncludingPlayer = AddPlayerToTilesList(tiles, ref player);

            // Add collectables to the tiles to check
            var tilesIncludingCollectables = AddCollectablesToTileList(tilesIncludingPlayer, collectables);
            
            var isGrounded = false;
            var sideWays = false;
            var tileBelow = tilesIncludingCollectables.FirstOrDefault(t => t.Position.X == position.X && t.position.Y == position.Y + tileSize);
            if (tileBelow != null)
            {
                if (tileBelow.tileStates.Contains(states.Player))
                {
                    if (isFalling)
                    {
                        // player Died
                    }
                    else
                    {
                        isGrounded = true;
                        isFalling = false;
                    }
                }
                else if (tileBelow.tileStates.Contains(states.Consumable))
                {
                    isGrounded = true;
                    isFalling = false;
                }
                else if (tileBelow.tileStates.Contains(states.Solid))
                {
                    var tileLeft = tilesIncludingCollectables.Any(t => t.Position.X == position.X - tileSize && t.Position.Y == Position.Y);
                    var tileBelowLeft = tilesIncludingCollectables.Any(t => t.Position.X == position.X - tileSize && t.Position.Y - tileSize == Position.Y);
                    var tileRight = tilesIncludingCollectables.Any(t => t.Position.X == position.X + tileSize && t.Position.Y == Position.Y);
                    var tileBelowRight = tilesIncludingCollectables.Any(t => t.Position.X == position.X + tileSize && t.Position.Y - tileSize == Position.Y);

                    if ((!tileLeft && !tileBelowLeft) && (tileRight || tileBelowRight))
                    {
                        position.X -= tileSize;
                    }

                    else if ((tileLeft || tileBelowLeft) && (!tileRight && !tileBelowRight))
                    {
                        position.X += tileSize;
                    }

                    else if (!tileLeft && !tileBelowLeft && !tileRight && !tileBelowRight)
                    {
                        var rnd = new Random();
                        var leftOrRight = rnd.Next(0, 2);
                        position.X += leftOrRight > 0 ? -tileSize : tileSize;
                    }

                    else
                    {
                        isFalling = false;
                        isGrounded = true;
                    }   
                }
            }

            if (!isGrounded)
            {
                if(!sideWays)
                    position.Y = position.Y + tileSize;
                isFalling = true;
            }
        }

        private List<Tile> AddCollectablesToTileList(List<Tile> tiles, List<Collectable> collectables)
        {
            var localTiles = new List<Tile>();
            localTiles.AddRange(tiles);
            foreach (var item in collectables)
            {
                var collectableTile = new Tile()
                {
                    position = item.Position,
                    tileStates = new List<states> { states.Solid }
                };
                localTiles.Add(collectableTile);
            }

            return localTiles;
        }

        private List<Tile> AddPlayerToTilesList(List<Tile> tiles, ref Player player)
        {
            var localTiles = new List<Tile>();
            localTiles.AddRange(tiles);
            var playerTile = new Tile()
            {
                position = player.Image.Position,
                tileStates = new List<states> { states.Player }
            };
            localTiles.Add(playerTile);

            return localTiles;
        }

        private bool PushableTileLogic(string direction, List<Tile> tiles, GameTime gameTime)
        {
            switch (direction)
            {
                case "right":
                    var tileToTheRight = tiles.Any(t => t.Position.Y == position.Y && t.Position.X == position.X + tileSize);
                    if(pushedRightTime > 500 && !tileToTheRight)
                    {
                        position.X += tileSize;
                        pushedRightTime = 0;
                        return true;
                    }
                    else
                    {
                        pushedRightTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                        return false;
                    }
                    

                case "left":
                    var tileToTheLeft = tiles.Any(t => t.Position.Y == position.Y && t.Position.X == position.X - tileSize);
                    if (pushedLeftTime > 500 && !tileToTheLeft)
                    {
                        position.X -= tileSize;
                        pushedLeftTime = 0;
                        return true;
                    }
                    else
                    {
                        pushedLeftTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                        return false;
                    }
                    

                default:
                    return false;
            }
        }

        private void ConsumableTileLogic(List<Tile> tiles)
        {
            tiles.Remove(this);
        }
    }
}
