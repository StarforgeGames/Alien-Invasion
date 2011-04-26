using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LispInterpreter
{
    public class LispList : LispElement
    {
        List<dynamic> list;

        public void Add(dynamic e)
        {
            list.Add(e);
        }

        public dynamic[] Elems
        {
            get
            {
                return list.ToArray();
            }
        }

        public dynamic Eval(dynamic e, LispEnvironment env)
        {
            dynamic elem = First;

            return elem.Eval(null, env).Eval(Rest, env);

            /*if (First is LispSymbol)
            {
                elem = First.Eval(null, env);
            }
            
            if (isFunction(elem))
            {
                return elem.Eval(null, env).Eval(Rest, env);
            }
            else
            {
                return elem.Eval(Rest, env);
            }*/
        }

        private dynamic getParams(dynamic elem)
        {
            return elem.Rest.First;
        }

        private dynamic getBody(dynamic elem)
        {
            return elem.Rest.Rest.First;
        }

        private bool isFunction(dynamic elem)
        {
            if (elem is LispList)
            {
                return true;
                //return elem.First is LispLambda;
            }
            else
            { 
                return false;
            }
        }

        public dynamic First
        {
            get
            {
                return list.First();
            }
        }

        public dynamic Rest
        {
            get
            {
                return new LispList(list.GetRange(1, list.Count - 1));
            }
        }

        public LispList(List<dynamic> list)
        {
            this.list = list;
        }

        public LispList()
        {
            list = new List<dynamic>();
        }

        public IEnumerator<dynamic> Enumerator
        {
            get
            {
                return list.GetEnumerator();
            }
        }

        public override string ToString()
        {
            if (First is LispQuote)
            {
                return "'" + Rest.First.ToString();
            }
            StringBuilder str = new StringBuilder("(");
            var elem = Enumerator;

            bool first = true;
            while (elem.MoveNext())
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    str.Append(" ");
                }

                str.Append(elem.Current.ToString());
            }
            str.Append(")");

            return str.ToString();
        }

        public dynamic this [int index]
        {
            get
            {
                return list[index];
            }
        }
    }
}
