using CIS598Project.Collisions;
using CIS598Project.Game_Entities;
using CIS598Project.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


namespace CIS598Project.Rooms
{

	public class Ballpit_Tower : GameScreen
	{
		Game game;

		Texture2D[] background = new Texture2D[9];

		double animationTimer;

		int frameCounter;

		Texture2D[] victoryScreen = new Texture2D[2];

		Texture2D crashScreen;

		Texture2D overlay;

		string[] ourple = { "Why did you try\nto run away?", "I showed you\nwhere your friends\nwere didn't I?", "Hmm...You didn't \nrecognize them? Well \nthey were just sleeping.", "I can make you just like them\nyou can sleep with them.", "No?", "What do you mean no?\nAre you sure, you will\nbe with them forever.", "Okay, I will put you back together..." };

		int ourpleCount = 0;

		double ourpleTimer = 0;

		Song backgroundSong;

		SoundEffect losing;

		SoundEffect airHorn;

		SoundEffect scream;

		SoundEffect Crash;

		SoundEffect laugh;

		bool overlayShow = false;

		bool showAnything = false;

		bool victory = false;

		bool loss = false;

		bool crash = false;

		KeyboardState pastKeyboardState;

		KeyboardState currentKeyboardState;

		SpriteFont font;

		ContentManager _content;

		int score = 0;

		int onepress = 0;


		Player player;

		charType character;

		int counter;

		SoundEffectInstance horn;

		SoundEffectInstance screaming;

		SoundEffectInstance lost;

		SoundEffectInstance crashing;

		SoundEffectInstance laughing;
		Pity_System pity;

		public Ballpit_Tower(Game game, Player player) 
		{

			this.player = player;

			this.game = game;

			pity = new(this.player);

			int randomValue = pity.randomVal();

			if (randomValue <= 70)
			{
				score = 0;
				loss = true;
				character = charType.Bonnie;
			}
			else if (randomValue > 70 && randomValue <= 84)
			{
				score = 1000;
				victory = true;
				character = charType.Bonnie;
			}
			else if (randomValue > 84 && randomValue < 95)
			{
				score = 2000;
				victory = true;
				character = charType.Bonnie;
			}
			else if (randomValue >= 95 && !player.foundSecret[3])
			{
				score = 5000;
				victory = true;
				character = charType.GFred;
			}
			else
			{
				score = 5000;
				victory = true;
				character = charType.Bonnie;
			}

		}


		public override void Activate()
		{
			base.Activate();

			if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

			for (int i = 0; i < background.Length; i++)
			{
				string add = "";
				add = ("000" + (i + 1));
				if (character == charType.Bonnie)
				{
					background[i] = _content.Load<Texture2D>("Ballpit_Tower/Characters/" + character.ToString() + "/" + add);
				}
				else 
				{
					background[i] = _content.Load<Texture2D>("Ballpit_Tower/Characters/Springbonnie/" + add);
				}
			}

			if (character == charType.Bonnie) 
			{
				backgroundSong = _content.Load<Song>("Ballpit_Tower/Sounds/Music/Turtle_Crusher");
				victoryScreen[0] = _content.Load<Texture2D>("Ballpit_Tower/Characters/Bonnie/Bonnie_No_Hit");
				victoryScreen[1] = _content.Load<Texture2D>("Ballpit_Tower/Characters/Bonnie/Bonnie_Win");
			}
			else 
			{
				backgroundSong = _content.Load<Song>("Ballpit_Tower/Sounds/Music/Turtle_Crusher_Found");
				victoryScreen[0] = _content.Load<Texture2D>("Ballpit_Tower/Characters/Springbonnie/Found_you");
				game.Window.Title = "I know you're here...";
			}
			

			font = _content.Load<SpriteFont>("MiniGame_Font");
			overlay = _content.Load<Texture2D>("Balloon_Barrel/Backgrounds/Overlay");
			laugh = _content.Load<SoundEffect>("Ballpit_Tower/Sounds/Soundeffects/ominous_laugh");
			losing = _content.Load<SoundEffect>("Ballpit_Tower/Sounds/Soundeffects/lose");
			scream = _content.Load<SoundEffect>("Ballpit_Tower/Sounds/Soundeffects/scream");
			airHorn = _content.Load<SoundEffect>("Balloon_Barrel/Sounds/Soundeffect/win");
			Crash = _content.Load<SoundEffect>("Duck_Pond/Duck_Pond_Sounds/df");
			crashScreen = _content.Load<Texture2D>("Duck_Pond/Crash");

			MediaPlayer.IsRepeating = true;
			MediaPlayer.Play(backgroundSong);
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

			if (currentKeyboardState.IsKeyDown(Keys.Space) && pastKeyboardState.IsKeyUp(Keys.Space) && onepress == 0)
			{
				onepress++;
				if (character == charType.Bonnie)
				{
					if (loss)
					{
						if (lost == null)
						{
							lost = losing.CreateInstance();
							lost.Play();
						}
					}
					if (victory)
					{
						if (horn == null)
						{
							horn = airHorn.CreateInstance();
							horn.Play();
						}
					}
					MediaPlayer.Stop();
					player.ticketAmount += score;
				}
				else 
				{
					MediaPlayer.Stop();
					laughing.Stop();
					player.ticketAmount += score;
					overlayShow = true;
					game.Window.Title = "Found you...";
				}
			}
			else 
			{
				if (onepress == 0 && character == charType.GFred)
				{
					if (laughing == null)
					{
						laughing = laugh.CreateInstance();
						laughing.Play();
					}
					if (laughing.State != SoundState.Playing)
					{
						laughing = null;
					}
				}
			}



			if (onepress > 0)
			{
				if (lost != null)
				{
					if (lost.State != SoundState.Playing)
					{
						player.ballpitTowerLosses++;
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
				if (horn != null)
				{
					if (horn.State != SoundState.Playing)
					{
						player.ballpitTowerLosses = 0;
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

			if (showAnything == true) 
			{
				if (screaming == null) 
				{
					screaming = scream.CreateInstance();
					screaming.Play();
				}
				if (screaming.State != SoundState.Playing) 
				{
					crash = true;
				}
			}

			if (crash == true) 
			{
				if (crashing == null) 
				{
					crashing = Crash.CreateInstance();
					crashing.Play();
				}
				if (crashing.State != SoundState.Playing) 
				{
					player.foundSecret[3] = true;
					player.ballpitTowerLosses = 0;
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

		public override void Draw(GameTime gameTime)
		{
			var graphics = ScreenManager.GraphicsDevice;
			var spriteBatch = ScreenManager.SpriteBatch;

			graphics.Clear(Color.Black);

			spriteBatch.Begin();
			if (!showAnything)
			{
				animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (animationTimer > .5)
				{
					frameCounter++;
					if (frameCounter == 9)
					{
						frameCounter = 0;
					}
					animationTimer -= .5;
				}
				spriteBatch.Draw(background[frameCounter], Vector2.Zero, Color.White);

				if (character != charType.GFred && onepress > 0 && loss)
				{
					spriteBatch.Draw(victoryScreen[0], Vector2.Zero, Color.White);

				}
				else if (character != charType.GFred && onepress > 0 && victory)
				{
					spriteBatch.Draw(victoryScreen[1], Vector2.Zero, Color.White);
				}
				else if (character == charType.GFred && onepress > 0 && victory && !crash)
				{
					spriteBatch.Draw(victoryScreen[0], Vector2.Zero, Color.White);
					if (overlayShow)
					{
						ourpleTimer += gameTime.ElapsedGameTime.TotalSeconds;
						spriteBatch.Draw(overlay, Vector2.Zero, Color.White);
						spriteBatch.DrawString(font, ourple[ourpleCount], new Vector2(graphics.Viewport.Width / 2 - 350, 250), Color.Purple);
						if (ourpleTimer > 10)
						{
							ourpleCount++;
							ourpleTimer -= 10;
						}
						if (ourpleCount >= ourple.Length)
						{
							showAnything = true;
						}
					}
				}

				if (!overlayShow)
				{
					if (onepress == 0)
					{
						spriteBatch.DrawString(font, "Score: ", Vector2.Zero, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);

					}

					if (counter >= 25 && onepress == 0)
					{
						spriteBatch.DrawString(font, "PRESS SPACE", new Vector2(graphics.Viewport.Width / 2, 0), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
					}

					if (loss == true && onepress > 0)
					{
						spriteBatch.DrawString(font, "Score: " + score, Vector2.Zero, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
						if (counter >= 10)
						{
							spriteBatch.DrawString(font, "TRY AGAIN!!!", new Vector2(graphics.Viewport.Width / 2, 0), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
						}
					}

					if (victory == true && onepress > 0)
					{
						spriteBatch.DrawString(font, "Score: " + score, Vector2.Zero, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
						if (counter >= 5)
						{
							spriteBatch.DrawString(font, "WINNER!!!", new Vector2(graphics.Viewport.Width / 2, 0), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
						}
					}
					counter++;
					if (onepress == 0)
					{
						if (counter > 50)
						{
							counter = 0;
						}
					}
					else
					{
						if (loss)
						{
							if (counter > 20)
							{
								counter = 0;
							}
						}
						if (victory)
						{
							if (counter > 10)
							{
								counter = 0;
							}
						}
					}
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
