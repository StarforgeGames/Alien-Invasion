using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using LispInterpreter.BuiltIns;


namespace LispInterpreter
{
    public class LispEnvironment : ICloneable
    {
        Dictionary<LispSymbol, dynamic> env
            = new Dictionary<LispSymbol, dynamic>();

        internal LispEnvironment Parent
        {
            get;
            set;
        }

        internal LispEnvironment(LispEnvironment parent)
        {
            Parent = parent;
        }

        public void Add(LispSymbol symbol, LispElement element)
        {
            env[symbol] = element;
        }

        public void Add(LispSymbol symbol, Func<LispList, LispEnvironment, dynamic> func, string alias)
        {
            env[symbol] = new LispBuiltInFunction(func, alias);
        }

        public void Add(LispSymbol symbol, Func<LispList, LispEnvironment, dynamic> func)
        {
            Add(symbol, func, symbol.ToString());
        }

        public LispElement Lookup(LispSymbol symbol)
        {
            if (env.ContainsKey(symbol))
            {
                return env[symbol];
            }
            else if (Parent != null)
            {
                return Parent.Lookup(symbol);
            }
            else
            {
                return null;
            }
        }

        public object Clone()
        {
            //ToDo: make deep copy of environment!!!!
            return this;
            //throw new NotImplementedException();
        }

        internal void Load(params Type[] builtIns)
        {
            ParameterExpression localEnv = Expression.Parameter(typeof(LispEnvironment), "env");
            ParameterExpression args = Expression.Parameter(typeof(LispList), "args");

            foreach (var builtIn in builtIns)
            {
                var methodInfos = builtIn.GetMethods();
                foreach (var methodInfo in methodInfos)
                {
                    if (methodInfo.IsStatic)
                    {

                        var method = Expression.Call(methodInfo, args, localEnv);

                        var lambda = Expression.Lambda<Func<LispList, LispEnvironment, dynamic>>(method, args, localEnv);

                        var cLambda = lambda.Compile();

                        if (methodInfo.GetCustomAttributes(typeof(AliasAttribute), false).Any())
                        {
                            foreach (var attribute in methodInfo.GetCustomAttributes(typeof(AliasAttribute), false))
                            {
                                AliasAttribute attr = (AliasAttribute)attribute;
                                Add(new LispSymbol(attr.Alias), cLambda, builtIn.Name + "." + methodInfo.Name);
                            }
                        }
                        else
                        {
                            Add(new LispSymbol(methodInfo.Name), cLambda, builtIn.Name + "." + methodInfo.Name);
                        }
                    }
                }
            }
        }
    }
}
