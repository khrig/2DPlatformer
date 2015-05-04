#region Using Statements
using System;
using System.Collections.Generic;
using Gengine;
using Gengine.Commands;
using Gengine.Input;
using Gengine.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Platformer.States;
using Gengine.Resources;

#endregion

namespace Platformer {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Platformer : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D renderTarget;
        private readonly StateManager stateManager;
        private readonly CommandQueue commandQueue;
        private readonly IResourceManager resourceManager;

        private readonly IWorld world;

        public Platformer()
            : base() {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);
            //graphics.PreferredBackBufferWidth = 640;  // set this value to the desired width of your window
            //graphics.PreferredBackBufferHeight = 360;   // set this value to the desired height of your window
            


            world = new TwoDWorld(640, 360);
            stateManager = new StateManager();
            commandQueue = new CommandQueue();
            resourceManager = new ResourceManager();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here
            base.Initialize();

            resourceManager.AddTexture("environmentTexture", Content.Load<Texture2D>("phase-2"));
            resourceManager.AddTexture("player", Content.Load<Texture2D>("characters_7"));

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            renderTarget = new RenderTarget2D(GraphicsDevice, 640, 360);
            
            stateManager.Add("menu", new MenuState(world, Content.Load<SpriteFont>("monolight12")));
            stateManager.Add("game", new GameState(world, resourceManager, spriteBatch));

            stateManager.PushState("menu");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            stateManager.ChangeState();
            if (stateManager.IsEmpty())
                Exit();

            InputManager.Instance.HandleInput(commandQueue);
            stateManager.HandleCommands(commandQueue);
            stateManager.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            DrawWithRenderTarget();

            //DrawNormal();

            base.Draw(gameTime);
        }

        private void DrawNormal() {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            stateManager.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void DrawWithRenderTarget() {
            // Set the device to the render target
            graphics.GraphicsDevice.SetRenderTarget(renderTarget);

            graphics.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            stateManager.Draw(spriteBatch);
            spriteBatch.End();

            // Reset the device to the back buffer
            graphics.GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            spriteBatch.Draw((Texture2D)renderTarget, new Rectangle(0, 0, GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height), Color.White);
            spriteBatch.End();
        }
    }
}
