using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XNA_Monogame_Pixel_Collision
{
    internal class AnimatedTexture2D
    {
        private const double FrameDurationSeconds = 0.1;

        private readonly int _columns;
        private int _currentFrame;
        private readonly int _frameCount;
        private double _elapsedTime;
        private readonly int _frameHeight;
        private readonly int _frameWidth;
        private readonly Color[][] _framePixels;
        private Rectangle _source;
        private readonly Texture2D _texture;

        public Color Color { get; set; } = Color.White;
        public Color[] CurrentFramePixels => _framePixels[_currentFrame];
        public int FrameHeight => _frameHeight;
        public int FrameWidth => _frameWidth;
        public Vector2 Origin { get; set; } = Vector2.Zero;
        public Vector2 Position { get; set; } = Vector2.Zero;
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;

        public AnimatedTexture2D(ContentManager content, int columns, int lines, string assetName)
        {
            _texture = content.Load<Texture2D>(assetName);
            ValidateFrameLayout(columns, lines);

            _columns = columns;
            _frameCount = columns * lines;
            _frameWidth = _texture.Width / columns;
            _frameHeight = _texture.Height / lines;
            _framePixels = CacheFramePixels();

            UpdateSourceRectangle();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _texture,
                Position,
                _source,
                Color,
                Rotation,
                Origin,
                Scale,
                SpriteEffects.None,
                0f);
        }

        public void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

            //PLAY NEXT FRAME
            while (_elapsedTime >= FrameDurationSeconds)
            {
                _elapsedTime -= FrameDurationSeconds;
                AdvanceFrame();
            }

            UpdateSourceRectangle();
        }

        private void AdvanceFrame()
        {
            _currentFrame++;

            //RESET VALUES
            if (_currentFrame >= _frameCount)
            {
                _currentFrame = 0;
            }
        }

        private Color[][] CacheFramePixels()
        {
            var framePixels = new Color[_frameCount][];
            for (var frame = 0; frame < _frameCount; frame++)
            {
                framePixels[frame] = GetFramePixels(frame);
            }

            return framePixels;
        }

        private Color[] GetFramePixels(int frame)
        {
            var source = GetSourceRectangle(frame);
            var pixels = new Color[_frameWidth * _frameHeight];
            _texture.GetData(0, source, pixels, 0, pixels.Length);
            return pixels;
        }

        private Rectangle GetSourceRectangle(int frame)
        {
            var column = frame % _columns;
            var line = frame / _columns;
            return new Rectangle(
                column * _frameWidth,
                line * _frameHeight,
                _frameWidth,
                _frameHeight);
        }

        private void UpdateSourceRectangle()
        {
            _source = GetSourceRectangle(_currentFrame);
        }

        private void ValidateFrameLayout(int columns, int lines)
        {
            if (columns <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(columns), "The spritesheet must have at least one column.");
            }

            if (lines <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lines), "The spritesheet must have at least one line.");
            }

            if (_texture.Width % columns != 0 || _texture.Height % lines != 0)
            {
                throw new ArgumentException("The spritesheet dimensions must be evenly divisible by its frame layout.");
            }
        }
    }
}
