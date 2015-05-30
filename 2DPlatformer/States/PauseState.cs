using System.Collections.Generic;
using Gengine;
using Gengine.Commands;
using Gengine.State;
using Microsoft.Xna.Framework;

namespace _2DPlatformer.States {
    public class PauseState : State {
        private readonly List<MenuOption> _title;

        public PauseState(IWorld world)
            : base(world) {
            _title = new List<MenuOption>(1);
        }

        public override bool Update(float deltaTime) {
            return false;
        }

        public override void Init() {
            _title.Add(new MenuOption("text", "PAUSED", Color.Green, new Vector2(World.View.Center.X - 50, World.View.Center.Y - 50)));
            RegisterRenderTarget(_title);
        }

        public override void Unload() {
            UnregisterRenderTarget(_title);
            _title.Clear();
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
    }
}
