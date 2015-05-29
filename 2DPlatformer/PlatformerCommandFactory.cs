using Gengine.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DPlatformer {
    public class PlatformerCommandFactory : ICommandFactory {
        public ICommand CreateCommand(string name) {
            return new Command(name);
        }
    }
}
