using Gengine.Entities;
using Gengine.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DPlatformer {
    public class PreCheckingPlayer : IPlayer {
        public PreCheckingPlayer(Vector2 position, TileMap tileMap) {
            Position = position;
            this.tileMap = tileMap;
            sourceRectangle = new Rectangle(0, 0, 32, 32);
            isJumping = true;
            moveBoundingBox = new Rectangle(0, 0, 32, 32);
        }

        private readonly TileMap tileMap;

        private const float MoveAcceleration = 12500.0f;
        private const float MaxMoveSpeed = 1750.0f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.51f;

        private const float GRAVITY = 1400f;
        private const float JUMP = 500f;

        public RenderType Type { get { return RenderType.Sprite; } }
        public string TextureName {
            get { return "player"; }
        }
        public string FontName {
            get { return "player"; }
        }
        public Vector2 RenderPosition {
            get { return Position; }
        }

        private Rectangle sourceRectangle;
        public Rectangle SourceRectangle {
            get { return sourceRectangle; }
        }

        public Vector2 Position {
            get { return position; }
            set { position = value; }
        }
        Vector2 position;

        public Vector2 Velocity {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;

        public bool IsOnGround {
            get { return isOnGround; }
        }
        bool isOnGround;

        private float movement;
        private bool isJumping;
        private bool jump;

        public void Update(float dt) {
            GetInput();

            ApplyPhysics(dt);

            Move(dt);

            movement = 0.0f;
        }

        private void GetInput() {
            KeyboardState keyboardState = Keyboard.GetState();
            if (//gamePadState.IsButtonDown(Buttons.DPadLeft) ||
                keyboardState.IsKeyDown(Keys.Left) ||
                keyboardState.IsKeyDown(Keys.A)) {
                movement = -1.0f;
            } else if (//gamePadState.IsButtonDown(Buttons.DPadRight) ||
                       keyboardState.IsKeyDown(Keys.Right) ||
                       keyboardState.IsKeyDown(Keys.D)) {
                movement = 1.0f;
            }

            jump =
                //gamePadState.IsButtonDown(JumpButton) ||
                keyboardState.IsKeyDown(Keys.Space) ||
                keyboardState.IsKeyDown(Keys.Up) ||
                keyboardState.IsKeyDown(Keys.W);
        }

        private void ApplyPhysics(float dt) {
            // Base velocity is a combination of horizontal movement control and
            // acceleration downward due to gravity.
            velocity.X += movement * MoveAcceleration * dt;
            
            if (jump && !isJumping) {
                velocity.Y = -JUMP;
                isJumping = true;
            }

            if (isJumping) {
                velocity.Y += GRAVITY * dt;
            }

            // Apply pseudo-drag horizontally.
            if (IsOnGround)
                velocity.X *= GroundDragFactor;
            else
                velocity.X *= AirDragFactor;

            // Prevent the player from running faster than his top speed.            
            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);
        }
        
        Rectangle moveBoundingBox;
        private void Move(float dt) {
            int directionX = Math.Sign(velocity.X);
            int directionY = Math.Sign(velocity.Y);
            int newX = (int)Math.Floor(position.X); // round down if float?
            int newY = (int)Math.Floor(position.Y); // round down if float?

            moveBoundingBox.X = newX;
            moveBoundingBox.Y = newY;
            isOnGround = false;
            isJumping = true;

            // Move X first
            if (velocity.X > 0) {
                for (int x = 1; x <= Math.Abs(velocity.X * dt); x++) {
                    moveBoundingBox.X += directionX;
                    Tile t1 = tileMap.PositionToTile(moveBoundingBox.Right, moveBoundingBox.Top);
                    Tile t2 = tileMap.PositionToTile(moveBoundingBox.Right, moveBoundingBox.Bottom);
                    if (t1.IsSolid && t1.BoundingBox.Intersects(moveBoundingBox) || t2.IsSolid && t2.BoundingBox.Intersects(moveBoundingBox)) {
                        // Add slope handling
                        // Reset velocity x if hit
                        break;
                    }
                    newX = moveBoundingBox.X;
                }
            }
            if (velocity.X < 0) {
                for (int x = 1; x <= Math.Abs(velocity.X * dt); x++) {
                    moveBoundingBox.X += directionX;
                    Tile t1 = tileMap.PositionToTile(moveBoundingBox.Left, moveBoundingBox.Top);
                    Tile t2 = tileMap.PositionToTile(moveBoundingBox.Left, moveBoundingBox.Bottom);
                    if (t1.IsSolid && t1.BoundingBox.Intersects(moveBoundingBox) || t2.IsSolid && t2.BoundingBox.Intersects(moveBoundingBox)) {
                        // Add slope handling
                        // Reset velocity x if hit
                        break;
                    }
                    newX = moveBoundingBox.X;
                }
            }

            // Move Y
            if (velocity.Y > 0) {
                for (int y = 1; y < Math.Abs(velocity.Y * dt); y++) {
                    moveBoundingBox.Y += directionY;
                    // Bottom tiles
                    Tile t1 = tileMap.PositionToTile(newX, moveBoundingBox.Bottom);
                    Tile t2 = tileMap.PositionToTile(newX + moveBoundingBox.Width - 1, moveBoundingBox.Bottom);
                    if (t1.IsSolid && t1.BoundingBox.Intersects(moveBoundingBox) || t2.IsSolid && t2.BoundingBox.Intersects(moveBoundingBox)) {
                        // Add slope handling
                        // Reset velocity y if hit on bottom
                        // directionY == 1 ? Velocity.Y = 0
                        isOnGround = true;
                        isJumping = false;
                        break;
                    }
                    newY = moveBoundingBox.Y;
                }
            }
            if (velocity.Y < 0) {
                for (int y = 1; y < Math.Abs(velocity.Y * dt); y++) {
                    moveBoundingBox.Y += directionY;
                    // Bottom tiles
                    Tile t1 = tileMap.PositionToTile(newX, moveBoundingBox.Top);
                    Tile t2 = tileMap.PositionToTile(newX + moveBoundingBox.Width - 1, moveBoundingBox.Top);
                    if (t1.IsSolid && t1.BoundingBox.Intersects(moveBoundingBox) || t2.IsSolid && t2.BoundingBox.Intersects(moveBoundingBox)) {
                        // Add slope handling
                        // Reset velocity y if hit on bottom
                        // directionY == 1 ? Velocity.Y = 0
                        //isJumping = false;
                        velocity.Y = 0;
                        break;
                    }
                    newY = moveBoundingBox.Y;
                }
            }
            
            // Update current position
            position.X = newX;
            position.Y = newY;
        }
    }
}
