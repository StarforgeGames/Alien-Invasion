using System.Collections.Generic;
using Game;
using Graphics.Primitives;
using Device = SlimDX.Direct3D10.Device;
using Resource = Game.Resources.Resource;

namespace Graphics
{

    public class Renderer
    {
        public static Device Device { get; set; }
        public List<IRenderable> ScreenObjects { get; set; }

        public Renderer(Device device)
        {
            Renderer.Device = device;
            ScreenObjects = new List<IRenderable>();

            // TODO: Makeshift solution. Needs to come from BaseGame somehow.
            Resource sprite = BaseGame.Resources.GetResource(@"Gfx\player.png");
            ScreenObjects.Add(new XPlane(sprite));
        }

        public void Render(float deltaTime)
        {
            foreach (IRenderable obj in ScreenObjects) {
                obj.Render();
            }
        }
    }

}
