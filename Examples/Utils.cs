using Raylib_cs;
using static Raylib_cs.MaterialMapType;

namespace Examples
{
    // Utility classes containing and tracking common unsafe interactions needed in the examples.
    public class Utils
    {
        public unsafe static void SetMaterialTexture(ref Model model, int materialIndex, MaterialMapType mapIndex, ref Texture2D texture)
        {
            Material *materials = (Material*)model.materials.ToPointer();
            MaterialMap* maps = (MaterialMap*)materials[0].maps.ToPointer();
            maps[(int)MAP_ALBEDO].texture = texture;
        }

        public unsafe static void SetMaterialShader(ref Model model, int materialIndex, ref Shader shader)
        {
            Material *materials = (Material*)model.materials.ToPointer();
            materials[0].shader = shader;
        }
    }
}