using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LispInterpreter
{
    struct LispQuote : LispElement
    {
        public dynamic Eval(dynamic e, LispEnvironment env)
        {
            if (e == null)
            {
                return this;
            }
            else
            {
                return e.First;
            }
        }

        public override string ToString()
        {
            return "'";
        }
    }
}
