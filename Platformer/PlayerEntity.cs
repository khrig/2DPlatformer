using Gengine.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer {
    class PlayerEntity : Entity, IRenderable {
        public PlayerEntity(InputComponent input, VisualComponent visual, MovementComponent position, AnimationComponent animation) {
            // Order matters
            AddComponent(input);
            AddComponent(position);
            AddComponent(animation);
            AddComponent(visual);
        }

        public Texture2D Texture { get { return GetComponent<VisualComponent>().Texture; } }
        public Vector2 Position { get { return GetComponent<MovementComponent>().Position; } }
        public Rectangle SourceRectangle { get { return GetComponent<AnimationComponent>().SourceRectangle; } }
    }
}
