using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResourceManagement;
using Game.Entities;
using SlimDX;
using Game.Utility;

namespace Graphics
{
    public class Extractor
    {
        private Game.GameLogic game;

        public RenderObjects Scene
        {
            get
            {
                return backObjects;
            }
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
            var material = entity["Material"] as Attribute<ResourceHandle>;
            var mesh = entity["Mesh"] as Attribute<ResourceHandle>;
            var position = entity["Position"] as Attribute<Vector2D>;
            var dimensions = entity["Dimensions"] as Attribute<Vector2D>;

            Matrix mat = new Matrix();
            mat.M11 = dimensions.Value.X;
            mat.M22 = dimensions.Value.Y;
            mat.M33 = 1.0f;
            mat.M44 = 1.0f;
            mat.M14 = position.Value.X;
            mat.M24 = position.Value.Y;
            

            frontObjects.Add(new RenderObject(
                material.Value, mesh.Value, 
                mat));
        }


        public void OnUpdate(float deltaTime)
        {
            if (ExtractNext)
            {
                frontObjects.Clear();
                frontObjects.Camera = Matrix.OrthoOffCenterLH(0.0f, game.WorldWidth, 0.0f, game.WorldHeight, 0.0f, 1.0f);
                
                foreach (var GameObject in game.Entities)
                {
                    if (GameObject.Value["IsRenderable"] != null)
                    {
                        ExtractSingle(GameObject.Value);
                    }
                }
                swap();
            }
        }
    }
}
