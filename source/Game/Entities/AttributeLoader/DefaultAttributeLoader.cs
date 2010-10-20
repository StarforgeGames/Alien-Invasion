using System;
using System.Collections.Generic;
using System.Xml;
using Game.Entities.AttributeParser;

namespace Game.Entities.AttributeLoader
{

    class DefaultAttributeLoader : IAttributeLoader
    {
        private List<IAttributeParser> parsers = new List<IAttributeParser>();

        private delegate object Parser(XmlNode node);
        private Dictionary<string, Parser> loaderMap = new Dictionary<string, Parser>();

        public DefaultAttributeLoader()
        {
            parsers.Add(new MaterialParser());
            parsers.Add(new Vector2DParser());

            initializeLoaderMap();
        }

        private void initializeLoaderMap()
        {
            foreach (IAttributeParser p in parsers) {
                loaderMap.Add(p.Type, p.Parse);
            }
        }
        
        public object Load(XmlNode node)
        {
            if (!loaderMap.ContainsKey(node.Name)) {
                throw new NotSupportedException("No loader for attribute type: '" + node.Name + "' available");
            }

            return loaderMap[node.Name](node);
        }
    }

}
