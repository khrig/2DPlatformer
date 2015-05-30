using System.Collections.Generic;
using Gengine;
using Gengine.Commands;
using Gengine.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _2DPlatformer.Rendering;
using Gengine.Entities;
using System.Linq;

namespace _2DPlatformer.States {
    class MenuState : State {
        private readonly List<MenuOption> options = new List<MenuOption>();
        private int selectedOption;

        public MenuState(IWorld world) : base(world) {
        }

        public override bool Update(float deltaTime) {
            for (int i = 0; i < options.Count; i++) {
                options[i].Color = i == selectedOption ? Color.LightGreen : Color.White;
            }
            return false;
        }

        public override bool Draw(SpriteBatch spriteBatch) {
            return false;
        }

        public override IEnumerable<IRenderable> GetRenderTargets() {
            return options;
        }

        public override void Init() {
            options.Clear();
            options.Add(new MenuOption("text", "Start", Color.White, new Vector2(100, World.View.Center.Y)));
            options.Add(new MenuOption("text", "Exit", Color.White, new Vector2(100, World.View.Center.Y + 40)));
            selectedOption = 0;
        }

        private void DrawTitle(SpriteBatch spriteBatch) {
            string title = "PLATFORM YEAH";
            //textRenderer.DrawCenteredString(spriteBatch, title, World.View.Center.Y - 50, Color.Green);
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
                if (options[selectedOption].Text == "Start") {
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
