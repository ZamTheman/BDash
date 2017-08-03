using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BDash
{
    public class Spaceship : SpriteBase
    {
        public void Update(GameTime gameTime, int nrGases, Player player)
        {
            if (nrGases == 0)
            {
                if (!Image.SpriteSheetEffect.IsActive)
                    Image.SpriteSheetEffect.IsActive = true;

                if(Image.Position == player.Image.Position)
                {
                    // Player won
                }
            }

            else
            {
                if (Image.SpriteSheetEffect.IsActive)
                {
                    Image.SpriteSheetEffect.IsActive = false;
                    Image.SourceRect = new Rectangle(0, 0, 32, 32);
                }
                    

                if (Image.Position == player.Image.Position)
                {
                    player.Image.Position = player.OldPosition;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Image.TileDraw(spriteBatch);
        }
    }
}
