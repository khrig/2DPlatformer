using System.Collections.Generic;
using Gengine;
using Gengine.Commands;
using Gengine.Entities;
using Gengine.State;
using Gengine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.States {
    class GameState : State {
        private readonly PlayerEntity player;
        private readonly TileMap tileMap;

        // Should not be here probably
        private readonly RenderingSystem renderingSystem;
        private readonly CollisionSystem collisionSystem;

        // 
        // If we move creation of entities to a worl object it can hold all relevant lists like entities
        //
        private List<ICollidable> collidableObjects = new List<ICollidable>();

        public GameState(IWorld world, SpriteBatch spriteBatch, Texture2D playerTexture, Texture2D environmentTexture) : base(world) {

            /* Should bew moved to Init all of it ! */

            player = new PlayerEntity(
                new InputComponent(), 
                new VisualComponent(playerTexture), 
                new MovementComponent(new Vector2(100, 100)), 
                new AnimationComponent(new Rectangle(0,0,32,32)));

            tileMap = new TileMap(environmentTexture);

            renderingSystem = new RenderingSystem(spriteBatch);
            collisionSystem = new CollisionSystem();

            collidableObjects = new List<ICollidable>();
            collidableObjects.Add(player);
            collidableObjects.AddRange(tileMap.CollisionMap);
        }

        public override bool Update(float deltaTime) {
            player.Update(deltaTime);

            var collidables = new List<ICollidable>();
            collidables.Add(player);

            collisionSystem.HandleCollisions(collidableObjects);
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
        }

        public override bool Draw(SpriteBatch spriteBatch) {
            renderingSystem.Draw(tileMap.Tiles);
            renderingSystem.Draw(new List<IRenderable> {player});
            return false;
        }
    }
}
