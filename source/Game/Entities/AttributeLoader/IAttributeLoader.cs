using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Game.Entities.AttributeLoader
{

    interface IAttributeLoader
    {
        object Load(XmlNode node);
    }

}
