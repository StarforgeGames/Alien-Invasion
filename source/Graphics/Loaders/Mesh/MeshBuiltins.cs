using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LispInterpreter;
using SlimDX.Direct3D10;
using Graphics.Resources;
using SlimDX;
using System.IO;

namespace Graphics.Loaders.Mesh
{
    static class MeshBuiltins
    {
        static public dynamic vertextopology(LispList args, LispEnvironment env)
        {
            return Enum.Parse(typeof(PrimitiveTopology), args.First.Eval(env).Value, true);
        }

        static public dynamic vertexlayout(LispList args, LispEnvironment env)
        {
            int accum = 0;

            InputElement[] elements = new InputElement[args.Elems.Length];

            int i = 0;
            foreach (LispElement arg in args)
            {
                InputElement current = arg.Eval(env);

                current.AlignedByteOffset = accum;

                accum += VertexFormats.getByteCountOf(current.Format);

                elements[i] = current;
                i++;
            }

            return new { elements, elementSize = accum };
        }

        static public dynamic element(LispList args, LispEnvironment env)
        {
            LispSymbol name = args[0].Eval(env);
            LispString format = args[1].Eval(env);
            
            env.Add(name, VertexFormats.getParserFor(format.Value));

            return new InputElement(name.Value, 0, VertexFormats.getDXFormatOf(format.Value), 0, 0);
        }

        static public dynamic vertexdata(LispList args, LispEnvironment env)
        {
            using (var stream = new MemoryStream())
            {
                int elementCount = 0;
                foreach (LispElement arg in args)
                {
                    dynamic vertices = arg.Eval(env);
                    if (vertices is LispList)
                    {
                        foreach (var vertex in vertices)
                        {
                            writeVertexToStream(vertex, stream);
                            elementCount++;
                        }
                    }
                    else
                    {
                        writeVertexToStream(vertices, stream);
                        elementCount++;
                    }

                }

                var dataStream = new SlimDX.DataStream(stream.Length, false, true);
                stream.WriteTo(dataStream);
                dataStream.Position = 0;

                return dataStream;
            }
        }

        static public dynamic indexdata(LispList args, LispEnvironment env)
        {
            using (var stream = new MemoryStream())
            {
                foreach (LispElement arg in args)
                {
                    dynamic indexes = arg.Eval(env);
                    if (indexes is LispList)
                    {
                        foreach (var curIndexes in indexes)
                        {
                            stream.Write(curIndexes, 0, curIndexes.Length);
                        }
                    }
                    else
                    {
                        stream.Write(indexes, 0, indexes.Length);
                    }

                }

                var dataStream = new SlimDX.DataStream(stream.Length, false, true);
                stream.WriteTo(dataStream);
                dataStream.Position = 0;

                return dataStream;
            }
        }

        static public dynamic mesh(LispList args, LispEnvironment env)
        {
            IEnumerator<dynamic> arg = args.GetEnumerator();
            arg.MoveNext();
            var topology = arg.Current.Eval(env);
            arg.MoveNext();
            var layout = arg.Current.Eval(env);

            var resource = new MeshResource();

            resource.elementSize = layout.elementSize;

            resource.inputLayout = layout.elements;
            resource.primitiveTopology = topology;

            env.Add(new LispSymbol("vertexSize"), new LispInteger(resource.elementSize));

            arg.MoveNext();
            resource.vertexstream = arg.Current.Eval(env);
            resource.elementCount = (int)resource.vertexstream.Length / resource.elementSize;

            if (arg.MoveNext())
            {
                resource.indexed = true;
                if (resource.elementCount <= ushort.MaxValue)
                {
                    env.Add(new LispSymbol("indexes"), VertexFormats.R16UIntIndexes);
                    resource.indexFormat = SlimDX.DXGI.Format.R16_UInt;
                }
                else
                {
                    env.Add(new LispSymbol("indexes"), VertexFormats.R32UIntIndexes);
                    resource.indexFormat = SlimDX.DXGI.Format.R32_UInt;
                }
                resource.indexstream = arg.Current.Eval(env);
                if (resource.indexFormat == SlimDX.DXGI.Format.R16_UInt)
                {
                    resource.indexCount = (int)resource.indexstream.Length / 2;
                }
                else
                {
                    resource.indexCount = (int)resource.indexstream.Length / 4;
                }
            }
            else
            {
                resource.indexed = false;
                resource.indexFormat = SlimDX.DXGI.Format.Unknown;
            }

            return resource;
        }



        static private void writeVertexToStream(dynamic vertex, Stream stream)
        {
            foreach (byte[] element in vertex)
            {
                stream.Write(element, 0, element.Length);
            }
        }

        static public dynamic vertex(LispList args, LispEnvironment env)
        {
            LinkedList<byte[]> bytes = new LinkedList<byte[]>();
            foreach (LispElement arg in args)
            {
                bytes.AddLast(arg.Eval(env));
            }
            return bytes;
        }

        static private SlimDX.DXGI.Format parseFormat(string ordering, string type)
        {
            return (SlimDX.DXGI.Format)Enum.Parse(typeof(SlimDX.DXGI.Format), ordering + "_" + type, true);
        }
    }
}
