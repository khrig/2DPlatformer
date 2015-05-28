using _2DPlatformer.Rendering;
using Gengine;
using Gengine.Commands;
using Gengine.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DPlatformer.States {
    public class PauseState : State {
        private readonly TextRenderer textRenderer;

        public PauseState(IWorld world, SpriteFont font)
            : base(world) {
            textRenderer = new TextRenderer(world, font);
        }

        public override bool Update(float deltaTime) {
            return false;
        }

        public override bool Draw(SpriteBatch spriteBatch) {
            textRenderer.DrawCenteredString(spriteBatch, "PAUSED", World.View.Center.Y - 50, Color.Green);
            return false;
        }

        public override void HandleCommands(CommandQueue commandQueue) {
            while (commandQueue.HasCommands()) {
                string command = commandQueue.GetNext();
                HandleCommand(command);
            }
        }

        private void HandleCommand(string command) {
            if (command == "Escape")
                StateManager.PopState();
        }

        public override void Init() {
        }
    }
}
