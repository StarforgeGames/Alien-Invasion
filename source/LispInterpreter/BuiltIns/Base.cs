using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LispInterpreter.BuiltIns
{
    static class Base
    {
        static public dynamic first(LispList args, LispEnvironment env)
        {
            return args[0].Eval(null, env).First;
        }

        static public dynamic rest(LispList args, LispEnvironment env)
        {
            return args[0].Eval(null, env).Rest;
        }

        static public dynamic quote(LispList args, LispEnvironment env)
        {
            return args[0];
        }

        static public dynamic label(LispList args, LispEnvironment env)
        {
            dynamic func = args[1].Rest;
            return new LispFunction(func.First, func.Rest.First, env, args[0]);
        }

        static public dynamic lambda(LispList args, LispEnvironment env)
        {
            return new LispFunction(args[0], args[1], env);
        }

        [Alias("+")]
        static public dynamic add(LispList args, LispEnvironment env)
        {
            return args[0].Eval(null, env) + args[1].Eval(null, env);
        }

        [Alias("-")]
        static public dynamic sub(LispList args, LispEnvironment env)
        {
            return args[0].Eval(null, env) - args[1].Eval(null, env);
        }

        [Alias("*")]
        static public dynamic mul(LispList args, LispEnvironment env)
        {
            return args[0].Eval(null, env) * args[1].Eval(null, env);
        }

        [Alias("/")]
        static public dynamic div(LispList args, LispEnvironment env)
        {
            return args[0].Eval(null, env) / args[1].Eval(null, env);
        }

        static public dynamic eval(LispList args, LispEnvironment env)
        {
            return args[0].Eval(null, env).Eval(null, env);
        }

        static public dynamic objTest(LispList args, LispEnvironment env)
        {
            return new LispObject("hallo welt".ToCharArray());
        }
    }
}
