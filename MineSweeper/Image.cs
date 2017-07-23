using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BDash
{
    public class Image
    {
        public float Alpha;
        public string Text, FontName, Path;
        public Vector2 Position;
        public Vector2 Scale;
        public Rectangle SourceRect;
        public bool IsActive;

        private Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
        }

        Vector2 origin;
        ContentManager content;
        RenderTarget2D renderTarget;
        SpriteFont font;
        Dictionary<string, ImageEffect> effectList;
        public string Effects;

        public FadeEffect FadeEffect;
        public SpriteSheetEffect SpriteSheetEffect;

        public Image()
        {
            Path = Text = Effects = String.Empty;
            FontName = "DefaultFont";
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Alpha = 1.0f;
            effectList = new Dictionary<string, ImageEffect>();
        }

        void SetEffect<T>(ref T effect)
        {
            if (effect == null)
                effect = (T)Activator.CreateInstance(typeof(T));
            else
            {
                (effect as ImageEffect).IsActive = true;
                var obj = this;
                (effect as ImageEffect).LoadContent(ref obj);
            }
            
            effectList.Add(effect.GetType().ToString().Replace("BDash.", ""), (effect as ImageEffect)); 
        }

        public void ActivateEffect(string effect)
        {
            if (effectList.ContainsKey(effect))
            {
                effectList[effect].IsActive = true;
                var obj = this;
                effectList[effect].LoadContent(ref obj);
            }
        }

        public void DeactivateEffect(string effect)
        {
            if (effectList.ContainsKey(effect))
            {
                effectList[effect].IsActive = false;
                effectList[effect].UnloadContent();
            }
        }

        public void StoreEffects()
        {
            Effects = string.Empty;
            foreach (var item in effectList)
            {
                if (item.Value.IsActive)
                    Effects += item.Key + ":";
            }
            if(Effects != string.Empty)
                Effects.Remove(Effects.Length - 1);
        }

        public void RestoreEffects()
        {
            foreach (var item in effectList)
                DeactivateEffect(item.Key);

            string[] split = Effects.Split(':');
            foreach (var item in split)
                ActivateEffect(item);
        }

        public void LoadContent()
        {
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            if (Path != String.Empty)
                texture = content.Load<Texture2D>(Path);

            font = content.Load<SpriteFont>(FontName);

            Vector2 dimensions = Vector2.Zero;

            if (texture != null)
                dimensions.X += texture.Width;
            
            dimensions.X += font.MeasureString(Text).X;

            if (texture != null)
                dimensions.Y = Math.Max(texture.Height, font.MeasureString(Text).Y);
            else
                 dimensions.Y = font.MeasureString(Text).Y;

            if (SourceRect == Rectangle.Empty)
                SourceRect = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);

            renderTarget = new RenderTarget2D(ScreenManager.Instance.GraphicsDevice, (int)dimensions.X, (int)dimensions.Y);

            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(renderTarget);
            ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);
            ScreenManager.Instance.SpriteBatch.Begin();
            if(texture != null)
                ScreenManager.Instance.SpriteBatch.Draw(texture, Vector2.Zero, Color.White);

            ScreenManager.Instance.SpriteBatch.DrawString(font, Text, Vector2.Zero, Color.White);
            ScreenManager.Instance.SpriteBatch.End();

            texture = renderTarget;

            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);

            SetEffect(ref FadeEffect);
            SetEffect(ref SpriteSheetEffect);

            if(Effects != string.Empty)
            {
                string[] split = Effects.Split(':');
                foreach (string item in split)
                {
                    ActivateEffect(item);
                }
            }
        }

        public void UnloadContent()
        {
            content.Unload();
            foreach (var effect in effectList)
            {
                DeactivateEffect(effect.Key);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var effect in effectList)
            {
                if(effect.Value.IsActive)
                    effect.Value.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            origin = new Vector2(SourceRect.Width / 2, SourceRect.Height / 2);
            spriteBatch.Draw(texture, Position + origin, SourceRect, 
                Color.White * Alpha, 0.0f, origin, Scale, SpriteEffects.None, 0.0f);
        }
    }
}
