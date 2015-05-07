using System.Collections.Generic;
using Gengine;
using Gengine.Commands;
using Gengine.Entities;
using Gengine.State;
using Gengine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Gengine.Map;
using Gengine.Resources;
using _Platformer2D;

namespace _2DPlatformer.States {
    class GameState : State {
        //private readonly PlayerEntity player;
        private CollisionHandlingPlayer player;
        private readonly TileMap tileMap;

        // Should not be here probably
        private readonly RenderingSystem renderingSystem;
        private readonly CollisionSystem collisionSystem;

        private readonly IMapRepository mapRepository;

        // 
        // If we move creation of entities to a worl object it can hold all relevant lists like entities
        //
        private List<ICollidable> collidableObjects = new List<ICollidable>();

        public GameState(IWorld world, IMapRepository mapRepository, IResourceManager resourceManager, SpriteBatch spriteBatch) : base(world) {
            this.mapRepository = mapRepository;
            tileMap = mapRepository.LoadMap("Maps\\room2.tmap");


            //player = new PlayerEntity(
                //new InputComponent(), 
                //new MovementComponent(new Vector2(100, 100)), 
                //new AnimationComponent("player", new Rectangle(0,0,32,32)));

            //tileMap = new TileMap("environmentTexture");

            renderingSystem = new RenderingSystem(resourceManager, spriteBatch);
            collisionSystem = new CollisionSystem();

            //collidableObjects = new List<ICollidable>();
            //collidableObjects.Add(player);
            //collidableObjects.AddRange(tileMap.CollisionMap);
        }

        public override bool Update(float deltaTime) {
            player.Update(deltaTime);

            //var collidables = new List<ICollidable>();
            //collidables.Add(player);

            //collisionSystem.HandleCollisions(collidableObjects);
            return false;
        }

        public override void HandleCommands(CommandQueue commandQueue) {
            while (commandQueue.HasCommands()) {
                if (commandQueue.GetNext() == "Escape") {
                    StateManager.PopState();
                    StateManager.PushState("menu");
                    return;
                }
            }
        }

        public override void Init() {
            player = new CollisionHandlingPlayer(tileMap, new Vector2(100, 100));
        }

        public override bool Draw(SpriteBatch spriteBatch) {
            renderingSystem.Draw(tileMap.RenderableTiles);
            renderingSystem.Draw(new List<IRenderable> {player});
            return false;
        }
    }
}
