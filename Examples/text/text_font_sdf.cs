/*******************************************************************************************
*
*   raylib [text] example - TTF loading and usage
*
*   This example has been created using raylib 1.3.0 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2015 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.TextureFilter;

namespace Examples
{
    public class text_font_sdf
    {
        public unsafe static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [text] example - SDF fonts");

            // NOTE: Textures/Fonts MUST be loaded after Window initialization (OpenGL context is required)
            string msg = "Signed Distance Fields";

            // Loading file to memory
            uint fileSize = 0;
            byte* fileData = LoadFileData("resources/fonts/anonymous_pro_bold.ttf", ref fileSize);

            // Default font generation from TTF font
            Font fontDefault = new Font();
            fontDefault.baseSize = 16;
            fontDefault.glyphCount = 95;

            // Loading font data from memory data
            // Parameters > font size: 16, no chars array provided (0), chars count: 95 (autogenerate chars array)
            fontDefault.glyphs = LoadFontData(fileData, (int)fileSize, 16, null, 95, FontType.FONT_DEFAULT);
            // Parameters > chars count: 95, font size: 16, chars padding in image: 4 px, pack method: 0 (default)
            Image atlas = GenImageFontAtlas(fontDefault.glyphs, &fontDefault.recs, 95, 16, 4, 0);
            fontDefault.texture = LoadTextureFromImage(atlas);
            UnloadImage(atlas);

            // SDF font generation from TTF font
            Font fontSDF = new Font();
            fontSDF.baseSize = 16;
            fontSDF.glyphCount = 95;
            // Parameters > font size: 16, no chars array provided (0), chars count: 0 (defaults to 95)
            fontSDF.glyphs = LoadFontData(fileData, (int)fileSize, 16, null, 0, FontType.FONT_SDF);
            // Parameters > chars count: 95, font size: 16, chars padding in image: 0 px, pack method: 1 (Skyline algorythm)
            atlas = GenImageFontAtlas(fontSDF.glyphs, &fontSDF.recs, 95, 16, 0, 1);
            fontSDF.texture = LoadTextureFromImage(atlas);
            UnloadImage(atlas);

            // Free memory from loaded file
            UnloadFileData(fileData);

            // Load SDF required shader (we use default vertex shader)
            Shader shader = LoadShader(null, "resources/shaders/glsl330/sdf.fs");
            // Required for SDF font
            SetTextureFilter(fontSDF.texture, TEXTURE_FILTER_BILINEAR);

            Vector2 fontPosition = new Vector2(40, screenHeight / 2 - 50);
            Vector2 textSize = new Vector2(0.0f);
            float fontSize = 16.0f;
            // 0 - fontDefault, 1 - fontSDF
            int currentFont = 0;

            SetTargetFPS(60);
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                fontSize += GetMouseWheelMove() * 8.0f;

                if (fontSize < 6)
                {
                    fontSize = 6;
                }

                if (IsKeyDown(KEY_SPACE))
                {
                    currentFont = 1;
                }
                else
                {
                    currentFont = 0;
                }

                if (currentFont == 0)
                {
                    textSize = MeasureTextEx(fontDefault, msg, fontSize, 0);
                }
                else
                {
                    textSize = MeasureTextEx(fontSDF, msg, fontSize, 0);
                }

                fontPosition.X = GetScreenWidth() / 2 - textSize.X / 2;
                fontPosition.Y = GetScreenHeight() / 2 - textSize.Y / 2 + 80;
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                if (currentFont == 1)
                {
                    // NOTE: SDF fonts require a custom SDf shader to compute fragment color
                    BeginShaderMode(shader);
                    DrawTextEx(fontSDF, msg, fontPosition, fontSize, 0, BLACK);
                    EndShaderMode();

                    DrawTexture(fontSDF.texture, 10, 10, BLACK);
                }
                else
                {
                    DrawTextEx(fontDefault, msg, fontPosition, fontSize, 0, BLACK);
                    DrawTexture(fontDefault.texture, 10, 10, BLACK);
                }

                if (currentFont == 1)
                {
                    DrawText("SDF!", 320, 20, 80, RED);
                }
                else
                {
                    DrawText("default font", 315, 40, 30, GRAY);
                }

                DrawText("FONT SIZE: 16.0", GetScreenWidth() - 240, 20, 20, DARKGRAY);
                DrawText(string.Format("RENDER SIZE: {0:00.00}", fontSize), GetScreenWidth() - 240, 50, 20, DARKGRAY);
                DrawText("Use MOUSE WHEEL to SCALE TEXT!", GetScreenWidth() - 240, 90, 10, DARKGRAY);

                DrawText("PRESS SPACE to USE SDF FONT VERSION!", 340, GetScreenHeight() - 30, 20, MAROON);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadFont(fontDefault);    // Default font unloading
            UnloadFont(fontSDF);        // SDF font unloading
            UnloadShader(shader);       // Unload SDF shader

            CloseWindow();              // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
