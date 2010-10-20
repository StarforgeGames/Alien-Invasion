namespace Game.Entities
{

    public class Attribute<T>
    {
        public T Value { get; set; }

        public Attribute(T value)
        {
            this.Value = value;
        }

        public static implicit operator T(Attribute<T> attribute)
        {
            return attribute.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

}
