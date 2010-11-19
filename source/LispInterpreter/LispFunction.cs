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

        public LispFunction(dynamic parameters, LispElement body)
        {
            this.parameters = parameters.Elems;
            this.body = body;
        }

        public dynamic Eval(dynamic e, LispEnvironment env)
        {
            var par = parameters.AsEnumerable().GetEnumerator();
            var val = e.Enumerator;

            LispEnvironment childEnv = new LispEnvironment(env);

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
