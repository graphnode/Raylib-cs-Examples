/*******************************************************************************************
*
*   raylib [core] example - split screen
*
*   This example has been created using raylib 3.7 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by Jeffery Myers (@JeffM2501) and reviewed by Ramon Santamaria (@raysan5)
*
*   Copyright (c) 2021 Jeffery Myers (@JeffM2501)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.TextureFilter;
using static Raylib_cs.TextureWrap;

namespace Examples
{
    public unsafe class core_split_screen
    {
        static Texture2D textureGrid;
        static Camera3D cameraPlayer1;
        static Camera3D cameraPlayer2;

        // Scene drawing
        static void DrawScene()
        {
            int count = 5;
            float spacing = 4;

            // Grid of cube trees on a plane to make a "world"
            // Simple world plane
            DrawPlane(new Vector3(0, 0, 0), new Vector2(50, 50), BEIGE);

            for (float x = -count * spacing; x <= count * spacing; x += spacing)
            {
                for (float z = -count * spacing; z <= count * spacing; z += spacing)
                {
                    DrawCubeTexture(textureGrid, new Vector3(x, 1.5f, z), 1, 1, 1, GREEN);
                    DrawCubeTexture(textureGrid, new Vector3(x, 0.5f, z), 0.25f, 1, 0.25f, BROWN);
                }
            }

            // Draw a cube at each player's position
            DrawCube(cameraPlayer1.position, 1, 1, 1, RED);
            DrawCube(cameraPlayer2.position, 1, 1, 1, BLUE);
        }

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [core] example - split screen");

            // Generate a simple texture to use for trees
            Image img = GenImageChecked(256, 256, 32, 32, DARKGRAY, WHITE);
            textureGrid = LoadTextureFromImage(img);
            UnloadImage(img);
            SetTextureFilter(textureGrid, TEXTURE_FILTER_ANISOTROPIC_16X);
            SetTextureWrap(textureGrid, TEXTURE_WRAP_CLAMP);

            // Setup player 1 camera and screen
            cameraPlayer1.fovy = 45.0f;
            cameraPlayer1.up.Y = 1.0f;
            cameraPlayer1.target.Y = 1.0f;
            cameraPlayer1.position.Z = -3.0f;
            cameraPlayer1.position.Y = 1.0f;

            RenderTexture2D screenPlayer1 = LoadRenderTexture(screenWidth / 2, screenHeight);

            // Setup player two camera and screen
            cameraPlayer2.fovy = 45.0f;
            cameraPlayer2.up.Y = 1.0f;
            cameraPlayer2.target.Y = 3.0f;
            cameraPlayer2.position.X = -3.0f;
            cameraPlayer2.position.Y = 3.0f;

            RenderTexture2D screenPlayer2 = LoadRenderTexture(screenWidth / 2, screenHeight);

            // Build a flipped rectangle the size of the split view to use for drawing later
            Rectangle splitScreenRect = new Rectangle(0.0f, 0.0f, (float)screenPlayer1.texture.width, (float)-screenPlayer1.texture.height);

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                // If anyone moves this frame, how far will they move based on the time since the last frame
                // this moves thigns at 10 world units per second, regardless of the actual FPS
                float offsetThisFrame = 10.0f * GetFrameTime();

                // Move Player1 forward and backwards (no turning)
                if (IsKeyDown(KEY_W))
                {
                    cameraPlayer1.position.Z += offsetThisFrame;
                    cameraPlayer1.target.Z += offsetThisFrame;
                }
                else if (IsKeyDown(KEY_S))
                {
                    cameraPlayer1.position.Z -= offsetThisFrame;
                    cameraPlayer1.target.Z -= offsetThisFrame;
                }

                // Move Player2 forward and backwards (no turning)
                if (IsKeyDown(KEY_UP))
                {
                    cameraPlayer2.position.X += offsetThisFrame;
                    cameraPlayer2.target.X += offsetThisFrame;
                }
                else if (IsKeyDown(KEY_DOWN))
                {
                    cameraPlayer2.position.X -= offsetThisFrame;
                    cameraPlayer2.target.X -= offsetThisFrame;
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                // Draw Player1 view to the render texture
                BeginTextureMode(screenPlayer1);
                ClearBackground(SKYBLUE);

                BeginMode3D(cameraPlayer1);
                DrawScene();
                EndMode3D();

                DrawText("PLAYER 1 W/S to move", 10, 10, 20, RED);
                EndTextureMode();

                // Draw Player2 view to the render texture
                BeginTextureMode(screenPlayer2);
                ClearBackground(SKYBLUE);

                BeginMode3D(cameraPlayer2);
                DrawScene();
                EndMode3D();

                DrawText("PLAYER 2 UP/DOWN to move", 10, 10, 20, BLUE);
                EndTextureMode();

                // Draw both views render textures to the screen side by side
                BeginDrawing();
                ClearBackground(BLACK);

                DrawTextureRec(screenPlayer1.texture, splitScreenRect, new Vector2(0, 0), WHITE);
                DrawTextureRec(screenPlayer2.texture, splitScreenRect, new Vector2(screenWidth / 2.0f, 0), WHITE);

                EndDrawing();
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadRenderTexture(screenPlayer1); // Unload render texture
            UnloadRenderTexture(screenPlayer2); // Unload render texture
            UnloadTexture(textureGrid);         // Unload texture

            CloseWindow();                      // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
