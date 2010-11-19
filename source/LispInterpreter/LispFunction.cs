using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LispInterpreter
{
    class LispFunction : LispElement
    {
        private dynamic[] parameters;
        private LispElement body;
        private LispSymbol funcName;

        public LispFunction(dynamic parameters, LispElement body)
            : this((object)parameters, body, null)
        {
            
        }

        public LispFunction(dynamic parameters, LispElement body, LispSymbol funcName)
        {
            this.parameters = parameters.Elems;
            this.body = body;
            this.funcName = funcName;
        }

        public dynamic Eval(dynamic e, LispEnvironment env)
        {
            var par = parameters.AsEnumerable().GetEnumerator();
            var val = e.Enumerator;

            LispEnvironment childEnv = new LispEnvironment(env);

            // we need support for recursion
            if (funcName != null)
                childEnv.Add(funcName, this);

            foreach (var parameter in parameters)
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
            foreach (var parameter in parameters)
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
