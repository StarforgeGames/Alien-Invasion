using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LispInterpreter
{
    class LispFloat : LispElement
    {
        private double value;

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
    }
}
