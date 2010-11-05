using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResourceManagement;
using ResourceManagement.Resources;
using SlimDX.Direct3D10;
using SlimDX;

namespace Graphics.Resources
{
    public class MaterialResource : AResource
    {
        private EffectResource effect;

        public Effect Effect { get { return effect.effect; } }

        public TextureResource texture;
        
        private ResourceHandle effectHandle;
        
        private ResourceHandle textureHandle;

        //List<KeyValuePair<string, ResourceHandle>> shaderResourceHandles;
        //public List<TextureResource> shaderResources;

        public MaterialResource(ResourceHandle effectHandle, ResourceHandle textureHandle)
        {
            this.effectHandle = effectHandle;
            this.textureHandle = textureHandle;
        }

        public override void Acquire()
        {
            base.Acquire();
            effect = (EffectResource)effectHandle.Acquire();
            texture = (TextureResource)textureHandle.Acquire();
        }

        public override void Dispose()
        {
            effect.Dispose();
            texture.Dispose();
            base.Dispose();
        }

        public void Apply()
        {
            effect.effect.GetVariableByName("tex2D").AsResource().SetResource(texture.texture);
        }

        public void Set(string name, bool value)
        {
            effect.effect.GetVariableByName(name).AsScalar().Set(value);
        }
        
        public void Set(string name, float value)
        {
            effect.effect.GetVariableByName(name).AsScalar().Set(value);
        }

        public void Set(string name, int value)
        {
            effect.effect.GetVariableByName(name).AsScalar().Set(value);
        }

        public void Set(string name, Vector2 value)
        {
            effect.effect.GetVariableByName(name).AsVector().Set(value);
        }

        public void Set(string name, Vector3 value)
        {
            effect.effect.GetVariableByName(name).AsVector().Set(value);
        }

        public void Set(string name, Vector4 value)
        {
            effect.effect.GetVariableByName(name).AsVector().Set(value);
        }

        public void Set(string name, Color4 value)
        {
            effect.effect.GetVariableByName(name).AsVector().Set(value);
        }

        public void Set(string name, Matrix value)
        {
            effect.effect.GetVariableByName(name).AsMatrix().SetMatrix(value);
        }

        public void Set(string name, ShaderResourceView value)
        {
            effect.effect.GetVariableByName(name).AsResource().SetResource(value);
        }

        public void Set(string name, SlimDX.Direct3D10.Buffer value)
        {
            effect.effect.GetVariableByName(name).AsConstantBuffer().SetConstantBuffer(value);
        }

        public void SetArray(string name, params Matrix[] value)
        {
            effect.effect.GetVariableByName(name).AsMatrix().SetMatrix(value);
        }

        public void SetArray(string name, params Vector4[] value)
        {
            effect.effect.GetVariableByName(name).AsVector().Set(value);
        }

        public void SetArray(string name, params Color4[] value)
        {
            effect.effect.GetVariableByName(name).AsVector().Set(value);
        }
    }
}
