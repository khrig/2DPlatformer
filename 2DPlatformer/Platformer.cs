using _2DPlatformer.States;
using Gengine;
using Gengine.Commands;
using Gengine.Input;
using Gengine.Map;
using Gengine.Resources;
using Gengine.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2DPlatformer {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Platformer : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D renderTarget;
        private readonly StateManager stateManager;
        private readonly CommandQueue commandQueue;
        private readonly IResourceManager resourceManager;

        private readonly IWorld world;

        private int IngameWidth = 640;
        private int IngameHeight = 360;

        private int WindowHeight;
        private int WindowWidth;

        public Platformer()
            : base() {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);

            world = new TwoDWorld(IngameWidth, IngameHeight);
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

            resourceManager.AddTexture("environmentTexture", Content.Load<Texture2D>("Sprites/phase-2"));
            resourceManager.AddTexture("player", Content.Load<Texture2D>("Sprites/characters_7"));
            resourceManager.AddTexture("tiles32.png", Content.Load<Texture2D>("Sprites/tiles32"));

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // set the resolution to the monitor (for fullscreen)
            //WindowWidth = graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            //WindowHeight = graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            //graphics.IsFullScreen = true;

            // For window (debugging)
            WindowWidth = graphics.PreferredBackBufferWidth = 1280;
            WindowHeight = graphics.PreferredBackBufferHeight = 720;

            graphics.ApplyChanges();

            // Ingame resolution
            renderTarget = new RenderTarget2D(GraphicsDevice, IngameWidth, IngameHeight);

            stateManager.Add("menu", new MenuState(world, Content.Load<SpriteFont>("monolight12")));
            stateManager.Add("game", new GameState(world, new MapRepository(true), resourceManager, spriteBatch));

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

            base.Draw(gameTime);
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
            spriteBatch.Draw((Texture2D)renderTarget, new Rectangle(0, 0, WindowWidth, WindowHeight), Color.White);
            spriteBatch.End();
        }
    }
}
