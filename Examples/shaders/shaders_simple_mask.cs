/*******************************************************************************************
*
*   raylib [shaders] example - Simple shader mask
*
*   This example has been created using raylib 2.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by Chris Camacho (@codifies) and reviewed by Ramon Santamaria (@raysan5)
*
*   Copyright (c) 2019 Chris Camacho (@codifies) and Ramon Santamaria (@raysan5)
*
********************************************************************************************
*
*   After a model is loaded it has a default material, this material can be
*   modified in place rather than creating one from scratch...
*   While all of the maps have particular names, they can be used for any purpose
*   except for three maps that are applied as cubic maps (see below)
*
********************************************************************************************/

using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;

namespace Examples
{
    public class shaders_simple_mask
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib - simple shader mask");

            // Define the camera to look into our 3d world
            Camera3D camera = new Camera(0);
            camera.position = new Vector3(0.0f, 1.0f, 2.0f);
            camera.target = new Vector3(0.0f, 0.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.type = CAMERA_PERSPECTIVE;

            // Define our three models to show the shader on
            Mesh torus = GenMeshTorus(.3, 1, 16, 32);
            Model model1 = LoadModelFromMesh(torus);

            Mesh cube = GenMeshCube(.8, .8, .8);
            Model model2 = LoadModelFromMesh(cube);

            // Generate model to be shaded just to see the gaps in the other two
            Mesh sphere = GenMeshSphere(1, 16, 16);
            Model model3 = LoadModelFromMesh(sphere);

            // Load the shader
            Shader shader = LoadShader("resources/shaders/glsl330/mask.vs", "resources/shaders/glsl330/mask.fs");

            // Load and apply the diffuse texture (colour map)
            Texture texDiffuse = LoadTexture("resources/plasma.png");
            model1.materials[0].maps[MAP_DIFFUSE].texture = texDiffuse;
            model2.materials[0].maps[MAP_DIFFUSE].texture = texDiffuse;

            // Using MAP_EMISSION as a spare slot to use for 2nd texture
            // NOTE: Don't use MAP_IRRADIANCE, MAP_PREFILTER or  MAP_CUBEMAP
            // as they are bound as cube maps
            Texture texMask = LoadTexture("resources/mask.png");
            model1.materials[0].maps[MAP_EMISSION].texture = texMask;
            model2.materials[0].maps[MAP_EMISSION].texture = texMask;
            shader.locs[LOC_MAP_EMISSION] = GetShaderLocation(shader, "mask");

            // Frame is incremented each frame to animate the shader
            int shaderFrame = GetShaderLocation(shader, "framesCounter");

            // Apply the shader to the two models
            model1.materials[0].shader = shader;
            model2.materials[0].shader = shader;

            int framesCounter = 0;
            Vector3 rotation = new Vector3(0);       // Model rotation angles

            SetTargetFPS(60);               // Set  to run at 60 frames-per-second
                                            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                framesCounter++;
                rotation.x += 0.01f;
                rotation.y += 0.005f;
                rotation.z -= 0.0025f;

                // Send frames counter to shader for animation
                SetShaderValue(shader, shaderFrame, ref, UNIFORM_INT);

                // Rotate one of the models
                model1.transform = MatrixRotateXYZ(rotation);

                UpdateCamera(ref);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();

                ClearBackground(DARKBLUE);

                BeginMode3D(camera);

                DrawModel(model1, new Vector3(0.5, 0, 0), 1, WHITE);
                DrawModelEx(model2, new Vector3(-.5, 0, 0}, (Vector3){ 1,1,0}, 50, (Vector3){
                1,1,1), WHITE);
                DrawModel(model3, new Vector3(0, 0, -1.5), 1, WHITE);
                DrawGrid(10, 1.0f);        // Draw a grid

                EndMode3D();

                DrawRectangle(16, 698, MeasureText(FormatText("Frame: %i", framesCounter), 20) + 8, 42, BLUE);
                DrawText(FormatText("Frame: %i", framesCounter), 20, 700, 20, WHITE);

                DrawFPS(10, 10);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadModel(model1);
            UnloadModel(model2);
            UnloadModel(model3);

            UnloadTexture(texDiffuse);  // Unload default diffuse texture
            UnloadTexture(texMask);     // Unload texture mask

            UnloadShader(shader);       // Unload shader

            CloseWindow();              // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }

    }

}