namespace LispInterpreter
{
    public interface LispElement
    {
        dynamic Eval(LispEnvironment env, dynamic e = null);
    }
}
