using System.Xml;

namespace Game.Entities.AttributeLoader
{

    interface IAttributeLoader
    {
        object Load(XmlNode node);
    }

}
