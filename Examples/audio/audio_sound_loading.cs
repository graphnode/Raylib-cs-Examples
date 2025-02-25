/*******************************************************************************************
*
*   raylib [audio] example - Sound loading and playing
*
*   NOTE: This example requires OpenAL Soft library installed
*
*   This example has been created using raylib 1.0 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2014 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;

namespace Examples
{
    public class audio_sound_loading
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [audio] example - sound loading and playing");
            InitAudioDevice();      // Initialize audio device

            Sound fxWav = LoadSound("resources/audio/sound.wav");       // Load WAV audio file
            Sound fxOgg = LoadSound("resources/audio/target.ogg");      // Load OGG audio file

            SetTargetFPS(60);
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                if (IsKeyPressed(KEY_SPACE))
                    PlaySound(fxWav);      // Play WAV sound
                if (IsKeyPressed(KEY_ENTER))
                    PlaySound(fxOgg);      // Play OGG sound
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                DrawText("Press SPACE to PLAY the WAV sound!", 200, 180, 20, LIGHTGRAY);
                DrawText("Press ENTER to PLAY the OGG sound!", 200, 220, 20, LIGHTGRAY);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadSound(fxWav);     // Unload sound data
            UnloadSound(fxOgg);     // Unload sound data

            CloseAudioDevice();     // Close audio device

            CloseWindow();          // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
