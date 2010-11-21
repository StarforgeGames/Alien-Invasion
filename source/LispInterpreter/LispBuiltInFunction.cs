using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LispInterpreter
{
    class LispBuiltInFunction : LispElement
    {
        Func<LispList, LispEnvironment, dynamic> func;
        string funcName;

        public LispBuiltInFunction(Func<LispList, LispEnvironment, dynamic> func, string funcName)
        {
            this.funcName = "built-in-" + funcName;
            this.func = func;
        }

        public dynamic Eval(dynamic args, LispEnvironment env)
        {
            return func(args, env);
        }

        public override string ToString()
        {
            return funcName;
        }

    }
}
