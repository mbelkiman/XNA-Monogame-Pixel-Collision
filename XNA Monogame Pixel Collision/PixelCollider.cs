using System;
using Microsoft.Xna.Framework;

namespace XNA_Monogame_Pixel_Collision
{
    /*
     * Immutable snapshot of a sprite frame and the transform used to draw it.
     *
     * Intersects follows this sequence:
     * 1. Receive cached pixels for the current sprite frame. The image is not cropped
     *    or read from the GPU during collision detection.
     * 2. Transform the four sprite corners to calculate an axis-aligned screen-space
     *    bound for each collider.
     * 3. Visit only the screen pixels inside the overlap of those two bounds.
     * 4. Convert each screen-pixel center back into the local space of both sprites,
     *    accounting for position, origin, rotation, and scale.
     * 5. Read the matching cached pixels. A collision exists when both have alpha
     *    greater than zero.
     */
    internal readonly struct PixelCollider
    {
        private readonly Color[] _pixels;
        private readonly int _width;
        private readonly int _height;
        private readonly Vector2 _position;
        private readonly Vector2 _origin;
        private readonly Vector2 _scale;
        private readonly float _cosRotation;
        private readonly float _sinRotation;

        public PixelCollider(Color[] pixels, int width, int height, Vector2 position, Vector2 origin, float rotation, Vector2 scale)
        {
            _pixels = pixels;
            _width = width;
            _height = height;
            _position = position;
            _origin = origin;
            _scale = scale;
            _cosRotation = MathF.Cos(rotation);
            _sinRotation = MathF.Sin(rotation);
        }

        public bool Intersects(in PixelCollider other)
        {
            // Use transformed bounds as a cheap first pass before sampling pixels.
            var bounds = GetBounds();
            var otherBounds = other.GetBounds();
            var top = Math.Max(bounds.Top, otherBounds.Top);
            var bottom = Math.Min(bounds.Bottom, otherBounds.Bottom);
            var left = Math.Max(bounds.Left, otherBounds.Left);
            var right = Math.Min(bounds.Right, otherBounds.Right);

            for (var y = top; y < bottom; y++)
            {
                for (var x = left; x < right; x++)
                {
                    // Sample the center of each screen pixel in both local sprite spaces.
                    var point = new Vector2(x + 0.5f, y + 0.5f);
                    // A collision happens only when both sampled pixels are visible.
                    if (TryGetPixel(point, out var color) &&
                        other.TryGetPixel(point, out var otherColor) &&
                        color.A != 0 && otherColor.A != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private Rectangle GetBounds()
        {
            // Transform all four local corners to create an axis-aligned screen-space bound.
            var topLeft = TransformToWorld(Vector2.Zero);
            var topRight = TransformToWorld(new Vector2(_width, 0));
            var bottomLeft = TransformToWorld(new Vector2(0, _height));
            var bottomRight = TransformToWorld(new Vector2(_width, _height));

            var minX = MathF.Min(MathF.Min(topLeft.X, topRight.X), MathF.Min(bottomLeft.X, bottomRight.X));
            var maxX = MathF.Max(MathF.Max(topLeft.X, topRight.X), MathF.Max(bottomLeft.X, bottomRight.X));
            var minY = MathF.Min(MathF.Min(topLeft.Y, topRight.Y), MathF.Min(bottomLeft.Y, bottomRight.Y));
            var maxY = MathF.Max(MathF.Max(topLeft.Y, topRight.Y), MathF.Max(bottomLeft.Y, bottomRight.Y));

            return new Rectangle(
                (int)MathF.Floor(minX),
                (int)MathF.Floor(minY),
                (int)MathF.Ceiling(maxX) - (int)MathF.Floor(minX),
                (int)MathF.Ceiling(maxY) - (int)MathF.Floor(minY));
        }

        private Vector2 TransformToWorld(Vector2 localPosition)
        {
            // Match SpriteBatch's transform order: move around the origin, scale, rotate, then translate.
            var translatedPosition = (localPosition - _origin) * _scale;
            return _position + new Vector2(
                translatedPosition.X * _cosRotation - translatedPosition.Y * _sinRotation,
                translatedPosition.X * _sinRotation + translatedPosition.Y * _cosRotation);
        }

        private bool TryGetPixel(Vector2 worldPosition, out Color color)
        {
            color = Color.Transparent;
            if (_scale.X == 0 || _scale.Y == 0)
            {
                return false;
            }

            // Apply the inverse transform to find which source pixel covers this screen point.
            var translatedPosition = worldPosition - _position;
            var unrotatedPosition = new Vector2(
                translatedPosition.X * _cosRotation + translatedPosition.Y * _sinRotation,
                -translatedPosition.X * _sinRotation + translatedPosition.Y * _cosRotation);
            var localPosition = new Vector2(
                unrotatedPosition.X / _scale.X + _origin.X,
                unrotatedPosition.Y / _scale.Y + _origin.Y);
            var x = (int)MathF.Floor(localPosition.X);
            var y = (int)MathF.Floor(localPosition.Y);

            if ((uint)x >= _width || (uint)y >= _height)
            {
                return false;
            }

            color = _pixels[x + y * _width];
            return true;
        }
    }
}
