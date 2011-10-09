using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LispInterpreter
{
    class LispNil : LispElement
    {
        public dynamic Eval(LispEnvironment env, dynamic e = null)
        {
            return this;
        }
        public override string ToString()
        {
            return "nil";
        }
    }
}
