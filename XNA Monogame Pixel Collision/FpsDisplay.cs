using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XNA_Monogame_Pixel_Collision
{
    internal sealed class FpsDisplay
    {
        private const double SampleDurationSeconds = 1;

        private readonly GraphicsDevice _graphicsDevice;
        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _font;
        private double _elapsedTime;
        private int _frameCount;
        private int _framesPerSecond;

        public FpsDisplay(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, ContentManager content)
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
            _font = content.Load<SpriteFont>("FpsFont");
        }

        public void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (_elapsedTime < SampleDurationSeconds)
            {
                return;
            }

            _framesPerSecond = (int)Math.Round(_frameCount / _elapsedTime);
            _frameCount = 0;
            _elapsedTime = 0;
        }

        public void Draw()
        {
            _frameCount++;

            var text = $"FPS: {_framesPerSecond}";
            var textSize = _font.MeasureString(text);
            var position = new Vector2(_graphicsDevice.Viewport.Width - textSize.X - 8, 8);
            _spriteBatch.DrawString(_font, text, position, Color.White);
        }
    }
}
