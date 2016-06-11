using System.Collections.Generic;
using ResourceManagement;
using ResourceManagement.Resources;
using SlimDX;

namespace Graphics.Resources
{
    public class MaterialResource : AResource
    {
        private List<KeyValuePair<string, ResourceHandle>> textures = new List<KeyValuePair<string, ResourceHandle>>();

        private List<KeyValuePair<string, dynamic>> constants = new List<KeyValuePair<string, dynamic>>();

        public List<KeyValuePair<string, dynamic>> Constants
        {
            get { return constants; }
        }

        public void AddTexture(string binding, ResourceHandle texture)
        {
            textures.Add(new KeyValuePair<string, ResourceHandle>(binding, texture));
        }
        
        private ResourceHandle effectHandle;

        public MaterialResource(ResourceHandle effectHandle, string technique)
        {
            this.effectHandle = effectHandle;
            this.Technique = technique;
        }

        public string Technique { get; private set; }

        public EffectResource AcquireEffect()
        {
            return (EffectResource)effectHandle.Acquire();
        }

        public ResourceList<TextureResource> AcquireTextures()
        {
            return new ResourceList<TextureResource>(textures);
        }

        public void AddConstant(string name, dynamic value)
        {
            constants.Add(new KeyValuePair<string, dynamic>(name, value));
        }

        public Vector2 frameDimensions;

        /*
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
         * */
    }
}
