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

        public string TextureName {
            get { throw new NotImplementedException(); }
        }

        public Vector2 RenderPosition {
            get { throw new NotImplementedException(); }
        }

        public Rectangle SourceRectangle {
            get { throw new NotImplementedException(); }
        }

        public string FontName {
            get { throw new NotImplementedException(); }
        }
    }
}
