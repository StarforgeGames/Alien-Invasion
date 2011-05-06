using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LispInterpreter
{
    public class LispString : LispElement, IEquatable<LispString>
    {
        private string p;

        public string Value
        {
            get { return p; }
        }

        public LispString(string p)
        {
            this.p = p;
        }

        public dynamic Eval(dynamic e, LispEnvironment env)
        {
            return this;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LispString);
        }
        public bool Equals(LispString other)
        {
            if (other == null)
            {
                return false;
            }
            else
            {
                return this.p == other.p;
            }
        }

        public override int GetHashCode()
        {
            return p.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder("\"");
            str.Append(p);
            str.Append("\"");
            return str.ToString();
        }

        public static LispString operator +(LispString c1, LispString c2)
        {
            return new LispString(c1.p + c2.p);
        }
    }
  
}
