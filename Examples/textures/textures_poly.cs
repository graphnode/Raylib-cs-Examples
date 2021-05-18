/*******************************************************************************************
*
*   raylib [shapes] example - Draw Textured Polygon
*
*   This example has been created using raylib 99.98 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2014 Ramon Santamaria (@raysan5)
*   Copyright (c) 2021 Chris Camacho (codifies - bedroomcoders.co.uk)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;

namespace Examples
{
    public class textures_poly
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            int numPnts = 11;  // 10 points and back to the start

            Vector2[] tPnts = new[] {
                new Vector2(0.75f, 0),
                new Vector2(0.25f, 0),
                new Vector2(0, 0.5f),
                new Vector2(0, 0.75f),
                new Vector2(0.25f, 1),
                new Vector2(0.375f, 0.875f),
                new Vector2(0.625f, 0.875f),
                new Vector2(0.75f, 1),
                new Vector2(1, 0.75f),
                new Vector2(1, 0.5f),
                // Close the poly
                new Vector2(0.75f, 0)
            };

            Vector2[] pnts = new Vector2[numPnts];

            // create the poly coords from the UV's
            // you don't have to do this you can specify
            // them however you want
            for (int i = 0; i < numPnts; i++)
            {
                pnts[i].X = (tPnts[i].X - 0.5f) * 256.0f;
                pnts[i].Y = (tPnts[i].Y - 0.5f) * 256.0f;
            }

            InitWindow(screenWidth, screenHeight, "raylib [textures] example - Textured Polygon");

            Texture2D tex = LoadTexture("resources/cat.png");
            float ang = 0;

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                // Update your variables here
                //----------------------------------------------------------------------------------
                ang += 1;

                Vector2[] dPnts = new Vector2[numPnts];
                for (int i = 0; i < numPnts; i++)
                {
                    dPnts[i] = Raymath.Vector2Rotate(pnts[i], ang);
                }

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                DrawText("Textured Polygon", 20, 20, 20, DARKGRAY);
                DrawTexturePoly(tex, new Vector2(screenWidth / 2, screenHeight / 2), dPnts, tPnts, numPnts, WHITE);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            UnloadTexture(tex);

            // De-Initialization
            //--------------------------------------------------------------------------------------
            CloseWindow();        // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
