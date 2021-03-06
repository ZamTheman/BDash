﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BDash
{
    public class Player : SpriteBase
    {
        private enum moveDirection
        {
            None,
            Up,
            Down,
            Right,
            Left
        }
        
        List<moveDirection> lastPressed;
        public Vector2 OldPosition;
        public float MoveSpeed;
        double timeSinceMove;

        public Player()
        {
            lastPressed = new List<moveDirection> { moveDirection.None };
        }
        
        public void Update(GameTime gameTime)
        {
            timeSinceMove += gameTime.ElapsedGameTime.TotalMilliseconds;
            OldPosition = Image.Position;
            if (InputManager.Instance.KeyDown(Keys.Up))
                AddToLastClicked(moveDirection.Up);
            else
                RemoveFromLastClicked(moveDirection.Up);

            if (InputManager.Instance.KeyDown(Keys.Down))
                AddToLastClicked(moveDirection.Down);
            else
                RemoveFromLastClicked(moveDirection.Down);

            if (InputManager.Instance.KeyDown(Keys.Right))
                AddToLastClicked(moveDirection.Right);
            else
                RemoveFromLastClicked(moveDirection.Right);

            if (InputManager.Instance.KeyDown(Keys.Left))
                AddToLastClicked(moveDirection.Left);
            else
                RemoveFromLastClicked(moveDirection.Left);
            
            if(timeSinceMove > MoveSpeed)
            {
                switch (lastPressed[lastPressed.Count - 1])
                {
                    case moveDirection.None:
                        Image.SpriteSheetEffect.CurrentFrame.Y = 0;
                        break;
                    case moveDirection.Up:
                        Image.Position.Y -= tileSize;
                        break;
                    case moveDirection.Down:
                        Image.Position.Y += tileSize;
                        break;
                    case moveDirection.Right:
                        Image.SpriteSheetEffect.CurrentFrame.Y = 1;
                        Image.Position.X += tileSize;
                        break;
                    case moveDirection.Left:
                        Image.SpriteSheetEffect.CurrentFrame.Y = 2;
                        Image.Position.X -= tileSize;
                        break;
                    default:
                        break;
                }
                timeSinceMove = 0;
            }
            base.Update(gameTime);
        }

        private void RemoveFromLastClicked(moveDirection direction)
        {
            if (lastPressed != null && lastPressed.Count > 0)
            {
                for (int i = 0; i < lastPressed.Count; i++)
                    if (lastPressed[i] == direction)
                        lastPressed.RemoveAt(i);
            }
        }

        private void AddToLastClicked(moveDirection direction)
        {
            if (!lastPressed.Any(d => d == direction))
                lastPressed.Add(direction);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Image.PlayerDraw(spriteBatch);
        }
    }
}
