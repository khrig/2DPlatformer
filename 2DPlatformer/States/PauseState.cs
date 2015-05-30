using _2DPlatformer.Rendering;
using Gengine;
using Gengine.Commands;
using Gengine.Entities;
using Gengine.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DPlatformer.States {
    public class PauseState : State {
        public PauseState(IWorld world)
            : base(world) {
        }

        public override bool Update(float deltaTime) {
            return false;
        }

        public override bool Draw(SpriteBatch spriteBatch) {
            //textRenderer.DrawCenteredString(spriteBatch, "PAUSED", World.View.Center.Y - 50, Color.Green);
            return false;
        }

        public override void HandleCommands(CommandQueue commandQueue) {
            while (commandQueue.HasCommands()) {
                var command = commandQueue.GetNext();
                HandleCommand(command);
            }
        }

        private void HandleCommand(ICommand command) {
            if (command.Name == "Escape")
                StateManager.PopState();
        }

        public override void Init() {
        }

        public override IEnumerable<IRenderable> GetRenderTargets() {
            return Enumerable.Empty<IRenderable>();
        }
    }
}
