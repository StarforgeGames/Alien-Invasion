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
        LispEnvironment global;
        LispEnvironment builtIn = new LispEnvironment(null);
        Parser parser = new Parser();

        public LispEnvironment BuiltIn
        {
            get { return builtIn; }
        }

        public LispEnvironment Global
        {
            get { return global; }
        }

        public Interpreter()
        {
            Load(typeof(Base));
            global = new LispEnvironment(builtIn);
        }

        public dynamic Eval(Stream stream)
        {
            return Eval(new StreamReader(stream).ReadToEnd().ToCharArray());
        }

        public dynamic Eval(char[] code)
        {
            dynamic elem = parser.parse(code.AsEnumerable().GetEnumerator());
            string str = elem.ToString();
            return elem.Eval(null, Global);
        }

        public void ResetGlobal()
        {
            global = new LispEnvironment(builtIn);
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
