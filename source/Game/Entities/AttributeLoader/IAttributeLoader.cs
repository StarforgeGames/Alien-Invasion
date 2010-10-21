using System.Xml;
using Game.Entities.AttributeParser;

namespace Game.Entities.AttributeLoader
{

    interface IAttributeLoader
    {
        object Load(XmlNode node);
        void Add(IAttributeParser parser);
        void Remove(string type);
    }

}
