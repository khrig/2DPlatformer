using System;
using System.Collections.Generic;
using System.Linq;
using Gengine.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.States {
    class MenuState : State {
        private readonly List<string> options = new List<string>();
        private int selectedOption;
        private Vector2 startPosition;
        private SpriteFont font;

        public MenuState(SpriteFont font, Vector2 startPosition) {
            this.font = font;
            this.startPosition = startPosition;
        }

        public override bool Update(float deltaTime) {
            return false;
        }

        public override bool Draw(SpriteBatch spriteBatch) {
            Vector2 pos = startPosition;

            pos.Y -= 100;
            pos.X -= 50;
            spriteBatch.DrawString(font, "PLATFORM YEAH", pos, Color.Green);

            pos = startPosition;
            for(int i = 0; i < options.Count; i++) {
                if(i == selectedOption)
                    spriteBatch.DrawString(font, options[i], pos, Color.LightGreen);
                else
                    spriteBatch.DrawString(font, options[i], pos, Color.White);
                pos.Y += 40;
            }

            return false;
        }

        public override void HandleInput(string key) {
            if (key == "Up")
                MoveUp();
            else if (key == "Down")
                MoveDown();
            else if(key == "Escape")
                StateManager.PopState();
            else if (key == "Enter") {
                if (options[selectedOption] == "Start") {
                    StateManager.PopState();
                    StateManager.PushState("game");
                }
                else {
                    StateManager.PopState();
                }
            }
        }

        private void MoveDown() {
            selectedOption++;
            if (selectedOption >= options.Count)
                selectedOption = 0;
        }

        private void MoveUp() {
            selectedOption--;
            if (selectedOption < 0)
                selectedOption = options.Count - 1;
        }

        public override void Init() {
            options.Clear();
            options.Add("Start");
            options.Add("Exit");
            selectedOption = 0;
        }
    }
}
