using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Device = SlimDX.Direct3D10.Device;
using SlimDX.DXGI;
using Graphics.Primitives;

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
            ScreenObjects.Add(new XPlane());
        }

        public void Render(float deltaTime)
        {
            foreach (IRenderable obj in ScreenObjects) {
                obj.Render();
            }
        }
    }

}
