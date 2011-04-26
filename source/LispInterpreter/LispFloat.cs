using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LispInterpreter
{
    class LispFloat : LispElement
    {
        private double value;

        public double Value
        {
            get { return value; }
        }

        public LispFloat(double value)
        {
            // TODO: Complete member initialization
            this.value = value;
        }

        #region LispElement Members

        public dynamic Eval(dynamic e, LispEnvironment env)
        {
            return this;
        }

        #endregion

        public override string ToString()
        {
            return value.ToString();
        }

        public static LispFloat operator +(LispFloat c1, LispFloat c2)
        {
            return new LispFloat(c1.value + c2.value);
        }

        public static LispFloat operator -(LispFloat c1, LispFloat c2)
        {
            return new LispFloat(c1.value - c2.value);
        }

        public static LispFloat operator *(LispFloat c1, LispFloat c2)
        {
            return new LispFloat(c1.value * c2.value);
        }

        public static LispFloat operator /(LispFloat c1, LispFloat c2)
        {
            return new LispFloat(c1.value / c2.value);
        }
    }
}
