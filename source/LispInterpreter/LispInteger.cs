using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LispInterpreter
{
    class LispInteger : LispElement
    {
        private int value;

        public LispInteger(int value)
        {
            // TODO: Complete member initialization
            this.value = value;
        }

        public dynamic Eval(dynamic e, LispEnvironment env)
        {
            return this;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public static LispInteger operator +(LispInteger c1, LispInteger c2)
        {
            return new LispInteger(c1.value + c2.value);
        }
    }
}
