![Raylib-cs Logo](https://github.com/ChrisDill/Raylib-cs/blob/master/Logo/raylib-cs_256x256.png "Raylib-cs Logo")

[discord](https://discord.gg/VkzNHUE)

# Raylib-cs

C# bindings for raylib 3.0, a simple and easy-to-use library to learn videogames programming (www.raylib.com)

## Getting Started

### Building

1. `git clone https://github.com/ChrisDill/Raylib-cs`

2. `cd Raylib-cs`

3. `dotnet build --Configuration Release`

### Running the Tests

1. `dotnet test`

### Running the Examples

1. `dotnet run --project=Examples/Examples.csproj --Configuration Release`

## Installation
So far, I have only done a few tests on Windows and Linux.

1. Copy or reference the bindings folder in your project. See the test projects for reference.

2. The bindings need a native library to load. It should match your platform and configuration. You can either:
    - Download a raylib [release](https://github.com/raysan5/raylib/releases).

    - Build raylib from source. Use this if your using module bindings that are not built in relesaes by default.

3. Make sure the native library is in a place your project can find it. This will vary for your platform. See https://www.mono-project.com/docs/advanced/pinvoke/ for more details.

4. Start coding!

```csharp
using Raylib_cs;

class Program
{
    static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        Raylib.InitWindow(screenWidth, screenHeight, "raylib-cs - Hello World!");

        Raylib.SetTargetFPS(60);
        //--------------------------------------------------------------------------------------

        // Main game loop
        while (!Raylib.WindowShouldClose())    // Detect window close button or ESC key
        {
            // Update
            //----------------------------------------------------------------------------------
            // TODO: Update your variables here
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            Raylib.BeginDrawing();

            Raylib.ClearBackground(Color.RAYWHITE);

            Raylib.DrawText("Hello world!", 190, 200, 20, Color.MAROON);

            Raylib.EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        Raylib.CloseWindow();        // Close window and OpenGL context
        //--------------------------------------------------------------------------------------

        return 0;
    }
}
```

## Tech notes
- Certain funtions take a enum instead of a int such as `IsKeyPressed`.
- Colors stored in the `Color` struct. `RED` changes to `Color.RED`
- Uses `string.Format` instead of `TextFormat`.
- Adds constructors for structs.
- Adds operator overloads for math structs.

## Contributing
If you have any ideas, feel free to open an issue and tell me what you think.
If you'd like to contribute, please fork the repository and make changes as
you'd like. Pull requests are warmly welcome.

If you want to [request features](https://github.com/raysan5/raylib/pulls) or [report bugs](https://github.com/raysan5/raylib/issues) related to the library (in contrast to this binding), please refer to the [author's project repo](https://github.com/raysan5/raylib).

## License
raylib-cs (and raylib) is licensed under an unmodified zlib/libpng license, which is an OSI-certified, BSD-like license that allows static linking with closed source software. Check [LICENSE](LICENSE.md) for further details.
