using Gengine.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Platformer {
    class PlayerEntity : Entity, IRenderable, ICollidable {
        public PlayerEntity(InputComponent input, VisualComponent visual, MovementComponent position, AnimationComponent animation) {
            // Order matters
            AddComponent(input);
            AddComponent(position);
            AddComponent(animation);
            AddComponent(visual);

            boundingBox = new Rectangle();
            boundingBox.Width = GetComponent<AnimationComponent>().SourceRectangle.Width;
            boundingBox.Height = GetComponent<AnimationComponent>().SourceRectangle.Height;
        }

        public Texture2D Texture { get { return GetComponent<VisualComponent>().Texture; } }
        public Vector2 Position { get { return GetComponent<MovementComponent>().Position; } }
        public Rectangle SourceRectangle { get { return GetComponent<AnimationComponent>().SourceRectangle; } }

        public string Identifier
        {
            get { return "player"; }
        }

        public void Collide(ICollidable target) {
            var movement = GetComponent<MovementComponent>();
            if (target.Identifier == "ground" && !movement.IsOnGround) {
                movement.IsOnGround = true;
                movement.CorrectPositionAfterCollision(target);
            }
        }

        private Rectangle boundingBox;
        public Rectangle BoundingBox {
            get {
                boundingBox.X = (int)GetComponent<MovementComponent>().Position.X;
                boundingBox.Y = (int)GetComponent<MovementComponent>().Position.Y;
                return boundingBox;
            }
        }
    }
}
