/*******************************************************************************************
*
*   raylib [shaders] example - Apply a postprocessing shader and connect a custom uniform variable
*
*   NOTE: This example requires raylib OpenGL 3.3 or ES2 versions for shaders support,
*         OpenGL 1.1 does not support shaders, recompile raylib to OpenGL 3.3 version.
*
*   NOTE: Shaders used in this example are #version 330 (OpenGL 3.3), to test this example
*         on OpenGL ES 2.0 platforms (Android, Raspberry Pi, HTML5), use #version 100 shaders
*         raylib comes with shaders ready for both versions, check raylib/shaders install folder
*
*   This example has been created using raylib 1.3 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2015 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.CameraProjection;
using static Raylib_cs.CameraMode;
using static Raylib_cs.MaterialMapIndex;

namespace Examples
{
    public class shaders_custom_uniform
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT);      // Enable Multi Sampling Anti Aliasing 4x (if available)

            InitWindow(screenWidth, screenHeight, "raylib [shaders] example - custom uniform variable");

            // Define the camera to look into our 3d world
            Camera3D camera = new Camera3D();
            camera.position = new Vector3(8.0f, 8.0f, 8.0f);
            camera.target = new Vector3(0.0f, 1.5f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.projection = CAMERA_PERSPECTIVE;

            Model model = LoadModel("resources/models/barracks.obj");                   // Load OBJ model
            Texture2D texture = LoadTexture("resources/models/barracks_diffuse.png");   // Load model texture (diffuse map)

            // Set model diffuse texture
            Raylib.SetMaterialTexture(ref model, 0, MATERIAL_MAP_ALBEDO, ref texture);

            // Set model position
            Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);

            // Load postpro shader
            Shader shader = LoadShader("resources/shaders/glsl330/base.vs",
                                       "resources/shaders/glsl330/swirl.fs");

            // Get variable (uniform) location on the shader to connect with the program
            // NOTE: If uniform variable could not be found in the shader, function returns -1
            int swirlCenterLoc = GetShaderLocation(shader, "center");

            float[] swirlCenter = new float[2] { (float)screenWidth / 2, (float)screenHeight / 2 };

            // Create a RenderTexture2D to be used for render to texture
            RenderTexture2D target = LoadRenderTexture(screenWidth, screenHeight);

            // Setup orbital camera
            SetCameraMode(camera, CAMERA_ORBITAL);  // Set an orbital camera mode

            SetTargetFPS(60);                       // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())            // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                Vector2 mousePosition = GetMousePosition();

                swirlCenter[0] = mousePosition.X;
                swirlCenter[1] = screenHeight - mousePosition.Y;

                // Send new value to the shader to be used on drawing
                Raylib.SetShaderValue(shader, swirlCenterLoc, swirlCenter, ShaderUniformDataType.SHADER_UNIFORM_VEC2);

                UpdateCamera(ref camera);              // Update camera
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                BeginTextureMode(target);   // Enable drawing to texture
                ClearBackground(RAYWHITE);

                BeginMode3D(camera);

                DrawModel(model, position, 0.5f, WHITE);   // Draw 3d model with texture
                DrawGrid(10, 1.0f);     // Draw a grid

                EndMode3D();

                DrawText("TEXT DRAWN IN RENDER TEXTURE", 200, 10, 30, RED);

                EndTextureMode();           // End drawing to texture (now we have a texture available for next passes)

                BeginShaderMode(shader);

                // NOTE: Render texture must be y-flipped due to default OpenGL coordinates (left-bottom)
                DrawTextureRec(target.texture, new Rectangle(0, 0, target.texture.width, -target.texture.height), new Vector2(0, 0), WHITE);

                EndShaderMode();

                DrawText("(c) Barracks 3D model by Alberto Cano", screenWidth - 220, screenHeight - 20, 10, GRAY);

                DrawFPS(10, 10);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadShader(shader);           // Unload shader
            UnloadTexture(texture);         // Unload texture
            UnloadModel(model);             // Unload model
            UnloadRenderTexture(target);    // Unload render texture

            CloseWindow();                  // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
