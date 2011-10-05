using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LispInterpreter
{
    public class LispList : LispElement, IEnumerable<object>
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

        public dynamic Eval(LispEnvironment env, dynamic e = null)
        {
            dynamic elem = First;

            return elem.Eval(env).Eval(env, Rest);
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

        public override string ToString()
        {
            if (First is LispQuote)
            {
                return "'" + Rest.First.ToString();
            }
            StringBuilder str = new StringBuilder("(");
            var elem = GetEnumerator();

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
        /*
        public List<object>.Enumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }*/

        public IEnumerator<object> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
