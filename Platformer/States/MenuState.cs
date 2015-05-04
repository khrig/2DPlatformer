using System.Collections.Generic;
using Gengine;
using Gengine.Commands;
using Gengine.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.States {
    class MenuState : State {
        private readonly List<string> options = new List<string>();
        private int selectedOption;
        private readonly SpriteFont font;

        public MenuState(IWorld world, SpriteFont font) : base(world) {
            this.font = font;
        }

        public override bool Update(float deltaTime) {
            return false;
        }

        public override bool Draw(SpriteBatch spriteBatch) {
            DrawTitle(spriteBatch);
            DrawMenuOptions(spriteBatch);
            return false;
        }

        public override void Init() {
            options.Clear();
            options.Add("Start");
            options.Add("Exit");
            selectedOption = 0;
        }

        private void DrawTitle(SpriteBatch spriteBatch) {
            string title = "PLATFORM YEAH";
            DrawCenteredString(spriteBatch, title, World.View.Center.Y - 50, Color.Green);
        }

        private void DrawMenuOptions(SpriteBatch spriteBatch) {
            float y = World.View.Center.Y;
            for (int i = 0; i < options.Count; i++) {
                DrawCenteredString(spriteBatch, options[i], y, i == selectedOption ? Color.LightGreen : Color.White);
                y += 40;
            }
        }

        private void DrawCenteredString(SpriteBatch spriteBatch, string text, float y, Color color) {
            Vector2 strV = font.MeasureString(text);
            var pos = new Vector2(World.View.Center.X - (strV.Length() / 2), y);
            
             spriteBatch.DrawString(font, text, pos, color);
        }

        public override void HandleCommands(CommandQueue commandQueue) {
            while (commandQueue.HasCommands()) {
                string command = commandQueue.GetNext();
                HandleCommand(command);
            }
        }

        private void HandleCommand(string command) {
            if (command == "Up")
                MoveUp();
            else if (command == "Down")
                MoveDown();
            else if (command == "Escape")
                StateManager.PopState();
            else if (command == "Enter") {
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
    }
}
