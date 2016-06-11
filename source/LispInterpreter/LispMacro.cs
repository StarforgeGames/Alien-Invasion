using System;

namespace LispInterpreter
{
    class LispMacro : LispElement
    {

        public LispMacro(LispList parameters, LispElement body, LispEnvironment funcEnv)
        {
            throw new NotImplementedException();
        }

        public dynamic Eval(LispEnvironment env, dynamic e = null)
        {
            throw new NotImplementedException();
        }
    }
}
