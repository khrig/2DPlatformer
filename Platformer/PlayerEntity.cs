using Gengine.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer {
    class PlayerEntity : Entity {
        private Texture2D texture;
        private Vector2 position;
        private Rectangle sourceRectangle;

        public PlayerEntity(Texture2D texture, Vector2 position) {
            this.texture = texture;
            this.position = position;

            sourceRectangle = new Rectangle(0,0,32,32);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, position, sourceRectangle, Color.White);
        }
    }
}
