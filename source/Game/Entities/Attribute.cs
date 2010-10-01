using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Entities
{

    class Attribute<T> : IAttribute
    {
        public T Value { get; set; }

        public Attribute(T value)
        {
            Value = value;
        }
    }

}
