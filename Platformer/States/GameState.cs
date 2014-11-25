using System;
using System.Collections.Generic;
using Gengine.Entities;
using Gengine.State;
using Gengine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.States {
    class GameState : State {
        private Texture2D environmentTexture;

        private PlayerEntity player;

        // Should not be here
        private Renderer renderer;

        public GameState(SpriteBatch spriteBatch, Texture2D player, Texture2D environment) {
            this.environmentTexture = environment;

            this.player = new PlayerEntity(
                new VisualComponent(player), 
                new PositionComponent(new Vector2(100, 470)), 
                new AnimationComponent(new Rectangle(0,0,32,32)));

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
            Vector2 groundPos = new Vector2(0, 480);
            for (int i = 1; i < 26; i++) {
                spriteBatch.Draw(environmentTexture, groundPos, new Rectangle(0, 0, 32, 32), Color.White);
                groundPos.X += 32;
            }

            renderer.Draw(new List<IRenderable> { player });
            return false;
        }
    }
}
