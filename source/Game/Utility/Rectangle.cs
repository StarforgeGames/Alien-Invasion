namespace Game.Utility
{

    class Rectangle
    {
        public float Left
        {
            get { return Position.X; }
        }
        public float Right
        {
            get { return Position.X + Width; }
        }
        public float Top
        {
            get { return Position.Y; }
        }
        public float Bottom
        {
            get { return Position.Y + Height; }
        }

        public Vector2D Position { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public Rectangle(Vector2D position, float width, float height)
        {
            this.Position = position;
            this.Width = width;
            this.Height = height;
        }
    }

}
