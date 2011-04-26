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
        #region LispElement Members

        public dynamic Eval(dynamic e, LispEnvironment env)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
