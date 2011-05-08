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
        static public dynamic topology(LispList args, LispEnvironment env)
        {
            return Enum.Parse(typeof(PrimitiveTopology), args.First.Eval(env).Value, true);
        }

        static public dynamic layout(LispList args, LispEnvironment env)
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

        static public dynamic mesh(LispList args, LispEnvironment env)
        {
            var topology = args[0].Eval(env);
            var layout = args[1].Eval(env);

            var resource = new MeshResource();

            resource.elementSize = layout.elementSize;
            resource.elementCount = args.Count() - 2;

            resource.inputLayout = layout.elements;
            resource.primitiveTopology = topology;

            env.Add(new LispSymbol("vertexSize"), new LispInteger(resource.elementSize));

            using (var stream = new MemoryStream())
            {
                int elementCount = 0;
                foreach (LispElement arg in args.Skip(2))
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
                resource.elementCount = elementCount;

                resource.stream = new SlimDX.DataStream(resource.elementSize * resource.elementCount, false, true);
                stream.WriteTo(resource.stream);
            }

            resource.stream.Position = 0;

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
