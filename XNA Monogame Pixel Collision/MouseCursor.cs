using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNA_Monogame_Pixel_Collision
{
    internal sealed class MouseCursor
    {
        private readonly Color[] _pixels;
        private readonly Texture2D _texture;

        public int Height => _texture.Height;
        public Color[] Pixels => _pixels;
        public Vector2 Position { get; private set; }
        public int Width => _texture.Width;

        public MouseCursor(ContentManager content, string assetName)
        {
            _texture = content.Load<Texture2D>(assetName);
            _pixels = new Color[_texture.Width * _texture.Height];
            _texture.GetData(_pixels);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color.White);
        }

        public void Update()
        {
            var mouseState = Mouse.GetState();
            Position = new Vector2(mouseState.X, mouseState.Y);
        }
    }
}
