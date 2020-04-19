using System;
using System.IO;
using System.Reflection;
using static Raylib_cs.Raylib;

namespace Examples
{
    class Program
    {
        static void RunExample(string fileName)
        {
            if (File.Exists(fileName))
            {
                try
                {
                    string name = Path.GetFileNameWithoutExtension(fileName);
                    Type.GetType("Examples." + name)?.GetMethod("Main")?.Invoke(null, null);
                }
                catch (TargetInvocationException e)
                {
                    Console.WriteLine(e.InnerException.Message);
                    Console.WriteLine(e.InnerException.StackTrace);
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Could not find " + fileName);
            }
        }

        static void Main(string[] args)
        {
            ChangeDirectory("./Examples");

            // [core] module examples
            ChangeDirectory("./core");
            RunExample("core_2d_camera_platformer.cs");
            RunExample("core_2d_camera.cs");
            RunExample("core_3d_camera_first_person.cs");
            RunExample("core_3d_camera_free.cs");
            RunExample("core_3d_camera_mode.cs");
            RunExample("core_3d_picking.cs");
            RunExample("core_basic_window.cs");
            RunExample("core_custom_logging.cs");
            RunExample("core_drop_files.cs");
            RunExample("core_input_gamepad.cs");
            RunExample("core_input_gestures.cs");
            RunExample("core_input_keys.cs");
            RunExample("core_input_mouse_wheel.cs");
            RunExample("core_input_mouse.cs");
            RunExample("core_input_multitouch.cs");
            RunExample("core_random_values.cs");
            RunExample("core_scissor_test.cs");
            RunExample("core_storage_values.cs");
            RunExample("core_vr_simulator.cs");
            RunExample("core_window_letterbox.cs");
            RunExample("core_world_screen.cs");
            ChangeDirectory("../");

            // [shapes] module examples
            ChangeDirectory("./shapes");
            RunExample("shapes_basic_shapes.cs");
            RunExample("shapes_bouncing_ball.cs");
            RunExample("shapes_collision_area.cs");
            RunExample("shapes_colors_palette.cs");
            RunExample("shapes_easings_ball_anim.cs");
            RunExample("shapes_easings_box_anim.cs");
            RunExample("shapes_easings_rectangle_array.cs");
            RunExample("shapes_following_eyes.cs");
            RunExample("shapes_lines_bezier.cs");
            RunExample("shapes_logo_raylib_anim.cs");
            RunExample("shapes_logo_raylib.cs");
            RunExample("shapes_rectangle_scaling.cs");
            ChangeDirectory("../");

            // [textures] module examples
            ChangeDirectory("./textures");
            RunExample("textures_background_scrolling.cs");
            RunExample("textures_bunnymark.cs");
            RunExample("textures_image_9patch.cs");
            RunExample("textures_image_drawing.cs");
            RunExample("textures_image_generation.cs");
            RunExample("textures_image_loading.cs");
            RunExample("textures_image_processing.cs");
            RunExample("textures_image_text.cs");
            RunExample("textures_logo_raylib.cs");
            RunExample("textures_mouse_painting.cs");
            RunExample("textures_npatch_drawing.cs");
            RunExample("textures_particles_blending.cs");
            RunExample("textures_raw_data.cs");
            RunExample("textures_rectangle.cs");
            // RunExample("textures_sprite_button.cs");
            // RunExample("textures_sprite_explosion.cs");
            RunExample("textures_srcrec_dstrec.cs");
            RunExample("textures_to_image.cs");
            ChangeDirectory("../");

            // [text] module examples
            ChangeDirectory("./text");
            RunExample("text_font_filters.cs");
            RunExample("text_font_loading.cs");
            RunExample("text_font_spritefont.cs");
            RunExample("text_format_text.cs");
            RunExample("text_input_box.cs");
            RunExample("text_raylib_fonts.cs");
            RunExample("text_rectangle_bounds.cs");
            RunExample("text_writing_anim.cs");
            ChangeDirectory("../");

            // [models] module examples
            ChangeDirectory("./models");
            RunExample("models_animation.cs");
            RunExample("models_billboard.cs");
            RunExample("models_box_collisions.cs");
            RunExample("models_cubicmap.cs");
            RunExample("models_first_person_maze.cs");
            RunExample("models_geometric_shapes.cs");
            RunExample("models_heightmap.cs");
            RunExample("models_loading.cs");
            RunExample("models_material_pbr.cs");
            RunExample("models_mesh_generation.cs");
            RunExample("models_mesh_picking.cs");
            RunExample("models_orthographic_projection.cs");
            RunExample("models_rlgl_solar_system.cs");
            RunExample("models_skybox.cs");
            RunExample("models_waving_cubes.cs");
            RunExample("models_yaw_pitch_roll.cs");
            ChangeDirectory("../");

            // [shaders] module examples
            ChangeDirectory("./shaders");
            RunExample("shaders_basic_lighting.cs");
            RunExample("shaders_custom_uniform.cs");
            RunExample("shaders_eratosthenes.cs");
            RunExample("shaders_fog.cs");
            RunExample("shaders_julia_set.cs");
            RunExample("shaders_model_shader.cs");
            RunExample("shaders_palette_switch.cs");
            RunExample("shaders_postprocessing.cs");
            RunExample("shaders_raymarching.cs");
            RunExample("shaders_shapes_textures.cs");
            RunExample("shaders_simple_mask.cs");
            RunExample("shaders_texture_drawing.cs");
            RunExample("shaders_texture_waves.cs");
            ChangeDirectory("../");

            // [audio] module examples
            ChangeDirectory("./audio");
            //RunExample("audio_module_playing.cs");
            //RunExample("audio_multichannel_sound.cs");
            //RunExample("audio_music_stream.cs");
            //RunExample("audio_raw_stream.cs");
            //RunExample("audio_sound_loading.cs");
            ChangeDirectory("../");
        }
    }
}
