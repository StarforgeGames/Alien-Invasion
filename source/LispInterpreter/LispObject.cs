using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace LispInterpreter
{
    public class LispObject : LispElement
    {
        dynamic obj;
        Type objType;

        public dynamic Value
        {
            get { return obj; }
        }

        public LispObject(dynamic obj)
        {
            this.obj = obj;
            this.objType = obj.GetType();
        }

        public dynamic Eval(LispEnvironment env, dynamic e = null)
        {
            dynamic[] args = (dynamic[])e.Elems;

            if (args.Any())
            {
                var par = (from p in args.Skip(1)
                           select p.Eval(env));

                object[] methParams = unmarshal(par);
                Type[] paramTypes = (from mParam in methParams
                                     select mParam.GetType()).ToArray();


                // MethodInfo meth = objType.GetMethod(par.First().ToString(), BindingFlags.Default, Type.DefaultBinder, paramTypes, null);
                MethodInfo meth = objType.GetMethod(args[0].ToString(), paramTypes);
                Type resultType = meth.ReturnType;

                return marshal(meth.Invoke(obj, methParams), resultType);
            }
            else
            {
                return this;
            }
            
        }

        private object[] unmarshal(IEnumerable<dynamic> pars)
        {
            return (from par in pars
                   select par.Value).ToArray();
        }

        private dynamic marshal(dynamic obj, Type objType)
        {

            if (objType == typeof(float))
            {
                return new LispFloat(obj);
            }
            else if (objType == typeof(double))
            {
                return new LispDouble(obj);
            }
            else if (objType == typeof(int))
            {
                return new LispInteger(obj);
            }
            else if (objType == typeof(string))
            {
                return new LispString(obj);
            }
            else if (isLispType(objType))
            {
                return obj;
            }
            else
            {
                return new LispObject(obj);
            }
        }

        private bool isLispType(Type resultType)
        {
            return resultType == typeof(LispFloat)
                || resultType == typeof(LispDouble)
                || resultType == typeof(LispInteger)
                || resultType == typeof(LispFunction)
                || resultType == typeof(LispList)
                || resultType == typeof(LispString)
                || resultType == typeof(LispObject);
        }
    }
}
