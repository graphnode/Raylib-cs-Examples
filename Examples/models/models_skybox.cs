/*******************************************************************************************
*
*   raylib [models] example - Skybox loading and drawing
*
*   Example originally created with raylib 1.8, last time updated with raylib 4.0
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2017-2022 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.IO;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.CameraMode;
using static Raylib_cs.CameraProjection;
using static Raylib_cs.MaterialMapIndex;
using static Raylib_cs.ShaderUniformDataType;

using TextureCubemap = Raylib_cs.Texture2D;

namespace Examples
{
    public class models_skybox
    {
        private const int GLSL_VERSION = 330;

        public static unsafe int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [models] example - skybox loading and drawing");

            // Define the camera to look into our 3d world
            Camera3D camera = new Camera3D(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(4.0f, 1.0f, 4.0f), new Vector3(0.0f, 1.0f, 0.0f), 45.0f, CAMERA_PERSPECTIVE);

            // Load skybox model
            Mesh cube = GenMeshCube(1.0f, 1.0f, 1.0f);
            Model skybox = LoadModelFromMesh(cube);

            const bool useHDR = true;

            // Load skybox shader and set required locations
            // NOTE: Some locations are automatically set at shader loading
            Shader shader = LoadShader($"resources/shaders/glsl{GLSL_VERSION}/skybox.vs", $"resources/shaders/glsl{GLSL_VERSION}/skybox.fs");
            SetMaterialShader(ref skybox, 0, ref shader);
            SetShaderValue(shader, GetShaderLocation(shader, "environmentMap"), (int)MATERIAL_MAP_CUBEMAP, SHADER_UNIFORM_INT);
            SetShaderValue(shader, GetShaderLocation(shader, "doGamma"), useHDR ? 1 : 0, SHADER_UNIFORM_INT);
            SetShaderValue(shader, GetShaderLocation(shader, "vflipped"), useHDR ? 1 : 0, SHADER_UNIFORM_INT);

            // Load cubemap shader and setup required shader locations
            Shader shdrCubemap = LoadShader($"resources/shaders/glsl{GLSL_VERSION}/cubemap.vs", $"resources/shaders/glsl{GLSL_VERSION}/cubemap.fs");
            SetShaderValue(shdrCubemap, GetShaderLocation(shdrCubemap, "equirectangularMap"), 0, SHADER_UNIFORM_INT);

            string skyboxFileName = null;

            Texture2D panorama;

            if (useHDR)
            {
                skyboxFileName = "resources/dresden_square_2k.hdr";

                // Load HDR panorama (sphere) texture
                panorama = LoadTexture(skyboxFileName);

                // Generate cubemap (texture with 6 quads-cube-mapping) from panorama HDR texture
                // NOTE 1: New texture is generated rendering to texture, shader calculates the sphere->cube coordinates mapping
                // NOTE 2: It seems on some Android devices WebGL, fbo does not properly support a FLOAT-based attachment,
                // despite texture can be successfully created.. so using PIXELFORMAT_UNCOMPRESSED_R8G8B8A8 instead of PIXELFORMAT_UNCOMPRESSED_R32G32B32A32
                skybox.materials[0].maps[(int)MaterialMapIndex.MATERIAL_MAP_CUBEMAP].texture = GenTextureCubemap(shdrCubemap, panorama, 1024, PixelFormat.PIXELFORMAT_UNCOMPRESSED_R8G8B8A8);

                //UnloadTexture(panorama);    // Texture not required anymore, cubemap already generated
            }
            else
            {
                skyboxFileName = "resources/skybox.png";

                Image img = LoadImage(skyboxFileName);
                skybox.materials[0].maps[(int)MaterialMapIndex.MATERIAL_MAP_CUBEMAP].texture = LoadTextureCubemap(img, CubemapLayout.CUBEMAP_LAYOUT_AUTO_DETECT);    // CUBEMAP_LAYOUT_PANORAMA
                UnloadImage(img);
            }

            SetCameraMode(camera, CAMERA_FIRST_PERSON);  // Set a first person camera mode

            SetTargetFPS(60);                       // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())            // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera);              // Update camera

                // Load new cubemap texture on drag&drop
                // Notice: Not implemented in binding yet. (4.2?)
                // if (IsFileDropped())
                // {
                //     FilePathList droppedFiles = LoadDroppedFiles();
                //
                //     if (droppedFiles.count == 1)         // Only support one file dropped
                //     {
                //         if (IsFileExtension(droppedFiles.paths[0], ".png;.jpg;.hdr;.bmp;.tga"))
                //         {
                //             // Unload current cubemap texture and load new one
                //             UnloadTexture(skybox.materials[0].maps[MATERIAL_MAP_CUBEMAP].texture);
                //             if (useHDR)
                //             {
                //                 panorama = LoadTexture(droppedFiles.paths[0]);
                //
                //                 // Generate cubemap from panorama texture
                //                 skybox.materials[0].maps[MATERIAL_MAP_CUBEMAP].texture = GenTextureCubemap(shdrCubemap, panorama, 1024, PIXELFORMAT_UNCOMPRESSED_R8G8B8A8);
                //                 UnloadTexture(panorama);
                //             }
                //             else
                //             {
                //                 Image img = LoadImage(droppedFiles.paths[0]);
                //                 skybox.materials[0].maps[MATERIAL_MAP_CUBEMAP].texture = LoadTextureCubemap(img, CUBEMAP_LAYOUT_AUTO_DETECT);
                //                 UnloadImage(img);
                //             }
                //
                //             TextCopy(skyboxFileName, droppedFiles.paths[0]);
                //         }
                //     }
                //
                //     UnloadDroppedFiles(droppedFiles);    // Unload filepaths from memory
                // }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();

                    ClearBackground(RAYWHITE);

                    BeginMode3D(camera);

                        // We are inside the cube, we need to disable backface culling!
                        Rlgl.rlDisableBackfaceCulling();
                        Rlgl.rlDisableDepthMask();
                        DrawModel(skybox, Vector3.Zero, 1.0f, WHITE);
                        Rlgl.rlEnableBackfaceCulling();
                        Rlgl.rlEnableDepthMask();

                        DrawGrid(10, 1.0f);

                    EndMode3D();

                    //DrawTextureEx(panorama, (Vector2){ 0, 0 }, 0.0f, 0.5f, WHITE);

                    if (useHDR) DrawText($"Panorama image from hdrihaven.com: {Path.GetFileName(skyboxFileName)}", 10, GetScreenHeight() - 20, 10, BLACK);
                    else DrawText($": {Path.GetFileName(skyboxFileName)}", 10, GetScreenHeight() - 20, 10, BLACK);

                    DrawFPS(10, 10);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadModel(skybox);        // Unload skybox model (and textures)

            CloseWindow();              // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }

        // Generate cubemap texture from HDR texture
        static unsafe TextureCubemap GenTextureCubemap(Shader shader, Texture2D panorama, int size, PixelFormat format)
        {
            TextureCubemap cubemap;

            Rlgl.rlDisableBackfaceCulling();     // Disable backface culling to render inside the cube

            // STEP 1: Setup framebuffer
            //------------------------------------------------------------------------------------------
            uint rbo = Rlgl.rlLoadTextureDepth(size, size, true);
            cubemap.id = Rlgl.rlLoadTextureCubemap(null, size, format);

            uint fbo = Rlgl.rlLoadFramebuffer(size, size);
            Rlgl.rlFramebufferAttach(fbo, rbo, FramebufferAttachType.RL_ATTACHMENT_DEPTH, FramebufferAttachTextureType.RL_ATTACHMENT_RENDERBUFFER, 0);
            Rlgl.rlFramebufferAttach(fbo, cubemap.id, FramebufferAttachType.RL_ATTACHMENT_COLOR_CHANNEL0, FramebufferAttachTextureType.RL_ATTACHMENT_CUBEMAP_POSITIVE_X, 0);

            // Check if framebuffer is complete with attachments (valid)
            if (Rlgl.rlFramebufferComplete(fbo)) TraceLog(TraceLogLevel.LOG_INFO, $"FBO: [ID {fbo}] Framebuffer object created successfully");
            //------------------------------------------------------------------------------------------

            // STEP 2: Draw to framebuffer
            //------------------------------------------------------------------------------------------
            // NOTE: Shader is used to convert HDR equirectangular environment map to cubemap equivalent (6 faces)
            Rlgl.rlEnableShader(shader.id);

            // Define projection matrix and send it to shader
            Matrix4x4 matFboProjection = Raymath.MatrixPerspective(90.0*DEG2RAD, 1.0, Rlgl.RL_CULL_DISTANCE_NEAR, Rlgl.RL_CULL_DISTANCE_FAR);
            Rlgl.rlSetUniformMatrix(shader.locs[(int)ShaderLocationIndex.SHADER_LOC_MATRIX_PROJECTION], matFboProjection);

            // Define view matrix for every side of the cubemap
            Matrix4x4[] fboViews = {
                Raymath.MatrixLookAt(Vector3.Zero,  Vector3.UnitX, -Vector3.UnitY),
                Raymath.MatrixLookAt(Vector3.Zero, -Vector3.UnitX, -Vector3.UnitY),
                Raymath.MatrixLookAt(Vector3.Zero,  Vector3.UnitY,  Vector3.UnitZ),
                Raymath.MatrixLookAt(Vector3.Zero, -Vector3.UnitY, -Vector3.UnitZ),
                Raymath.MatrixLookAt(Vector3.Zero,  Vector3.UnitZ, -Vector3.UnitY),
                Raymath.MatrixLookAt(Vector3.Zero, -Vector3.UnitZ, -Vector3.UnitY)
            };

            Rlgl.rlViewport(0, 0, size, size);   // Set viewport to current fbo dimensions

            // Activate and enable texture for drawing to cubemap faces
            Rlgl.rlActiveTextureSlot(0);
            Rlgl.rlEnableTexture(panorama.id);

            for (int i = 0; i < 6; i++)
            {
                // Set the view matrix for the current cube face
                Rlgl.rlSetUniformMatrix(shader.locs[(int)ShaderLocationIndex.SHADER_LOC_MATRIX_VIEW], fboViews[i]);

                // Select the current cubemap face attachment for the fbo
                // WARNING: This function by default enables->attach->disables fbo!!!
                Rlgl.rlFramebufferAttach(fbo, cubemap.id, FramebufferAttachType.RL_ATTACHMENT_COLOR_CHANNEL0, FramebufferAttachTextureType.RL_ATTACHMENT_CUBEMAP_POSITIVE_X + i, 0);
                Rlgl.rlEnableFramebuffer(fbo);

                // Load and draw a cube, it uses the current enabled texture
                Rlgl.rlClearScreenBuffers();
                Rlgl.rlLoadDrawCube();

                // ALTERNATIVE: Try to use internal batch system to draw the cube instead of rlLoadDrawCube
                // for some reason this method does not work, maybe due to cube triangles definition? normals pointing out?
                // TODO: Investigate this issue...
                //rlSetTexture(panorama.id); // WARNING: It must be called after enabling current framebuffer if using internal batch system!
                //rlClearScreenBuffers();
                //DrawCubeV(Vector3Zero(), Vector3One(), WHITE);
                //rlDrawRenderBatchActive();
            }
            //------------------------------------------------------------------------------------------

            // STEP 3: Unload framebuffer and reset state
            //------------------------------------------------------------------------------------------
            Rlgl.rlDisableShader();          // Unbind shader
            Rlgl.rlDisableTexture();         // Unbind texture
            Rlgl.rlDisableFramebuffer();     // Unbind framebuffer
            Rlgl.rlUnloadFramebuffer(fbo);   // Unload framebuffer (and automatically attached depth texture/renderbuffer)

            // Reset viewport dimensions to default
            Rlgl.rlViewport(0, 0, Rlgl.rlGetFramebufferWidth(), Rlgl.rlGetFramebufferHeight());
            Rlgl.rlEnableBackfaceCulling();
            //------------------------------------------------------------------------------------------

            cubemap.width = size;
            cubemap.height = size;
            cubemap.mipmaps = 1;
            cubemap.format = format;

            return cubemap;
        }
    }
}
