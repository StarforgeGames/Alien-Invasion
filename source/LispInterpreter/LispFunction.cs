using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LispInterpreter
{
    class LispFunction : LispElement
    {
        private LispList parameters;
        private LispElement body;
        private LispSymbol funcName;

        public LispFunction(LispList parameters, LispElement body)
            : this(parameters, body, null)
        {
            
        }

        public LispFunction(LispList parameters, LispElement body, LispSymbol funcName)
        {
            this.parameters = parameters;
            this.body = body;
            this.funcName = funcName;
        }

        public dynamic Eval(dynamic e, LispEnvironment env)
        {
            var val = e.Enumerator;

            LispEnvironment childEnv = new LispEnvironment(env);

            // we need support for recursion
            if (funcName != null)
                childEnv.Add(funcName, this);

            foreach (var parameter in parameters.Elems)
            {
                val.MoveNext();
                childEnv.Add(parameter, val.Current.Eval(null, env));
            }

            return body.Eval(null, childEnv);
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder("(lambda (");
            bool first = true;
            foreach (var parameter in parameters.Elems)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    str.Append(" ");
                }
                str.Append(parameter.ToString());
            }
            str.Append(") ");
            str.Append(body.ToString());
            str.Append(")");

            return str.ToString();
        }
    }
}
