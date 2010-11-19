using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LispInterpreter
{
    public class LispEnvironment
    {
        Dictionary<LispSymbol, dynamic> env
            = new Dictionary<LispSymbol,dynamic>();
        LispEnvironment parent;

        public LispEnvironment(LispEnvironment parent)
        {
            this.parent = parent;
        }

        public void Add(LispSymbol symbol, LispElement element)
        {
            env.Add(symbol, element);
        }

        public void Add(LispSymbol symbol, Func<dynamic[], LispEnvironment, dynamic> func)
        {
            env.Add(symbol, new LispBuiltInFunction(func, symbol.ToString()));
        }

        public LispElement Lookup(LispSymbol symbol)
        {
            if (env.ContainsKey(symbol))
            {
                return env[symbol];
            }
            else if (parent != null)
            {
                return parent.Lookup(symbol);
            }
            else
            {
                return null;
            }
        }

    }
}
