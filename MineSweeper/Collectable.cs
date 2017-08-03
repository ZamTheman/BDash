using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace BDash
{
    public class Collectable : SpriteBase
    {
        public Vector2 Position;

        public override void Draw(SpriteBatch spriteBatch)
        {
            Image.Position = Position;
            Image.TileDraw(spriteBatch);
        }

        public void Update(GameTime gameTime, ref Player player, List<Collectable> collectables)
        {
            if (player.Image.Position == Position)
                collectables.Remove(this);
            base.Update(gameTime);
        }
    }
}
