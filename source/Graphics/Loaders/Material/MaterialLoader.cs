﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResourceManagement;
using ResourceManagement.Loaders;
using ResourceManagement.Resources;
using System.IO;
using Graphics.Resources;
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
            /*using (var file = File.Open(converter.getFilenameFrom(name), FileMode.Open))
            {
                var globalEnvironment = inter.createEnvironment();
                dynamic result = inter.Eval(file, globalEnvironment);
            }*/

            // currently dummy code since true loading is not implemented yet.
            MaterialResource res = new MaterialResource(
                effectHandle: manager.GetResource("default", "effect"),
                textureHandle: manager.GetResource(name, "texture"));
            return res;
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