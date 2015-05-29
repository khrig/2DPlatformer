using System.Collections.Generic;
using Gengine;
using Gengine.Commands;
using Gengine.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _2DPlatformer.Rendering;

namespace _2DPlatformer.States {
    class MenuState : State {
        private readonly List<string> options = new List<string>();
        private int selectedOption;
        private readonly TextRenderer textRenderer;

        public MenuState(IWorld world, SpriteFont font) : base(world) {
            textRenderer = new TextRenderer(world, font);
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
            textRenderer.DrawCenteredString(spriteBatch, title, World.View.Center.Y - 50, Color.Green);
        }

        private void DrawMenuOptions(SpriteBatch spriteBatch) {
            float y = World.View.Center.Y;
            for (int i = 0; i < options.Count; i++) {
                textRenderer.DrawCenteredString(spriteBatch, options[i], y, i == selectedOption ? Color.LightGreen : Color.White);
                y += 40;
            }
        }

        public override void HandleCommands(CommandQueue commandQueue) {
            while (commandQueue.HasCommands()) {
                var command = commandQueue.GetNext();
                HandleCommand(command);
            }
        }

        private void HandleCommand(ICommand command) {
            if (command.Name == "Up")
                MoveUp();
            else if (command.Name == "Down")
                MoveDown();
            else if (command.Name == "Escape")
                StateManager.PopState();
            else if (command.Name == "Enter") {
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
