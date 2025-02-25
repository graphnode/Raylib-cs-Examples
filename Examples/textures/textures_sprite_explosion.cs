/*******************************************************************************************
*
*   raylib [textures] example - sprite explosion
*
*   This example has been created using raylib 2.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2019 Anata and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.MouseButton;

namespace Examples
{
    public class textures_sprite_explosion
    {
        const int NUM_FRAMES_PER_LINE = 5;
        const int NUM_LINES = 5;

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [textures] example - sprite explosion");
            InitAudioDevice();

            // Load explosion sound
            Sound fxBoom = LoadSound("resources/audio/boom.wav");

            // Load explosion texture
            Texture2D explosion = LoadTexture("resources/explosion.png");

            // Init variables for animation

            // Sprite one frame rectangle width
            int frameWidth = explosion.width / NUM_FRAMES_PER_LINE;

            // Sprite one frame rectangle height
            int frameHeight = explosion.height / NUM_LINES;

            int currentFrame = 0;
            int currentLine = 0;

            Rectangle frameRec = new Rectangle(0, 0, frameWidth, frameHeight);
            Vector2 position = new Vector2(0.0f, 0.0f);

            bool active = false;
            int framesCounter = 0;

            SetTargetFPS(120);
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------

                // Check for mouse button pressed and activate explosion (if not active)
                if (IsMouseButtonPressed(MOUSE_LEFT_BUTTON) && !active)
                {
                    position = GetMousePosition();
                    active = true;

                    position.X -= frameWidth / 2;
                    position.Y -= frameHeight / 2;

                    PlaySound(fxBoom);
                }

                // Compute explosion animation frames
                if (active)
                {
                    framesCounter++;

                    if (framesCounter > 2)
                    {
                        currentFrame++;

                        if (currentFrame >= NUM_FRAMES_PER_LINE)
                        {
                            currentFrame = 0;
                            currentLine++;

                            if (currentLine >= NUM_LINES)
                            {
                                currentLine = 0;
                                active = false;
                            }
                        }

                        framesCounter = 0;
                    }
                }

                frameRec.x = frameWidth * currentFrame;
                frameRec.y = frameHeight * currentLine;
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                // Draw explosion required frame rectangle
                if (active)
                {
                    DrawTextureRec(explosion, frameRec, position, WHITE);
                }

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadTexture(explosion);   // Unload texture
            UnloadSound(fxBoom);        // Unload sound

            CloseAudioDevice();

            CloseWindow();              // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
