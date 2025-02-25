/*******************************************************************************************
*
*   raylib [shapes] example - easings box anim
*
*   This example has been created using raylib 2.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2014-2019 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;

namespace Examples
{
    public class shapes_easing_box_anim
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [shapes] example - easings box anim");

            // Box variables to be animated with easings
            Rectangle rec = new Rectangle(GetScreenWidth() / 2, -100, 100, 100);
            float rotation = 0.0f;
            float alpha = 1.0f;

            int state = 0;
            int framesCounter = 0;

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                switch (state)
                {
                    // Move box down to center of screen
                    case 0:
                        framesCounter += 1;

                        // NOTE: Remember that 3rd parameter of easing function refers to
                        // desired value variation, do not confuse it with expected final value!
                        rec.y = Easings.EaseElasticOut(framesCounter, -100, GetScreenHeight() / 2 + 100, 120);

                        if (framesCounter >= 120)
                        {
                            framesCounter = 0;
                            state = 1;
                        }
                        break;
                    // Scale box to an horizontal bar
                    case 1:
                        framesCounter += 1;
                        rec.height = Easings.EaseBounceOut(framesCounter, 100, -90, 120);
                        rec.width = Easings.EaseBounceOut(framesCounter, 100, GetScreenWidth(), 120);

                        if (framesCounter >= 120)
                        {
                            framesCounter = 0;
                            state = 2;
                        }
                        break;
                    // Rotate horizontal bar rectangle
                    case 2:
                        framesCounter += 1;
                        rotation = Easings.EaseQuadOut(framesCounter, 0.0f, 270.0f, 240);

                        if (framesCounter >= 240)
                        {
                            framesCounter = 0;
                            state = 3;
                        }
                        break;
                    // Increase bar size to fill all screen
                    case 3:
                        framesCounter += 1;
                        rec.height = Easings.EaseCircOut(framesCounter, 10, GetScreenWidth(), 120);

                        if (framesCounter >= 120)
                        {
                            framesCounter = 0;
                            state = 4;
                        }
                        break;
                    // Fade out animation
                    case 4:
                        framesCounter++;
                        alpha = Easings.EaseSineOut(framesCounter, 1.0f, -1.0f, 160);

                        if (framesCounter >= 160)
                        {
                            framesCounter = 0;
                            state = 5;
                        }
                        break;
                    default:
                        break;
                }

                // Reset animation at any moment
                if (IsKeyPressed(KEY_SPACE))
                {
                    rec = new Rectangle(GetScreenWidth() / 2, -100, 100, 100);
                    rotation = 0.0f;
                    alpha = 1.0f;
                    state = 0;
                    framesCounter = 0;
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                DrawRectanglePro(rec, new Vector2(rec.width / 2, rec.height / 2), rotation, ColorAlpha(BLACK, alpha));
                DrawText("PRESS [SPACE] TO RESET BOX ANIMATION!", 10, GetScreenHeight() - 25, 20, LIGHTGRAY);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            CloseWindow();        // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
