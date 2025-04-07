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


namespace CIS598Project.Rooms
{
	public enum charType 
	{
		Fredbear,
		Chica,
		Bonnie,
		Freddy,
		Foxy,
		GFred

	}
	public class BalloonBarrel : GameScreen
	{
		Game game;

		Texture2D[] background;

		Texture2D[] victoryScreen = new Texture2D[2];

		Texture2D crashScreen;

		string[] ourple = { "You aren't sad about\na balloon, are you?", "You miss your friends,\ndon't you?", "Lucky for you, I know\nwhere they all are.", "They're waiting to give you\na birthday surprise!", "Follow me and I'll\ntake you to them..." };

		int ourpleCount = 0;

		double ourpleTimer = 0;

		Texture2D overlay;

		double animationTimer;

		int frameCounter;

		Song backgroundSong;

		SoundEffect airHorn;

		SoundEffect crying;

		SoundEffect Crash;

		bool overlayShow = false;

		bool victory = false;

		bool crash = false;

		KeyboardState pastKeyboardState;

		KeyboardState currentKeyboardState;

		SpriteFont font;

		ContentManager _content;

		int score = 0;

		int playOnce = 0;

		Player player;

		charType character;

		int counter;

		SoundEffectInstance horn;

		SoundEffectInstance cry;

		SoundEffectInstance crashing;


		public BalloonBarrel(Game game, Player player) 
		{

			this.player = player;

			this.game = game;

			if (!player.foundSecret[0])
			{
				if (player.consecutivePlays[0] == 0)
				{
					character = charType.Fredbear;
					background = new Texture2D[20];
				}
				else if (player.consecutivePlays[0] == 1)
				{
					character = charType.Chica;
					background = new Texture2D[20];
					game.Window.Title = "Susie, lost her dog.";
				}
				else if (player.consecutivePlays[0] == 2)
				{
					character = charType.Bonnie;
					background = new Texture2D[20];
					game.Window.Title = "Jeremy, given a gift.";
				}
				else if (player.consecutivePlays[0] == 3)
				{
					character = charType.Freddy;
					background = new Texture2D[20];
					game.Window.Title = "Gabriel, lost in a dance.";
				}
				else if (player.consecutivePlays[0] == 4)
				{
					character = charType.Foxy;
					background = new Texture2D[20];
					game.Window.Title = "Fritz, sailed the seven seas.";
				}
				else
				{
					game.Window.Title = "Cassidy...";
					character = charType.GFred;
					background = new Texture2D[10];
				}
			}
			else 
			{
				character = charType.Fredbear;
				background = new Texture2D[20];
			}

		}


		public override void Activate()
		{
			base.Activate();

			if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

			for (int i = 0; i < background.Length; i++)
			{
				string add = "";
				if ((i + 1) < 10)
				{
					add = ("000" + (i + 1));
				}
				if ((i + 1) >= 10)
				{
					add = ("00" + (i + 1));
				}
				background[i] = _content.Load<Texture2D>("Balloon_Barrel/Backgrounds/" + character.ToString() + "/" + add);
			}


			if (!player.foundSecret[0])
			{
				if (player.consecutivePlays[0] == 0)
				{
					backgroundSong = _content.Load<Song>("Balloon_Barrel/Sounds/Music/A_slice_and_a_scoop");
					victoryScreen[0] = _content.Load<Texture2D>("Balloon_Barrel/Backgrounds/Fredbear/FredbearB");
				}
				else if (player.consecutivePlays[0] == 1)
				{
					backgroundSong = _content.Load<Song>("Balloon_Barrel/Sounds/Music/A_slice_and_a_scoop_Chica");
					victoryScreen[0] = _content.Load<Texture2D>("Balloon_Barrel/Backgrounds/Chica/ChicaB");
				}
				else if (player.consecutivePlays[0] == 2)
				{
					backgroundSong = _content.Load<Song>("Balloon_Barrel/Sounds/Music/A_slice_and_a_scoop_Bonnie");
					victoryScreen[0] = _content.Load<Texture2D>("Balloon_Barrel/Backgrounds/Bonnie/BonnieB");
				}
				else if (player.consecutivePlays[0] == 3)
				{
					backgroundSong = _content.Load<Song>("Balloon_Barrel/Sounds/Music/A_slice_and_a_scoop_Freddy");
					victoryScreen[0] = _content.Load<Texture2D>("Balloon_Barrel/Backgrounds/Freddy/FreddyB");
				}
				else if (player.consecutivePlays[0] == 4)
				{
					backgroundSong = _content.Load<Song>("Balloon_Barrel/Sounds/Music/A_slice_and_a_scoop_Foxy");
					victoryScreen[0] = _content.Load<Texture2D>("Balloon_Barrel/Backgrounds/Foxy/FoxyB");
				}
				else
				{
					backgroundSong = _content.Load<Song>("Balloon_Barrel/Sounds/Music/Save_me");
					victoryScreen[0] = _content.Load<Texture2D>("Balloon_Barrel/Backgrounds/GFred/Dont");
					victoryScreen[1] = _content.Load<Texture2D>("Balloon_Barrel/Backgrounds/GFred/Follow");
				}
			}
			else
			{
				backgroundSong = _content.Load<Song>("Balloon_Barrel/Sounds/Music/A_slice_and_a_scoop");
				victoryScreen[0] = _content.Load<Texture2D>("Balloon_Barrel/Backgrounds/Fredbear/FredbearB");
			}

			font = _content.Load<SpriteFont>("MiniGame_Font");
			overlay = _content.Load<Texture2D>("Balloon_Barrel/Backgrounds/Overlay");
			crying = _content.Load<SoundEffect>("Balloon_Barrel/Sounds/Soundeffect/cry");
			airHorn = _content.Load<SoundEffect>("Balloon_Barrel/Sounds/Soundeffect/win");
			Crash = _content.Load<SoundEffect>("Duck_Pond/Duck_Pond_Sounds/df");
			crashScreen = _content.Load<Texture2D>("Duck_Pond/Crash");

			MediaPlayer.IsRepeating = true;
			MediaPlayer.Play(backgroundSong);
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



			if (character != charType.GFred && victory != true)
			{
				if (currentKeyboardState.IsKeyDown(Keys.Space) && pastKeyboardState.IsKeyUp(Keys.Space))
				{
					victory = true;
					score += 1500;
					MediaPlayer.Stop();
				}
			}
			else if(character == charType.GFred && !victory) 
			{
				if (currentKeyboardState.IsKeyDown(Keys.Space) && pastKeyboardState.IsKeyUp(Keys.Space))
				{
					victory = true;
					score += 1500;
					MediaPlayer.Stop();
				}
			}

			if (victory == true && character != charType.GFred)
			{
				if (counter >= 0 && horn == null)
				{
					horn = airHorn.CreateInstance();
					horn.Play();
				}
				if (horn.State != SoundState.Playing)
				{
					player.ticketAmount += score;
					player.consecutivePlays[0]++;
					for (int i = 0; i < player.consecutivePlays.Length; i++)
					{
						if (i != 0)
						{
							player.consecutivePlays[i] = 0;
						}

					}
					foreach (var screen in ScreenManager.GetScreens())
						screen.ExitScreen();

					ScreenManager.AddScreen(new GameSelect(player, game), PlayerIndex.One);
				}
			}

			if (victory == true && character == charType.GFred)
			{
				if (cry == null)
				{
					cry = crying.CreateInstance();
					cry.Play();
				}
				if (cry.State != SoundState.Playing)
				{
					overlayShow = true;
				}
			}

			if (victory == true && crash == true) 
			{
				if (crashing == null) 
				{
					crashing = Crash.CreateInstance();
					crashing.Play();
				}
				if (crashing.State != SoundState.Playing) 
				{
					player.consecutivePlays[0]++;
					player.foundSecret[0] = true;
					for (int i = 0; i < player.consecutivePlays.Length; i++)
					{
						if (i != 0)
						{
							player.consecutivePlays[i] = 0;
						}

					}

					player.ticketAmount += score;


					foreach (var screen in ScreenManager.GetScreens())
						screen.ExitScreen();

					ScreenManager.AddScreen(new GameSelect(player, game), PlayerIndex.One);
				
				}
			}
		}

		public override void Draw(GameTime gameTime)
		{
			var graphics = ScreenManager.GraphicsDevice;
			var spriteBatch = ScreenManager.SpriteBatch;

			graphics.Clear(Color.Black);

			spriteBatch.Begin();
			if (character != charType.GFred && victory == false)
			{
				animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (animationTimer > .25)
				{
					frameCounter++;
					if (frameCounter == 19)
					{
						frameCounter = 0;
					}
					animationTimer -= .25;
				}
			}
			else if (character == charType.GFred && victory == false)
			{
				animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (animationTimer > .5)
				{
					frameCounter++;
					if (frameCounter == 10)
					{
						frameCounter = 0;
					}
					animationTimer -= .5;
				}
			}

			spriteBatch.Draw(background[frameCounter], Vector2.Zero, Color.White);

			if (character != charType.GFred && victory)
			{
				spriteBatch.Draw(victoryScreen[0], Vector2.Zero, Color.White);
			}
			else if (character == charType.GFred && victory && !crash)
			{
				spriteBatch.Draw(victoryScreen[0], Vector2.Zero, Color.White);
				if (overlayShow) 
				{
					ourpleTimer += gameTime.ElapsedGameTime.TotalSeconds;
					spriteBatch.Draw(victoryScreen[1], Vector2.Zero, Color.White);
					spriteBatch.Draw(overlay, Vector2.Zero, Color.White);
					spriteBatch.DrawString(font, ourple[ourpleCount], new Vector2(graphics.Viewport.Width / 2 - 350, 250), Color.Purple);
					if (ourpleTimer > 10) 
					{
						ourpleCount++;
						ourpleTimer -= 10;
					}
					if (ourpleCount >= ourple.Length) 
					{
						crash = true;
					}
				}
			}

			if (!overlayShow)
			{
				if (character != charType.GFred || !victory)
				{
					spriteBatch.DrawString(font, "Score: " + score, Vector2.Zero, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);

				}
				else if(character == charType.GFred && victory)
				{
					spriteBatch.DrawString(font, "Score: my friends are gone", Vector2.Zero, Color.White);
				}

				if (counter >= 100 && victory != true)
				{
					spriteBatch.DrawString(font, "PRESS SPACE", new Vector2(graphics.Viewport.Width / 2, 0), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
				}
				counter++;
			}
			if (crash) 
			{
				spriteBatch.Draw(crashScreen, Vector2.Zero, Color.White);
			}

			spriteBatch.End();
		}


	}
}
