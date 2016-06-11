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
