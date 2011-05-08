using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LispInterpreter
{
    public interface LispElement
    {
        dynamic Eval(LispEnvironment env, dynamic e = null);
    }
}
