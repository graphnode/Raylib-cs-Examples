/*******************************************************************************************
*
*   raylib [models] example - rlgl module usage with push/pop matrix transformations
*
*   This example uses [rlgl] module funtionality (pseudo-OpenGL 1.1 style coding)
*
*   This example has been created using raylib 2.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2018 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Rlgl;
using static Raylib_cs.Color;
using static Raylib_cs.CameraMode;
using static Raylib_cs.CameraProjection;

namespace Examples
{
    public class models_rlgl_solar_system
    {
        //------------------------------------------------------------------------------------
        // Program main entry point
        //------------------------------------------------------------------------------------
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            const float sunRadius = 4.0f;
            const float earthRadius = 0.6f;
            const float earthOrbitRadius = 8.0f;
            const float moonRadius = 0.16f;
            const float moonOrbitRadius = 1.5f;

            InitWindow(screenWidth, screenHeight, "raylib [models] example - rlgl module usage with push/pop matrix transformations");

            // Define the camera to look into our 3d world
            Camera3D camera = new Camera3D();
            camera.position = new Vector3(16.0f, 16.0f, 16.0f);
            camera.target = new Vector3(0.0f, 0.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.projection = CAMERA_PERSPECTIVE;

            SetCameraMode(camera, CAMERA_FREE);

            float rotationSpeed = 0.2f;         // General system rotation speed

            float earthRotation = 0.0f;         // Rotation of earth around itself (days) in degrees
            float earthOrbitRotation = 0.0f;    // Rotation of earth around the Sun (years) in degrees
            float moonRotation = 0.0f;          // Rotation of moon around itself
            float moonOrbitRotation = 0.0f;     // Rotation of moon around earth in degrees

            SetTargetFPS(60);                   // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())        // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera);

                earthRotation += (5.0f * rotationSpeed);
                earthOrbitRotation += (365 / 360.0f * (5.0f * rotationSpeed) * rotationSpeed);
                moonRotation += (2.0f * rotationSpeed);
                moonOrbitRotation += (8.0f * rotationSpeed);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                BeginMode3D(camera);

                rlPushMatrix();
                rlScalef(sunRadius, sunRadius, sunRadius);          // Scale Sun
                DrawSphereBasic(GOLD);                              // Draw the Sun
                rlPopMatrix();

                rlPushMatrix();
                rlRotatef(earthOrbitRotation, 0.0f, 1.0f, 0.0f);    // Rotation for Earth orbit around Sun
                rlTranslatef(earthOrbitRadius, 0.0f, 0.0f);         // Translation for Earth orbit
                rlRotatef(-earthOrbitRotation, 0.0f, 1.0f, 0.0f);   // Rotation for Earth orbit around Sun inverted

                rlPushMatrix();
                rlRotatef(earthRotation, 0.25f, 1.0f, 0.0f);       // Rotation for Earth itself
                rlScalef(earthRadius, earthRadius, earthRadius);// Scale Earth

                DrawSphereBasic(BLUE);                          // Draw the Earth
                rlPopMatrix();

                rlRotatef(moonOrbitRotation, 0.0f, 1.0f, 0.0f);     // Rotation for Moon orbit around Earth
                rlTranslatef(moonOrbitRadius, 0.0f, 0.0f);          // Translation for Moon orbit
                rlRotatef(-moonOrbitRotation, 0.0f, 1.0f, 0.0f);    // Rotation for Moon orbit around Earth inverted
                rlRotatef(moonRotation, 0.0f, 1.0f, 0.0f);          // Rotation for Moon itself
                rlScalef(moonRadius, moonRadius, moonRadius);       // Scale Moon

                DrawSphereBasic(LIGHTGRAY);                         // Draw the Moon
                rlPopMatrix();

                // Some reference elements (not affected by previous matrix transformations)
                DrawCircle3D(new Vector3(0.0f, 0.0f, 0.0f), earthOrbitRadius, new Vector3(1, 0, 0), 90.0f, ColorAlpha(RED, 0.5f));
                DrawGrid(20, 1.0f);

                EndMode3D();

                DrawText("EARTH ORBITING AROUND THE SUN!", 400, 10, 20, MAROON);
                DrawFPS(10, 10);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            CloseWindow();        // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }

        //--------------------------------------------------------------------------------------------
        // Module Functions Definitions (local)
        //--------------------------------------------------------------------------------------------

        // Draw sphere without any matrix transformation
        // NOTE: Sphere is drawn in world position ( 0, 0, 0 ) with radius 1.0f
        static void DrawSphereBasic(Color color)
        {
            int rings = 16;
            int slices = 16;

            rlBegin(DrawMode.TRIANGLES);
            rlColor4ub(color.r, color.g, color.b, color.a);

            for (int i = 0; i < (rings + 2); i++)
            {
                for (int j = 0; j < slices; j++)
                {
                    rlVertex3f(MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * i)) * MathF.Sin(DEG2RAD * (j * 360 / slices)),
                               MathF.Sin(DEG2RAD * (270 + (180 / (rings + 1)) * i)),
                               MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * i)) * MathF.Cos(DEG2RAD * (j * 360 / slices)));
                    rlVertex3f(MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))) * MathF.Sin(DEG2RAD * ((j + 1) * 360 / slices)),
                               MathF.Sin(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))),
                               MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))) * MathF.Cos(DEG2RAD * ((j + 1) * 360 / slices)));
                    rlVertex3f(MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))) * MathF.Sin(DEG2RAD * (j * 360 / slices)),
                               MathF.Sin(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))),
                               MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))) * MathF.Cos(DEG2RAD * (j * 360 / slices)));

                    rlVertex3f(MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * i)) * MathF.Sin(DEG2RAD * (j * 360 / slices)),
                               MathF.Sin(DEG2RAD * (270 + (180 / (rings + 1)) * i)),
                               MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * i)) * MathF.Cos(DEG2RAD * (j * 360 / slices)));
                    rlVertex3f(MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i))) * MathF.Sin(DEG2RAD * ((j + 1) * 360 / slices)),
                               MathF.Sin(DEG2RAD * (270 + (180 / (rings + 1)) * (i))),
                               MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i))) * MathF.Cos(DEG2RAD * ((j + 1) * 360 / slices)));
                    rlVertex3f(MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))) * MathF.Sin(DEG2RAD * ((j + 1) * 360 / slices)),
                               MathF.Sin(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))),
                               MathF.Cos(DEG2RAD * (270 + (180 / (rings + 1)) * (i + 1))) * MathF.Cos(DEG2RAD * ((j + 1) * 360 / slices)));
                }
            }
            rlEnd();
        }
    }
}
