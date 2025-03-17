using CIS598Project.Rooms;
using CIS598Project.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace CIS598Project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
		private readonly ScreenManager _screens;

		public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
			DisplayMode screen = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
			Content.RootDirectory = "Content";
            IsMouseVisible = true;
			Window.Title = "Fredbear and Friends: Arcade";
			var screenFactory = new ScreenFactory();
			Services.AddService(typeof(IScreenFactory), screenFactory);
			_graphics.PreferredBackBufferWidth = 1920;
			_graphics.PreferredBackBufferHeight = 1080;

			_screens = new ScreenManager(this);
			Components.Add(_screens);
			_screens.AddScreen(new BalloonBarrel(this, new Game_Entities.Player()), null);

		}

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _screens.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
