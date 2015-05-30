using Gengine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2DPlatformer.Rendering {
    public class TextRenderer {
        private readonly SpriteFont _font;
        private readonly IWorld _world;
        public TextRenderer(IWorld world, SpriteFont font) {
            _font = font;
            _world = world;
        }

        public void DrawCenteredString(SpriteBatch spriteBatch, string text, float y, Color color) {
            var strV = _font.MeasureString(text);
            var pos = new Vector2(_world.View.Center.X - (strV.Length() / 2), y);

            spriteBatch.DrawString(_font, text, pos, color);
        }
    }
}
