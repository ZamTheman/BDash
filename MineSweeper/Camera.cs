using Microsoft.Xna.Framework;

namespace BDash
{
    public class Camera
    {
        public Vector2 Position;
        public Vector2 ViewSize;
        public Vector2 MapSize;
        public Vector2 Offset;

        private static Camera instance;
        public static Camera Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Camera();
                }
                return instance;
            }
        }

        private Camera()
        {
            ViewSize.X = ScreenManager.Instance.GraphicsDevice.PresentationParameters.BackBufferWidth;
            ViewSize.Y = ScreenManager.Instance.GraphicsDevice.PresentationParameters.BackBufferHeight;
        }

        public void Update(Vector2 playerPosition)
        {
            if (playerPosition.X <= ViewSize.X / 2)
                Position.X = ViewSize.X / 2;
            else if (playerPosition.X + ViewSize.X / 2 > MapSize.X)
                Position.X = MapSize.X - ViewSize.X / 2;
            else
                Position.X = playerPosition.X;
                

            if (playerPosition.Y <= ViewSize.Y / 2)
                Position.Y = ViewSize.Y / 2;
            else if (playerPosition.Y + ViewSize.Y / 2 > MapSize.Y)
                Position.Y = MapSize.Y - ViewSize.Y / 2;
            else
                Position.Y = playerPosition.Y;
            
            Offset.X = Position.X - ViewSize.X / 2;
            Offset.Y = Position.Y - ViewSize.Y / 2;
        }
    }
}
