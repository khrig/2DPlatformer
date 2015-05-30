using Gengine;
using Gengine.Commands;
using Gengine.Map;
using Gengine.State;
using Microsoft.Xna.Framework;

namespace _2DPlatformer.States {
    public class GameState : State {
        private readonly IMapRepository _mapRepository;
        private IPlayer _player;
        private TileMap _tileMap;

        public GameState(IWorld world, IMapRepository mapRepository) : base(world) {
            _mapRepository = mapRepository;
        }

        public override bool Update(float deltaTime) {
            _player.Update(deltaTime);
            return false;
        }

        public override void HandleCommands(CommandQueue commandQueue) {
            while (commandQueue.HasCommands()) {
                var command = commandQueue.GetNext();
                if (command.Name == "Escape") {
                    StateManager.PopState();
                    StateManager.PushState("menu");
                    return;
                }
                if (command.Name == "Pause") {
                    StateManager.PushState("pause");
                    return;
                }
            }
        }

        public override void Init() {
            _tileMap = _mapRepository.LoadMap("Maps\\largeroom.tmap");
            //player = new CollisionHandlingPlayer(new Vector2(100, 100), tileMap);
            _player = new PreCheckingPlayer(new Vector2(100, 100), _tileMap);
            RegisterRenderTarget(_tileMap.RenderableTiles);
            RegisterRenderTarget(_player);
        }

        public override void Unload() {
            UnregisterRenderTarget(_tileMap.RenderableTiles);
            UnregisterRenderTarget(_player);
            _player = null;
            _tileMap = null;
        }
    }
}
