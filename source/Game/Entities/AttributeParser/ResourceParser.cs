using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Entities.AttributeParser;
using ResourceManagement;

namespace SpaceInvaders.Views
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
            get { return "resource"; }
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
