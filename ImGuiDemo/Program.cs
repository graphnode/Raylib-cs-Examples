using Raylib_cs;

namespace ImGuiDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            const int screenWidth = 1280;
            const int screenHeight = 720;

            // Initialization
            //--------------------------------------------------------------------------------------
            Raylib.SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT | ConfigFlags.FLAG_VSYNC_HINT | ConfigFlags.FLAG_WINDOW_RESIZABLE);
            Raylib.InitWindow(screenWidth, screenHeight, "ImGui demo");
            Raylib.SetTargetFPS(60);

            Raylib.InitAudioDevice();

            ImguiController controller = new ImguiController();
            EditorScreen editor = new EditorScreen();

            controller.Load(screenWidth, screenHeight);
            editor.Load();
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!Raylib.WindowShouldClose())
            {
                // Update
                //----------------------------------------------------------------------------------
                float dt = Raylib.GetFrameTime();

                // Feed the input events to our ImGui controller, which passes them through to ImGui.
                controller.Update(dt);
                editor.Update(dt);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                Raylib.BeginDrawing();
                Raylib.ClearBackground(editor.GetClearColor());

                Raylib.DrawText("Hello, world!", 12, 12, 20, Color.BLACK);

                controller.Draw();

                Raylib.EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            editor.Unload();
            controller.Dispose();
            Raylib.CloseAudioDevice();
            Raylib.CloseWindow();
            //--------------------------------------------------------------------------------------
        }
    }
}
