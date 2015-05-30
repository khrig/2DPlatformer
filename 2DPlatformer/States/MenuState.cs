using System.Collections.Generic;
using System.Linq;
using Gengine;
using Gengine.Commands;
using Gengine.State;
using Microsoft.Xna.Framework;

namespace _2DPlatformer.States {
    public class MenuState : State {
        private readonly List<MenuOption> _options;
        private readonly List<MenuOption> _title;
        private int _selectedOption;

        public MenuState(IWorld world) : base(world) {
            _options = new List<MenuOption>(5);
            _title = new List<MenuOption>(1);
        }

        public override bool Update(float deltaTime) {
            for (var i = 0; i < _options.Count; i++) {
                _options[i].Color = i == _selectedOption ? Color.LightGreen : Color.White;
            }
            return false;
        }
        
        public override void Init() {
            _options.Clear();
            _options.Add(new MenuOption("text", "Start", Color.White, new Vector2(World.View.Center.X - 20, World.View.Center.Y)));
            _options.Add(new MenuOption("text", "Exit", Color.White, new Vector2(World.View.Center.X - 15, World.View.Center.Y + 40)));
            _selectedOption = 0;
            _title.Clear();
            _title.Add(new MenuOption("text", "PLATFORM YEAH", Color.Green, new Vector2(World.View.Center.X - 50, World.View.Center.Y - 50)));
            RegisterRenderTarget(_options.Union(_title));
        }

        public override void HandleCommands(CommandQueue commandQueue) {
            while (commandQueue.HasCommands()) {
                var command = commandQueue.GetNext();
                HandleCommand(command);
            }
        }

        private void HandleCommand(ICommand command)
        {
            switch (command.Name)
            {
                case "Up":
                    MoveUp();
                    break;
                case "Down":
                    MoveDown();
                    break;
                case "Escape":
                    StateManager.PopState();
                    break;
                case "Enter":
                    if (_options[_selectedOption].Text == "Start") {
                        StateManager.PopState();
                        StateManager.PushState("game");
                    }
                    else {
                        StateManager.PopState();
                    }
                    break;
            }
        }

        private void MoveDown() {
            _selectedOption++;
            if (_selectedOption >= _options.Count)
                _selectedOption = 0;
        }

        private void MoveUp() {
            _selectedOption--;
            if (_selectedOption < 0)
                _selectedOption = _options.Count - 1;
        }

        public override void Unload() {
            UnregisterRenderTarget(_options.Union(_title));
        }
    }
}
