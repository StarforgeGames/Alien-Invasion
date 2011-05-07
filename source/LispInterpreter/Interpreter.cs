using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Linq.Expressions;
using System.Globalization;
using LispInterpreter.BuiltIns;
using System.IO;

namespace LispInterpreter
{
    public class Interpreter
    {
        LispEnvironment builtIn = new LispEnvironment(null);
        Parser parser = new Parser();
        
        public Interpreter()
        {
            Load(typeof(Base));
        }

        public dynamic Eval(Stream stream, LispEnvironment global)
        {
            global.Parent = builtIn;
            var expressions = parser.parse(stream);
            dynamic result = null;
            foreach (LispElement expression in expressions)
            {
                result = expression.Eval(null, global);
            }
            global.Parent = null;
            return result;
        }

        public LispEnvironment createEnvironment()
        {
            return new LispEnvironment(builtIn);
        }

        public void Load(Type builtIns)
        {
            ParameterExpression localEnv = Expression.Parameter(typeof(LispEnvironment), "env");
            ParameterExpression args = Expression.Parameter(typeof(LispList), "args");

            var methodInfos = builtIns.GetMethods();//BindingFlags.Static);
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
                            builtIn.Add(new LispSymbol(attr.Alias), cLambda, builtIns.Name + "." + methodInfo.Name);
                        }
                    }
                    else
                    {
                        builtIn.Add(new LispSymbol(methodInfo.Name), cLambda, builtIns.Name + "." + methodInfo.Name);
                    }
                }
            }

        }
    }
}
