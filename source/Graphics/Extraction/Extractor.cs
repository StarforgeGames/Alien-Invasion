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
            var bounds = entity["Bounds"] as Attribute<Rectangle>;

            frontObjects.Add(new RenderObject(
                material.Value, mesh.Value, 
                convertToRelative(position.Value),
                convertToRelative(bounds.Value.Dimensions)));
        }

        private Vector2 convertToRelative(Vector2D vec)
        {
            return new Vector2(vec.X / game.WorldWidth, vec.Y / game.WorldHeight);
        }

        public void OnUpdate(float deltaTime)
        {
            if (ExtractNext)
            {
                frontObjects.Clear();
                frontObjects.SetCamera(new Matrix());
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
