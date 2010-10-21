using System;
using System.Collections.Generic;
using System.Xml;
using Game.Entities.AttributeParser;
using ResourceManagement;

namespace Game.Entities.AttributeLoader
{

    class DefaultAttributeLoader : IAttributeLoader
    {
        private List<IAttributeParser> parsers = new List<IAttributeParser>();

        private delegate object Parser(XmlNode node);
        private Dictionary<string, Parser> loaderMap = new Dictionary<string, Parser>();
        private ResourceManager resourceManager;

        public DefaultAttributeLoader(ResourceManager resourceManager)
        {
            this.resourceManager = resourceManager;
            parsers.Add(new Vector2DParser());
            parsers.Add(new ResourceParser(resourceManager));

            initializeLoaderMap();
        }

        private void initializeLoaderMap()
        {
            foreach (IAttributeParser p in parsers) {
                loaderMap.Add(p.Type, p.Parse);
            }
        }

        #region IAttributeLoader Members

        public object Load(XmlNode node)
        {
            if (!loaderMap.ContainsKey(node.Name)) {
                throw new NotSupportedException("No loader for attribute type: '" + node.Name + "' available");
            }

            return loaderMap[node.Name](node);
        }

        #endregion
    }

}
