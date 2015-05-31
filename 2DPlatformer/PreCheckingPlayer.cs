using System;
using Gengine.Entities;
using Gengine.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace _2DPlatformer {
    public class PreCheckingPlayer : IPlayer {
        public PreCheckingPlayer(Vector2 position, TileMap tileMap) {
            _physicsComponent = new PhysicsComponent(position);
            _tileMap = tileMap;
            _moveWithCollisionComponent = new MoveWithCollisionComponent(new Rectangle(0, 0, 32, 32), _physicsComponent);
            _animationComponent = new Animation("player", 32, 32);
        }

        private readonly TileMap _tileMap;
        private readonly PhysicsComponent _physicsComponent;
        private readonly MoveWithCollisionComponent _moveWithCollisionComponent;
        private readonly Animation _animationComponent;

        public RenderType Type { get { return RenderType.Sprite; } }
        public string TextureName {
            get { return "player"; }
        }
        public string FontName { get { throw new NotImplementedException(); } }
        public string Text { get { throw new NotImplementedException(); } }
        public Color Color { get { throw new NotImplementedException(); } }
        public Vector2 RenderPosition {
            get { return _physicsComponent.Position; }
        }

        public Rectangle SourceRectangle {
            get { return _animationComponent.SourceRectangle; }
        }
        
        public void Update(float dt) {
            GetInput();

            _physicsComponent.ApplyPhysics(dt);
            _moveWithCollisionComponent.Move(dt, _tileMap);
            _animationComponent.Update(dt);

            _physicsComponent.Movement = 0.0f;
        }
        
        private void GetInput() {
            KeyboardState keyboardState = Keyboard.GetState();
            if (//gamePadState.IsButtonDown(Buttons.DPadLeft) ||
                keyboardState.IsKeyDown(Keys.Left) ||
                keyboardState.IsKeyDown(Keys.A)) {
                    _physicsComponent.Movement = -1.0f;
            } else if (//gamePadState.IsButtonDown(Buttons.DPadRight) ||
                       keyboardState.IsKeyDown(Keys.Right) ||
                       keyboardState.IsKeyDown(Keys.D)) {
                           _physicsComponent.Movement = 1.0f;
            }

            _physicsComponent.WantToJump =
                //gamePadState.IsButtonDown(JumpButton) ||
                keyboardState.IsKeyDown(Keys.Space) ||
                keyboardState.IsKeyDown(Keys.Up) ||
                keyboardState.IsKeyDown(Keys.W);
        }
    }
}
