using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2DPlatformer {
    public class Animation {
        private readonly string _textureName;
        private readonly int _width;
        private readonly int _height;
        private readonly int frames;

        public Rectangle SourceRectangle { get; private set; }

        public Animation(string textureName, int width, int height) {
            _textureName = textureName;
            _width = width;
            _height = height;
            SourceRectangle = new Rectangle(0, 0, width, height);
        }


        public void Update(float dt)
        {
            
        }
    }
}
