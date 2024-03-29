﻿using ResourceManagement;
using ResourceManagement.Loaders;
using ResourceManagement.Resources;
using System.IO;
using LispInterpreter;

namespace Graphics.Loaders.Material
{
    public class MaterialLoader : ABasicLoader, IFileLoader
    {
        ResourceManager manager;
        Interpreter inter = new Interpreter();

        public ResourceNameConverter Converter
        {
            get { return converter; }
        }

        private readonly ResourceNameConverter converter =
            new ResourceNameConverter(@"data\materials\", @".mtl");

        public MaterialLoader(ResourceManager manager)
        {
            this.manager = manager;
            inter.Load(typeof(MaterialBuiltins));
        }

        protected override AResource doLoad(string name)
        {
            using (var file = File.Open(converter.getFilenameFrom(name), FileMode.Open))
            {
                var globalEnvironment = inter.createEnvironment();
                globalEnvironment.Add(new LispSymbol("manager"), new LispObject(manager));
                dynamic result = inter.Eval(file, globalEnvironment);

                return result;
            }

            

            // currently dummy code since true loading is not implemented yet.
            /*
            MaterialResource res = new MaterialResource(
                effectHandle: manager.GetResource("default", "effect"));
            res.AddTexture("tex2D", manager.GetResource(name, "texture"));
            res.frameDimensions = new Vector2(2.0f, 2.0f); // currently does not support animation, so there will only be one single frame*/
            //return res;
        }

        protected override void doUnload(AResource resource)
        {
            // do nothing since the material resource does not consume much memory and will be collected by the gc.
        }

        public override string Type
        {
            get { return "material"; }
        }
    }
}
