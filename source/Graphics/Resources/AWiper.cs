﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphics.Resources
{
    public abstract class AWiper
    {
        protected Dictionary<string, Dictionary<string, ResourceHandle>> resources;
        public void SetResources(Dictionary<string, Dictionary<string, ResourceHandle>> resources)
        {
            this.resources = resources;
        }

        public abstract void Start();
        public abstract void Stop();
    }
}