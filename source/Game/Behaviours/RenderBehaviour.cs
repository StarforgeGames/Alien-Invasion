using System;
using Game.Entities;
using SlimDX;
using SlimDX.D3DCompiler;
using SlimDX.Direct3D10;
using SlimDX.DXGI;
using Buffer = SlimDX.Direct3D10.Buffer;

namespace Game.Behaviours
{

    class RenderBehaviour : IBehaviour
    {
        private Entity entity;

        private EffectTechnique technique;
        private EffectPass pass;
        private Buffer vertices;
        private InputLayout layout;

        public RenderBehaviour(Entity entity)
        {
            this.entity = entity;

            var effect = Effect.FromFile(BaseGame.D3DDevice, "MiniTri.fx", "fx_4_0", ShaderFlags.None,
                EffectFlags.None, null, null);
            technique = effect.GetTechniqueByIndex(0);
            pass = technique.GetPassByIndex(0);
            layout = new InputLayout(BaseGame.D3DDevice, pass.Description.Signature, new[] {
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

            vertices = new SlimDX.Direct3D10.Buffer(BaseGame.D3DDevice, stream, new BufferDescription() {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = 3 * 32,
                Usage = ResourceUsage.Default
            });
            stream.Dispose();
        }

        #region IBehaviour Members

        public void OnUpdate(float deltaTime)
        {
            BaseGame.D3DDevice.InputAssembler.SetInputLayout(layout);
            BaseGame.D3DDevice.InputAssembler.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
            BaseGame.D3DDevice.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, 32, 0));

            for (int i = 0; i < technique.Description.PassCount; ++i) {
                pass.Apply();
                BaseGame.D3DDevice.Draw(3, 0);
            }
        }

        public void OnMessage(Message msg)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
