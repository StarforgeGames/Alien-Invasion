using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LispInterpreter
{
    class LispNil : LispElement
    {
        public dynamic Eval(dynamic e, LispEnvironment env)
        {
            return this;
        }
        public override string ToString()
        {
            return "nil";
        }
    }
}
