using CIS598Project.Collisions;
using CIS598Project.Game_Entities;
using CIS598Project.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Threading;
using System.Data;
using SharpDX.WIC;




namespace CIS598Project.Rooms
{

	public enum GameStateFishing
	{
		Start = 0,
		Play,
		Win
	}
	public class Fishing : GameScreen
	{

		/// <summary>
		/// The game this class belongs to
		/// </summary>
		Game game;

		Texture2D[] backgrounds = new Texture2D[2];

		Texture2D[] Screens = new Texture2D[3];

		Texture2D[] Foxy = new Texture2D[6];

		Texture2D Freddy;

		Texture2D Plunger;

		Texture2D crashScreen;

		Song backgroundMusic;

		SoundEffect release;

		SoundEffect GameStateChange;

		SoundEffect Crash;

		SoundEffectInstance StateChange;


		SoundEffectInstance crashing;

		SoundEffectInstance released;


		GameStateFishing state = GameStateFishing.Play;


		Fish[][] fishes = new Fish[3][];


		bool invert = true;

		bool crash = false;

		bool end = false;

		int j = 0;

		private double lowerTime = 0;

		private double textTime = 0;

		private double startTime = 0;

		private double roundTimer = 0;

		string[] ourple = { "Hello there,\nwhat's wrong?", "Hmm? You don't want to\ntalk? Why not?", "Your parents say you\ncan't talk to strangers?", "Well they are smart, you \nshouldn't talk to strangers.", "However, you want to \nknow something cool?", "I'm not a stranger,\nI'm a friend of yours.", "Okay, you'll talk.\nThat's great!", "What was your problem...", "Hmm, none of your friends showed\nup for your birthday?", "Well, who needs them?\nI'm your friend and I know \njust how to celebrate your birthday.", "Just follow me and I'll take you\nto a special party room just for you." };

		int ourpleCount = 0;

		bool purpleDialogue = false;

		double ourpleTimer = 0;

		int chicaCount = 5;

		double chicaTimer = 0;



		private KeyboardState pastKeyboardState;
		private KeyboardState currentKeyboardState;

		SpriteFont font;

		ContentManager _content;

		int round = 0; //Starts at 0 and ends at 2 to correlate with the fish array
			
		BoundingRectangle plunger = new(0, 0, 104, 68);

		Vector2 plungerPosition = new(0, 0);

		Vector2 fredPosition = new(0,125);

		int score = 0;

		Player player;

		public Fishing(Game game,Player player) 
		{
			this.game = game;
			this.player = player;
		}

		public override void Activate()
		{
			base.Activate();

			if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

			Freddy = _content.Load<Texture2D>("Fishing/Textures/Ship/Fred/Fred_Left");
			Plunger = _content.Load<Texture2D>("Fishing/Textures/Ship/Fishing_supplies/Plunger");

			Screens[0] = _content.Load<Texture2D>("Fruity_Maze/Textures/Screen/scanlines");
			Screens[1] = _content.Load<Texture2D>("Fruity_Maze/Textures/Screen/Screen");
			Screens[2] = _content.Load<Texture2D>("Fruity_Maze/Textures/Screen/ScreenBorder");

			backgroundMusic = _content.Load<Song>("Fishing/Sounds/Song/Water_theme");

			backgrounds[0] = _content.Load<Texture2D>("Fishing/Textures/Backgrounds/Background");
			backgrounds[1] = _content.Load<Texture2D>("Fishing/Textures/Backgrounds/Water");

			font = _content.Load<SpriteFont>("MiniGame_Font");
			
			for (int i = 5; i >= 0; i--) 
			{
				Foxy[i] = _content.Load<Texture2D>("Fruity_Maze/Textures/Chica/Chica" + (i + 1));
			}

			MediaPlayer.Play(backgroundMusic);
			MediaPlayer.IsRepeating = true;
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, false);

			pastKeyboardState = currentKeyboardState;
			currentKeyboardState = Keyboard.GetState();

			if (!ScreenManager.Game.IsActive)
			{
				// Pause the music or stop sound effects when the game loses focus
				if (MediaPlayer.State == MediaState.Playing)
				{
					MediaPlayer.Pause();
				}
				return;
			}
			else
			{
				// Resume music if the game becomes active again
				if (MediaPlayer.State == MediaState.Paused)
				{
					MediaPlayer.Resume();
				}
			}

			if (state == GameStateFishing.Play)
			{
				if (currentKeyboardState.IsKeyDown(Keys.D) && pastKeyboardState.IsKeyUp(Keys.A))
				{
					fredPosition.X += 10;
				}
				if (currentKeyboardState.IsKeyDown(Keys.A) && pastKeyboardState.IsKeyUp(Keys.D))
				{
					fredPosition.X -= 10;
				}

				if (fredPosition.X >= game.GraphicsDevice.Viewport.Width - 300)
				{
					fredPosition.X = game.GraphicsDevice.Viewport.Width - 300;
				}
				else if (fredPosition.X <= 25)
				{
					fredPosition.X = 25;
				}

				roundTimer += .025;

				if (roundTimer >= 60) 
				{
					roundTimer = 0;
				}
			}
			

		}

		double duckAnimationTimer_idle = 0;
		double duckAnimationTimer_select = 0;

		/// <summary>
		/// Draws the sprite using the supplied SpriteBatch
		/// </summary>
		/// <param name="gameTime">The game time</param>
		public override void Draw(GameTime gameTime)
		{
			var graphics = ScreenManager.GraphicsDevice;
			var spriteBatch = ScreenManager.SpriteBatch;


			graphics.Clear(Color.Black);


			spriteBatch.Begin();


			spriteBatch.Draw(backgrounds[0], Vector2.Zero, Color.White);

			if (state != GameStateFishing.Start)
			{
				spriteBatch.Draw(Freddy, fredPosition, Color.White);

				spriteBatch.DrawString(font, "Score: " + score, new Vector2(125, 25), Color.AntiqueWhite);

				spriteBatch.DrawString(font, ((int)(60 - roundTimer)).ToString(), new Vector2(100, graphics.Viewport.Height - 200), Color.AntiqueWhite, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
			}

			spriteBatch.Draw(backgrounds[1], Vector2.Zero, Color.White);

			spriteBatch.Draw(Screens[0], Vector2.Zero, Color.White);
			spriteBatch.Draw(Screens[1], Vector2.Zero, Color.White);
			spriteBatch.Draw(Screens[2], Vector2.Zero, Color.White);


			spriteBatch.End();

			

        }
	}
}
