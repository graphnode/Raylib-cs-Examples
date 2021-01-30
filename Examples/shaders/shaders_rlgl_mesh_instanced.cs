/*******************************************************************************************
*
*   raylib [shaders] example - rlgl module usage for instanced meshes
*
*   This example uses [rlgl] module funtionality (pseudo-OpenGL 1.1 style coding)
*
*   This example has been created using raylib 3.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by @seanpringle and reviewed by Ramon Santamaria (@raysan5)
*
*   Copyright (c) 2020 @seanpringle
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.ConfigFlag;
using static Raylib_cs.Color;
using static Raylib_cs.CameraType;
using static Raylib_cs.ShaderLocationIndex;
using static Raylib_cs.ShaderUniformDataType;
using static Raylib_cs.MaterialMapType;
using static Raylib_cs.CameraMode;

namespace Examples
{
    public class shaders_rlgl_mesh_instanced
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            SetConfigFlags(FLAG_MSAA_4X_HINT);  // Enable Multi Sampling Anti Aliasing 4x (if available)
            InitWindow(screenWidth, screenHeight, "raylib [shaders] example - rlgl mesh instanced");

            // Define the camera to look into our 3d world
            Camera3D camera = new Camera3D();
            camera.position = new Vector3(125.0f, 125.0f, 125.0f);
            camera.target = new Vector3(0.0f, 0.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.type = CAMERA_PERSPECTIVE;

            // Number of instances to display
            const int count = 10000;
            Mesh cube = GenMeshCube(1.0f, 1.0f, 1.0f);

            Matrix4x4[] rotations = new Matrix4x4[count];    // Rotation state of instances
            Matrix4x4[] rotationsInc = new Matrix4x4[count]; // Per-frame rotation animation of instances
            Matrix4x4[] translations = new Matrix4x4[count]; // Locations of instances

            // Scatter random cubes around
            for (int i = 0; i < count; i++)
            {
                float x = GetRandomValue(-50, 50);
                float y = GetRandomValue(-50, 50);
                float z = GetRandomValue(-50, 50);
                translations[i] = Matrix4x4.CreateTranslation(x, y, z);

                x = GetRandomValue(0, 360);
                y = GetRandomValue(0, 360);
                z = GetRandomValue(0, 360);
                Vector3 axis = Vector3.Normalize(new Vector3(x, y, z));
                float angle = (float)GetRandomValue(0, 10) * DEG2RAD;

                rotationsInc[i] = Matrix4x4.CreateFromAxisAngle(axis, angle);
                rotations[i] = Matrix4x4.Identity;
            }

            Matrix4x4[] transforms = new Matrix4x4[count];   // Pre-multiplied transformations passed to rlgl
            Shader shader = LoadShader("resources/shaders/glsl330/base_lighting_instanced.vs", "resources/shaders/glsl330/lighting.fs");

            // Get some shader loactions
            unsafe
            {
                int* locs = (int*)shader.locs;
                locs[(int)LOC_MATRIX_MVP] = GetShaderLocation(shader, "mvp");
                locs[(int)LOC_VECTOR_VIEW] = GetShaderLocation(shader, "viewPos");
                locs[(int)LOC_MATRIX_MODEL] = GetShaderLocationAttrib(shader, "instance");
            }

            // Ambient light level
            int ambientLoc = GetShaderLocation(shader, "ambient");
            Utils.SetShaderValueV(shader, ambientLoc, new float[] { 0.2f, 0.2f, 0.2f, 1.0f }, UNIFORM_VEC4, 4);

            Rlights.CreateLight(0, LightType.LIGHT_DIRECTIONAL, new Vector3(50, 50, 0), Vector3.Zero, WHITE, shader);

            Material material = LoadMaterialDefault();
            material.shader = shader;
            unsafe
            {
                MaterialMap* maps = (MaterialMap*)material.maps.ToPointer();
                maps[(int)MAP_ALBEDO].color = RED;
            }

            SetCameraMode(camera, CAMERA_FREE); // Set a free camera mode

            SetTargetFPS(60);                   // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())        // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera);

                // Update the light shader with the camera view position
                float[] cameraPos = new[] { camera.position.X, camera.position.Y, camera.position.Z };
                Utils.SetShaderValueV(shader, (int)LOC_VECTOR_VIEW, cameraPos, UNIFORM_VEC3, 3);

                // Apply per-instance rotations
                for (int i = 0; i < count; i++)
                {
                    rotations[i] = Matrix4x4.Multiply(rotations[i], rotationsInc[i]);
                    transforms[i] = Matrix4x4.Transpose(Matrix4x4.Multiply(rotations[i], translations[i]));
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                BeginMode3D(camera);
                Rlgl.rlDrawMeshInstanced(cube, material, transforms, count);
                EndMode3D();

                DrawText("A CUBE OF DANCING CUBES!", 490, 10, 20, MAROON);

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
    }
}
