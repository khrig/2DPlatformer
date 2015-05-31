using Microsoft.Xna.Framework;

namespace _2DPlatformer {
    public class Animation {
        private readonly int _width;
        private readonly int _frames;
        private readonly float _delay;
        private float _elapsed;
        private int _currentFrame;

        private Rectangle _sourceRectangle;
        public Rectangle SourceRectangle { get { return _sourceRectangle; } }

        public Animation(int x, int y, int width, int height, int frames, float delay) {
            _width = width;
            _frames = frames;
            _delay = delay;
            _sourceRectangle = new Rectangle(x, y, width, height);
        }

        public void Update(float dt) {
            _elapsed += dt;
            if (!(_elapsed > _delay)) return;
            
            _currentFrame++;
            if (_currentFrame > _frames)
                _currentFrame = 0;
            _sourceRectangle.X = _currentFrame * _width;
            _elapsed = 0;
        }

        public void Reset() {
            _elapsed = 0;
            _currentFrame = 0;
            _sourceRectangle.X = 0;
        }
    }
}
