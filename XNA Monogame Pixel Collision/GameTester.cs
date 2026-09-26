using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNA_Monogame_Pixel_Collision
{
    public class GameTester : Game
    {
        private const int WindowWidth = 320;
        private const int WindowHeight = 240;
        private const float EnemyCRotationSpeed = MathHelper.PiOver2;
        private const float EnemyDMovementDistance = 120f;
        private const float EnemyDMovementSpeed = 1.5f;
        private const float EnemyDScaleSpeed = 2.5f;
        private const float EnemyDVerticalPosition = 195f;

        private SpriteBatch _spriteBatch = null!;
        private FpsDisplay _fpsDisplay = null!;
        private MouseCursor _mouseCursor = null!;

        private AnimatedTexture2D _enemyA = null!;
        private AnimatedTexture2D _enemyB = null!;
        private AnimatedTexture2D _enemyC = null!;
        private AnimatedTexture2D _enemyD = null!;

        public GameTester()
        {
            new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = WindowWidth,
                PreferredBackBufferHeight = WindowHeight
            };
            Content.RootDirectory = "Content";
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            DrawEnemies();
            _mouseCursor.Draw(_spriteBatch);
            _fpsDisplay.Draw();
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _fpsDisplay = new FpsDisplay(GraphicsDevice, _spriteBatch, Content);
            _mouseCursor = new MouseCursor(Content, "hand");

            _enemyA = CreateEnemy("enemyA", 5, new Vector2(10, 10));
            _enemyB = CreateEnemy("enemyB", 3, new Vector2(275, 145));
            _enemyC = CreateEnemy("enemyA", 5, new Vector2(WindowWidth / 2f, 80), centerOrigin: true);
            _enemyD = CreateEnemy("enemyB", 3, Vector2.Zero, centerOrigin: true);
        }

        protected override void Update(GameTime gameTime)
        {
            _fpsDisplay.Update(gameTime);
            _mouseCursor.Update();
            UpdateEnemies(gameTime);
            UpdateEnemyTransforms(gameTime);
            UpdateEnemyCollisionColors();

            base.Update(gameTime);
        }

        private AnimatedTexture2D CreateEnemy(string assetName, int columns, Vector2 position, bool centerOrigin = false)
        {
            var enemy = new AnimatedTexture2D(Content, columns, 1, assetName)
            {
                Position = position
            };

            if (centerOrigin)
            {
                enemy.Origin = new Vector2(enemy.FrameWidth / 2f, enemy.FrameHeight / 2f);
            }

            return enemy;
        }

        private void DrawEnemies()
        {
            _enemyA.Draw(_spriteBatch);
            _enemyB.Draw(_spriteBatch);
            _enemyC.Draw(_spriteBatch);
            _enemyD.Draw(_spriteBatch);
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            _enemyA.Update(gameTime);
            _enemyB.Update(gameTime);
            _enemyC.Update(gameTime);
            _enemyD.Update(gameTime);
        }

        private void UpdateEnemyTransforms(GameTime gameTime)
        {
            var totalSeconds = (float)gameTime.TotalGameTime.TotalSeconds;
            _enemyC.Rotation = totalSeconds * EnemyCRotationSpeed;

            _enemyD.Position = new Vector2(
                WindowWidth / 2f + MathF.Sin(totalSeconds * EnemyDMovementSpeed) * EnemyDMovementDistance,
                EnemyDVerticalPosition);
            var scale = 1f + (MathF.Sin(totalSeconds * EnemyDScaleSpeed) + 1f) * 0.25f;
            _enemyD.Scale = new Vector2(scale);
        }

        private void UpdateEnemyCollisionColors()
        {
            var mouseCollider = PixelCollisionUtils.CreatePixelCollider(_mouseCursor);
            UpdateEnemyCollisionColor(_enemyA, mouseCollider);
            UpdateEnemyCollisionColor(_enemyB, mouseCollider);
            UpdateEnemyCollisionColor(_enemyC, mouseCollider);
            UpdateEnemyCollisionColor(_enemyD, mouseCollider);
        }

        private static void UpdateEnemyCollisionColor(AnimatedTexture2D enemy, in PixelCollider mouseCollider)
        {
            enemy.Color = PixelCollisionUtils.CreatePixelCollider(enemy).Intersects(mouseCollider)
                ? Color.DarkRed
                : Color.White;
        }
    }
}
