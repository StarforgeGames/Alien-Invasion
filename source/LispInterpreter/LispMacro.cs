using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
