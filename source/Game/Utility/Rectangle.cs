namespace Game.Utility
{

    public class Rectangle
    {
        public Vector2D Position { get; set; }
        public Vector2D Dimensions { get; set; }

        public float Left
        {
            get { return Position.X; }
        }
        public float Right
        {
            get { return Position.X + Dimensions.X; }
        }
        public float Top
        {
            get { return Position.Y; }
        }
        public float Bottom
        {
            get { return Position.Y + Dimensions.Y; }
        }

        public float Width 
        {
            get { return Dimensions.X; }
        }
        public float Height
        {
            get { return Dimensions.Y; }
        }
        public Rectangle(float x, float y, float width, float height)
        {
            Position = new Vector2D(x, y);
            Dimensions = new Vector2D(width, height);
        }

        public Rectangle(Vector2D position, Vector2D dimensions)
        {
            Position = position;
            Dimensions = dimensions;
        }
    }

}
