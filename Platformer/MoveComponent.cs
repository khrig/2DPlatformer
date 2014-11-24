using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gengine.Entities;

namespace Platformer {
    class MoveComponent : EntityComponent {
        public MoveComponent(Entity entity) : base(entity) {}
        public override void Update(float deltaTime) {
            throw new NotImplementedException();
        }
    }
}
