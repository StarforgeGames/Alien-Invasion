using System;

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

        public dynamic Eval(LispEnvironment env, dynamic args = null)
        {
            return func(args, env);
        }

        public override string ToString()
        {
            return funcName;
        }

    }
}
