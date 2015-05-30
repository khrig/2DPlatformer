using System;
using Gengine.Entities;
using Gengine.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace _2DPlatformer {
    public class PreCheckingPlayer : IPlayer {
        public PreCheckingPlayer(Vector2 position, TileMap tileMap) {
            Position = position;
            _tileMap = tileMap;
            _sourceRectangle = new Rectangle(0, 0, 32, 32);
            _isJumping = true;
            _moveBoundingBox = new Rectangle(0, 0, 32, 32);
        }

        private readonly TileMap _tileMap;

        private const float MoveAcceleration = 12500.0f;
        private const float MaxMoveSpeed = 1750.0f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.51f;

        private const float Gravity = 1400f;
        private const float Jump = 500f;

        public RenderType Type { get { return RenderType.Sprite; } }
        public string TextureName {
            get { return "player"; }
        }
        public string FontName { get { throw new NotImplementedException(); } }
        public string Text { get { throw new NotImplementedException(); } }
        public Color Color { get { throw new NotImplementedException(); } }
        public Vector2 RenderPosition {
            get { return Position; }
        }

        private readonly Rectangle _sourceRectangle;
        public Rectangle SourceRectangle {
            get { return _sourceRectangle; }
        }

        Vector2 _position;
        public Vector2 Position {
            get { return _position; }
            set { _position = value; }
        }

        Vector2 _velocity;
        public Vector2 Velocity {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public bool IsOnGround { get; private set; }

        private float _movement;
        private bool _isJumping;
        private bool _jump;

        public void Update(float dt) {
            GetInput();

            ApplyPhysics(dt);

            Move(dt);

            _movement = 0.0f;
        }

        private void GetInput() {
            KeyboardState keyboardState = Keyboard.GetState();
            if (//gamePadState.IsButtonDown(Buttons.DPadLeft) ||
                keyboardState.IsKeyDown(Keys.Left) ||
                keyboardState.IsKeyDown(Keys.A)) {
                _movement = -1.0f;
            } else if (//gamePadState.IsButtonDown(Buttons.DPadRight) ||
                       keyboardState.IsKeyDown(Keys.Right) ||
                       keyboardState.IsKeyDown(Keys.D)) {
                _movement = 1.0f;
            }

            _jump =
                //gamePadState.IsButtonDown(JumpButton) ||
                keyboardState.IsKeyDown(Keys.Space) ||
                keyboardState.IsKeyDown(Keys.Up) ||
                keyboardState.IsKeyDown(Keys.W);
        }

        private void ApplyPhysics(float dt) {
            // Base velocity is a combination of horizontal movement control and
            // acceleration downward due to gravity.
            _velocity.X += _movement * MoveAcceleration * dt;
            
            if (_jump && !_isJumping) {
                _velocity.Y = -Jump;
                _isJumping = true;
            }

            if (_isJumping) {
                _velocity.Y += Gravity * dt;
            }

            // Apply pseudo-drag horizontally.
            if (IsOnGround)
                _velocity.X *= GroundDragFactor;
            else
                _velocity.X *= AirDragFactor;

            // Prevent the player from running faster than his top speed.            
            _velocity.X = MathHelper.Clamp(_velocity.X, -MaxMoveSpeed, MaxMoveSpeed);
        }
        
        Rectangle _moveBoundingBox;
        private void Move(float dt) {
            int directionX = Math.Sign(_velocity.X);
            int directionY = Math.Sign(_velocity.Y);
            int newX = (int)Math.Floor(_position.X); // round down if float?
            int newY = (int)Math.Floor(_position.Y); // round down if float?

            _moveBoundingBox.X = newX;
            _moveBoundingBox.Y = newY;
            IsOnGround = false;
            _isJumping = true;

            // Move X first
            if (_velocity.X > 0) {
                for (int x = 1; x <= Math.Abs(_velocity.X * dt); x++) {
                    _moveBoundingBox.X += directionX;
                    Tile t1 = _tileMap.PositionToTile(_moveBoundingBox.Right, _moveBoundingBox.Top);
                    Tile t2 = _tileMap.PositionToTile(_moveBoundingBox.Right, _moveBoundingBox.Bottom);
                    if (t1.IsSolid && t1.BoundingBox.Intersects(_moveBoundingBox) || t2.IsSolid && t2.BoundingBox.Intersects(_moveBoundingBox)) {
                        // Add slope handling
                        // Reset velocity x if hit
                        break;
                    }
                    newX = _moveBoundingBox.X;
                }
            }
            if (_velocity.X < 0) {
                for (int x = 1; x <= Math.Abs(_velocity.X * dt); x++) {
                    _moveBoundingBox.X += directionX;
                    Tile t1 = _tileMap.PositionToTile(_moveBoundingBox.Left, _moveBoundingBox.Top);
                    Tile t2 = _tileMap.PositionToTile(_moveBoundingBox.Left, _moveBoundingBox.Bottom);
                    if (t1.IsSolid && t1.BoundingBox.Intersects(_moveBoundingBox) || t2.IsSolid && t2.BoundingBox.Intersects(_moveBoundingBox)) {
                        // Add slope handling
                        // Reset velocity x if hit
                        break;
                    }
                    newX = _moveBoundingBox.X;
                }
            }

            // Move Y
            if (_velocity.Y > 0) {
                for (int y = 1; y < Math.Abs(_velocity.Y * dt); y++) {
                    _moveBoundingBox.Y += directionY;
                    // Bottom tiles
                    Tile t1 = _tileMap.PositionToTile(newX, _moveBoundingBox.Bottom);
                    Tile t2 = _tileMap.PositionToTile(newX + _moveBoundingBox.Width - 1, _moveBoundingBox.Bottom);
                    if (t1.IsSolid && t1.BoundingBox.Intersects(_moveBoundingBox) || t2.IsSolid && t2.BoundingBox.Intersects(_moveBoundingBox)) {
                        // Add slope handling
                        // Reset velocity y if hit on bottom
                        // directionY == 1 ? Velocity.Y = 0
                        IsOnGround = true;
                        _isJumping = false;
                        break;
                    }
                    newY = _moveBoundingBox.Y;
                }
            }
            if (_velocity.Y < 0) {
                for (int y = 1; y < Math.Abs(_velocity.Y * dt); y++) {
                    _moveBoundingBox.Y += directionY;
                    // Bottom tiles
                    Tile t1 = _tileMap.PositionToTile(newX, _moveBoundingBox.Top);
                    Tile t2 = _tileMap.PositionToTile(newX + _moveBoundingBox.Width - 1, _moveBoundingBox.Top);
                    if (t1.IsSolid && t1.BoundingBox.Intersects(_moveBoundingBox) || t2.IsSolid && t2.BoundingBox.Intersects(_moveBoundingBox)) {
                        // Add slope handling
                        // Reset velocity y if hit on bottom
                        // directionY == 1 ? Velocity.Y = 0
                        //isJumping = false;
                        _velocity.Y = 0;
                        break;
                    }
                    newY = _moveBoundingBox.Y;
                }
            }
            
            // Update current position
            _position.X = newX;
            _position.Y = newY;
        }
    }
}
