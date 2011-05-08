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
        LispEnvironment builtInFunctions = new LispEnvironment(null);
        Parser parser = new Parser();
        
        public Interpreter()
        {
            Load(typeof(Base));
        }

        public dynamic Eval(Stream stream, LispEnvironment global)
        {
            global.Parent = builtInFunctions;
            var expressions = parser.parse(stream);
            dynamic result = null;
            foreach (LispElement expression in expressions)
            {
                result = expression.Eval(global);
            }
            global.Parent = null;
            return result;
        }

        public LispEnvironment createEnvironment()
        {
            return new LispEnvironment(builtInFunctions);
        }

        public void Load(params Type[] builtIns)
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
                                builtInFunctions.Add(new LispSymbol(attr.Alias), cLambda, builtIn.Name + "." + methodInfo.Name);
                            }
                        }
                        else
                        {
                            builtInFunctions.Add(new LispSymbol(methodInfo.Name), cLambda, builtIn.Name + "." + methodInfo.Name);
                        }
                    }
                }   
            }
        }
    }
}
