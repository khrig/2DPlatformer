using Microsoft.Xna.Framework;

namespace _2DPlatformer
{
    public class PhysicsComponent {
        private const float MoveAcceleration = 12500.0f;
        private const float MaxMoveSpeed = 1750.0f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.51f;
        private const float Gravity = 1400f;
        private const float Jump = 500f;

        private Vector2 _position;
        public Vector2 Position { get { return _position; } set { _position = value; } }

        Vector2 _velocity;
        public Vector2 Velocity {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public float Movement { get; set; }
        public bool IsJumping { get; set; }
        public bool WantToJump { get; set; }
        public bool IsOnGround { get; set; }

        public PhysicsComponent(Vector2 position) {
            _position = position;
            IsJumping = true;
        }

        public void ApplyPhysics(float dt) {
            // Base velocity is a combination of horizontal movement control and
            // acceleration downward due to gravity.
            _velocity.X = Velocity.X + Movement * MoveAcceleration * dt;

            if (WantToJump && !IsJumping) {
                _velocity.Y = -Jump;
                IsJumping = true;
            }

            if (IsJumping) {
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

        public void SetPosition(int newX, int newY) {
            _position.X = newX;
            _position.Y = newY;
        }

        public void ResetYVelocity() {
            _velocity.Y = 0;
        }
    }
}