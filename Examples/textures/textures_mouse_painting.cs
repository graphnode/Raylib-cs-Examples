/*******************************************************************************************
*
*   raylib [textures] example - Mouse painting
*
*   This example has been created using raylib 2.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by Chris Dill (@MysteriousSpace) and reviewed by Ramon Santamaria (@raysan5)
*
*   Copyright (c) 2019 Chris Dill (@MysteriousSpace) and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.MouseButton;

namespace Examples
{
    public class textures_mouse_painting
    {
        public const int MAX_COLORS_COUNT = 23;          // Number of colors available

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [textures] example - mouse painting");

            // Colours to choose from
            Color[] colors = new Color[] {
            RAYWHITE, YELLOW, GOLD, ORANGE, PINK, RED, MAROON, GREEN, LIME, DARKGREEN,
            SKYBLUE, BLUE, DARKBLUE, PURPLE, VIOLET, DARKPURPLE, BEIGE, BROWN, DARKBROWN,
            LIGHTGRAY, GRAY, DARKGRAY, BLACK };

            // Define colorsRecs data (for every rectangle)
            Rectangle[] colorsRecs = new Rectangle[MAX_COLORS_COUNT];

            for (int i = 0; i < MAX_COLORS_COUNT; i++)
            {
                colorsRecs[i].x = 10 + 30 * i + 2 * i;
                colorsRecs[i].y = 10;
                colorsRecs[i].width = 30;
                colorsRecs[i].height = 30;
            }

            int colorSelected = 0;
            int colorSelectedPrev = colorSelected;
            int colorMouseHover = 0;
            int brushSize = 20;

            Rectangle btnSaveRec = new Rectangle(750, 10, 40, 30);
            bool btnSaveMouseHover = false;
            bool showSaveMessage = false;
            int saveMessageCounter = 0;

            // Create a RenderTexture2D to use as a canvas
            RenderTexture2D target = LoadRenderTexture(screenWidth, screenHeight);

            // Clear render texture before entering the game loop
            BeginTextureMode(target);
            ClearBackground(colors[0]);
            EndTextureMode();

            SetTargetFPS(120);              // Set our game to run at 120 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                Vector2 mousePos = GetMousePosition();

                // Move between colors with keys
                if (IsKeyPressed(KEY_RIGHT))
                    colorSelected++;
                else if (IsKeyPressed(KEY_LEFT))
                    colorSelected--;

                if (colorSelected >= MAX_COLORS_COUNT)
                    colorSelected = MAX_COLORS_COUNT - 1;
                else if (colorSelected < 0)
                    colorSelected = 0;

                // Choose color with mouse
                for (int i = 0; i < MAX_COLORS_COUNT; i++)
                {
                    if (CheckCollisionPointRec(mousePos, colorsRecs[i]))
                    {
                        colorMouseHover = i;
                        break;
                    }
                    else
                        colorMouseHover = -1;
                }

                if ((colorMouseHover >= 0) && IsMouseButtonPressed(MOUSE_LEFT_BUTTON))
                {
                    colorSelected = colorMouseHover;
                    colorSelectedPrev = colorSelected;
                }

                // Change brush size
                brushSize += (int)(GetMouseWheelMove() * 5);
                if (brushSize < 2)
                    brushSize = 2;
                if (brushSize > 50)
                    brushSize = 50;

                if (IsKeyPressed(KEY_C))
                {
                    // Clear render texture to clear color
                    BeginTextureMode(target);
                    ClearBackground(colors[0]);
                    EndTextureMode();
                }

                if (IsMouseButtonDown(MOUSE_LEFT_BUTTON))
                {
                    // Paint circle into render texture
                    // NOTE: To avoid discontinuous circles, we could store
                    // previous-next mouse points and just draw a line using brush size
                    BeginTextureMode(target);
                    if (mousePos.Y > 50)
                        DrawCircle((int)mousePos.X, (int)mousePos.Y, brushSize, colors[colorSelected]);
                    EndTextureMode();
                }
                else if (IsMouseButtonDown(MOUSE_RIGHT_BUTTON))
                {
                    colorSelected = 0;

                    // Erase circle from render texture
                    BeginTextureMode(target);
                    if (mousePos.Y > 50)
                        DrawCircle((int)mousePos.X, (int)mousePos.Y, brushSize, colors[0]);
                    EndTextureMode();
                }
                else
                {
                    colorSelected = colorSelectedPrev;
                }

                // Check mouse hover save button
                if (CheckCollisionPointRec(mousePos, btnSaveRec))
                    btnSaveMouseHover = true;
                else
                    btnSaveMouseHover = false;

                // Image saving logic
                // NOTE: Saving painted texture to a default named image
                if ((btnSaveMouseHover && IsMouseButtonReleased(MOUSE_LEFT_BUTTON)) || IsKeyPressed(KEY_S))
                {
                    Image image = LoadImageFromTexture(target.texture);
                    ImageFlipVertical(ref image);
                    ExportImage(image, "my_amazing_texture_painting.png");
                    UnloadImage(image);
                    showSaveMessage = true;
                }

                if (showSaveMessage)
                {
                    // On saving, show a full screen message for 2 seconds
                    saveMessageCounter++;
                    if (saveMessageCounter > 240)
                    {
                        showSaveMessage = false;
                        saveMessageCounter = 0;
                    }
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                // NOTE: Render texture must be y-flipped due to default OpenGL coordinates (left-bottom)
                Rectangle source = new Rectangle(0, 0, target.texture.width, -target.texture.height);
                DrawTextureRec(target.texture, source, new Vector2(0, 0), WHITE);

                // Draw drawing circle for reference
                if (mousePos.Y > 50)
                {
                    if (IsMouseButtonDown(MOUSE_RIGHT_BUTTON))
                    {
                        DrawCircleLines((int)mousePos.X, (int)mousePos.Y, brushSize, colors[colorSelected]);
                    }
                    else
                    {
                        DrawCircle(GetMouseX(), GetMouseY(), brushSize, colors[colorSelected]);
                    }
                }

                // Draw top panel
                DrawRectangle(0, 0, GetScreenWidth(), 50, RAYWHITE);
                DrawLine(0, 50, GetScreenWidth(), 50, LIGHTGRAY);

                // Draw color selection rectangles
                for (int i = 0; i < MAX_COLORS_COUNT; i++)
                {
                    DrawRectangleRec(colorsRecs[i], colors[i]);
                }

                DrawRectangleLines(10, 10, 30, 30, LIGHTGRAY);

                if (colorMouseHover >= 0)
                {
                    DrawRectangleRec(colorsRecs[colorMouseHover], ColorAlpha(WHITE, 0.6f));
                }

                Rectangle rec = new Rectangle(
                    colorsRecs[colorSelected].x - 2,
                    colorsRecs[colorSelected].y - 2,
                    colorsRecs[colorSelected].width + 4,
                    colorsRecs[colorSelected].height + 4
                );
                DrawRectangleLinesEx(rec, 2, BLACK);

                // Draw save image button
                DrawRectangleLinesEx(btnSaveRec, 2, btnSaveMouseHover ? RED : BLACK);
                DrawText("SAVE!", 755, 20, 10, btnSaveMouseHover ? RED : BLACK);

                // Draw save image message
                if (showSaveMessage)
                {
                    DrawRectangle(0, 0, GetScreenWidth(), GetScreenHeight(), ColorAlpha(RAYWHITE, 0.8f));
                    DrawRectangle(0, 150, GetScreenWidth(), 80, BLACK);
                    DrawText("IMAGE SAVED:  my_amazing_texture_painting.png", 150, 180, 20, RAYWHITE);
                }

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadRenderTexture(target);    // Unload render texture

            CloseWindow();                  // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
