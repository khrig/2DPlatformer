using Gengine.Commands;

namespace _2DPlatformer {
    public class PlatformerCommandFactory : ICommandFactory {
        public ICommand CreateCommand(string name) {
            return new Command(name);
        }
    }
}
