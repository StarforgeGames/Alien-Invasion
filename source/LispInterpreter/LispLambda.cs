using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LispInterpreter
{
    /*class LispLambda : LispElement
    {
        #region LispElement Members

        public dynamic Eval(dynamic e, LispEnvironment env)
        {
            LispList list = (LispList)e;
            LispList parameters = (LispList)list.First;
            LispList body = (LispList)(((LispList)list.Rest).First);
            return new LispFunction(parameters, body);
        }

        #endregion
    }*/
}
