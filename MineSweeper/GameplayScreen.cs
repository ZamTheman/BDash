using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BDash
{
    public class GameplayScreen : GameScreen
    {
        Player player;
        Map map;
        Collectable collectableTemplate;
        Spaceship spaceship;
        List<Collectable> collectables;

        public GameplayScreen()
        {
            collectables = new List<Collectable>();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            XmlManager<Player> playerLoader = new XmlManager<Player>();
            XmlManager<Map> mapLoader = new XmlManager<Map>();
            XmlManager<Collectable> collectableLoader = new XmlManager<Collectable>();
            XmlManager<Spaceship> spaceshipLoader = new XmlManager<Spaceship>();
            player = playerLoader.Load("Load/Player.xml");
            map = mapLoader.Load("Load/Map/Map1.xml");
            spaceship = spaceshipLoader.Load("Load/Spaceship.xml");
            collectableTemplate = collectableLoader.Load("Load/Collectable.xml");
            player.LoadContent();
            spaceship.LoadContent();
            Vector2 mapSize = map.LoadContent(player, collectables, spaceship);
            mapSize.X -= 32;
            Camera.Instance.MapSize = mapSize;
            collectableTemplate.LoadContent();
            foreach (var item in collectables)
                item.Image = collectableTemplate.Image;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            player.UnloadContent();
            map.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            player.Update(gameTime);
            map.Update(gameTime, ref player, collectables);
            spaceship.Update(gameTime, collectables.Count, player);
            for (int i = 0; i < collectables.Count; i++)
               collectables[i].Update(gameTime, ref player, collectables);

            Camera.Instance.Update(player.Image.Position);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            map.Draw(spriteBatch);
            player.Draw(spriteBatch);
            spaceship.Draw(spriteBatch);
            foreach (var item in collectables)
                item.Draw(spriteBatch);
        }
    }
}
