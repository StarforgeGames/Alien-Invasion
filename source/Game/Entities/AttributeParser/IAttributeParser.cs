using System.Xml;

namespace Game.Entities.AttributeParser
{

    interface IAttributeParser
    {
        string Type { get; }

        object Parse(XmlNode node);
    }

}
