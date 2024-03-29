﻿using System.Collections.Generic;
using ResourceManagement;
using SlimDX;

namespace Graphics
{
    public class RenderObjects
    {
        public Dictionary<ResourceHandle, Dictionary<ResourceHandle, List<RenderObject>>> Objs = new Dictionary<ResourceHandle,Dictionary<ResourceHandle,List<RenderObject>>>();

        public Matrix Camera { get; set; }

        internal void Clear()
        {
            Objs.Clear();
        }

        public void Add(RenderObject obj)
        {
            Dictionary<ResourceHandle, List<RenderObject>> curObj;
            if (!Objs.ContainsKey(obj.mesh))
            {
                curObj = new Dictionary<ResourceHandle, List<RenderObject>>();
                Objs.Add(obj.mesh, curObj);
            }
            else
            {
                curObj = Objs[obj.mesh];
            }
            List<RenderObject> curRender;
            if (!curObj.ContainsKey(obj.material))
            {
                curRender = new List<RenderObject>();
                curObj.Add(obj.material, curRender);
            }
            else
            {
                curRender = curObj[obj.material];
            }
            curRender.Add(obj);
        }
    }
}
