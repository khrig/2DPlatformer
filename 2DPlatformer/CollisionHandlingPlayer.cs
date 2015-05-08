using _2DPlatformer;
using Gengine.Entities;
using Gengine.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _Platformer2D {
    public class CollisionHandlingPlayer : IPlayer {
        private const float Gravity = 300f;
        private const float ACCEL = 600f;
        private const float FRICTION = 300f;
        private const float JUMP = 10000f;
        private const float MAXDX = 200f;
        private const float MAXDY = 1200f;

        private Vector2 position;
        public Vector2 Position {
            get { return position; }
            private set {
                position = value;
            }
        }

        public Vector2 RenderPosition {
            get { return Position; }
        }
        
        private Vector2 Velocity;

        private TileMap tileMap;

        public string TextureName {
            get { return "player"; }
        }

        private Rectangle sourceRectangle;
        public Rectangle SourceRectangle {
            get { return sourceRectangle; }
        }

        bool jumping = true;

        public CollisionHandlingPlayer(Vector2 position, TileMap tileMap) {
            Position = position;
            Velocity = Vector2.Zero;
            this.tileMap = tileMap;
            sourceRectangle = new Rectangle(0, 0, 32, 32);
        }

        public void Update(float deltaTime) {
           
                bool left = false;
                bool right = false;
                bool jump = false;

                KeyboardState currentKeyBoardState = Keyboard.GetState();
                if (currentKeyBoardState.IsKeyDown(Keys.Up)) {
                    jump = true;
                }
                if (currentKeyBoardState.IsKeyDown(Keys.Left)) {
                    left = true;
                }
                if (currentKeyBoardState.IsKeyDown(Keys.Right)) {
                    right = true;
                }

                if (left) {
                    Velocity.X -= ACCEL * deltaTime;
                } else if (right) {
                    Velocity.X += ACCEL * deltaTime;
                }

                if (jump && !jumping) {
                    Velocity.Y = -JUMP * deltaTime;
                    jumping = true;
                }

                if (Velocity.X > 0) {
                    Velocity.X -= FRICTION * deltaTime;
                    if (Velocity.X < 0)
                        Velocity.X = 0;

                } else if (Velocity.X < 0) {
                    Velocity.X += FRICTION * deltaTime;
                    if (Velocity.X > 0)
                        Velocity.X = 0;
                }

                if (jumping) {
                    Velocity.Y += Gravity * deltaTime;
                }

                Velocity.X = MathHelper.Clamp(Velocity.X, -MAXDX, MAXDX);

                Move(deltaTime);
            
        }

        private void Move(float deltaTime) {
            Vector2 newPos = new Vector2(Position.X + Velocity.X * deltaTime, Position.Y + Velocity.Y * deltaTime);
            Rectangle box = new Rectangle((int)newPos.X, (int)Position.Y, 32, 32);
            Vector2 topLeftPoint = new Vector2(box.X, box.Y);
            Vector2 topRightPoint = new Vector2(box.X + box.Width, box.Y);
            Vector2 bottomLeftPoint = new Vector2(box.X, box.Y + box.Height);
            Vector2 bottomRightPoint = new Vector2(box.X + box.Width, box.Y + box.Height);
            var tile = tileMap.PositionToTile(topLeftPoint);
            var tileRight = tileMap.PositionToTile(topRightPoint);
            var tileBottomLeft = tileMap.PositionToTile(bottomLeftPoint);
            var tileBottomRight = tileMap.PositionToTile(bottomRightPoint);

            if (!tileBottomLeft.IsSolid && !tileBottomRight.IsSolid)
                jumping = true;

            if (Velocity.X > 0.0f) {
                if (tileRight.IsSolid && !tile.IsSolid && box.Intersects(tileRight.BoundingBox)
                    || (tileBottomRight.IsSolid && !tileBottomLeft.IsSolid && box.Intersects(tileBottomRight.BoundingBox) && Velocity.Y != 0)) {
                    newPos.X = (tileRight.Position.X * 32) - box.Width;
                    Velocity.X = 0;
                }
            } else if (Velocity.X < 0.0f) {
                if (tile.IsSolid && !tileRight.IsSolid && box.Intersects(tile.BoundingBox)
                    || (tileBottomLeft.IsSolid && !tileBottomRight.IsSolid && box.Intersects(tileBottomLeft.BoundingBox) && Velocity.Y != 0)) {
                    newPos.X = (tile.Position.X * 32) + 32;
                    Velocity.X = 0;
                }
            }

            box = new Rectangle((int)Position.X, (int)newPos.Y, 32, 32);

            if (Velocity.Y > 0.0f) {
                if (tileBottomLeft.IsSolid && box.Intersects(tileBottomLeft.BoundingBox)) {
                    newPos.Y = (tile.Position.Y * 32);
                    Velocity.Y = 0;
                    jumping = false;
                } else if (tileBottomRight.IsSolid && box.Intersects(tileBottomRight.BoundingBox)) {
                    newPos.Y = (tileBottomRight.Position.Y * 32) - 32;
                    Velocity.Y = 0;
                    jumping = false;
                }
            } else if (Velocity.Y < 0.0f) {
                if (tile.IsSolid && box.Intersects(tile.BoundingBox)) {
                    newPos.Y = (tile.Position.Y * 32) + 32;
                    Velocity.Y = 0;
                } else if (tileRight.IsSolid && box.Intersects(tileRight.BoundingBox)) {
                    newPos.Y = (tileRight.Position.Y * 32) + 32;
                    Velocity.Y = 0;
                }
            }

            position.X = newPos.X;
            position.Y = newPos.Y;
        }
    }
}
