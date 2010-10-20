using System.Xml;

namespace Game.Entities.AttributeParser
{

    class MaterialParser : IAttributeParser
    {
        public string Type
        {
            get { return "Material"; }
        }

        public object Parse(XmlNode node)
        {
            return null;
        }
    }

}
