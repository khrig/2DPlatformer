using Gengine;
using Gengine.Camera;
using Gengine.Commands;
using Gengine.Map;
using Gengine.State;
using Microsoft.Xna.Framework;

namespace _2DPlatformer.States {
    public class GameState : State {
        private readonly IMapRepository _mapRepository;
        private IPlayer _player;
        private TileMap _tileMap;
        private readonly SimpleCamera2D _simpleCamera2D;
        
        public GameState(IWorld world, IMapRepository mapRepository) : base(world) {
            _mapRepository = mapRepository;
            _simpleCamera2D = new SimpleCamera2D(world);
        }

        public override bool Update(float deltaTime) {
            _player.Update(deltaTime);
            _simpleCamera2D.SetPosition(_player.RenderPosition);
            SetTransformation(_simpleCamera2D.GetTransformMatrix());
            return false;
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
            SetTransformation(null);
        }

        public override void HandleCommands(CommandQueue commandQueue) {
            while (commandQueue.HasCommands()) {
                var command = commandQueue.GetNext();
                if (command.Name == "Escape") {
                    StateManager.PopState();
                    StateManager.PushState(States.Menu);
                    return;
                }
                if (command.Name == "Pause") {
                    StateManager.PushState(States.Pause);
                    return;
                }
            }
        }
    }
}
