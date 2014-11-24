using System;
using Gengine.Entities;
using Gengine.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.States {
    class GameState : State {
        private Texture2D playerTexture;
        private Texture2D environmentTexture;

        private Entity player;

        public GameState(Texture2D player, Texture2D environment) {
            this.playerTexture = player;
            this.environmentTexture = environment;

            this.player = new PlayerEntity(playerTexture, new Vector2(100, 470));
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

            player.Draw(spriteBatch);
            return false;
        }
    }
}
