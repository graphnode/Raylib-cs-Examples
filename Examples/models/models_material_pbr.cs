/*******************************************************************************************
*
*   raylib [models] example - PBR material
*
*   This example has been created using raylib 1.8 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2017 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.CameraMode;
using static Raylib_cs.CameraProjection;
using static Examples.Rlights;

namespace Examples
{
    public class models_material_pbr
    {
        public const int CUBEMAP_SIZE = 1024;
        public const int IRRADIANCE_SIZE = 32;
        public const int PREFILTERED_SIZE = 256;
        public const int BRDF_SIZE = 512;

        public const float LIGHT_DISTANCE = 3.5f;
        public const float LIGHT_HEIGHT = 1.0f;

        public unsafe static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT);  // Enable Multi Sampling Anti Aliasing 4x (if available)
            InitWindow(screenWidth, screenHeight, "raylib [models] example - pbr material");

            // Define the camera to look into our 3d world
            Camera3D camera = new Camera3D();
            camera.position = new Vector3(4.0f, 4.0f, 4.0f);
            camera.target = new Vector3(0.0f, 0.5f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.projection = CAMERA_PERSPECTIVE;

            // Load model and PBR material
            Model model = LoadModel("resources/pbr/trooper.obj");

            // Unsafe pointers into model arrays.
            Material* materials = (Material*)model.materials.ToPointer();
            Mesh* meshes = (Mesh*)model.meshes.ToPointer();

            materials[0] = PBR.LoadMaterialPBR(new Color(255, 255, 255, 255), 1.0f, 1.0f);

            // Define lights attributes
            // NOTE: Shader is passed to every light on creation to define shader bindings internally
            CreateLight(0, LightType.LIGHT_POINT, new Vector3(LIGHT_DISTANCE, LIGHT_HEIGHT, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Color(255, 0, 0, 255), materials[0].shader);
            CreateLight(1, LightType.LIGHT_POINT, new Vector3(0.0f, LIGHT_HEIGHT, LIGHT_DISTANCE), new Vector3(0.0f, 0.0f, 0.0f), new Color(0, 255, 0, 255), materials[0].shader);
            CreateLight(2, LightType.LIGHT_POINT, new Vector3(-LIGHT_DISTANCE, LIGHT_HEIGHT, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Color(0, 0, 255, 255), materials[0].shader);
            CreateLight(3, LightType.LIGHT_DIRECTIONAL, new Vector3(0.0f, LIGHT_HEIGHT * 2.0f, -LIGHT_DISTANCE), new Vector3(0.0f, 0.0f, 0.0f), new Color(255, 0, 255, 255), materials[0].shader);

            SetCameraMode(camera, CAMERA_ORBITAL);  // Set an orbital camera mode

            SetTargetFPS(60);                       // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose()) // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera);              // Update camera

                // Send to material PBR shader camera view position
                float[] cameraPos = { camera.position.X, camera.position.Y, camera.position.Z };
                Utils.SetShaderValue(materials[0].shader, (int)ShaderLocationIndex.SHADER_LOC_VECTOR_VIEW, cameraPos, ShaderUniformDataType.SHADER_UNIFORM_VEC3);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                BeginMode3D(camera);

                DrawModel(model, Vector3.Zero, 1.0f, WHITE);
                DrawGrid(10, 1.0f);

                EndMode3D();

                DrawFPS(10, 10);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadMaterial(materials[0]); // Unload material: shader and textures

            UnloadModel(model);  // Unload model

            CloseWindow();       // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
