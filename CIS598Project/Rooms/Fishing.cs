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
using System.Threading.Tasks;






namespace CIS598Project.Rooms
{

	public enum GameStateFishing
	{
		Start = 0,
		Play,
		Win,
		Secret
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

		Texture2D Pearl;

		Texture2D crashScreen;

		Song backgroundMusic;

		SoundEffect release;

		SoundEffect GameStateChange;

		SoundEffect Crash;

		SoundEffectInstance StateChange;

		int showOnce = 0;

		SoundEffectInstance crashing;

		SoundEffectInstance released;


		GameStateFishing state = GameStateFishing.Start;


		Fish[][] fishes = new Fish[3][];


		bool releasePlunger = false;

		bool collides = false;

		bool crash = false;

		bool end = false;

		int j = 0;

		private double lowerTime = 0;

		private double textTime = 0;

		private double startTime = 0;

		private double roundTimer = 0;

		string[] ourple = { "Impressive, I've never seen \nanyone fish like that before.", "That's right, I'm \nyour pal, Fredbear.", "Say you seem like you would\nbe an excellent crew on a ship.", "Do you know what that means?", "Yes, Foxy!", "Foxy wants to meet you backstage\nto make you his second mate", "Really!", "Follow me and \nI'll take you to him..." };

		int ourpleCount = 0;

		bool purpleDialogue = false;

		double ourpleTimer = 0;

		int foxyCount = 5;

		double foxyTimer = 0;

		bool pearlMoveLeft = false;

		int fishRow = 0;

		int fishColumn = 0;


		private KeyboardState pastKeyboardState;
		private KeyboardState currentKeyboardState;

		SpriteFont font;

		ContentManager _content;

		int round = 0; //Starts at 0 and ends at 2 to correlate with the fish array

		Vector2 pearlLocation;
			
		BoundingRectangle plunger = new(0, 0, 104, 68);

		BoundingRectangle floor = new(0, 980, 1920, 100);

		BoundingRectangle pearl = new(0, 0, 128, 128);

		Vector2 plungerPosition = new(0, 200);

		Vector2 fredPosition = new(0,125);

		int score = 0;

		Player player;

		public Fishing(Game game,Player player) 
		{
			this.game = game;
			this.player = player;
			for (int i = 0; i < fishes.Length; i++)
			{
				fishes[i] = new Fish[12];
			}
        }

		public override void Activate()
		{
			base.Activate();

			if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");


			Freddy = _content.Load<Texture2D>("Fishing/Textures/Ship/Fred/Fred_Left");
			Plunger = _content.Load<Texture2D>("Fishing/Textures/Ship/Fishing_supplies/Plunger");


			//Round 1 Fish
			fishes[0][0] = new(new Vector2(500, 325), 100, game.GraphicsDevice.Viewport.Width - 150);
			fishes[0][1] = new(new Vector2(550, 425), 200, game.GraphicsDevice.Viewport.Width - 150);
			fishes[0][2] = new(new Vector2(900, 450), 300, game.GraphicsDevice.Viewport.Width - 150);
			fishes[0][3] = new(new Vector2(0, 500), 400, game.GraphicsDevice.Viewport.Width - 150);
			fishes[0][4] = new(new Vector2(875, 575), 500, game.GraphicsDevice.Viewport.Width - 150);
			fishes[0][5] = new(new Vector2(655, 775), 600, game.GraphicsDevice.Viewport.Width - 150);
			fishes[0][6] = new(new Vector2(950, 300), 100, game.GraphicsDevice.Viewport.Width - 150);
			fishes[0][7] = new(new Vector2(350, 350), 200, game.GraphicsDevice.Viewport.Width - 150);
			fishes[0][8] = new(new Vector2(90, 470), 300, game.GraphicsDevice.Viewport.Width - 150);
			fishes[0][9] = new(new Vector2(250, 525), 400, game.GraphicsDevice.Viewport.Width - 150);
			fishes[0][10] = new(new Vector2(23, 590), 500, game.GraphicsDevice.Viewport.Width - 150);
			fishes[0][11] = new(new Vector2(900, 700), 600, game.GraphicsDevice.Viewport.Width - 150);

			//Round 2 Fish
			fishes[1][0] = new(new Vector2(500, 325), 100, game.GraphicsDevice.Viewport.Width - 150);
			fishes[1][1] = new(new Vector2(550, 425), 200, game.GraphicsDevice.Viewport.Width - 150);
			fishes[1][2] = new(new Vector2(900, 450), 300, game.GraphicsDevice.Viewport.Width - 150);
			fishes[1][3] = new(new Vector2(0, 500), 400, game.GraphicsDevice.Viewport.Width - 150);
			fishes[1][4] = new(new Vector2(875, 575), 500, game.GraphicsDevice.Viewport.Width - 150);
			fishes[1][5] = new(new Vector2(655, 775), 600, game.GraphicsDevice.Viewport.Width - 150);
			fishes[1][6] = new(new Vector2(950, 300), 100, game.GraphicsDevice.Viewport.Width - 150);
			fishes[1][7] = new(new Vector2(350, 350), 200, game.GraphicsDevice.Viewport.Width - 150);
			fishes[1][8] = new(new Vector2(90, 470), 300, game.GraphicsDevice.Viewport.Width - 150);
			fishes[1][9] = new(new Vector2(250, 525), 400, game.GraphicsDevice.Viewport.Width - 150);
			fishes[1][10] = new(new Vector2(23, 590), 500, game.GraphicsDevice.Viewport.Width - 150);
			fishes[1][11] = new(new Vector2(900, 700), 600, game.GraphicsDevice.Viewport.Width - 150);

			//Round 3 Fish
			fishes[2][0] = new(new Vector2(500, 325), 100, game.GraphicsDevice.Viewport.Width - 150);
			fishes[2][1] = new(new Vector2(550, 425), 200, game.GraphicsDevice.Viewport.Width - 150);
			fishes[2][2] = new(new Vector2(900, 450), 300, game.GraphicsDevice.Viewport.Width - 150);
			fishes[2][3] = new(new Vector2(0, 500), 400, game.GraphicsDevice.Viewport.Width - 150);
			fishes[2][4] = new(new Vector2(875, 575), 500, game.GraphicsDevice.Viewport.Width - 150);
			fishes[2][5] = new(new Vector2(655, 775), 600, game.GraphicsDevice.Viewport.Width - 150);
			fishes[2][6] = new(new Vector2(950, 300), 100, game.GraphicsDevice.Viewport.Width - 150);
			fishes[2][7] = new(new Vector2(350, 350), 200, game.GraphicsDevice.Viewport.Width - 150);
			fishes[2][8] = new(new Vector2(90, 470), 300, game.GraphicsDevice.Viewport.Width - 150);
			fishes[2][9] = new(new Vector2(250, 525), 400, game.GraphicsDevice.Viewport.Width - 150);
			fishes[2][10] = new(new Vector2(23, 590), 500, game.GraphicsDevice.Viewport.Width - 150);
			fishes[2][11] = new(new Vector2(900, 700), 600, game.GraphicsDevice.Viewport.Width - 150);






			Screens[0] = _content.Load<Texture2D>("Fruity_Maze/Textures/Screen/scanlines");
			Screens[1] = _content.Load<Texture2D>("Fruity_Maze/Textures/Screen/Screen");
			Screens[2] = _content.Load<Texture2D>("Fruity_Maze/Textures/Screen/ScreenBorder");

            for (int i = 0; i < fishes.Length; i++)
            {
				for (int j = 0; j < fishes[i].Length; j++) 
				{
					fishes[i][j].LoadContent(_content);
				}
            }

			Pearl = _content.Load<Texture2D>("Fishing/Textures/Ship/Fishing_supplies/pearl");
			release = _content.Load<SoundEffect>("Fishing/Sounds/Soundeffects/plunger_release");
			GameStateChange = _content.Load<SoundEffect>("Fishing/Sounds/Soundeffects/plunger_collect");
            backgroundMusic = _content.Load<Song>("Fishing/Sounds/Song/Water_theme");

			Crash = _content.Load<SoundEffect>("Duck_Pond/Duck_Pond_Sounds/df");
			crashScreen = _content.Load<Texture2D>("Duck_Pond/Crash");

			backgrounds[0] = _content.Load<Texture2D>("Fishing/Textures/Backgrounds/Background");
			backgrounds[1] = _content.Load<Texture2D>("Fishing/Textures/Backgrounds/Water");

			pearlLocation = new Vector2(0, game.GraphicsDevice.Viewport.Height - 200);

			font = _content.Load<SpriteFont>("MiniGame_Font");
			
			for (int i = 5; i >= 0; i--) 
			{
				Foxy[i] = _content.Load<Texture2D>("Fishing/Textures/Backgrounds/Foxy/Foxy" + (i + 1));
			}

			MediaPlayer.Play(backgroundMusic);
			MediaPlayer.IsRepeating = true;
		}

        public override void Unload()
        {
            base.Unload();


            _content.Unload();
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

			if (state == GameStateFishing.Start) 
			{
                plungerPosition = new(-50, 200);

                fredPosition = new(0, 125);

				roundTimer = 0;

				pearlLocation = new Vector2(0, game.GraphicsDevice.Viewport.Height - 200);

			}

			if (state == GameStateFishing.Play)
			{
				if (releasePlunger == false)
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


					if (pearlLocation.X >= game.GraphicsDevice.Viewport.Width - 125)
					{
						pearlMoveLeft = true;
					}
					else if (pearlLocation.X <= 0)
					{
						pearlMoveLeft = false;
					}

					if (pearlMoveLeft)
					{
						pearlLocation.X -= 4;
					}
					else
					{
						pearlLocation.X += 4;
					}

					pearl.X = pearlLocation.X;  // Update bounds position X
					pearl.Y = pearlLocation.Y;  // Update bounds position Y

					roundTimer += .025;

					if (roundTimer >= 60)
					{
						score += 0;
						round += 1;
						state = GameStateFishing.Start;
					}

					if (currentKeyboardState.IsKeyDown(Keys.Space) && pastKeyboardState.IsKeyUp(Keys.Space))
					{
						release.Play();
						releasePlunger = true;
					}

					for (int i = round; i < (1 + round); i++)
					{
						for (int j = 0; j < fishes[i].Length; j++)
						{
							fishes[i][j].Update(gameTime);
						}
					}


					plungerPosition.X = fredPosition.X - 50;

				}


				if (releasePlunger)
				{
					if (collides == false)
					{
						plungerPosition.Y += 10;

						for (int i = round; i < (1 + round); i++)
						{
							for (int j = 0; j < fishes[i].Length; j++)
							{
								fishes[i][j].Update(gameTime);
							}
						}

						if (pearlLocation.X >= game.GraphicsDevice.Viewport.Width - 125)
						{
							pearlMoveLeft = true;
						}
						else if (pearlLocation.X <= 0)
						{
							pearlMoveLeft = false;
						}

						if (pearlMoveLeft)
						{
							pearlLocation.X -= 4;
						}
						else
						{
							pearlLocation.X += 4;
						}

						pearl.X = pearlLocation.X;  // Update bounds position X
						pearl.Y = pearlLocation.Y;  // Update bounds position Y

					}

					for (int i = round; i < (1 + round); i++)
					{
						for (int j = 0; j < fishes[i].Length; j++)
						{
							if (plunger.collidesWith(fishes[i][j].bounds))
							{
								collides = true;
								fishRow = i;
								fishColumn = j;
							}
						}
					}

					if (plunger.collidesWith(pearl))
					{
						collides = true;
						if (StateChange == null)
						{
							score += 1000;
							StateChange = GameStateChange.CreateInstance();
							StateChange.Play();
						}
						if (StateChange.State != SoundState.Playing)
						{
							round += 1;
							state = GameStateFishing.Start;
							StateChange = null;
							releasePlunger = false;
							collides = false;
						}
					}

					if (plunger.collidesWith(floor))
					{
						collides = true;
						if (StateChange == null)
						{
							score += 0;
							StateChange = GameStateChange.CreateInstance();
							StateChange.Play();
						}
						if (StateChange.State != SoundState.Playing)
						{
							releasePlunger = false;
							collides = false;
							round += 1;
							state = GameStateFishing.Start;
							StateChange = null;
						}
					}

					if (collides)
					{
						if (StateChange == null)
						{
							score += fishes[fishRow][fishColumn].score;
							StateChange = GameStateChange.CreateInstance();
							StateChange.Play();
						}
						if (StateChange.State != SoundState.Playing)
						{
							round += 1;
							state = GameStateFishing.Start;
							StateChange = null;
							releasePlunger = false;
							collides = false;
						}

					}

					}
				}

			if (round == 3)
			{
				StateChange = null;
				releasePlunger = false;
				collides = false;
				if (score < 1500 || player.foundSecret[7])
				{
					state = GameStateFishing.Win;
				}
				else
				{
					MediaPlayer.Stop();
					state = GameStateFishing.Secret;
				}
			}

			if (state == GameStateFishing.Win || state == GameStateFishing.Secret)
			{
				if (released == null)
				{
					MediaPlayer.Stop();
					released = release.CreateInstance();
					released.Play();
				}
				if (released.State != SoundState.Playing)
				{
					if (player.foundSecret[7] || score < 1500)
					{
						player.ticketAmount += score;
						for (int i = 0; i < player.consecutivePlays.Length; i++)
						{
								player.consecutivePlays[i] = 0;

						}
						foreach (var screen in ScreenManager.GetScreens())
							screen.ExitScreen();

						Unload();

						ScreenManager.AddScreen(new MainGame_Screen(player, game), PlayerIndex.One);
					}
				}
			}

			if (crash)
				{
					if (crashing == null)
					{
						crashing = Crash.CreateInstance();
						crashing.Play();
					}
					if (crashing.State != SoundState.Playing)
					{
					for (int i = 0; i < player.consecutivePlays.Length; i++)
					{
							player.consecutivePlays[i] = 0;
					}
						if (state == GameStateFishing.Secret)
						{
							player.foundSecret[7] = true;
							player.ticketAmount += score;
						}

						foreach (var screen in ScreenManager.GetScreens())
							screen.ExitScreen();

						Unload();

						ScreenManager.AddScreen(new MainGame_Screen(player, game), PlayerIndex.One);
					}
				}

				plunger.X = plungerPosition.X;
				plunger.Y = plungerPosition.Y;

			}


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

			if (state == GameStateFishing.Start) 
			{
				textTime += gameTime.ElapsedGameTime.TotalSeconds;

				if (textTime <= 2)
				{
					spriteBatch.DrawString(font, "ROUND " + (round + 1) + "!!!", new Vector2(graphics.Viewport.Width / 2 - 150, graphics.Viewport.Height / 2 - 100), Color.AntiqueWhite);
				}
				else if (textTime >= 2 && textTime < 4)
				{
					spriteBatch.DrawString(font, "READY!!!", new Vector2(graphics.Viewport.Width / 2 - 150, graphics.Viewport.Height / 2 - 100), Color.AntiqueWhite);
				}
				else if (textTime >= 4 && textTime < 6)
				{
					spriteBatch.DrawString(font, "GO!!!", new Vector2(graphics.Viewport.Width / 2 - 150, graphics.Viewport.Height / 2 - 100), Color.AntiqueWhite);
				}
				else { 
					state = GameStateFishing.Play;
                    textTime = 0;
                }
            }

			if (state != GameStateFishing.Start)
			{

				spriteBatch.Draw(Freddy, fredPosition, Color.White);


				if (round != 3)
				{
					for (int i = round; i < (round + 1); i++)
					{
						for (int j = 0; j < fishes[i].Length; j++)
						{
							fishes[i][j].Draw(gameTime, spriteBatch);
						}
					}
				}
				else 
				{
					for (int i = 2; i < 3; i++)
					{
						for (int j = 0; j < fishes[i].Length; j++)
						{
							fishes[i][j].Draw(gameTime, spriteBatch);
						}
					}
				}

				spriteBatch.Draw(Pearl, pearlLocation, Color.White);

				spriteBatch.Draw(Plunger, plungerPosition, Color.White);

				spriteBatch.DrawString(font, "Score: " + score, new Vector2(125, 25), Color.AntiqueWhite);

				spriteBatch.DrawString(font, ((int)(60 - roundTimer)).ToString(), new Vector2(100, graphics.Viewport.Height - 200), Color.AntiqueWhite, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
			}

			if (state == GameStateFishing.Win || state == GameStateFishing.Secret)
			{

				spriteBatch.DrawString(font, "You scored: " + score + " tickets!!!", new Vector2(graphics.Viewport.Width / 2 - 150, graphics.Viewport.Height / 2 - 100), Color.AntiqueWhite);
			}


			spriteBatch.Draw(backgrounds[1], Vector2.Zero, Color.White);

			spriteBatch.Draw(Screens[0], Vector2.Zero, Color.White);
			spriteBatch.Draw(Screens[1], Vector2.Zero, Color.White);
			spriteBatch.Draw(Screens[2], Vector2.Zero, Color.White);


			if (state == GameStateFishing.Secret)
			{
				foxyTimer += gameTime.ElapsedGameTime.TotalSeconds;

				if (foxyTimer >= 3 && foxyCount > 0)
				{
					foxyCount--;
					foxyTimer -= 3;
				}

				if (foxyCount == 0)
				{
					purpleDialogue = true;
				}

				spriteBatch.Draw(Foxy[foxyCount], Vector2.Zero, Color.White);
			}

			if (purpleDialogue && crash == false)
			{
				ourpleTimer += gameTime.ElapsedGameTime.TotalSeconds;
				spriteBatch.DrawString(font, ourple[ourpleCount], new Vector2(graphics.Viewport.Width / 2 - 250, graphics.Viewport.Height / 2 - 100), Color.Purple);
				if (ourpleTimer > 10)
				{
					ourpleCount++;
					ourpleTimer -= 10;
				}
				if (ourpleCount == ourple.Length)
				{
					crash = true;
				}
			}

			if (crash)
			{
				spriteBatch.Draw(crashScreen, Vector2.Zero, Color.White);
			}


			spriteBatch.End();

			

        }
	}
}
