![Raylib-cs Logo](https://github.com/ChrisDill/Raylib-cs/blob/master/Logo/raylib-cs_256x256.png "Raylib-cs Logo")

# Raylib-cs-Examples

[![GitHub contributors](https://img.shields.io/github/contributors/ChrisDill/Raylib-cs-Examples)](https://github.com/ChrisDill/Raylib-cs-Examples/graphs/contributors)

[![License](https://img.shields.io/badge/license-zlib%2Flibpng-blue.svg)](LICENSE.md)
[![Chat on Discord](https://img.shields.io/discord/426912293134270465.svg?logo=discord)](https://discord.gg/VkzNHUE)
[![GitHub stars](https://img.shields.io/github/stars/ChrisDill/Raylib-cs-Examples?style=social)](https://github.com/ChrisDill/Raylib-cs-Examples/stargazers)

C# examples for raylib-cs, a simple and easy-to-use library to learn videogames programming (www.raylib.com)

## Getting Started

### Building

1. `git clone https://github.com/ChrisDill/Raylib-cs-Examples.git`

2. `cd Raylib-cs-Examples`

3. `dotnet build -c Release`

### Running

1. `dotnet run -p Examples/Examples.csproj -c Release`

## Contributing

If you have any ideas, feel free to open an issue and tell me what you think.
If you'd like to contribute, please fork the repository and make changes as
you'd like. Pull requests are warmly welcome.

If you want to [request features](https://github.com/raysan5/raylib/pulls) or [report bugs](https://github.com/raysan5/raylib/issues) related to the library (in contrast to this binding), please refer to the [author's project repo](https://github.com/raysan5/raylib).

## License

raylib-cs (and raylib) is licensed under an unmodified zlib/libpng license, which is an OSI-certified, BSD-like license that allows static linking with closed source software. Check [LICENSE](LICENSE.md) for further details.

// Send to shader light enabled state and type
/*IntPtr enabledPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Int32)));
Marshal.WriteInt32(enabledPtr, light.enabled ? ((Int32)1) : ((Int32)0));
SetShaderValue(shader, light.enabledLoc, enabledPtr, UNIFORM_INT);

IntPtr lightPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Int32)));
Marshal.WriteInt32(lightPtr, (Int32)light.type);
SetShaderValue(shader, light.typeLoc, lightPtr, UNIFORM_INT);

// Send to shader light position values
float[] position = new[] { light.position.X, light.position.Y, light.position.Z };

// Send to shader light target position values
float[] target = new[] { light.target.X, light.target.Y, light.target.Z };
IntPtr targetPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * target.Length);
Marshal.Copy(target, 0, targetPtr, target.Length);
SetShaderValue(shader, light.targetLoc, targetPtr, UNIFORM_VEC3);

// Send to shader light color values
float[] diff = new[] { (float)light.color.r / (float)255, (float)light.color.g / (float)255, (float)light.color.b / (float)255, (float)light.color.a / (float)255 };
IntPtr diffPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * diff.Length);
Marshal.Copy(diff, 0, diffPtr, diff.Length);
SetShaderValue(shader, light.colorLoc, diffPtr, UNIFORM_VEC4);*/
