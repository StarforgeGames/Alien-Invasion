using System;
using SlimDX;
using SlimDX.D3DCompiler;
using SlimDX.Direct3D10;
using SlimDX.DXGI;
using Buffer = SlimDX.Direct3D10.Buffer;

namespace Graphics.Primitives
{

    /// <summary>
    /// Class to create a plane. It's called XPlane to make it sound cool... No, really: It's to avoid naming 
    /// conflicts with SlimDX or other graphics APIs.
    /// </summary>
    class XPlane : IRenderable
    {
        private EffectTechnique technique;
        private EffectPass pass;
        private Buffer vertices;
        private InputLayout layout;

        public XPlane()
        {
            var effect = Effect.FromFile(Renderer.Device, @"Shaders\SimplePassThrough.fx", "fx_4_0", ShaderFlags.None,
                EffectFlags.None, null, null);
            technique = effect.GetTechniqueByIndex(0);
            pass = technique.GetPassByIndex(0);
            layout = new InputLayout(Renderer.Device, pass.Description.Signature, new[] {
                new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
                new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 16, 0) 
            });

            var stream = new DataStream(3 * 32, true, true);
            stream.WriteRange(new[] {
                new Vector4(0.0f, 0.5f, 0.5f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                new Vector4(0.5f, -0.5f, 0.5f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                new Vector4(-0.5f, -0.5f, 0.5f, 1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f)
            });
            stream.Position = 0;

            vertices = new SlimDX.Direct3D10.Buffer(Renderer.Device, stream, new BufferDescription() {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = 3 * 32,
                Usage = ResourceUsage.Default
            });
            stream.Dispose();
        }

        #region IRenderable Members

        public void Render()
        {
            Renderer.Device.InputAssembler.SetInputLayout(layout);
            Renderer.Device.InputAssembler.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
            Renderer.Device.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, 32, 0));

            for (int i = 0; i < technique.Description.PassCount; ++i) {
                pass.Apply();
                Renderer.Device.Draw(3, 0);
            }
        }

        #endregion
    }

}
