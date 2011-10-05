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
            builtInFunctions.Load(builtIns);
        }
    }
}
