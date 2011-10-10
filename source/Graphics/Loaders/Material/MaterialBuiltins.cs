using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LispInterpreter;
using Graphics.Resources;
using ResourceManagement;
using ResourceManagement.Resources;
using SlimDX;

namespace Graphics.Loaders.Material
{
    static class MaterialBuiltins
    {
        static public dynamic material(LispList args, LispEnvironment env)
        {
            IEnumerator<dynamic> arg = args.GetEnumerator();
            arg.MoveNext();
            var technique = arg.Current.Eval(env);
            arg.MoveNext();
            var effect = arg.Current.Eval(env);
            arg.MoveNext();
            List<KeyValuePair<string, ResourceHandle>> textures = arg.Current.Eval(env);
            arg.MoveNext();
            var constants = arg.Current.Eval(env);
            
            var mat = new MaterialResource(effect, technique);
            foreach (var texture in textures)
            {
                mat.AddTexture(texture.Key, texture.Value);
            }

            foreach(var constant in constants)
            {
                mat.AddConstant(constant.Key, constant.Value);
            }

            return mat;
            //var material = new MaterialResource();
            throw new NotImplementedException();
        }

        static public dynamic technique(LispList args, LispEnvironment env)
        {
            return args.First.Eval(env).Value;
        }

        static public dynamic effect(LispList args, LispEnvironment env)
        {
            ResourceManager manager = ((LispObject)env.Lookup(new LispSymbol("manager"))).Value;
            return manager.GetResource(args.First.Eval(env).Value, "effect");
        }

        static public dynamic textures(LispList args, LispEnvironment env)
        {
            var texList = new List<KeyValuePair<string, ResourceHandle>>(args.Elems.Length);

            foreach (LispElement arg in args)
            {
                var textures = arg.Eval(env);

                if(textures is LispList)
                {
                    texList.AddRange(textures);
                }
                else
                {
                    texList.Add(textures);
                }
            }
            
            return texList;
        }

        static public dynamic texture(LispList args, LispEnvironment env)
        {
            ResourceManager manager = ((LispObject)env.Lookup(new LispSymbol("manager"))).Value;
            return new KeyValuePair<string, ResourceHandle>(
                args.Elems[0].Eval(env).Value,
                manager.GetResource(args.Elems[1].Eval(env).Value, "texture"));
        }

        static public dynamic constants(LispList args, LispEnvironment env)
        {
            var constList = new List<dynamic>(args.Elems.Length);
            foreach (LispElement arg in args)
            {
                var constants = arg.Eval(env);

                if(constants is LispList)
                {
                    constList.AddRange(constants);
                }
                else
                {
                    constList.Add(constants);
                }
            }

            return constList;
        }

        static public dynamic framelayout(LispList args, LispEnvironment env)
        {
            return null;//throw new NotImplementedException();
        }

        static public dynamic instancelayout(LispList args, LispEnvironment env)
        {
            return null;//throw new NotImplementedException();
        }

        static public dynamic framedata(LispList args, LispEnvironment env)
        {
            return null;//throw new NotImplementedException();
        }

        static public dynamic instancedata(LispList args, LispEnvironment env)
        {
            return null;//throw new NotImplementedException();
        }

        static public dynamic element(LispList args, LispEnvironment env)
        {
            return null;//throw new NotImplementedException();
        }

        static public dynamic shaderbinding(LispList args, LispEnvironment env)
        {
            return null;//throw new NotImplementedException();
        }

        static public dynamic scalar(LispList args, LispEnvironment env)
        {
            IEnumerator<dynamic> arg = args.GetEnumerator();
            arg.MoveNext();
            string name = arg.Current.Eval(env).Value;
            arg.MoveNext();
            float x = arg.Current;
            return new KeyValuePair<string, float>(name, x);
        }           

        static public dynamic vector2(LispList args, LispEnvironment env)
        {
            IEnumerator<dynamic> arg = args.GetEnumerator();
            arg.MoveNext();
            string name = arg.Current.Eval(env).Value;
            arg.MoveNext();
            float x = arg.Current.Value;
            arg.MoveNext();
            float y = arg.Current.Value;
            return new KeyValuePair<string, Vector2>(name, new Vector2(x, y));
        }

        static public dynamic vector3(LispList args, LispEnvironment env)
        {
            IEnumerator<dynamic> arg = args.GetEnumerator();
            arg.MoveNext();
            string name = arg.Current.Eval(env).Value;
            arg.MoveNext();
            float x = arg.Current.Value;
            arg.MoveNext();
            float y = arg.Current.Value;
            arg.MoveNext();
            float z = arg.Current.Value;
            return new KeyValuePair<string, Vector3>(name, new Vector3(x, y, z));
        }

        static public dynamic vector4(LispList args, LispEnvironment env)
        {
            IEnumerator<dynamic> arg = args.GetEnumerator();
            arg.MoveNext();
            string name = arg.Current.Eval(env).Value;
            arg.MoveNext();
            float x = arg.Current.Value;
            arg.MoveNext();
            float y = arg.Current.Value;
            arg.MoveNext();
            float z = arg.Current.Value;
            arg.MoveNext();
            float w = arg.Current.Value;
            return new KeyValuePair<string, Vector4>(name, new Vector4(x, y, z, w));
        }
    }
}
