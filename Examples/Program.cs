using Raylib_cs;

namespace Examples
{
    class Program
    {
        static unsafe void Main(string[] args)
        {
            Raylib.SetTraceLogCallback(&Logging.LogConsole);

            RunCoreExamples();
            RunShapesExamples();
            RunTextureExamples();
            RunTextExamples();
            RunModelExamples();
            RunShaderExamples();
            RunAudioExamples();
        }

        static void RunCoreExamples()
        {
            core_2d_camera_platformer.Main();
            core_2d_camera.Main();
            core_3d_camera_first_person.Main();
            core_3d_camera_free.Main();
            core_3d_camera_mode.Main();
            core_3d_picking.Main();
            core_basic_screen_manager.Main();
            core_basic_window.Main();
            core_custom_logging.Main();
            core_drop_files.Main();
            core_input_gamepad.Main();
            core_input_gestures.Main();
            core_input_keys.Main();
            core_input_mouse_wheel.Main();
            core_input_mouse.Main();
            core_input_multitouch.Main();
            core_quat_conversion.Main();
            core_random_values.Main();
            core_scissor_test.Main();
            core_smooth_pixelperfect.Main();
            core_split_screen.Main();
            core_storage_values.Main();
            core_vr_simulator.Main();
            core_window_flags.Main();
            core_window_letterbox.Main();
            core_world_screen.Main();
        }

        static void RunShapesExamples()
        {
            shapes_basic_shapes.Main();
            shapes_bouncing_ball.Main();
            shapes_collision_area.Main();
            shapes_colors_palette.Main();
            shapes_easings_ball_anim.Main();
            // shapes_easings_box_anim.Main();
            shapes_easings_rectangle_array.Main();
            shapes_following_eyes.Main();
            shapes_lines_bezier.Main();
            shapes_logo_raylib_anim.Main();
            shapes_logo_raylib.Main();
            shapes_rectangle_scaling.Main();
        }

        static void RunTextureExamples()
        {
            textures_background_scrolling.Main();
            textures_blend_modes.Main();
            textures_bunnymark.Main();
            textures_draw_tiled.Main();
            textures_image_drawing.Main();
            textures_image_generation.Main();
            textures_image_loading.Main();
            textures_image_processing.Main();
            textures_image_text.Main();
            textures_logo_raylib.Main();
            textures_mouse_painting.Main();
            textures_npatch_drawing.Main();
            textures_particles_blending.Main();
            textures_polygon.Main();
            textures_raw_data.Main();
            textures_rectangle.Main();
            textures_sprite_button.Main();
            textures_sprite_explosion.Main();
            textures_srcrec_dstrec.Main();
            textures_to_image.Main();
        }

        static void RunTextExamples()
        {
            // text_draw_3d.Main();
            text_font_filters.Main();
            text_font_loading.Main();
            text_font_sdf.Main();
            text_font_spritefont.Main();
            text_format_text.Main();
            text_input_box.Main();
            text_raylib_fonts.Main();
            text_rectangle_bounds.Main();
            text_writing_anim.Main();
        }

        static void RunModelExamples()
        {
            models_animation.Main();
            models_billboard.Main();
            models_box_collisions.Main();
            models_cubicmap.Main();
            models_first_person_maze.Main();
            models_geometric_shapes.Main();
            models_heightmap.Main();
            models_loading_gltf.Main();
            models_loading_vox.Main();
            models_loading.Main();
            models_mesh_generation.Main();
            models_mesh_picking.Main();
            models_orthographic_projection.Main();
            models_rlgl_solar_system.Main();
            models_skybox.Main();
            models_waving_cubes.Main();
            models_yaw_pitch_roll.Main();
        }

        static void RunShaderExamples()
        {
            shaders_basic_lighting.Main();
            shaders_custom_uniform.Main();
            shaders_eratosthenes.Main();
            shaders_fog.Main();
            shaders_hot_reloading.Main();
            shaders_julia_set.Main();
            shaders_model_shader.Main();
            shaders_multi_sample2d.Main();
            shaders_palette_switch.Main();
            shaders_postprocessing.Main();
            shaders_raymarching.Main();
            shaders_mesh_instancing.Main();
            shaders_shapes_textures.Main();
            shaders_simple_mask.Main();
            shaders_spotlight.Main();
            shaders_texture_drawing.Main();
            shaders_texture_outline.Main();
            shaders_texture_waves.Main();
        }

        static void RunAudioExamples()
        {
            audio_module_playing.Main();
            audio_multichannel_sound.Main();
            audio_music_stream.Main();
            audio_raw_stream.Main();
            audio_sound_loading.Main();
        }
    }
}
