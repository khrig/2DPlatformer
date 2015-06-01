using Gengine.Entities;
using Microsoft.Xna.Framework;

namespace _2DPlatformer {
    public class Background : IRenderable {
        public Background(string textureName, Vector2 renderPosition, Rectangle sourceRectangle) {
            TextureName = textureName;
            RenderPosition = renderPosition;
            SourceRectangle = sourceRectangle;
        }

        public RenderType Type { get {return RenderType.Sprite;} }
        public string TextureName { get; private set; }
        public Vector2 RenderPosition { get; private set; }
        public Rectangle SourceRectangle { get; private set; }
        public string FontName { get; private set; }
        public string Text { get; private set; }
        public Color Color { get; private set; }

    }
}
