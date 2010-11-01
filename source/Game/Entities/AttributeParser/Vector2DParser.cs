using System.Globalization;
using System.Xml;
using Game.Utility;

namespace Game.Entities.AttributeParser
{

    class Vector2DParser : IAttributeParser
    {
        public string Type
        {
            get { return "Vector2D"; }
        }

        public object Parse(XmlNode node)
        {
            float x = float.Parse(node.SelectSingleNode("x").InnerText, NumberStyles.Number,
                        CultureInfo.InvariantCulture.NumberFormat);
            float y = float.Parse(node.SelectSingleNode("y").InnerText, NumberStyles.Number,
                CultureInfo.InvariantCulture.NumberFormat);

            return new Vector2D(x, y);
        }
    }

}
