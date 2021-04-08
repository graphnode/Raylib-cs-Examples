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

using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Rlgl;
using static Raylib_cs.TextureFilter;
using static Raylib_cs.ShaderLocationIndex;
using static Raylib_cs.ShaderUniformDataType;

namespace Examples
{
    public static class PBR
    {
        public const int CUBEMAP_SIZE = 1024;
        public const int IRRADIANCE_SIZE = 32;
        public const int PREFILTERED_SIZE = 256;
        public const int BRDF_SIZE = 512;

        // Load PBR material (Supports: ALBEDO, NORMAL, METALNESS, ROUGHNESS, AO, EMMISIVE, HEIGHT maps)
        // NOTE: PBR shader is loaded inside this function
        public static unsafe Material LoadMaterialPBR(Color albedo, float metalness, float roughness)
        {
            // NOTE: All maps textures are set to { 0 )
            Material mat = Raylib.LoadMaterialDefault();

            string shaderPath = "resources/shaders/glsl330";
            string PATH_PBR_VS = $"{shaderPath}/pbr.vs";
            string PATH_PBR_FS = $"{shaderPath}/pbr.fs";
            mat.shader = LoadShader(PATH_PBR_VS, PATH_PBR_FS);

            // Temporary unsafe pointers into material arrays.
            MaterialMap* maps = (MaterialMap*)mat.maps.ToPointer();
            int* locs = (int*)mat.shader.locs.ToPointer();

            // Get required locations points for PBR material
            // NOTE: Those location names must be available and used in the shader code
            locs[(int)ShaderLocationIndex.LOC_MAP_ALBEDO] = GetShaderLocation(mat.shader, "albedo.sampler");
            locs[(int)ShaderLocationIndex.LOC_MAP_METALNESS] = GetShaderLocation(mat.shader, "metalness.sampler");
            locs[(int)ShaderLocationIndex.LOC_MAP_NORMAL] = GetShaderLocation(mat.shader, "normals.sampler");
            locs[(int)ShaderLocationIndex.LOC_MAP_ROUGHNESS] = GetShaderLocation(mat.shader, "roughness.sampler");
            locs[(int)ShaderLocationIndex.LOC_MAP_OCCLUSION] = GetShaderLocation(mat.shader, "occlusion.sampler");
            locs[(int)ShaderLocationIndex.LOC_MAP_IRRADIANCE] = GetShaderLocation(mat.shader, "irradianceMap");
            locs[(int)ShaderLocationIndex.LOC_MAP_PREFILTER] = GetShaderLocation(mat.shader, "prefilterMap");
            locs[(int)ShaderLocationIndex.LOC_MAP_BRDF] = GetShaderLocation(mat.shader, "brdfLUT");

            // Set view matrix location
            locs[(int)ShaderLocationIndex.LOC_MATRIX_MODEL] = GetShaderLocation(mat.shader, "matModel");
            locs[(int)ShaderLocationIndex.LOC_VECTOR_VIEW] = GetShaderLocation(mat.shader, "viewPos");

            // Set PBR standard maps
            maps[(int)MaterialMapIndex.MAP_ALBEDO].texture = LoadTexture("resources/pbr/trooper_albedo.png");
            maps[(int)MaterialMapIndex.MAP_NORMAL].texture = LoadTexture("resources/pbr/trooper_normals.png");
            maps[(int)MaterialMapIndex.MAP_METALNESS].texture = LoadTexture("resources/pbr/trooper_metalness.png");
            maps[(int)MaterialMapIndex.MAP_ROUGHNESS].texture = LoadTexture("resources/pbr/trooper_roughness.png");
            maps[(int)MaterialMapIndex.MAP_OCCLUSION].texture = LoadTexture("resources/pbr/trooper_ao.png");

            // Set textures filtering for better quality
            SetTextureFilter(maps[(int)MaterialMapIndex.MAP_ALBEDO].texture, FILTER_BILINEAR);
            SetTextureFilter(maps[(int)MaterialMapIndex.MAP_NORMAL].texture, FILTER_BILINEAR);
            SetTextureFilter(maps[(int)MaterialMapIndex.MAP_METALNESS].texture, FILTER_BILINEAR);
            SetTextureFilter(maps[(int)MaterialMapIndex.MAP_ROUGHNESS].texture, FILTER_BILINEAR);
            SetTextureFilter(maps[(int)MaterialMapIndex.MAP_OCCLUSION].texture, FILTER_BILINEAR);

            // Enable sample usage in shader for assigned textures
            ShaderUniformDataType uniformType = ShaderUniformDataType.UNIFORM_INT;
            Utils.SetShaderValue(mat.shader, GetShaderLocation(mat.shader, "albedo.useSampler"), 1, uniformType);
            Utils.SetShaderValue(mat.shader, GetShaderLocation(mat.shader, "normals.useSampler"), 1, uniformType);
            Utils.SetShaderValue(mat.shader, GetShaderLocation(mat.shader, "metalness.useSampler"), 1, uniformType);
            Utils.SetShaderValue(mat.shader, GetShaderLocation(mat.shader, "roughness.useSampler"), 1, uniformType);
            Utils.SetShaderValue(mat.shader, GetShaderLocation(mat.shader, "occlusion.useSampler"), 1, uniformType);

            int renderModeLoc = GetShaderLocation(mat.shader, "renderMode");
            Utils.SetShaderValue(mat.shader, renderModeLoc, 0, uniformType);

            // Set up material properties color
            maps[(int)MaterialMapIndex.MAP_ALBEDO].color = albedo;
            maps[(int)MaterialMapIndex.MAP_NORMAL].color = new Color(128, 128, 255, 255);
            maps[(int)MaterialMapIndex.MAP_METALNESS].value = metalness;
            maps[(int)MaterialMapIndex.MAP_ROUGHNESS].value = roughness;
            maps[(int)MaterialMapIndex.MAP_OCCLUSION].value = 1.0f;
            maps[(int)MaterialMapIndex.MAP_EMISSION].value = 0.5f;
            maps[(int)MaterialMapIndex.MAP_HEIGHT].value = 0.5f;

            // Load shaders for material
            string PATH_CUBEMAP_VS = $"{shaderPath}/cubemap.vs"; // Path to equirectangular to cubemap vertex shader
            string PATH_CUBEMAP_FS = $"{shaderPath}/cubemap.fs"; // Path to equirectangular to cubemap fragment shader
            string PATH_SKYBOX_VS = $"{shaderPath}/skybox.vs";  // Path to skybox vertex shader
            string PATH_IRRADIANCE_FS = $"{shaderPath}/irradiance.fs"; // Path to irradiance (GI) calculation fragment shader
            string PATH_PREFILTER_FS = $"{shaderPath}/prefilter.fs"; // Path to reflection prefilter calculation fragment shader
            string PATH_BRDF_VS = $"{shaderPath}/brdf.vs"; // Path to bidirectional reflectance distribution function vertex shader
            string PATH_BRDF_FS = $"{shaderPath}/brdf.fs"; // Path to bidirectional reflectance distribution function fragment shader

            Shader shdrCubemap = LoadShader(PATH_CUBEMAP_VS, PATH_CUBEMAP_FS);
            Shader shdrIrradiance = LoadShader(PATH_SKYBOX_VS, PATH_IRRADIANCE_FS);
            Shader shdrPrefilter = LoadShader(PATH_SKYBOX_VS, PATH_PREFILTER_FS);
            Shader shdrBRDF = LoadShader(PATH_BRDF_VS, PATH_BRDF_FS);

            // Generate cubemap from panorama texture
            //--------------------------------------------------------------------------------------------------------
            Texture2D panorama = LoadTexture("resources/dresden_square_1k.hdr");
            Texture2D cubemap = GenTextureCubemap(shdrCubemap, panorama, CUBEMAP_SIZE, PixelFormat.UNCOMPRESSED_R32G32B32);
            Utils.SetShaderValue(shdrCubemap, GetShaderLocation(shdrCubemap, "equirectangularMap"), 0, uniformType);

            // Generate irradiance map from cubemap texture
            //--------------------------------------------------------------------------------------------------------
            Utils.SetShaderValue(shdrIrradiance, GetShaderLocation(shdrIrradiance, "environmentMap"), 0, uniformType);
            maps[(int)MaterialMapIndex.MAP_IRRADIANCE].texture = GenTextureIrradiance(shdrIrradiance, cubemap, IRRADIANCE_SIZE);

            // Generate prefilter map from cubemap texture
            //--------------------------------------------------------------------------------------------------------
            Utils.SetShaderValue(shdrPrefilter, GetShaderLocation(shdrPrefilter, "environmentMap"), 0, uniformType);
            maps[(int)MaterialMapIndex.MAP_PREFILTER].texture = GenTexturePrefilter(shdrPrefilter, cubemap, PREFILTERED_SIZE);

            // Generate BRDF (bidirectional reflectance distribution function) texture (using shader)
            //--------------------------------------------------------------------------------------------------------
            maps[(int)MaterialMapIndex.MAP_BRDF].texture = GenTextureBRDF(shdrBRDF, BRDF_SIZE);

            // Unload temporary shaders and textures
            UnloadShader(shdrCubemap);
            UnloadShader(shdrIrradiance);
            UnloadShader(shdrPrefilter);
            UnloadShader(shdrBRDF);

            UnloadTexture(panorama);
            UnloadTexture(cubemap);

            return mat;
        }

        // Texture maps generation (PBR)
        //-------------------------------------------------------------------------------------------
        // Generate cubemap texture from HDR texture
        public static Texture2D GenTextureCubemap(Shader shader, Texture2D panorama, int size, PixelFormat format)
        {
            Texture2D cubemap;

            rlDisableBackfaceCulling();     // Disable backface culling to render inside the cube

            // STEP 1: Setup framebuffer
            //------------------------------------------------------------------------------------------
            uint rbo = rlLoadTextureDepth(size, size, true);
            cubemap.id = rlLoadTextureCubemap(IntPtr.Zero, size, format);

            uint fbo = rlLoadFramebuffer(size, size);
            // rlFramebufferAttach(fbo, rbo, RL_ATTACHMENT_DEPTH, RL_ATTACHMENT_RENDERBUFFER, 0);
            // rlFramebufferAttach(fbo, cubemap.id, RL_ATTACHMENT_COLOR_CHANNEL0, RL_ATTACHMENT_CUBEMAP_POSITIVE_X, 0);

            // Check if framebuffer is complete with attachments (valid)
            if (rlFramebufferComplete(fbo))
                TraceLog(TraceLogLevel.LOG_INFO, $"FBO: [ID {fbo}] Framebuffer object created successfully");
            //------------------------------------------------------------------------------------------

            // STEP 2: Draw to framebuffer
            //------------------------------------------------------------------------------------------
            // NOTE: Shader is used to convert HDR equirectangular environment map to cubemap equivalent (6 faces)
            rlEnableShader(shader.id);

            // Define projection matrix and send it to shader
            Matrix4x4 matFboProjection = Matrix4x4.CreatePerspective(90.0f * DEG2RAD, 1.0f, RL_CULL_DISTANCE_NEAR, RL_CULL_DISTANCE_FAR);
            // rlSetUniformMatrix(shader.locs[LOC_MATRIX_PROJECTION], matFboProjection);

            // Define view matrix for every side of the cubemap
            Matrix4x4[] fboViews = new[] {
                Matrix4x4.CreateLookAt(Vector3.Zero, new Vector3( 1.0f,  0.0f,  0.0f), new Vector3(0.0f, -1.0f,  0.0f)),
                Matrix4x4.CreateLookAt(Vector3.Zero, new Vector3(-1.0f,  0.0f,  0.0f), new Vector3(0.0f, -1.0f,  0.0f)),
                Matrix4x4.CreateLookAt(Vector3.Zero, new Vector3( 0.0f,  1.0f,  0.0f), new Vector3(0.0f,  0.0f,  1.0f)),
                Matrix4x4.CreateLookAt(Vector3.Zero, new Vector3( 0.0f, -1.0f,  0.0f), new Vector3(0.0f,  0.0f, -1.0f)),
                Matrix4x4.CreateLookAt(Vector3.Zero, new Vector3( 0.0f,  0.0f,  1.0f), new Vector3(0.0f, -1.0f,  0.0f)),
                Matrix4x4.CreateLookAt(Vector3.Zero, new Vector3( 0.0f,  0.0f, -1.0f), new Vector3(0.0f, -1.0f,  0.0f))
            };

            rlViewport(0, 0, size, size);   // Set viewport to current fbo dimensions

            for (int i = 0; i < 6; i++)
            {
                // rlSetUniformMatrix(shader.locs[SHADER_LOC_MATRIX_VIEW], fboViews[i]);
                // rlFramebufferAttach(fbo, cubemap.id, RL_ATTACHMENT_COLOR_CHANNEL0, RL_ATTACHMENT_CUBEMAP_POSITIVE_X + i, 0);

                rlEnableFramebuffer(fbo);
                rlSetTexture(panorama.id);   // WARNING: It must be called after enabling current framebuffer if using internal batch system!

                rlClearScreenBuffers();
                DrawCubeV(Vector3.Zero, Vector3.One, Color.WHITE);
                rlDrawRenderBatchActive();
            }
            //------------------------------------------------------------------------------------------

            // STEP 3: Unload framebuffer and reset state
            //------------------------------------------------------------------------------------------
            rlDisableShader();          // Unbind shader
            rlDisableTexture();         // Unbind texture
            rlDisableFramebuffer();     // Unbind framebuffer
            rlUnloadFramebuffer(fbo);   // Unload framebuffer (and automatically attached depth texture/renderbuffer)

            // Reset viewport dimensions to default
            rlViewport(0, 0, rlGetFramebufferWidth(), rlGetFramebufferHeight());
            rlEnableBackfaceCulling();
            //------------------------------------------------------------------------------------------

            cubemap.width = size;
            cubemap.height = size;
            cubemap.mipmaps = 1;
            cubemap.format = PixelFormat.UNCOMPRESSED_R32G32B32;

            return cubemap;
        }

        // Generate irradiance texture using cubemap data
        public static Texture2D GenTextureIrradiance(Shader shader, Texture2D cubemap, int size)
        {
            Texture2D irradiance;

            rlDisableBackfaceCulling();     // Disable backface culling to render inside the cube

            // STEP 1: Setup framebuffer
            //------------------------------------------------------------------------------------------
            uint rbo = rlLoadTextureDepth(size, size, true);
            irradiance.id = rlLoadTextureCubemap(IntPtr.Zero, size, PixelFormat.UNCOMPRESSED_R32G32B32);

            uint fbo = rlLoadFramebuffer(size, size);
            rlFramebufferAttach(fbo, rbo, FramebufferAttachType.RL_ATTACHMENT_DEPTH, FramebufferAttachTextureType.RL_ATTACHMENT_RENDERBUFFER, 0);
            rlFramebufferAttach(fbo, cubemap.id, FramebufferAttachType.RL_ATTACHMENT_COLOR_CHANNEL0, FramebufferAttachTextureType.RL_ATTACHMENT_CUBEMAP_POSITIVE_X, 0);
            //------------------------------------------------------------------------------------------

            // STEP 2: Draw to framebuffer
            //------------------------------------------------------------------------------------------
            // NOTE: Shader is used to solve diffuse integral by convolution to create an irradiance cubemap
            rlEnableShader(shader.id);

            // Define projection matrix and send it to shader
            Matrix4x4 matFboProjection = Matrix4x4.CreatePerspective(90.0f * DEG2RAD, 1.0f, RL_CULL_DISTANCE_NEAR, RL_CULL_DISTANCE_FAR);
            // rlSetUniformMatrix(shader.locs[SHADER_LOC_MATRIX_PROJECTION], matFboProjection);

            // Define view matrix for every side of the cubemap
            Matrix4x4[] fboViews = new[] {
                Matrix4x4.CreateLookAt(Vector3.Zero, new Vector3( 1.0f,  0.0f,  0.0f), new Vector3(0.0f, -1.0f,  0.0f)),
                Matrix4x4.CreateLookAt(Vector3.Zero, new Vector3(-1.0f,  0.0f,  0.0f), new Vector3(0.0f, -1.0f,  0.0f)),
                Matrix4x4.CreateLookAt(Vector3.Zero, new Vector3( 0.0f,  1.0f,  0.0f), new Vector3(0.0f,  0.0f,  1.0f)),
                Matrix4x4.CreateLookAt(Vector3.Zero, new Vector3( 0.0f, -1.0f,  0.0f), new Vector3(0.0f,  0.0f, -1.0f)),
                Matrix4x4.CreateLookAt(Vector3.Zero, new Vector3( 0.0f,  0.0f,  1.0f), new Vector3(0.0f, -1.0f,  0.0f)),
                Matrix4x4.CreateLookAt(Vector3.Zero, new Vector3( 0.0f,  0.0f, -1.0f), new Vector3(0.0f, -1.0f,  0.0f))
            };

            rlActiveTextureSlot(0);
            rlEnableTextureCubemap(cubemap.id);

            rlViewport(0, 0, size, size);   // Set viewport to current fbo dimensions

            for (int i = 0; i < 6; i++)
            {
                // rlSetUniformMatrix(shader.locs[(int)LOC_MATRIX_VIEW], fboViews[i]);
                rlFramebufferAttach(fbo, irradiance.id, FramebufferAttachType.RL_ATTACHMENT_COLOR_CHANNEL0, FramebufferAttachTextureType.RL_ATTACHMENT_CUBEMAP_POSITIVE_X + i, 0);

                rlEnableFramebuffer(fbo);
                rlClearScreenBuffers();
                rlLoadDrawCube();
            }
            //------------------------------------------------------------------------------------------

            // STEP 3: Unload framebuffer and reset state
            //------------------------------------------------------------------------------------------
            rlDisableShader();          // Unbind shader
            rlDisableTexture();         // Unbind texture
            rlDisableFramebuffer();     // Unbind framebuffer
            rlUnloadFramebuffer(fbo);   // Unload framebuffer (and automatically attached depth texture/renderbuffer)

            // Reset viewport dimensions to default
            rlViewport(0, 0, rlGetFramebufferWidth(), rlGetFramebufferHeight());
            rlEnableBackfaceCulling();
            //------------------------------------------------------------------------------------------

            irradiance.width = size;
            irradiance.height = size;
            irradiance.mipmaps = 1;
            irradiance.format = PixelFormat.UNCOMPRESSED_R32G32B32;

            return irradiance;
        }

        // Generate prefilter texture using cubemap data
        public static Texture2D GenTexturePrefilter(Shader shader, Texture2D cubemap, int size)
        {
            Texture2D prefilter;

            rlDisableBackfaceCulling();     // Disable backface culling to render inside the cube

            // STEP 1: Setup framebuffer
            //------------------------------------------------------------------------------------------
            uint rbo = rlLoadTextureDepth(size, size, true);
            prefilter.id = rlLoadTextureCubemap(IntPtr.Zero, size, PixelFormat.UNCOMPRESSED_R32G32B32);
            rlTextureParameters(prefilter.id, RL_TEXTURE_MIN_FILTER, RL_TEXTURE_FILTER_MIP_LINEAR);

            uint fbo = rlLoadFramebuffer(size, size);
            // rlFramebufferAttach(fbo, rbo, FramebufferAttachType.RL_ATTACHMENT_DEPTH, FramebufferAttachType.RL_ATTACHMENT_RENDERBUFFER, 0);
            // rlFramebufferAttach(fbo, cubemap.id, FramebufferAttachType.RL_ATTACHMENT_COLOR_CHANNEL0, FramebufferAttachType.RL_ATTACHMENT_CUBEMAP_POSITIVE_X, 0);
            //------------------------------------------------------------------------------------------

            // Generate mipmaps for the prefiltered HDR texture
            //glGenerateMipmap(GL_TEXTURE_CUBE_MAP);    // TODO!

            // STEP 2: Draw to framebuffer
            //------------------------------------------------------------------------------------------
            // NOTE: Shader is used to prefilter HDR and store data into mipmap levels

            // Define projection matrix and send it to shader
            Matrix4x4 fboProjection = Matrix4x4.CreatePerspective(90.0f * DEG2RAD, 1.0f, RL_CULL_DISTANCE_NEAR, RL_CULL_DISTANCE_FAR);
            rlEnableShader(shader.id);
            // rlSetUniformMatrix(shader.locs[SHADER_LOC_MATRIX_PROJECTION], fboProjection);

            // Define view matrix for every side of the cubemap
            Matrix4x4[] fboViews = new[] {
                Matrix4x4.CreateLookAt(Vector3.Zero, new Vector3( 1.0f,  0.0f,  0.0f), new Vector3(0.0f, -1.0f,  0.0f)),
                Matrix4x4.CreateLookAt(Vector3.Zero, new Vector3(-1.0f,  0.0f,  0.0f), new Vector3(0.0f, -1.0f,  0.0f)),
                Matrix4x4.CreateLookAt(Vector3.Zero, new Vector3( 0.0f,  1.0f,  0.0f), new Vector3(0.0f,  0.0f,  1.0f)),
                Matrix4x4.CreateLookAt(Vector3.Zero, new Vector3( 0.0f, -1.0f,  0.0f), new Vector3(0.0f,  0.0f, -1.0f)),
                Matrix4x4.CreateLookAt(Vector3.Zero, new Vector3( 0.0f,  0.0f,  1.0f), new Vector3(0.0f, -1.0f,  0.0f)),
                Matrix4x4.CreateLookAt(Vector3.Zero, new Vector3( 0.0f,  0.0f, -1.0f), new Vector3(0.0f, -1.0f,  0.0f))
            };

            rlActiveTextureSlot(0);
            rlEnableTextureCubemap(cubemap.id);

            // TODO: Locations should be taken out of this function... too shader dependant...
            int roughnessLoc = rlGetLocationUniform(shader.id, "roughness");

            rlEnableFramebuffer(fbo);

            const int MAX_MIPMAP_LEVELS = 5;   // Max number of prefilter texture mipmaps

            for (int mip = 0; mip < MAX_MIPMAP_LEVELS; mip++)
            {
                // Resize framebuffer according to mip-level size.
                int mipWidth = size * (int)MathF.Pow(0.5f, (float)mip);
                int mipHeight = size * (int)MathF.Pow(0.5f, (float)mip);

                rlViewport(0, 0, mipWidth, mipHeight);

                float roughness = (float)mip / (float)(MAX_MIPMAP_LEVELS - 1);
                Utils.SetShaderValue(shader, roughnessLoc, roughness, UNIFORM_FLOAT);

                for (int i = 0; i < 6; i++)
                {
                    // rlSetUniformMatrix(shader.locs[(int)LOC_MATRIX_VIEW], fboViews[i]);
                    // rlFramebufferAttach(fbo, prefilter.id, RL_ATTACHMENT_COLOR_CHANNEL0, RL_ATTACHMENT_CUBEMAP_POSITIVE_X + i, mip);

                    rlClearScreenBuffers();
                    rlLoadDrawCube();
                }
            }
            //------------------------------------------------------------------------------------------

            // STEP 3: Unload framebuffer and reset state
            //------------------------------------------------------------------------------------------
            rlDisableShader();          // Unbind shader
            rlDisableTexture();         // Unbind texture
            rlDisableFramebuffer();     // Unbind framebuffer
            rlUnloadFramebuffer(fbo);   // Unload framebuffer (and automatically attached depth texture/renderbuffer)

            // Reset viewport dimensions to default
            rlViewport(0, 0, rlGetFramebufferWidth(), rlGetFramebufferHeight());
            rlEnableBackfaceCulling();
            //------------------------------------------------------------------------------------------

            prefilter.width = size;
            prefilter.height = size;
            prefilter.mipmaps = MAX_MIPMAP_LEVELS;
            prefilter.format = PixelFormat.UNCOMPRESSED_R32G32B32;

            return prefilter;
        }

        // Generate BRDF texture using cubemap data
        // TODO: Review implementation: https://github.com/HectorMF/BRDFGenerator
        static Texture2D GenTextureBRDF(Shader shader, int size)
        {
            Texture2D brdf;

            // STEP 1: Setup framebuffer
            //------------------------------------------------------------------------------------------
            uint rbo = rlLoadTextureDepth(size, size, true);
            brdf.id = rlLoadTexture(IntPtr.Zero, size, size, PixelFormat.UNCOMPRESSED_R32G32B32, 1);

            uint fbo = rlLoadFramebuffer(size, size);
            // rlFramebufferAttach(fbo, rbo, RL_ATTACHMENT_DEPTH, RL_ATTACHMENT_RENDERBUFFER, 0);
            // rlFramebufferAttach(fbo, brdf.id, RL_ATTACHMENT_COLOR_CHANNEL0, RL_ATTACHMENT_TEXTURE2D, 0);
            //------------------------------------------------------------------------------------------

            // STEP 2: Draw to framebuffer
            //------------------------------------------------------------------------------------------
            // NOTE: Render BRDF LUT into a quad using FBO
            rlEnableShader(shader.id);

            rlViewport(0, 0, size, size);

            rlEnableFramebuffer(fbo);
            rlClearScreenBuffers();

            rlLoadDrawQuad();
            //------------------------------------------------------------------------------------------

            // STEP 3: Unload framebuffer and reset state
            //------------------------------------------------------------------------------------------
            rlDisableShader();          // Unbind shader
            rlDisableTexture();         // Unbind texture
            rlDisableFramebuffer();     // Unbind framebuffer
            rlUnloadFramebuffer(fbo);   // Unload framebuffer (and automatically attached depth texture/renderbuffer)

            // Reset viewport dimensions to default
            rlViewport(0, 0, rlGetFramebufferWidth(), rlGetFramebufferHeight());
            //------------------------------------------------------------------------------------------

            brdf.width = size;
            brdf.height = size;
            brdf.mipmaps = 1;
            brdf.format = PixelFormat.UNCOMPRESSED_R32G32B32;

            return brdf;
        }
    }
}
