/*******************************************************************************************
*
*   raylib [core] example - quat conversions
*
*   Generally you should really stick to eulers OR quats...
*   This tests that various conversions are equivalent.
*
*   This example has been created using raylib 3.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by Chris Camacho (@chriscamacho) and reviewed by Ramon Santamaria (@raysan5)
*
*   Copyright (c) 2020 Chris Camacho (@chriscamacho) and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;

namespace Examples
{
    public static class core_quat_conversion
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [core] example - quat conversions");

            Camera3D camera = new Camera3D();
            camera.position = new Vector3(0.0f, 10.0f, 10.0f);
            camera.target = new Vector3(0.0f, 0.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.projection = CameraProjection.CAMERA_PERSPECTIVE;

            // Load a cylinder model for testing
            Model model = LoadModelFromMesh(GenMeshCylinder(0.2f, 1.0f, 32));

            // Generic quaternion for operations
            var q1 = new Quaternion();

            // Transform matrices required to draw 4 cylinders
            var m1 = new Matrix4x4();
            var m2 = new Matrix4x4();
            var m3 = new Matrix4x4();
            var m4 = new Matrix4x4();

            // Generic vectors for rotations
            var v1 = new Vector3();
            var v2 = new Vector3();

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //--------------------------------------------------------------------------------------
                float twoPi = MathF.PI * 2;
                if (v2.X < 0)
                {
                    v2.X += twoPi;
                }
                if (v2.Y < 0)
                {
                    v2.Y += twoPi;
                }
                if (v2.Z < 0)
                {
                    v2.Z += twoPi;
                }

                if (!IsKeyDown(KEY_SPACE))
                {
                    v1.X += 0.01f;
                    v1.Y += 0.03f;
                    v1.Z += 0.05f;
                }

                if (v1.X > twoPi)
                {
                    v1.X -= twoPi;
                }
                if (v1.Y > twoPi)
                {
                    v1.Y -= twoPi;
                }
                if (v1.Z > twoPi)
                {
                    v1.Z -= twoPi;
                }

                q1 = QuaternionFromEuler(v1.X, v1.Y, v1.Z);
                m1 = MatrixRotateZYX(v1);
                m2 = QuaternionToMatrix(q1);

                q1 = QuaternionFromMatrix(m1);
                m3 = QuaternionToMatrix(q1);

                v2 = QuaternionToEuler(q1);

                m4 = MatrixRotateZYX(v2);
                //--------------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                BeginMode3D(camera);

                model.transform = m1;
                DrawModel(model, new Vector3(-1, 0, 0), 1.0f, RED);
                model.transform = m2;
                DrawModel(model, new Vector3(1, 0, 0), 1.0f, RED);
                model.transform = m3;
                DrawModel(model, new Vector3(0, 0, 0), 1.0f, RED);
                model.transform = m4;
                DrawModel(model, new Vector3(0, 0, -1), 1.0f, RED);

                DrawGrid(10, 1.0f);

                EndMode3D();

                Color colorX = (v1.X == v2.X) ? GREEN : BLACK;
                Color colorY = (v1.Y == v2.Y) ? GREEN : BLACK;
                Color colorZ = (v1.Z == v2.Z) ? GREEN : BLACK;

                DrawText($"{v1.X}", 20, 20, 20, colorX);
                DrawText($"{v1.Y}", 20, 40, 20, colorY);
                DrawText($"{v1.Z}", 20, 60, 20, colorZ);

                DrawText($"{v2.X}", 200, 20, 20, colorX);
                DrawText($"{v2.Y}", 200, 40, 20, colorY);
                DrawText($"{v2.Z}", 200, 60, 20, colorZ);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadModel(model);   // Unload model data (mesh and materials)

            CloseWindow();        // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
