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






namespace CIS598Project.Rooms
{

	public enum GameStateSecurity 
	{
		Start = 0,
		Play,
		Guess,
		Results,
		Intermission,
		GameOver,
		Win,
		Secret
	}
	public class Security : GameScreen
	{

		/// <summary>
		/// The game this class belongs to
		/// </summary>
		Game game;

		Texture2D backgrounds;

		Texture2D[] bodies = new Texture2D[12];

		Texture2D[] bands = new Texture2D[8];

		Texture2D[] eyes = new Texture2D[4];

		bool showEyes = true;

		Texture2D[] hands = new Texture2D[12];

		Texture2D[] shirts = new Texture2D[12];

		Texture2D[] Screens = new Texture2D[3];

		Texture2D[] signs = new Texture2D[3];

		Texture2D[] Fred = new Texture2D[6];

		Texture2D[] background = new Texture2D[2];


		Texture2D crashScreen;

		Song backgroundMusic;

		SoundEffect selection;

		SoundEffect nextRound;

		SoundEffect failed;

		SoundEffect boo;

		SoundEffect yay;

		SoundEffect Crash;

		SoundEffect horn;

		SoundEffectInstance crashing;

		SoundEffectInstance result;

		SoundEffectInstance airHorn;

		BoundingRectangle increase;

		BoundingRectangle decrease;

		Security_Children chosenChild;

		Security_Children[][] children = new Security_Children[4][];

		GameStateSecurity gameState;

		bool crash = false;

		bool fail = false;


		private int childAnimationFrame = 0;

		private double childAnimationTimer = 0;

		private int chosenAnimationFrame = 0;

		private double chosenAnimationTimer = 0;

		double buffer = 0;

		Vector2 childPosition;

		int childCounter = 0;

		bool hasChildTouchedDoor = false;


		string[] ourple = { "Hi there, I noticed that\nyou just beat this game.", "You know what we do for\nguests that beat this game?", "We give them a special badge\nsince you know you're now a \npart of our security team!", "Hmm?", "Right, Freddy's Security Team!", "All we need to make you a part\nof the team is that badge.", "Follow me and I'll\nget you that badge." };

		int ourpleCount = 0;

		bool purpleDialogue = false;


		double ourpleTimer = 0;

		int fredCount = 5;

		double fredTimer = 0;

		double textTimer = 0;

		double resultsTimer = 0;

		int chosenChildPosition = 0;

		int guess = 0;

		bool showText = false;

		int currentRound = 1;

		TimeSpan currentPosition;

		private MouseState pastMousePosition;
		private MouseState currentMousePosition;

		SpriteFont font;

		ContentManager _content;

		int count = 0;
			
		BoundingRectangle mouse = new(0, 0, 64, 64);
		BoundingRectangle increaseBox;
		BoundingRectangle decreaseBox;
		BoundingRectangle GO;


        int score = 0;
        Player player;

		public Security(Game game,Player player) 
		{
			this.game = game;
			this.player = player;
			for (int i = 0; i < children.Length; i++) 
			{
				children[i] = new Security_Children[4 + (i * 2)];
			}

			
		}

		public override void Activate()
		{
			base.Activate();

			if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");
			bodies[0] = _content.Load<Texture2D>("Security/Textures/Children/Bodies/Kid1_Base_1");
			bodies[1] = _content.Load<Texture2D>("Security/Textures/Children/Bodies/Kid1_Base_2");
			bodies[2] = _content.Load<Texture2D>("Security/Textures/Children/Bodies/Kid1_Base_3");
			bodies[3] = _content.Load<Texture2D>("Security/Textures/Children/Bodies/Kid2_Base_1");
			bodies[4] = _content.Load<Texture2D>("Security/Textures/Children/Bodies/Kid2_Base_2");
			bodies[5] = _content.Load<Texture2D>("Security/Textures/Children/Bodies/Kid2_Base_3");
			bodies[6] = _content.Load<Texture2D>("Security/Textures/Children/Bodies/Kid3_Base_1");
			bodies[7] = _content.Load<Texture2D>("Security/Textures/Children/Bodies/Kid3_Base_2");
			bodies[8] = _content.Load<Texture2D>("Security/Textures/Children/Bodies/Kid3_Base_3");
			bodies[9] = _content.Load<Texture2D>("Security/Textures/Children/Bodies/Kid4_Base_1");
			bodies[10] = _content.Load<Texture2D>("Security/Textures/Children/Bodies/Kid4_Base_2");
			bodies[11] = _content.Load<Texture2D>("Security/Textures/Children/Bodies/Kid4_Base_2");

			hands[0] = _content.Load<Texture2D>("Security/Textures/Children/Hands/Hands1_1");
			hands[1] = _content.Load<Texture2D>("Security/Textures/Children/Hands/Hands1_2");
			hands[2] = _content.Load<Texture2D>("Security/Textures/Children/Hands/Hands1_3");
			hands[3] = _content.Load<Texture2D>("Security/Textures/Children/Hands/Hands2_1");
			hands[4] = _content.Load<Texture2D>("Security/Textures/Children/Hands/Hands2_2");
			hands[5] = _content.Load<Texture2D>("Security/Textures/Children/Hands/Hands2_3");
			hands[6] = _content.Load<Texture2D>("Security/Textures/Children/Hands/Hand3_1");
			hands[7] = _content.Load<Texture2D>("Security/Textures/Children/Hands/Hand3_2");
			hands[8] = _content.Load<Texture2D>("Security/Textures/Children/Hands/Hand3_3");
			hands[9] = _content.Load<Texture2D>("Security/Textures/Children/Hands/Hand4_1");
			hands[10] = _content.Load<Texture2D>("Security/Textures/Children/Hands/Han4_2");
			hands[11] = _content.Load<Texture2D>("Security/Textures/Children/Hands/Hand4_3");

			bands[0] = _content.Load<Texture2D>("Security/Textures/Children/Bands/Blue1");
			bands[1] = _content.Load<Texture2D>("Security/Textures/Children/Bands/Blue2");
			bands[2] = _content.Load<Texture2D>("Security/Textures/Children/Bands/Green1");
			bands[3] = _content.Load<Texture2D>("Security/Textures/Children/Bands/Green2");
			bands[4] = _content.Load<Texture2D>("Security/Textures/Children/Bands/Red1");
			bands[5] = _content.Load<Texture2D>("Security/Textures/Children/Bands/Red2");
			bands[6] = _content.Load<Texture2D>("Security/Textures/Children/Bands/Yellow1");
			bands[7] = _content.Load<Texture2D>("Security/Textures/Children/Bands/Yellow2");

			eyes[0] = _content.Load<Texture2D>("Security/Textures/Children/Eyes/Blue_eyes");
			eyes[1] = _content.Load<Texture2D>("Security/Textures/Children/Eyes/Green_eyes");
			eyes[2] = _content.Load<Texture2D>("Security/Textures/Children/Eyes/Hazel_eyes");
			eyes[3] = _content.Load<Texture2D>("Security/Textures/Children/Eyes/White_eyes");

			shirts[0] = _content.Load<Texture2D>("Security/Textures/Children/Shirts/shirt1_1");
			shirts[1] = _content.Load<Texture2D>("Security/Textures/Children/Shirts/shirt1_2");
			shirts[2] = _content.Load<Texture2D>("Security/Textures/Children/Shirts/shirt1_3");
			shirts[3] = _content.Load<Texture2D>("Security/Textures/Children/Shirts/shirt2_1");
			shirts[4] = _content.Load<Texture2D>("Security/Textures/Children/Shirts/shirt2_2");
			shirts[5] = _content.Load<Texture2D>("Security/Textures/Children/Shirts/shirt2_3");
			shirts[6] = _content.Load<Texture2D>("Security/Textures/Children/Shirts/shirt3_1");
			shirts[7] = _content.Load<Texture2D>("Security/Textures/Children/Shirts/shirt3_2");
			shirts[8] = _content.Load<Texture2D>("Security/Textures/Children/Shirts/shirt3_3");
			shirts[9] = _content.Load<Texture2D>("Security/Textures/Children/Shirts/shirt4_1");
			shirts[10] = _content.Load<Texture2D>("Security/Textures/Children/Shirts/shirt4_2");
			shirts[11] = _content.Load<Texture2D>("Security/Textures/Children/Shirts/shirt4_3");


			background[0] = _content.Load<Texture2D>("Security/Textures/Backgrounds/Camera_off");
			background[1] = _content.Load<Texture2D>("Security/Textures/Backgrounds/Camera_on");
			signs[0] = _content.Load <Texture2D>("Security/Textures/Misc/Increase");
			signs[1] = _content.Load<Texture2D>("Security/Textures/Misc/Decrease");
			signs[2] = _content.Load<Texture2D>("Security/Textures/Misc/Go");
			selection = _content.Load<SoundEffect>("Security/Sounds/Soundeffects/increaseDecrease");
			nextRound = _content.Load<SoundEffect>("Security/Sounds/Soundeffects/nextround");
			failed = _content.Load<SoundEffect>("Security/Sounds/Soundeffects/yourdone");
			Crash = _content.Load<SoundEffect>("Duck_Pond/Duck_Pond_Sounds/df");
			crashScreen = _content.Load<Texture2D>("Duck_Pond/Crash");
			font = _content.Load<SpriteFont>("MiniGame_Font");
			horn = _content.Load<SoundEffect>("Balloon_Barrel/Sounds/Soundeffect/win");
			backgroundMusic = _content.Load<Song>("Security/Sounds/Music/Alchemists_Fantasy");

			Screens[0] = _content.Load<Texture2D>("Fruity_Maze/Textures/Screen/scanlines");
			Screens[1] = _content.Load<Texture2D>("Fruity_Maze/Textures/Screen/Screen");
			Screens[2] = _content.Load<Texture2D>("Fruity_Maze/Textures/Screen/ScreenBorder");

			boo = _content.Load<SoundEffect>("Security/Sounds/Soundeffects/boo");
			yay = _content.Load<SoundEffect>("Security/Sounds/Soundeffects/yay");


			childPosition = new Vector2(0, game.GraphicsDevice.Viewport.Height / 2);
			increaseBox = new(game.GraphicsDevice.Viewport.Width / 2 + 150, game.GraphicsDevice.Viewport.Height / 2, 128, 128);
			decreaseBox = new(game.GraphicsDevice.Viewport.Width / 2 + 150, game.GraphicsDevice.Viewport.Height / 2 + 150, 128, 128);
			GO = new(game.GraphicsDevice.Viewport.Width / 2 + 350, game.GraphicsDevice.Viewport.Height / 2 + 100, 128, 128);

			for (int i = 5; i >= 0; i--) 
			{
				Fred[i] = _content.Load<Texture2D>("Security/Textures/Reflections/Fred/Fred" + (i + 1));
			}

			MediaPlayer.Play(backgroundMusic);
			MediaPlayer.IsRepeating = true;
		}

		private Security_Children GenerateChildren() 
		{
			Security_Children child = new();
			return child.generateChild(currentRound);
		}

		private void GenerateGroup() 
		{
			chosenChild = GenerateChildren();
			Random ran = new();
			int randomNum = ran.Next(0, children[currentRound - 1].Length);
			for (int i = 0; i < children[currentRound - 1].Length; i++) 
			{
				if (i == randomNum)
				{
					children[currentRound - 1][i] = chosenChild;
					chosenChildPosition = i;
				}
				else 
				{

					Security_Children child = GenerateChildren();
					while (chosenChild.bandColor == child.bandColor && chosenChild.color == child.color && chosenChild.shirtColor == child.shirtColor && chosenChild.eyeColor == child.eyeColor) 
					{
						child = GenerateChildren();
					}
					children[currentRound - 1][i] = child;
				}
			}
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, false);

			pastMousePosition = currentMousePosition;
			currentMousePosition = Mouse.GetState();

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



			Vector2 mousePosition = new Vector2(currentMousePosition.X, currentMousePosition.Y);

			if (gameState == GameStateSecurity.Play)
			{
				if (hasChildTouchedDoor == true && childPosition.X <= -250)
				{
					hasChildTouchedDoor = false;
					childPosition.X = -250;
					childCounter++;
				}

				if (childPosition.X < game.GraphicsDevice.Viewport.Width - 249 && hasChildTouchedDoor == false)
				{
					childPosition.X += 5f + (currentRound * 2.5f);
				}

				if (childPosition.X >= game.GraphicsDevice.Viewport.Width - 249 && hasChildTouchedDoor == false)
				{
					hasChildTouchedDoor = true;
				}

				if (hasChildTouchedDoor == true)
				{
				
					childPosition.X -= 5f + currentRound * 2.5f;
				}

				if (childCounter == children[currentRound - 1].Length)
				{
					gameState = GameStateSecurity.Guess;
					childCounter = 0;
				}
			}
			else if (gameState == GameStateSecurity.Guess) 
			{
				if (mouse.collidesWith(increaseBox))
				{
					if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
					{
						guess++;
						if (guess >= children[currentRound - 1].Length)
						{
							guess = 0;
						}
						selection.Play();
					}
				}
				else if (mouse.collidesWith(decreaseBox))
				{
					if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
					{
						guess--;
						if (guess <= 0)
						{
							guess = children[currentRound - 1].Length - 1;
						}
						selection.Play();
					}
				}
				else if (mouse.collidesWith(GO)) 
				{
					if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
					{
						currentPosition = MediaPlayer.PlayPosition;
						MediaPlayer.Stop();
						selection.Play();
						if (chosenChildPosition != guess) 
						{
							fail = true;
						}
						chosenAnimationFrame = 0;
						guess = 0;
						gameState = GameStateSecurity.Results;
					}
				}
			}

			if (gameState == GameStateSecurity.Win) 
			{
				if (airHorn == null)
				{
					MediaPlayer.Stop();
					airHorn = horn.CreateInstance();
					airHorn.Play();
				}
				if (airHorn.State != SoundState.Playing)
				{
					player.ticketAmount += score;
					if (player.foundSecret[6])
					{
						for (int i = 0; i < player.consecutivePlays.Length; i++)
						{
							if (i != 0)
							{
								player.consecutivePlays[i] = 0;
							}

						}
						foreach (var screen in ScreenManager.GetScreens())
							screen.ExitScreen();

						ScreenManager.AddScreen(new MainGame_Screen(player, game), PlayerIndex.One);
					}
					else
					{
						gameState = GameStateSecurity.Secret;
					}
				}
			}

			if (gameState == GameStateSecurity.GameOver) 
			{
				if (result == null)
				{
					result = failed.CreateInstance();
					result.Play();
				}
				if (result.State != SoundState.Playing)
				{
					player.ticketAmount += score;
					for (int i = 0; i < player.consecutivePlays.Length; i++)
					{
						if (i != 0)
						{
							player.consecutivePlays[i] = 0;
						}

					}

					foreach (var screen in ScreenManager.GetScreens())
						screen.ExitScreen();

					ScreenManager.AddScreen(new MainGame_Screen(player, game), PlayerIndex.One);
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
					player.foundSecret[6] = true;
					for (int i = 0; i < player.consecutivePlays.Length; i++)
					{
						if (i != 0)
						{
							player.consecutivePlays[i] = 0;
						}

					}
					foreach (var screen in ScreenManager.GetScreens())
						screen.ExitScreen();

					ScreenManager.AddScreen(new MainGame_Screen(player, game), PlayerIndex.One);
				}
			}

            mouse.X = mousePosition.X;
			mouse.Y = mousePosition.Y;

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

			if (gameState == GameStateSecurity.Play || gameState == GameStateSecurity.Guess)
			{
				spriteBatch.Draw(background[1], Vector2.Zero, Color.White);
				if (gameState == GameStateSecurity.Guess) 
				{
					textTimer += gameTime.ElapsedGameTime.TotalSeconds;
					if(textTimer >= 2) 
					{
						showText = !showText;
						textTimer -= 2;
					}
					if (showText)
					{
						spriteBatch.DrawString(font, "GUESSING TIME!!!\n" , new Vector2(graphics.Viewport.Width / 2 - 250, 50), Color.White);
					}
				}
				spriteBatch.DrawString(font, "Chosen kid: ", new Vector2(75, 50), Color.White);
				if (gameState == GameStateSecurity.Play) 
				{
					spriteBatch.DrawString(font, "Current kid: " + (childCounter + 1), new Vector2(75, 150), Color.White);
				}

				/*chosenAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (chosenAnimationTimer >= .66)
				{
					chosenAnimationFrame++;
					if (chosenAnimationFrame == 3)
					{
						chosenAnimationFrame = 0;
					}
					chosenAnimationTimer -= .66;
				}*/

				spriteBatch.Draw(bodies[((int)chosenChild.color * 3) + chosenAnimationFrame], new Vector2(500, 50), Color.White);
				spriteBatch.Draw(shirts[((int)chosenChild.shirtColor * 3) + chosenAnimationFrame], new Vector2(500, 50), Color.White);
				if (chosenAnimationFrame == 2)
				{
					spriteBatch.Draw(bands[(int)chosenChild.bandColor * 2 + 1], new Vector2(500, 50), Color.White);
				}
				else
				{
					spriteBatch.Draw(bands[(int)chosenChild.bandColor * 2], new Vector2(500, 50), Color.White);
				}
				spriteBatch.Draw(hands[((int)chosenChild.color * 3) + chosenAnimationFrame], new Vector2(500, 50), Color.White);
				if (chosenAnimationFrame == 0 || chosenAnimationFrame == 2) 
				{
					spriteBatch.Draw(eyes[(int)chosenChild.eyeColor], new Vector2(500, 50), Color.White);
				}

				if (childCounter <= children[currentRound - 1].Length && gameState == GameStateSecurity.Play)
				{
					childAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
					if (childAnimationTimer >= .66)
					{
						childAnimationFrame++;
						if (childAnimationFrame == 3)
						{
							childAnimationFrame = 0;
						}
						childAnimationTimer -= .66;
					}

					spriteBatch.Draw(bodies[((int)children[currentRound - 1][childCounter].color * 3) + childAnimationFrame], childPosition, Color.White);
					spriteBatch.Draw(shirts[((int)children[currentRound - 1][childCounter].shirtColor * 3) + childAnimationFrame], childPosition, Color.White);
					if (childAnimationFrame == 2)
					{
						spriteBatch.Draw(bands[(int)children[currentRound - 1][childCounter].bandColor * 2 + 1], childPosition, Color.White);
					}
					else
					{
						spriteBatch.Draw(bands[(int)children[currentRound - 1][childCounter].bandColor * 2], childPosition, Color.White);
					}
					spriteBatch.Draw(hands[((int)children[currentRound - 1][childCounter].color * 3) + childAnimationFrame], childPosition, Color.White);
					if (childAnimationFrame == 0 || childAnimationFrame == 2)
					{
						spriteBatch.Draw(eyes[(int)children[currentRound - 1][childCounter].eyeColor], childPosition, Color.White);
					}
				}
			}
			else 
			{
				spriteBatch.Draw(background[0], Vector2.Zero, Color.White);
			}

			if (gameState == GameStateSecurity.Guess) 
			{
				spriteBatch.Draw(signs[0], new Vector2(graphics.Viewport.Width / 2 + 150, graphics.Viewport.Height / 2), Color.White);
				spriteBatch.Draw(signs[1], new Vector2(graphics.Viewport.Width / 2 + 150, graphics.Viewport.Height / 2 + 150), Color.White);
				spriteBatch.DrawString(font, "Your guess: " + (guess + 1), new Vector2(graphics.Viewport.Width / 2 - 350, graphics.Viewport.Height / 2 + 100), Color.White);
				spriteBatch.Draw(signs[2], new Vector2(graphics.Viewport.Width / 2 + 350, graphics.Viewport.Height / 2 + 100), Color.White);
			}

			if (gameState == GameStateSecurity.Results) 
			{
				resultsTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (resultsTimer <= 2)
				{
					spriteBatch.DrawString(font, "YOU GUESSED...", new Vector2(graphics.Viewport.Width / 2 - 150, graphics.Viewport.Height / 2 - 100), Color.White);
				}
				else 
				{
					if (fail)
					{
						spriteBatch.DrawString(font, "INCORRECTLY!!!", new Vector2(graphics.Viewport.Width / 2 - 150, graphics.Viewport.Height / 2 - 100), Color.Red);
						if (result == null)
						{
							result = boo.CreateInstance();
							result.Play();
						}
						if (result.State != SoundState.Playing)
						{
							result = null;
							gameState = GameStateSecurity.GameOver;
							resultsTimer = 0;
						}
					}
					else 
					{
						spriteBatch.DrawString(font, "CORRECTLY!!!", new Vector2(graphics.Viewport.Width / 2 - 150, graphics.Viewport.Height / 2 - 100), Color.Green);
						if (result == null)
						{
							result = yay.CreateInstance();
							result.Play();
						}
						if (result.State != SoundState.Playing)
						{
							result = null;
							gameState = GameStateSecurity.Intermission;
							score += 250;
							currentRound++;
							if (currentRound == 5) 
							{
								gameState = GameStateSecurity.Win;
							}
							resultsTimer = 0;
						}
					}
				}
			}

			if (gameState == GameStateSecurity.Win) 
			{
				resultsTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (resultsTimer >= 1) 
				{
					showText = !showText;
					resultsTimer -= 1;
				}
				if (showText)
				{
					spriteBatch.DrawString(font, "YOU WIN!!!", new Vector2(graphics.Viewport.Width / 2 - 150, graphics.Viewport.Height / 2 - 100), Color.Green);
				}
			}

			if (gameState == GameStateSecurity.Secret) 
			{
				fredTimer += gameTime.ElapsedGameTime.TotalSeconds;

				resultsTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (resultsTimer >= 1)
				{
					showText = !showText;
					resultsTimer -= 1;
				}
				if (showText)
				{
					spriteBatch.DrawString(font, "YOU WIN!!!", new Vector2(graphics.Viewport.Width / 2 - 150, graphics.Viewport.Height / 2 - 100), Color.Green);
				}

				if (fredTimer >= 3 && fredCount > 0)
				{
					fredCount--;
					fredTimer -= 3;
				}

				if (fredCount == 0)
				{
					purpleDialogue = true;
				}

				spriteBatch.Draw(Fred[fredCount], Vector2.Zero, Color.White);


			}

			if (gameState == GameStateSecurity.GameOver)
			{
				resultsTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (resultsTimer >= 1)
				{
					showText = !showText;
					resultsTimer -= 1;
				}
				if (showText)
				{
					spriteBatch.DrawString(font, "GAME OVER!!!", new Vector2(graphics.Viewport.Width / 2 - 150, graphics.Viewport.Height / 2 - 100), Color.Red);
				}
			}

				if (gameState == GameStateSecurity.Start || gameState == GameStateSecurity.Intermission)
				{
					if (gameState == GameStateSecurity.Intermission && MediaPlayer.State != MediaState.Playing)
					{
						MediaPlayer.Play(backgroundMusic, currentPosition);
					}

					buffer += gameTime.ElapsedGameTime.TotalSeconds;

					if (buffer <= 2)
					{
						spriteBatch.DrawString(font, "ROUND " + currentRound + "!!!", new Vector2(graphics.Viewport.Width / 2 - 150, graphics.Viewport.Height / 2 - 100), Color.White);
					}
					else if (buffer > 2 && buffer < 4)
					{
						spriteBatch.DrawString(font, "READY!!!", new Vector2(graphics.Viewport.Width / 2 - 150, graphics.Viewport.Height / 2 - 100), Color.White);
					}
					else if (buffer >= 4 && buffer < 6)
					{
						spriteBatch.DrawString(font, "GO!!!", new Vector2(graphics.Viewport.Width / 2 - 150, graphics.Viewport.Height / 2 - 100), Color.White);
					}
					else
					{
						GenerateGroup();
						gameState = GameStateSecurity.Play;
						buffer = 0;
					}
				}

				spriteBatch.DrawString(font, "Score: " + score, new Vector2(graphics.Viewport.Width - 550, 50), Color.White);



				spriteBatch.Draw(Screens[0], Vector2.Zero, Color.White);
				spriteBatch.Draw(Screens[1], Vector2.Zero, Color.White);
				spriteBatch.Draw(Screens[2], Vector2.Zero, Color.White);

				if (purpleDialogue && crash == false)
				{
					ourpleTimer += gameTime.ElapsedGameTime.TotalSeconds;
					spriteBatch.DrawString(font, ourple[ourpleCount], new Vector2(graphics.Viewport.Width / 2 - 350, 250), Color.Purple);
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

				if (crash == true)
				{
					spriteBatch.Draw(crashScreen, Vector2.Zero, Color.White);
				}
			

				spriteBatch.End();
        }
	}
}
