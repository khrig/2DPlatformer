using _2DPlatformer.States;
using Gengine;
using Gengine.Commands;
using Gengine.Input;
using Gengine.Map;
using Gengine.Resources;
using Gengine.State;
using Gengine.Systems;
using Gengine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2DPlatformer {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Platformer : Game {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _renderTarget;
        private RenderingSystem _renderingSystem;
        private readonly StateManager _stateManager;
        private readonly CommandQueue _commandQueue;
        private readonly ICommandFactory _commandFactory;
        private readonly IResourceManager _resourceManager;
        private readonly IWorld _world;

        FrameCounter _frameCounter;

        private readonly int _ingameWidth = 640;
        private readonly int _ingameHeight = 360;

        private int _windowHeight;
        private int _windowWidth;

        public Platformer()
            : base() {
            Content.RootDirectory = "Content";

            _graphics = new GraphicsDeviceManager(this);

            _world = new TwoDWorld(_ingameWidth, _ingameHeight);
            _stateManager = new StateManager();
            _commandQueue = new CommandQueue();
            _commandFactory = new PlatformerCommandFactory();
            _resourceManager = new ResourceManager();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            base.Initialize();

            _resourceManager.AddFont("text", Content.Load<SpriteFont>("04b_03_10"));
            _resourceManager.AddTexture("environmentTexture", Content.Load<Texture2D>("Sprites/phase-2"));
            _resourceManager.AddTexture("player", Content.Load<Texture2D>("Sprites/characters_7"));
            _resourceManager.AddTexture("tiles32.png", Content.Load<Texture2D>("Sprites/tiles32"));
            _resourceManager.AddTexture("bkg", Content.Load<Texture2D>("Sprites/bkg"));

            _frameCounter = new FrameCounter(_resourceManager, "text", new Vector2(10, 10));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // set the resolution to the monitor (for fullscreen)
            //_windowWidth = _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            //_windowHeight = _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            //_graphics.IsFullScreen = true;

            // For window (debugging)
            _windowWidth = _graphics.PreferredBackBufferWidth = 1280;
            _windowHeight = _graphics.PreferredBackBufferHeight = 720;

            _graphics.ApplyChanges();

            // Ingame resolution
            _renderTarget = new RenderTarget2D(GraphicsDevice, _ingameWidth, _ingameHeight);

            _renderingSystem = new RenderingSystem(_resourceManager, _spriteBatch, _renderTarget, _windowWidth, _windowHeight);

            _stateManager.Add(States.States.Menu, new MenuState(_world));
            _stateManager.Add(States.States.Game, new GameState(_world, new MapRepository(true)));
            _stateManager.Add(States.States.Pause, new PauseState(_world));

            _stateManager.PushState(States.States.Menu);
        }

        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime) {
            _stateManager.ChangeState();
            if (_stateManager.IsEmpty())
                Exit();

            InputManager.Instance.HandleInput(_commandQueue, _commandFactory);
            _stateManager.HandleCommands(_commandQueue);
            _stateManager.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            _frameCounter.Update(gameTime);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            _renderingSystem.DrawWithRenderTarget(_stateManager.GetRenderTargets(), _stateManager.GetRenderTransformation());
            _frameCounter.Draw(_spriteBatch);
            base.Draw(gameTime);
        }
    }
}
