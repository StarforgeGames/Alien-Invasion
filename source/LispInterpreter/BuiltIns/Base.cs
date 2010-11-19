using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LispInterpreter.BuiltIns
{
    static class Base
    {
        static public dynamic first(dynamic[] args, LispEnvironment env)
        {
            return args[0].Eval(null, env).First;
        }

        static public dynamic rest(dynamic[] args, LispEnvironment env)
        {
            return args[0].Eval(null, env).Rest;
        }

        static public dynamic quote(dynamic[] args, LispEnvironment env)
        {
            return args[0];
        }

        static public dynamic label(dynamic[] args, LispEnvironment env)
        {
            dynamic func = args[1].Rest;
            return new LispFunction(func.First, func.Rest.First, args[0]);
        }

        static public dynamic lambda(dynamic[] args, LispEnvironment env)
        {
            return new LispFunction(args[0], args[1]);
        }

        [Alias("+")]
        static public dynamic add(dynamic[] args, LispEnvironment env)
        {
            return args[0].Eval(null, env) + args[1].Eval(null, env);
        }

        [Alias("-")]
        static public dynamic sub(dynamic[] args, LispEnvironment env)
        {
            return args[0].Eval(null, env) - args[1].Eval(null, env);
        }

        [Alias("*")]
        static public dynamic mul(dynamic[] args, LispEnvironment env)
        {
            return args[0].Eval(null, env) * args[1].Eval(null, env);
        }

        [Alias("/")]
        static public dynamic div(dynamic[] args, LispEnvironment env)
        {
            return args[0].Eval(null, env) / args[1].Eval(null, env);
        }
    }
}
