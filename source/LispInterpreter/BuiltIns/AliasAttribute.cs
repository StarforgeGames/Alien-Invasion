using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LispInterpreter.BuiltIns
{

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    sealed class AliasAttribute : Attribute
    {
        readonly string alias;

        public AliasAttribute(string alias)
        {
            this.alias = alias;
        }

        public string Alias
        {
            get { return alias; }
        }
    }
}
