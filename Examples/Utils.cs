using System;
using System.Runtime.InteropServices;
using Raylib_cs;

namespace Examples
{
    /*
    Utility functions for parts of the api that are not easy to interact with via pinvoke.
    Not included in the bindings as there are multiple ways to handle these cases.
    I prefer to leave that choice to the user.
    */
    public static class Utils
    {
        // Extension providing SubText
        public static string SubText(this string input, int position, int length)
        {
            return input.Substring(position, Math.Min(length, input.Length));
        }

        /*
        Utility to convert the IntPtr from GetDroppedFiles to a string[].

        GetDroppedFiles is a char** but the length varies based on MAX_FILEPATH_LENGTH.

        #if defined(__linux__)
            #define MAX_FILEPATH_LENGTH    4096     // Use Linux PATH_MAX value
        #else
            #define MAX_FILEPATH_LENGTH     512     // Use common value
        #endif

        Here is how it allocates the strings.

        // GLFW3 Window Drop Callback, runs when drop files into window
        // NOTE: Paths are stored in dynamic memory for further retrieval
        // Everytime new files are dropped, old ones are discarded
        static void WindowDropCallback(GLFWwindow *window, int count, const char **paths)
        {
            ClearDroppedFiles();

            CORE.Window.dropFilesPath = (char **)RL_MALLOC(sizeof(char *)*count);

            for (int i = 0; i < count; i++)
            {
                CORE.Window.dropFilesPath[i] = (char *)RL_MALLOC(sizeof(char)*MAX_FILEPATH_LENGTH);
                strcpy(CORE.Window.dropFilesPath[i], paths[i]);
            }

            CORE.Window.dropFilesCount = count;
        }

        If it was fixed I think the following could work.

        // Get dropped files names (memory should be freed)
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStr, SizeConst=512)]
        public static extern string[] GetDroppedFiles(ref int count);
        */
        public static string[] MarshalDroppedFiles(ref int count)
        {
            string[] droppedFileStrings = new string[count];
            IntPtr pointer = Raylib.GetDroppedFiles(ref count);

            string[] s = new string[count];
            char[] word;
            int i;
            int j;
            int size;

            // TODO: this is a mess, find a better way
            unsafe
            {
                byte** str = (byte**)pointer.ToPointer();

                i = 0;
                while (i < count)
                {
                    j = 0;
                    while (str[i][j] != 0)
                        j++;
                    size = j;
                    word = new char[size];
                    j = 0;
                    while (str[i][j] != 0)
                    {
                        word[j] = (char)str[i][j];
                        j++;
                    }
                    s[i] = new string(word);

                    i++;
                }
            }
            return s;
        }

        public unsafe static Material GetMaterial(ref Model model, int materialIndex)
        {
            Material* materials = (Material*)model.materials.ToPointer();
            return *materials;
        }

        public unsafe static Texture2D GetMaterialTexture(ref Model model, int materialIndex, MaterialMapType mapIndex)
        {
            Material* materials = (Material*)model.materials.ToPointer();
            MaterialMap* maps = (MaterialMap*)materials[0].maps.ToPointer();
            return maps[(int)mapIndex].texture;
        }

        public unsafe static void SetMaterialTexture(ref Model model, int materialIndex, MaterialMapType mapIndex, ref Texture2D texture)
        {
            Material* materials = (Material*)model.materials.ToPointer();
            MaterialMap* maps = (MaterialMap*)materials[0].maps.ToPointer();
            maps[(int)mapIndex].texture = texture;
        }

        public unsafe static void SetMaterialShader(ref Model model, int materialIndex, ref Shader shader)
        {
            Material* materials = (Material*)model.materials.ToPointer();
            materials[0].shader = shader;
        }

        /*
        Overloads for Shader Values.
        Currently Raylib.SetShaderValue is overloaded with ref int and ref float for the value.
        Same sort of usage in raylib so I assume it is converted to void * although this might not be good idea to rely on.
        */
        public static void SetShaderValue(Shader shader, int uniformLoc, int value, ShaderUniformDataType uniformType = ShaderUniformDataType.UNIFORM_INT)
        {
            Raylib.SetShaderValue(shader, uniformLoc, ref value, uniformType);
        }

        public static void SetShaderValue(Shader shader, int uniformLoc, float value, ShaderUniformDataType uniformType = ShaderUniformDataType.UNIFORM_FLOAT)
        {
            Raylib.SetShaderValue(shader, uniformLoc, ref value, uniformType);
        }

        public static void SetShaderValue(Shader shader, int uniformLoc, int[] value, ShaderUniformDataType uniformType)
        {
            IntPtr valuePtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * value.Length);
            Marshal.Copy(value, 0, valuePtr, value.Length);
            Raylib.SetShaderValue(shader, uniformLoc, valuePtr, uniformType);
            Marshal.FreeHGlobal(valuePtr);
        }

        public static void SetShaderValue(Shader shader, int uniformLoc, float[] value, ShaderUniformDataType uniformType)
        {
            IntPtr valuePtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * value.Length);
            Marshal.Copy(value, 0, valuePtr, value.Length);
            Raylib.SetShaderValue(shader, uniformLoc, valuePtr, uniformType);
            Marshal.FreeHGlobal(valuePtr);
        }

        public static void SetShaderValueV(Shader shader, int uniformLoc, int[] value, ShaderUniformDataType uniformType, int count)
        {
            IntPtr valuePtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * value.Length);
            Marshal.Copy(value, 0, valuePtr, value.Length);
            Raylib.SetShaderValueV(shader, uniformLoc, valuePtr, uniformType, count);
            Marshal.FreeHGlobal(valuePtr);
        }

        public static void SetShaderValueV(Shader shader, int uniformLoc, float[] value, ShaderUniformDataType uniformType, int count)
        {
            IntPtr valuePtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * value.Length);
            Marshal.Copy(value, 0, valuePtr, value.Length);
            Raylib.SetShaderValueV(shader, uniformLoc, valuePtr, uniformType, count);
            Marshal.FreeHGlobal(valuePtr);
        }
    }
}
