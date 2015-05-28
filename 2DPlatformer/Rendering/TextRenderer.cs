using Gengine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DPlatformer.Rendering {
    public class TextRenderer {
        private SpriteFont font;
        private IWorld world;
        public TextRenderer(IWorld world, SpriteFont font) {
            this.font = font;
            this.world = world;
        }

        public void DrawCenteredString(SpriteBatch spriteBatch, string text, float y, Color color) {
            Vector2 strV = font.MeasureString(text);
            var pos = new Vector2(world.View.Center.X - (strV.Length() / 2), y);

            spriteBatch.DrawString(font, text, pos, color);
        }
    }
}
