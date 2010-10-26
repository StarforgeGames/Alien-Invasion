using System.Xml;
using Game.Entities.AttributeParser;

namespace Game.Entities.AttributeLoader
{

    interface IAttributeLoader
    {
        object Load(XmlNode node);
    }

}
