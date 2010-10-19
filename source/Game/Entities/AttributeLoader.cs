using System;
using System.Collections.Generic;
using System.Xml;
using Game.Entities.AttributeParser;

namespace Game.Entities
{

    static class AttributeLoader
    {
        private delegate object Parser(XmlNode node);
        private static Dictionary<string, Parser> loader = new Dictionary<string, Parser>();

        static AttributeLoader()
        {
            loader.Add(Vector2DParser.Type, Vector2DParser.Parse);
        }
        
        public static object Load(XmlNode node)
        {
            if (!loader.ContainsKey(node.Name)) {
                throw new NotSupportedException("No loader for attribute type: '" + node.Name + "' available");
            }

            return loader[node.Name].Invoke(node);
        }
    }

}
