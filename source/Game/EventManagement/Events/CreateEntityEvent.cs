using System.Collections.Generic;
using System.Text;

namespace Game.EventManagement.Events
{

    public class CreateEntityEvent : Event
    {
        // Event Message Types
        public const string CREATE_ENTITY = "create_entity";

        public string EntityType { get; private set; }
        public Dictionary<string, object> Attributes { get; private set; }

        public CreateEntityEvent(string type, string entityType)
            : base(type)
        {
            this.EntityType = entityType;
            Attributes = new Dictionary<string, object>();
        }

        public static CreateEntityEvent New(string entityType)
        {
            return new CreateEntityEvent(CREATE_ENTITY, entityType);
        }

        /// <summary>
        /// Adds a new attribute to this entity.
        /// </summary>
        /// <param name="value">Value of the new attribute</param>
        /// <returns>Unique identifier for this attribute</returns>
        public void AddAttribute(string key, object value)
        {
            Attributes.Add(key, value);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, object> pair in Attributes)
            {
                builder.Append("{" + pair.Key + ", " + pair.Value + "}, ");
            }

            builder.Remove(builder.Length - 2, 2);

            return base.ToString() + " [Attributes: " + builder.ToString() + "]";
        }
    }

}
