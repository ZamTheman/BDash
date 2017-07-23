using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MineSweeper
{
    public class Player
    {
        public Image Image;
        public Vector2 Velocity;
        public float MoveSpeed;

        public Player()
        {
            Velocity = Vector2.Zero;
        }

        public void LoadContent()
        {
            Image.LoadContent();
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        double timeSinceMove;
        int tileSize = 32;
        public void Update(GameTime gameTime)
        {
            timeSinceMove += gameTime.ElapsedGameTime.TotalMilliseconds;
             
            if(timeSinceMove > MoveSpeed)
            {
                if (Velocity.X == 0)
                {
                    if (InputManager.Instance.KeyDown(Keys.Down))
                    {
                        Image.Position.Y += tileSize;
                    }
                    else if (InputManager.Instance.KeyDown(Keys.Up))
                    {
                        Image.Position.Y -= tileSize;
                    }
                }

                if (Velocity.Y == 0)
                {
                    if (InputManager.Instance.KeyDown(Keys.Right))
                    {
                        Image.Position.X += tileSize;
                    }
                    else if (InputManager.Instance.KeyDown(Keys.Left))
                    {
                        Image.Position.X -= tileSize;
                    }
                }
                timeSinceMove = 0;
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }
}
