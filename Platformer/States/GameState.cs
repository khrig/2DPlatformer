using System;
using System.Collections.Generic;
using Gengine;
using Gengine.Entities;
using Gengine.State;
using Gengine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.States {
    class GameState : State {
        private Texture2D environmentTexture;

        private readonly PlayerEntity player;
        private readonly TileMap tileMap;

        // Should not be here probably
        private readonly Renderer renderer;

        public GameState(SpriteBatch spriteBatch, Texture2D player, Texture2D environment) {
            this.environmentTexture = environment;

            this.player = new PlayerEntity(
                new VisualComponent(player), 
                new PositionComponent(new Vector2(100, 470)), 
                new AnimationComponent(new Rectangle(0,0,32,32)));

            tileMap = new TileMap(environment);

            renderer = new Renderer(spriteBatch);
        }

        public override bool Update(float deltaTime) {
            player.Update(deltaTime);
            return false;
        }

        public override void HandleInput(string key) {
            
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
