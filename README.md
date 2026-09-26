# XNA MonoGame Pixel Collision

An example of pixel-perfect collision between animated 2D sprites in MonoGame.
The project targets .NET 9 and uses MonoGame DesktopGL, so it runs on Windows,
macOS, and Linux.

![Pixel collision example](https://media.giphy.com/media/9ryCqmXXuNjA3s3HEv/giphy.gif)

## How pixel-perfect collision works

A rectangle intersection only says that the bounding boxes of two sprites touch.
Pixel-perfect collision goes further: it reports a collision only when two
non-transparent pixels overlap.

Each spritesheet frame is read once when content is loaded and its pixel data is
cached in memory. During an intersection test, the colliders first limit the
work to their overlapping screen-space bounds. For every pixel in that area,
the point is converted back to each sprite's local space and both alpha values
are checked. This makes the collision match position, origin, rotation, and
scale, rather than only untransformed rectangles.

## Project structure

- `PixelCollider.cs` contains the main collision logic. `Intersects` performs
  the transformed, alpha-based pixel comparison.
- `PixelCollisionUtils.cs` converts visual objects into `PixelCollider` values.
- `AnimatedTexture2D.cs` is responsible only for spritesheet animation and
  drawing. It advances frames with delta time, selects the source rectangle,
  caches frame pixels at load time, and renders the current frame. It does not
  contain collision logic.
- `MouseCursor.cs` owns mouse input, cursor rendering, and cached cursor pixels.
- `FpsDisplay.cs` calculates and renders the FPS counter.
- `GameTester.cs` coordinates the demo entities and their collision feedback.

## How to run it

Install .NET SDK 9.0.200 or later. From the repository root, run:

```sh
dotnet run --project Desktop/Desktop.csproj
```

On the first build, the local tool manifest restores `mgcb`, which compiles the
content pipeline assets automatically.

You can build without launching the game with:

```sh
dotnet build Desktop/Desktop.csproj --configuration Release
```

Open `XNA Monogame Pixel Collision/XNA Monogame Pixel Collision.slnx` in a
compatible IDE, or use the same SDK version to build the solution from the CLI.

## Credits

- Created by Marcelo Belkiman — marcelobelkiman@gmail.com
- Mouse hand by Joe Williamson (@JoeCreates)
- Monsters created by Stephen Challener (Redshrike) and Bonsaiheldin
- Collision example originally based on code by Dominique Louis (CartBlanche)

## License

MIT License
