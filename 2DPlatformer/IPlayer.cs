using Gengine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DPlatformer {
    internal interface IPlayer : IRenderable {
        void Update(float deltaTime);
    }
}
