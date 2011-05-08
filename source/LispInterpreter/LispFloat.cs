using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LispInterpreter
{
    public class LispFloat : LispElement
    {
        private float value;

        public float Value
        {
            get { return value; }
        }

        public LispFloat(float value)
        {
            this.value = value;
        }

        #region LispElement Members

        public dynamic Eval(LispEnvironment env, dynamic e = null)
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
