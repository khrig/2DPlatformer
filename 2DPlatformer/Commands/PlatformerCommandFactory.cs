using System.Collections.Generic;
using Gengine.Commands;

namespace _2DPlatformer {
    public class PlatformerCommandFactory : ICommandFactory {
        private readonly Dictionary<string, ICommand> _commands;
        public PlatformerCommandFactory() {
            _commands = new Dictionary<string, ICommand>();
        }

        public ICommand CreateCommand(string name) {
            if (!_commands.ContainsKey(name))
                _commands.Add(name, new Command(name));
            return _commands[name];
        }
    }
}
