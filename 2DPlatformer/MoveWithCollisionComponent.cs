using System;
using Gengine.Map;
using Microsoft.Xna.Framework;

namespace _2DPlatformer
{
    public class MoveWithCollisionComponent
    {
        private readonly PhysicsComponent _physicsComponent;
        private Rectangle _boundingBox;

        public MoveWithCollisionComponent(Rectangle boundingBox, PhysicsComponent physicsComponent)
        {
            _boundingBox = boundingBox;
            _physicsComponent = physicsComponent;
        }

        public void Move(float dt, TileMap tileMap) {
            int directionX = Math.Sign(_physicsComponent.Velocity.X);
            int directionY = Math.Sign(_physicsComponent.Velocity.Y);
            int newX = (int)Math.Floor(_physicsComponent.Position.X); // round down if float?
            int newY = (int)Math.Floor(_physicsComponent.Position.Y); // round down if float?

            _boundingBox.X = newX;
            _boundingBox.Y = newY;
            _physicsComponent.IsOnGround = false;
            _physicsComponent.IsJumping = true;

            // Move X first
            if (_physicsComponent.Velocity.X > 0) {
                for (int x = 1;x <= Math.Abs(_physicsComponent.Velocity.X * dt);x++) {
                    _boundingBox.X += directionX;
                    Tile t1 = tileMap.PositionToTile(_boundingBox.Right, _boundingBox.Top);
                    Tile t2 = tileMap.PositionToTile(_boundingBox.Right, _boundingBox.Bottom);
                    if (t1.IsSolid && t1.BoundingBox.Intersects(_boundingBox) || t2.IsSolid && t2.BoundingBox.Intersects(_boundingBox)) {
                        // Add slope handling
                        // Reset velocity x if hit
                        break;
                    }
                    newX = _boundingBox.X;
                }
            }
            if (_physicsComponent.Velocity.X < 0) {
                for (int x = 1;x <= Math.Abs(_physicsComponent.Velocity.X * dt);x++) {
                    _boundingBox.X += directionX;
                    Tile t1 = tileMap.PositionToTile(_boundingBox.Left, _boundingBox.Top);
                    Tile t2 = tileMap.PositionToTile(_boundingBox.Left, _boundingBox.Bottom);
                    if (t1.IsSolid && t1.BoundingBox.Intersects(_boundingBox) || t2.IsSolid && t2.BoundingBox.Intersects(_boundingBox)) {
                        // Add slope handling
                        // Reset velocity x if hit
                        break;
                    }
                    newX = _boundingBox.X;
                }
            }

            // Move Y
            if (_physicsComponent.Velocity.Y > 0) {
                for (int y = 1;y < Math.Abs(_physicsComponent.Velocity.Y * dt);y++) {
                    _boundingBox.Y += directionY;
                    // Bottom tiles
                    Tile t1 = tileMap.PositionToTile(newX, _boundingBox.Bottom);
                    Tile t2 = tileMap.PositionToTile(newX + _boundingBox.Width - 1, _boundingBox.Bottom);
                    if (t1.IsSolid && t1.BoundingBox.Intersects(_boundingBox) || t2.IsSolid && t2.BoundingBox.Intersects(_boundingBox)) {
                        // Add slope handling
                        // Reset velocity y if hit on bottom
                        // directionY == 1 ? Velocity.Y = 0
                        _physicsComponent.IsOnGround = true;
                        _physicsComponent.IsJumping = false;
                        break;
                    }
                    newY = _boundingBox.Y;
                }
            }
            if (_physicsComponent.Velocity.Y < 0) {
                for (int y = 1;y < Math.Abs(_physicsComponent.Velocity.Y * dt);y++) {
                    _boundingBox.Y += directionY;
                    // Bottom tiles
                    Tile t1 = tileMap.PositionToTile(newX, _boundingBox.Top);
                    Tile t2 = tileMap.PositionToTile(newX + _boundingBox.Width - 1, _boundingBox.Top);
                    if (t1.IsSolid && t1.BoundingBox.Intersects(_boundingBox) || t2.IsSolid && t2.BoundingBox.Intersects(_boundingBox)) {
                        // Add slope handling
                        // Reset velocity y if hit on bottom
                        // directionY == 1 ? Velocity.Y = 0
                        //isJumping = false;
                        _physicsComponent.ResetYVelocity();
                        break;
                    }
                    newY = _boundingBox.Y;
                }
            }

            _physicsComponent.SetPosition(newX, newY);
        }
    }
}