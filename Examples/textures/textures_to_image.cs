/*******************************************************************************************
*
*   raylib [textures] example - Retrieve image data from texture: GetTextureData()
*
*   NOTE: Images are loaded in CPU memory (RAM); textures are loaded in GPU memory (VRAM)
*
*   This example has been created using raylib 1.3 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2015 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;

namespace Examples
{
    public class textures_to_image
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [textures] example - texture to image");

            // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)

            Image image = LoadImage("resources/raylib-cs_logo.png");
            Texture2D texture = LoadTextureFromImage(image);
            UnloadImage(image);

            image = LoadImageFromTexture(texture);
            UnloadTexture(texture);

            texture = LoadTextureFromImage(image);
            UnloadImage(image);
            //---------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                // TODO: Update your variables here
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                int x = screenWidth / 2 - texture.width / 2;
                int y = screenHeight / 2 - texture.height / 2;
                DrawTexture(texture, x, y, WHITE);

                DrawText("this IS a texture loaded from an image!", 300, 370, 10, GRAY);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadTexture(texture);       // Texture unloading

            CloseWindow();                // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
