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
            _collisionComponent = new CollisionComponent(new Rectangle(0, 0, 32, 32), _physicsComponent);
            InitializeAnimation();
        }

        private readonly TileMap _tileMap;
        private readonly PhysicsComponent _physicsComponent;
        private readonly CollisionComponent _collisionComponent;
        private AnimationController _animationController;

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
            get { return _animationController.SourceRectangle; }
        }
        
        public void Update(float dt) {
            GetInput();

            _physicsComponent.ApplyPhysics(dt);
            _collisionComponent.Move(dt, _tileMap);
            UpdateAnimation(dt);

            _physicsComponent.Movement = 0.0f;
        }

        private void UpdateAnimation(float dt) {
            if (_physicsComponent.IsJumping)
                _animationController.SetRunningAnimation("jump");
            else if (_physicsComponent.Movement > 0)
                _animationController.SetRunningAnimation("moveright");
            else if (_physicsComponent.Movement < 0)
                _animationController.SetRunningAnimation("moveleft");
            else
                _animationController.SetRunningAnimation("idle");
            _animationController.Update(dt);
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

        private void InitializeAnimation() {
            _animationController = new AnimationController();
            _animationController.AddAnimation("moveright", new Animation(0, 0, 32, 32, 3, 0.1f));
            _animationController.AddAnimation("moveleft", new Animation(0, 32, 32, 32, 3, 0.1f));
            _animationController.AddAnimation("jump", new Animation(160, 0, 32, 32, 2, 0.3f));
            _animationController.AddAnimation("idle", new Animation(0, 0, 32, 32, 1, 0.5f));
            _animationController.SetRunningAnimation("jump");
        }
    }
}
