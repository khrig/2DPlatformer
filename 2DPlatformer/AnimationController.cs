using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace _2DPlatformer {
    public class AnimationController {
        private readonly Dictionary<string, Animation> _animations;
        private string _currentAnimation;

        public Rectangle SourceRectangle {
            get { return _animations[_currentAnimation].SourceRectangle; }
        }

        public AnimationController() {
            _animations = new Dictionary<string, Animation>();
        }

        public void AddAnimation(string name, Animation animation) {
            _animations.Add(name, animation);
        }

        public void SetRunningAnimation(string name) {
            if (!string.IsNullOrEmpty(_currentAnimation) && _currentAnimation != name)
                _animations[_currentAnimation].Reset();
            _currentAnimation = name;
        }

        public void Update(float dt) {
            _animations[_currentAnimation].Update(dt);
        }
    }
}
