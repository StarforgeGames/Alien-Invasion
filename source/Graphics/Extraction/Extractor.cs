using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResourceManagement;
using Game.Entities;
using SlimDX;
using Game.Utility;
using Game.Behaviors;

namespace Graphics
{
    public class Extractor
    {
        private Game.GameLogic game;

        public RenderObjects Scene
        {
            get { return backObjects; }
        }

        private RenderObjects frontObjects = new RenderObjects(),
            backObjects = new RenderObjects();

        private void swap()
        {
            backObjects = frontObjects;
            frontObjects = new RenderObjects();
        }

        public Extractor(Game.GameLogic game)
        {
            this.game = game;
        }

        public bool ExtractNext
        {
            private get;
            set;
        }

        public void ExtractSingle(Entity entity)
        {
            ResourceHandle material = entity[RenderBehavior.Key_Material];
            ResourceHandle mesh = entity[RenderBehavior.Key_Mesh];
            Vector2D position = entity[SpatialBehavior.Key_Position];
            Vector2D dimensions = entity[SpatialBehavior.Key_Dimensions];
            float frame = entity[RenderBehavior.Key_Frame];

            Matrix mat = new Matrix();
            mat.M11 = dimensions.X;
            mat.M22 = dimensions.Y;
            mat.M33 = 1.0f;
            mat.M44 = 1.0f;
            mat.M41 = position.X;
            mat.M42 = position.Y;

            frontObjects.Add(
                new RenderObject(material, mesh, mat, frame));
        }


        public void OnUpdate(float deltaTime)
        {
            if (ExtractNext)
            {
                frontObjects.Clear();
                var view = Matrix.LookAtLH(
                    new Vector3(game.World.Width/2.0f,game.World.Height/2.0f, -1.0f),
                    new Vector3(game.World.Width / 2.0f, game.World.Height / 2.0f, 0.0f), 
                    Vector3.UnitY);
                
                var projection = Matrix.OrthoLH(game.World.Width, game.World.Height, 0.0f, 1000.0f);

                frontObjects.Camera = view * projection;

                foreach (var GameObject in game.World.Entities)
                {
                    if (GameObject.Value[RenderBehavior.Key_IsRenderable] != null) 
                    {
                        ExtractSingle(GameObject.Value);
                    }
                }
                swap();
            }
        }
    }
}
