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
            velocity.Y = 100;
        }

        private readonly TileMap tileMap;

        private const float MoveAcceleration = 13000.0f;
        private const float MaxMoveSpeed = 1750.0f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;

        private const float GRAVITY = 400f;
        private const float JUMP = 250f;

        public string TextureName {
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
            Vector2 previousPosition = Position;

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

        private void Move(float dt) {
            int directionX = Math.Sign(velocity.X);
            int directionY = Math.Sign(velocity.Y);
            int newX = (int)Math.Floor(position.X); // round down if float?
            int newY = (int)Math.Floor(position.Y); // round down if float?

            Rectangle boundingBox = new Rectangle(newX, newY, 32, 32);
            isOnGround = false;

            // Move X first
            if (velocity.X > 0) {
                for (int x = 1; x <= Math.Abs(velocity.X * dt); x++) {
                    boundingBox.X += directionX;
                    Tile t1 = tileMap.PositionToTile(boundingBox.Right, boundingBox.Top);
                    Tile t2 = tileMap.PositionToTile(boundingBox.Right, boundingBox.Bottom);
                    if (t1.IsSolid && t1.BoundingBox.Intersects(boundingBox) || t2.IsSolid && t2.BoundingBox.Intersects(boundingBox)) {
                        // Add slope handling
                        // Reset velocity x if hit
                        break;
                    }
                    newX = boundingBox.X;
                }
            }
            if (velocity.X < 0) {
                for (int x = 1; x <= Math.Abs(velocity.X * dt); x++) {
                    boundingBox.X += directionX;
                    Tile t1 = tileMap.PositionToTile(boundingBox.Left, boundingBox.Top);
                    Tile t2 = tileMap.PositionToTile(boundingBox.Left, boundingBox.Bottom);
                    if (t1.IsSolid && t1.BoundingBox.Intersects(boundingBox) || t2.IsSolid && t2.BoundingBox.Intersects(boundingBox)) {
                        // Add slope handling
                        // Reset velocity x if hit
                        break;
                    }
                    newX = boundingBox.X;
                }
            }

            // Move Y
            if (velocity.Y > 0) {
                for (int y = 1; y < Math.Abs(velocity.Y * dt); y++) {
                    boundingBox.Y += directionY;
                    // Bottom tiles
                    Tile t1 = tileMap.PositionToTile(newX, boundingBox.Bottom);
                    Tile t2 = tileMap.PositionToTile(newX + boundingBox.Width - 1, boundingBox.Bottom);
                    if (t1.IsSolid && t1.BoundingBox.Intersects(boundingBox) || t2.IsSolid && t2.BoundingBox.Intersects(boundingBox)) {
                        // Add slope handling
                        // Reset velocity y if hit on bottom
                        // directionY == 1 ? Velocity.Y = 0
                        isOnGround = true;
                        isJumping = false;
                        break;
                    }
                    newY = boundingBox.Y;
                }
            }
            if (velocity.Y < 0) {
                for (int y = 1; y < Math.Abs(velocity.Y * dt); y++) {
                    boundingBox.Y += directionY;
                    // Bottom tiles
                    Tile t1 = tileMap.PositionToTile(newX, boundingBox.Top);
                    Tile t2 = tileMap.PositionToTile(newX + boundingBox.Width - 1, boundingBox.Top);
                    if (t1.IsSolid && t1.BoundingBox.Intersects(boundingBox) || t2.IsSolid && t2.BoundingBox.Intersects(boundingBox)) {
                        // Add slope handling
                        // Reset velocity y if hit on bottom
                        // directionY == 1 ? Velocity.Y = 0
                        //isJumping = false;
                        break;
                    }
                    newY = boundingBox.Y;
                }
            }
            
            // Update current position
            position.X = newX;
            position.Y = newY;

            /*
            // Apply velocity.
            Position += velocity * dt;
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));
             * */
        }
    }
}
