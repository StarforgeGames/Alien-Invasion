using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LispInterpreter;
using System.Runtime.InteropServices;
using System.IO;

namespace Graphics.Loaders.Mesh
{
    static class VertexFormats
    {
        public static Func<LispList, LispEnvironment, dynamic> getParserFor(string format)
        {
            switch (format)
            {
                case "R32G32B32A32Float":
                    return R32G32B32A32Float;
                case "R32G32Float":
                    return R32G32Float;
                case "R16UInt":
                    return R16UInt;
                case "R32UInt":
                    return R32UInt;
                default:
                    break;
            }
            throw new NotImplementedException();
        }

        public static SlimDX.DXGI.Format getDXFormatOf(string format)
        {
            switch (format)
            {
                case "R32G32B32A32Float":
                    return SlimDX.DXGI.Format.R32G32B32A32_Float;
                case "R32G32Float":
                    return SlimDX.DXGI.Format.R32G32_Float;
                case "R16UInt":
                    return SlimDX.DXGI.Format.R16_UInt;
                case "R32UInt":
                    return SlimDX.DXGI.Format.R32_UInt;
                default:
                    break;
            }
            throw new NotImplementedException();
        }

        public static dynamic R32G32B32A32Float(LispList args, LispEnvironment env)
        {
            return parse32Float(args, env, 4);
        }

        public static dynamic R32G32Float(LispList args, LispEnvironment env)
        {
            return parse32Float(args, env, 2);
        }

        private static byte[] parse32Float(LispList args, LispEnvironment env, int count)
        {
            byte[] result = new byte[count * 4];
            using (var stream = new MemoryStream(result))
            using (var writer = new BinaryWriter(stream))
            {
                foreach (LispElement arg in args)
                {
                    writer.Write((float)arg.Eval(env).Value);
                }
            }
            return result;
        }

        private static byte[] parse32UInt(LispList args, LispEnvironment env, int count)
        {
            byte[] result = new byte[count * sizeof(uint)];
            using (var stream = new MemoryStream(result))
            using (var writer = new BinaryWriter(stream))
            {
                foreach (LispElement arg in args)
                {
                    writer.Write((uint)arg.Eval(env).Value);
                }
            }
            return result;
        }

        private static byte[] parse16UInt(LispList args, LispEnvironment env, int count)
        {
            byte[] result = new byte[count * sizeof(ushort)];
            using (var stream = new MemoryStream(result))
            using (var writer = new BinaryWriter(stream))
            {
                foreach (LispElement arg in args)
                {
                    writer.Write((ushort)arg.Eval(env).Value);
                }
            }
            return result;
        }

        public static dynamic R32UInt(LispList args, LispEnvironment env)
        {

            return parse32UInt(args, env, 1);
        }

        public static dynamic R16UInt(LispList args, LispEnvironment env)
        {
            return parse16UInt(args, env, 1);
        }

        public static dynamic R32UIntIndexes(LispList args, LispEnvironment env)
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                foreach (LispElement arg in args)
                {
                    writer.Write((uint)arg.Eval(env).Value);
                }
                return stream.ToArray();
            }
        }

        public static dynamic R16UIntIndexes(LispList args, LispEnvironment env)
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                foreach (LispElement arg in args)
                {
                    writer.Write((ushort)arg.Eval(env).Value);
                }
                return stream.ToArray();
            }
        }

        public static int getByteCountOf(SlimDX.DXGI.Format format)
        {
            switch (format)
            {
                case SlimDX.DXGI.Format.A8_UNorm:
                    return 1;

                case SlimDX.DXGI.Format.B5G5R5A1_UNorm:
                case SlimDX.DXGI.Format.B5G6R5_UNorm:
                    return 2;


                case SlimDX.DXGI.Format.B8G8R8A8_Typeless:
                case SlimDX.DXGI.Format.B8G8R8A8_UNorm:
                case SlimDX.DXGI.Format.B8G8R8A8_UNorm_SRGB:
                case SlimDX.DXGI.Format.B8G8R8X8_Typeless:
                case SlimDX.DXGI.Format.B8G8R8X8_UNorm:
                case SlimDX.DXGI.Format.B8G8R8X8_UNorm_SRGB:
                    return 4;


                case SlimDX.DXGI.Format.BC1_Typeless:
                    break;
                case SlimDX.DXGI.Format.BC1_UNorm:
                    break;
                case SlimDX.DXGI.Format.BC1_UNorm_SRGB:
                    break;
                case SlimDX.DXGI.Format.BC2_Typeless:
                    break;
                case SlimDX.DXGI.Format.BC2_UNorm:
                    break;
                case SlimDX.DXGI.Format.BC2_UNorm_SRGB:
                    break;
                case SlimDX.DXGI.Format.BC3_Typeless:
                    break;
                case SlimDX.DXGI.Format.BC3_UNorm:
                    break;
                case SlimDX.DXGI.Format.BC3_UNorm_SRGB:
                    break;
                case SlimDX.DXGI.Format.BC4_SNorm:
                    break;
                case SlimDX.DXGI.Format.BC4_Typeless:
                    break;
                case SlimDX.DXGI.Format.BC4_UNorm:
                    break;
                case SlimDX.DXGI.Format.BC5_SNorm:
                    break;
                case SlimDX.DXGI.Format.BC5_Typeless:
                    break;
                case SlimDX.DXGI.Format.BC5_UNorm:
                    break;
                case SlimDX.DXGI.Format.BC6_SFloat16:
                    break;
                case SlimDX.DXGI.Format.BC6_Typeless:
                    break;
                case SlimDX.DXGI.Format.BC6_UFloat16:
                    break;
                case SlimDX.DXGI.Format.BC7_Typeless:
                    break;
                case SlimDX.DXGI.Format.BC7_UNorm:
                    break;
                case SlimDX.DXGI.Format.BC7_UNorm_SRGB:
                    break;

                case SlimDX.DXGI.Format.D16_UNorm:
                    return 2;
                case SlimDX.DXGI.Format.D24_UNorm_S8_UInt:
                case SlimDX.DXGI.Format.D32_Float:
                    return 4;

                case SlimDX.DXGI.Format.D32_Float_S8X24_UInt:
                    return 8;


                case SlimDX.DXGI.Format.G8R8_G8B8_UNorm:
                case SlimDX.DXGI.Format.R10G10B10A2_Typeless:
                case SlimDX.DXGI.Format.R10G10B10A2_UInt:
                case SlimDX.DXGI.Format.R10G10B10A2_UNorm:
                case SlimDX.DXGI.Format.R10G10B10_XR_Bias_A2_UNorm:
                case SlimDX.DXGI.Format.R11G11B10_Float:
                    return 4;


                case SlimDX.DXGI.Format.R16G16B16A16_Float:
                case SlimDX.DXGI.Format.R16G16B16A16_SInt:
                case SlimDX.DXGI.Format.R16G16B16A16_SNorm:
                case SlimDX.DXGI.Format.R16G16B16A16_Typeless:
                case SlimDX.DXGI.Format.R16G16B16A16_UInt:
                case SlimDX.DXGI.Format.R16G16B16A16_UNorm:
                    return 8;

                case SlimDX.DXGI.Format.R16G16_Float:
                case SlimDX.DXGI.Format.R16G16_SInt:
                case SlimDX.DXGI.Format.R16G16_SNorm:
                case SlimDX.DXGI.Format.R16G16_Typeless:
                case SlimDX.DXGI.Format.R16G16_UInt:
                case SlimDX.DXGI.Format.R16G16_UNorm:
                    return 4;

                case SlimDX.DXGI.Format.R16_Float:
                case SlimDX.DXGI.Format.R16_SInt:
                case SlimDX.DXGI.Format.R16_SNorm:
                case SlimDX.DXGI.Format.R16_Typeless:
                case SlimDX.DXGI.Format.R16_UInt:
                case SlimDX.DXGI.Format.R16_UNorm:
                    return 2;


                case SlimDX.DXGI.Format.R1_UNorm:
                    break;
                case SlimDX.DXGI.Format.R24G8_Typeless:
                case SlimDX.DXGI.Format.R24_UNorm_X8_Typeless:
                    return 4;

                case SlimDX.DXGI.Format.R32G32B32A32_Float:
                case SlimDX.DXGI.Format.R32G32B32A32_SInt:
                case SlimDX.DXGI.Format.R32G32B32A32_Typeless:
                case SlimDX.DXGI.Format.R32G32B32A32_UInt:
                    return 16;


                case SlimDX.DXGI.Format.R32G32B32_Float:
                case SlimDX.DXGI.Format.R32G32B32_SInt:
                case SlimDX.DXGI.Format.R32G32B32_Typeless:
                case SlimDX.DXGI.Format.R32G32B32_UInt:
                    return 12;


                case SlimDX.DXGI.Format.R32G32_Float:
                case SlimDX.DXGI.Format.R32G32_SInt:
                case SlimDX.DXGI.Format.R32G32_Typeless:
                case SlimDX.DXGI.Format.R32G32_UInt:
                case SlimDX.DXGI.Format.R32G8X24_Typeless:
                    return 8;


                case SlimDX.DXGI.Format.R32_Float:
                case SlimDX.DXGI.Format.R32_Float_X8X24_Typeless:
                case SlimDX.DXGI.Format.R32_SInt:
                case SlimDX.DXGI.Format.R32_Typeless:
                case SlimDX.DXGI.Format.R32_UInt:
                case SlimDX.DXGI.Format.R8G8B8A8_SInt:
                case SlimDX.DXGI.Format.R8G8B8A8_SNorm:
                case SlimDX.DXGI.Format.R8G8B8A8_Typeless:
                case SlimDX.DXGI.Format.R8G8B8A8_UInt:
                case SlimDX.DXGI.Format.R8G8B8A8_UNorm:
                case SlimDX.DXGI.Format.R8G8B8A8_UNorm_SRGB:
                case SlimDX.DXGI.Format.R8G8_B8G8_UNorm:
                    return 4;

                case SlimDX.DXGI.Format.R8G8_SInt:
                case SlimDX.DXGI.Format.R8G8_SNorm:
                case SlimDX.DXGI.Format.R8G8_Typeless:
                case SlimDX.DXGI.Format.R8G8_UInt:
                case SlimDX.DXGI.Format.R8G8_UNorm:
                    return 2;

                case SlimDX.DXGI.Format.R8_SInt:
                case SlimDX.DXGI.Format.R8_SNorm:
                case SlimDX.DXGI.Format.R8_Typeless:
                case SlimDX.DXGI.Format.R8_UInt:
                case SlimDX.DXGI.Format.R8_UNorm:
                    return 1;

                case SlimDX.DXGI.Format.R9G9B9E5_SharedExp:
                    break;
                case SlimDX.DXGI.Format.Unknown:
                    break;
                case SlimDX.DXGI.Format.X24_Typeless_G8_UInt:
                    return 4;

                case SlimDX.DXGI.Format.X32_Typeless_G8X24_UInt:
                    return 8;
                default:
                    break;
            }
            throw new NotImplementedException();
        }
    }
}
