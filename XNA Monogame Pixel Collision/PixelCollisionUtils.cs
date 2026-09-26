using Microsoft.Xna.Framework;

namespace XNA_Monogame_Pixel_Collision
{
    internal static class PixelCollisionUtils
    {
        public static PixelCollider CreatePixelCollider(AnimatedTexture2D animatedTexture)
        {
            return new PixelCollider(
                animatedTexture.CurrentFramePixels,
                animatedTexture.FrameWidth,
                animatedTexture.FrameHeight,
                animatedTexture.Position,
                animatedTexture.Origin,
                animatedTexture.Rotation,
                animatedTexture.Scale);
        }

        public static PixelCollider CreatePixelCollider(MouseCursor mouseCursor)
        {
            return new PixelCollider(
                mouseCursor.Pixels,
                mouseCursor.Width,
                mouseCursor.Height,
                mouseCursor.Position,
                Vector2.Zero,
                0f,
                Vector2.One);
        }
    }
}
