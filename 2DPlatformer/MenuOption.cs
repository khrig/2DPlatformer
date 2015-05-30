using Gengine.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DPlatformer {
    public class MenuOption : IRenderable {
        public RenderType Type {
            get { return RenderType.Text; }
        }

        public string Text { get; private set; }

        public string TextureName {
            get { throw new NotImplementedException(); }
        }

        public Vector2 RenderPosition { get; set; }

        public Rectangle SourceRectangle {
            get { throw new NotImplementedException(); }
        }

        public string FontName { get; set; }
        public Color Color { get; set; }

        public MenuOption(string fontName, string text, Color color, Vector2 position) {
            FontName = fontName;
            Text = text;
            Color = color;
            RenderPosition = position;
        }
    }
}
