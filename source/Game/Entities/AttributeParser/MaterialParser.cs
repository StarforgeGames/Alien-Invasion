using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Game.Entities.AttributeParser
{

    static class MaterialParser
    {
        public static string Type
        {
            get { return "Material"; }
        }

        public static object Parse(XmlNode node)
        {
            return null;
        }
    }

}
