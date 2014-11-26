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
        private readonly Renderer renderer;

        public GameState(SpriteBatch spriteBatch, Texture2D playerTexture, Texture2D environmentTexture) {
            this.player = new PlayerEntity(
                new InputComponent(), 
                new VisualComponent(playerTexture), 
                new MovementComponent(new Vector2(100, 470)), 
                new AnimationComponent(new Rectangle(0,0,32,32)));

            tileMap = new TileMap(environmentTexture);

            renderer = new Renderer(spriteBatch);
        }

        public override bool Update(float deltaTime) {
            player.Update(deltaTime);
            return false;
        }

        public override void HandleCommands(CommandQueue commandQueue) {
            while (commandQueue.HasCommands()) {
                player.HandleCommand(commandQueue.GetNext());
            }
        }

        public override void Init() {
        }

        public override bool Draw(SpriteBatch spriteBatch) {
            renderer.Draw(tileMap.Tiles);
            renderer.Draw(new List<IRenderable> { player });
            return false;
        }
    }
}
