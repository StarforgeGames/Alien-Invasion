using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResourceManagement;

namespace Game.Entities.AttributeParser
{
    public class ResourceParser : IAttributeParser
    {
        private ResourceManager resourceManager;

        public ResourceParser(ResourceManager resourceManager)
        {
            this.resourceManager = resourceManager;

        }

        #region IAttributeParser Members

        public string Type
        {
            get { return "Resource"; }
        }

        public object Parse(System.Xml.XmlNode node)
        {
            string type = node.Attributes["type"].Value;
            string name = node.Attributes["name"].Value;
            return resourceManager.GetResource(name, type);
        }

        #endregion
    }
}
