using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LispInterpreter
{
    public class LispSymbol : LispElement, IEquatable<LispSymbol>
    {
        private string p;

        public LispSymbol(string p)
        {
            this.p = p;
        }

        public dynamic Eval(dynamic e, LispEnvironment env)
        {
            return env.Lookup(this);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LispSymbol);
        }
        public bool Equals(LispSymbol other)
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
            return p;
        }

        public string Value
        {
            get { return p; }
        }
    }
}
