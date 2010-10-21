using System.Xml;

namespace Game.Entities.AttributeParser
{

    public interface IAttributeParser
    {
        string Type { get; }

        object Parse(XmlNode node);
    }

}
