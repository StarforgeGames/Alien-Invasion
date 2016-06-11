namespace LispInterpreter
{
    public class LispDouble : LispElement
    {
        private double value;

        public double Value
        {
            get { return value; }
        }

        public LispDouble(double value)
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

        public static LispDouble operator +(LispDouble c1, LispDouble c2)
        {
            return new LispDouble(c1.value + c2.value);
        }

        public static LispDouble operator -(LispDouble c1, LispDouble c2)
        {
            return new LispDouble(c1.value - c2.value);
        }

        public static LispDouble operator *(LispDouble c1, LispDouble c2)
        {
            return new LispDouble(c1.value * c2.value);
        }

        public static LispDouble operator /(LispDouble c1, LispDouble c2)
        {
            return new LispDouble(c1.value / c2.value);
        }
    }
}
