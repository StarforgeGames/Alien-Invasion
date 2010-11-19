using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Linq.Expressions;
using System.Globalization;
using LispInterpreter.BuiltIns;

namespace LispInterpreter
{
    public class Interpreter
    {
        LispEnvironment global = new LispEnvironment(null);
        Parser parser = new Parser();

        public LispEnvironment Global
        {
            get { return global; }
            set { global = value; }
        }

        public Interpreter()
        {
            //global.Add(new LispSymbol("lambda"), new LispLambda());
            Load(typeof(Base));
        }

        public void Eval(char[] code)
        {
            dynamic elem = parser.parse(code.AsEnumerable().GetEnumerator());
            string str = elem.ToString();
            var res = elem.Eval(null, Global);
        }

        

        public void Load(Type builtIns)
        {
            ParameterExpression localEnv = Expression.Parameter(typeof(LispEnvironment), "env");
            ParameterExpression args = Expression.Parameter(typeof(dynamic[]), "args");

            var methodInfos = builtIns.GetMethods();//BindingFlags.Static);
            foreach (var methodInfo in methodInfos)
            {
                if (methodInfo.IsStatic)
                {
                    var method = Expression.Call(methodInfo, args, localEnv);

                    var lambda = Expression.Lambda<Func<dynamic[], LispEnvironment, dynamic>>(method, args, localEnv);

                    global.Add(new LispSymbol(methodInfo.Name), lambda.Compile());
                }
            }

        }
    }
}
