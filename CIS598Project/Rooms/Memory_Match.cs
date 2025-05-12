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
using System.Formats.Tar;
using SharpDX.Direct2D1;

namespace CIS598Project.Rooms
{

	public enum GameStateMemory
	{
		tutorial,
		start,
		play,
		intermission,
		win,
		fail,
		secret
	}
	public class Memory_Match : GameScreen
	{

		/// <summary>
		/// The game this class belongs to
		/// </summary>
		Game game;

		Texture2D[] backgrounds = new Texture2D[8];

		Texture2D[] Screens = new Texture2D[3];

		Texture2D[] buttonIcons = new Texture2D[8];

		Texture2D[] Bon = new Texture2D[6];

		Texture2D crashScreen;

		Song backgroundMusic;

		SoundEffect[] roundIntermissions = new SoundEffect[5];

		SoundEffect[] speakingColors = new SoundEffect[8];

		SoundEffect win;

		SoundEffect gameOver;

		SoundEffect fail;

		SoundEffect Crash;

		SoundEffect select;

		SoundEffectInstance crashing;

		SoundEffectInstance failed;

		SoundEffectInstance gameOvered;

		SoundEffectInstance winner;

		SoundEffectInstance roundState;

		SoundEffectInstance colorState;

		BoundingRectangle[] boundingRectangles = new BoundingRectangle[8];

		MemoryButton[] buttons = new MemoryButton[8];

		bool[] canBePresed = new bool[8];

		bool showSequence = false;

		GameStateMemory gameState;


		bool crash = false;

		bool showAlternativeWin = false;


		string[] ourple = { "Heya there, pal.", "You're really smart if\nyou beat that game.", "It must've ate so\nmany of your tokens up.", "No?", "Well still no one has\nbeat that game before.", "Don't you want\nmto be rewarded?", "Really, I insist follow me and \nI'll give you a nice reward." };

		int ourpleCount = 0;

		bool purpleDialogue = false;

		double ourpleTimer = 0;

		int bonCounter = 5;

		double bonTimer = 0;

		double textTimer = 0;

		double resultsTimer = 0;

		int roundAmount = 5;

		int colorsToGoThrough = 0;

		List<buttonColors> pattern;

		int currentRound = 1;

		int guessingIndex = 0; //The current index of the pattern in which the player is guessing

		Vector2[] topButtonLocations = new Vector2[4];

		bool showText;


		private MouseState pastMousePosition;
		private MouseState currentMousePosition;

		SpriteFont font;

		ContentManager _content;
			
		BoundingRectangle mouse = new(0, 0, 64, 64);

        int score = 0;
        Player player;

		public Memory_Match(Game game,Player player) 
		{
			this.game = game;
			this.player = player;
			for (int i = 0; i < buttons.Length; i++) 
			{
				buttons[i] = new MemoryButton(i);
			}
		}

		public override void Activate()
		{
			base.Activate();

			if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");
			backgrounds[0] = _content.Load<Texture2D>("Memory/Textures/Backgrounds/Blue");
            backgrounds[1] = _content.Load<Texture2D>("Memory/Textures/Backgrounds/Green");
            backgrounds[2] = _content.Load<Texture2D>("Memory/Textures/Backgrounds/Red");
            backgrounds[3] = _content.Load<Texture2D>("Memory/Textures/Backgrounds/Yellow");
            backgrounds[4] = _content.Load<Texture2D>("Memory/Textures/Backgrounds/Orange");
            backgrounds[5] = _content.Load<Texture2D>("Memory/Textures/Backgrounds/Black");
            backgrounds[6] = _content.Load<Texture2D>("Memory/Textures/Backgrounds/White");
            backgrounds[7] = _content.Load<Texture2D>("Memory/Textures/Backgrounds/Purple");

			buttonIcons[0] = _content.Load<Texture2D>("Memory/Textures/Buttons/blue_button");
            buttonIcons[1] = _content.Load<Texture2D>("Memory/Textures/Buttons/green_button");
            buttonIcons[2] = _content.Load<Texture2D>("Memory/Textures/Buttons/red_button");
            buttonIcons[3] = _content.Load<Texture2D>("Memory/Textures/Buttons/yellow_button");
            buttonIcons[4] = _content.Load<Texture2D>("Memory/Textures/Buttons/orange_button");
            buttonIcons[5] = _content.Load<Texture2D>("Memory/Textures/Buttons/black_button");
            buttonIcons[6] = _content.Load<Texture2D>("Memory/Textures/Buttons/white_button");
            buttonIcons[7] = _content.Load<Texture2D>("Memory/Textures/Buttons/purple_button");

			select = _content.Load<SoundEffect>("Memory/Sounds/Soundeffects/Misc/colorSelect");

			foreach (MemoryButton button in buttons) 
			{
				button.LoadContent(_content);
			}
            
			fail = _content.Load<SoundEffect>("Memory/Sounds/Soundeffects/Misc/failed");
			Random random = new Random();
			int ran = random.Next(0, 100);
			if (ran < 99)
			{
				win = _content.Load<SoundEffect>("Memory/Sounds/Soundeffects/Intermission/youwin");
            }
			else 
			{
                win = _content.Load<SoundEffect>("Memory/Sounds/Soundeffects/Intermission/you'rewinner");
				showAlternativeWin = true;
            }
			gameOver = _content.Load<SoundEffect>("Memory/Sounds/Soundeffects/Intermission/gameover");
			Crash = _content.Load<SoundEffect>("Duck_Pond/Duck_Pond_Sounds/df");
			crashScreen = _content.Load<Texture2D>("Duck_Pond/Crash");
			font = _content.Load<SpriteFont>("MiniGame_Font");
			for (int i = 0; i < roundIntermissions.Length; i++) 
			{
				roundIntermissions[i] = _content.Load<SoundEffect>("Memory/Sounds/Soundeffects/Intermission/round" + (i + 1));
			}

			speakingColors[0] = _content.Load<SoundEffect>("Memory/Sounds/Soundeffects/Colors/blue");
            speakingColors[1] = _content.Load<SoundEffect>("Memory/Sounds/Soundeffects/Colors/green");
            speakingColors[2] = _content.Load<SoundEffect>("Memory/Sounds/Soundeffects/Colors/red");
            speakingColors[3] = _content.Load<SoundEffect>("Memory/Sounds/Soundeffects/Colors/yellow");
            speakingColors[4] = _content.Load<SoundEffect>("Memory/Sounds/Soundeffects/Colors/orange");
            speakingColors[5] = _content.Load<SoundEffect>("Memory/Sounds/Soundeffects/Colors/black");
            speakingColors[6] = _content.Load<SoundEffect>("Memory/Sounds/Soundeffects/Colors/white");
            speakingColors[7] = _content.Load<SoundEffect>("Memory/Sounds/Soundeffects/Colors/purple");

            backgroundMusic = _content.Load<Song>("Memory/Sounds/Ambience/buzz");

			Screens[0] = _content.Load<Texture2D>("Fruity_Maze/Textures/Screen/scanlines");
			Screens[1] = _content.Load<Texture2D>("Fruity_Maze/Textures/Screen/Screen");
			Screens[2] = _content.Load<Texture2D>("Fruity_Maze/Textures/Screen/ScreenBorder");

			for (int i = 5; i >= 0; i--) 
			{
				Bon[i] = _content.Load<Texture2D>("Memory/Textures/Bon/Bon" + (i + 1));
			}

			topButtonLocations[0] = new Vector2(-500, 300); //orange
            topButtonLocations[1] = new Vector2(-200, 300); //white
            topButtonLocations[2] = new Vector2(200, 300); //black
            topButtonLocations[3] = new Vector2(500, 300); //purple*/

			boundingRectangles[0] = new(game.GraphicsDevice.Viewport.Width / 2 - 500, game.GraphicsDevice.Viewport.Height - 300, 256, 256);
            boundingRectangles[1] = new(game.GraphicsDevice.Viewport.Width / 2 - 200, game.GraphicsDevice.Viewport.Height - 300, 256, 256);
            boundingRectangles[2] = new(game.GraphicsDevice.Viewport.Width / 2 + 200, game.GraphicsDevice.Viewport.Height - 300, 256, 256);
            boundingRectangles[3] = new(game.GraphicsDevice.Viewport.Width / 2 + 500, game.GraphicsDevice.Viewport.Height - 300, 256, 256);
            boundingRectangles[4] = new(game.GraphicsDevice.Viewport.Width / 2 - 500, 125, 256, 256);
            boundingRectangles[5] = new(game.GraphicsDevice.Viewport.Width / 2 - 200, 125, 256, 256);
            boundingRectangles[6] = new(game.GraphicsDevice.Viewport.Width / 2 + 200, 125, 256, 256);
            boundingRectangles[7] = new(game.GraphicsDevice.Viewport.Width / 2 + 500, 125, 256, 256);

            pattern = new();

			gameState = GameStateMemory.start;

			//MediaPlayer.Play(backgroundMusic);
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

			pastMousePosition = currentMousePosition;
			currentMousePosition = Mouse.GetState();

            Vector2 mousePosition = new Vector2(currentMousePosition.X, currentMousePosition.Y);

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

			if (gameState == GameStateMemory.start) 
			{
				if (roundState == null) 
				{
					pattern.Clear();
					roundState = roundIntermissions[currentRound - 1].CreateInstance();
					roundState.Play();
				}
				if (roundState.State != SoundState.Playing) 
				{
					gameState = GameStateMemory.intermission;
					roundState = null;
				}
			}

			if (gameState == GameStateMemory.intermission && showSequence == false)
			{
				int value = 0;
				Random ran = new();
				colorsToGoThrough = 0;
                if (currentRound == 1)
				{
					value = ran.Next(0, 4);
				}
				else if (currentRound == 2)
				{
                    value = ran.Next(0, 5);
                }
				else if (currentRound == 3)
				{
                    value = ran.Next(0, 6);
                }
				else if (currentRound == 4)
				{
                    value = ran.Next(0, 7);
                }
				else
				{
                    value = ran.Next(0, 8);
                }
				if (value == 0)
				{
					pattern.Add(buttonColors.blue);
				}
				else if (value == 1)
				{
					pattern.Add(buttonColors.green);
				}
				else if (value == 2)
				{
					pattern.Add(buttonColors.red);
				}
				else if (value == 3)
				{
					pattern.Add(buttonColors.yellow);
				}
				else if (value == 4)
				{
					pattern.Add(buttonColors.orange);
				}
				else if (value == 5)
				{
					pattern.Add(buttonColors.black);
				}
				else if (value == 6)
				{
					pattern.Add(buttonColors.white);
				}
				else if (value == 7) 
				{
					pattern.Add(buttonColors.purple);
				}
				showSequence = true;
			}

			if (gameState == GameStateMemory.play && guessingIndex != pattern.Count) 
			{
				bool check = true;
                if (mouse.collidesWith(boundingRectangles[0]))
                {
                    if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
                    {
                        select.Play();
                        check = comparison(buttonColors.blue);
                    }
                }
                if (mouse.collidesWith(boundingRectangles[1]))
                {
                    if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
                    {
                        select.Play();
                        check = comparison(buttonColors.green);
                    }
                }
                if (mouse.collidesWith(boundingRectangles[2]))
                {
                    if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
                    {
                        select.Play();
                        check = comparison(buttonColors.red);
                    }
                }
                if (mouse.collidesWith(boundingRectangles[3]))
                {
                    if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
                    {
                        select.Play();
                        check = comparison(buttonColors.yellow);
                    }
                }
                if (mouse.collidesWith(boundingRectangles[4]))
                {
                    if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
                    {
                        select.Play();
                        check = comparison(buttonColors.orange);
                    }
                }
                if (mouse.collidesWith(boundingRectangles[5]))
                {
                    if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
                    {
                        select.Play();
                        check = comparison(buttonColors.black);
                    }
                }
                if (mouse.collidesWith(boundingRectangles[6]))
                {
                    if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
                    {
						select.Play();
                        check = comparison(buttonColors.white);
                    }
                }
                if (mouse.collidesWith(boundingRectangles[7]))
                {
                    if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
                    {
                        select.Play();
                        check = comparison(buttonColors.purple);
                    }
                }

                if (check == false)
					{
						gameState = GameStateMemory.fail;
					}
            }

			if (gameState == GameStateMemory.fail) 
			{
				if (failed == null) 
				{
					failed = fail.CreateInstance();
					failed.Play();
				}
				if (failed.State != SoundState.Playing) 
				{
					if (roundState == null) 
					{
						roundState = gameOver.CreateInstance();
						roundState.Play();
					}
					if (roundState.State != SoundState.Playing) 
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

						Unload();

                        ScreenManager.AddScreen(new MainGame_Screen(player, game), PlayerIndex.One);
                    }
				}
			}

            if (gameState == GameStateMemory.win || gameState == GameStateMemory.secret)
            {
                if (failed == null)
                {
                    failed = fail.CreateInstance();
                    failed.Play();
                }
                if (failed.State != SoundState.Playing)
                {
                    if (roundState == null)
                    {
                        roundState = win.CreateInstance();
                        roundState.Play();
                    }
                    if (roundState.State != SoundState.Playing)
                    {
                        for (int i = 0; i < player.consecutivePlays.Length; i++)
                        {
                            if (i != 0)
                            {
                                player.consecutivePlays[i] = 0;
                            }

                        }
						if (gameState == GameStateMemory.win)
						{
                            player.ticketAmount += score;

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
                        player.foundSecret[5] = true;
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

						Unload();

                        ScreenManager.AddScreen(new MainGame_Screen(player, game), PlayerIndex.One);
                    }
                }
            }

            if (guessingIndex == pattern.Count && gameState == GameStateMemory.play) 
			{
				showSequence = false;
				gameState = GameStateMemory.intermission;
				if (guessingIndex >= currentRound + 4)
				{
					score += 250;
                    guessingIndex = 0;
                    colorsToGoThrough = 0;
                    gameState = GameStateMemory.start;
                    pattern.Clear();
                    currentRound++;
					if (currentRound > roundAmount) 
					{
						if (player.foundSecret[5])
						{
							gameState = GameStateMemory.win;
						}
						else 
						{
							gameState = GameStateMemory.secret;
						}
					}
                }
				else
				{
					guessingIndex = 0;
					colorsToGoThrough = 0;
				}
			}

            mouse.X = mousePosition.X;
			mouse.Y = mousePosition.Y;

		}

		private bool comparison(buttonColors color) 
		{
			if (color == pattern[guessingIndex]) 
			{
				guessingIndex++;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Draws the sprite using the supplied SpriteBatch
		/// </summary>
		/// <param name="gameTime">The game time</param>
		public override void Draw(GameTime gameTime)
		{
			var graphics = ScreenManager.GraphicsDevice;
			var spriteBatch = ScreenManager.SpriteBatch;


			graphics.Clear(Color.White);

			spriteBatch.Begin();

			if (gameState != GameStateMemory.start) 
			{
				if (showSequence) 
				{
					while (colorsToGoThrough != pattern.Count) 
					{
						if (colorState == null)
						{
							if (pattern[colorsToGoThrough] == buttonColors.blue)
							{
								colorState = speakingColors[0].CreateInstance();
							
							}
							if (pattern[colorsToGoThrough] == buttonColors.green)
							{
								colorState = speakingColors[1].CreateInstance();
						
							}
							if (pattern[colorsToGoThrough] == buttonColors.red)
							{
								colorState = speakingColors[2].CreateInstance();
								
							}
							if (pattern[colorsToGoThrough] == buttonColors.yellow)
							{
								colorState = speakingColors[3].CreateInstance();
								
							}
							if (pattern[colorsToGoThrough] == buttonColors.orange)
							{
								colorState = speakingColors[4].CreateInstance();
								
							}
							if (pattern[colorsToGoThrough] == buttonColors.black)
							{
								colorState = speakingColors[5].CreateInstance();
								
							}
							if (pattern[colorsToGoThrough] == buttonColors.white)
							{
								colorState = speakingColors[6].CreateInstance();
			
							}
							if (pattern[colorsToGoThrough] == buttonColors.purple)
							{
								colorState = speakingColors[7].CreateInstance();
								
							}
                            colorState.Play();
                        }
						if (colorState.State != SoundState.Playing)
						{
							colorState = null;
							colorsToGoThrough++;
							if (colorsToGoThrough == pattern.Count) 
							{
								gameState = GameStateMemory.play;
							}
						}
					}
					if (gameState != GameStateMemory.start && gameState != GameStateMemory.fail && gameState != GameStateMemory.win)
					{
						if (pattern[pattern.Count - 1] == buttonColors.blue)
						{
							spriteBatch.Draw(backgrounds[0], Vector2.Zero, Color.White);
							spriteBatch.DrawString(font, "Blue", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2), Color.Black);
						}
						if (pattern[pattern.Count - 1] == buttonColors.green)
						{
							spriteBatch.Draw(backgrounds[1], Vector2.Zero, Color.White);
							spriteBatch.DrawString(font, "Green", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2), Color.Black);
						}
						if (pattern[pattern.Count - 1] == buttonColors.red)
						{
							spriteBatch.Draw(backgrounds[2], Vector2.Zero, Color.White);
							spriteBatch.DrawString(font, "Red", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2), Color.Black);
						}
						if (pattern[pattern.Count - 1] == buttonColors.yellow)
						{
							spriteBatch.Draw(backgrounds[3], Vector2.Zero, Color.White);
							spriteBatch.DrawString(font, "Yellow", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2), Color.Black);
						}
						if (pattern[pattern.Count - 1] == buttonColors.orange)
						{
							spriteBatch.Draw(backgrounds[4], Vector2.Zero, Color.White);
							spriteBatch.DrawString(font, "Orange", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2), Color.Black);
						}
						if (pattern[pattern.Count - 1] == buttonColors.black)
						{
							spriteBatch.Draw(backgrounds[5], Vector2.Zero, Color.White);
							spriteBatch.DrawString(font, "Black", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2), Color.Gray);
						}
						if (pattern[pattern.Count - 1] == buttonColors.white)
						{
							spriteBatch.Draw(backgrounds[6], Vector2.Zero, Color.White);
							spriteBatch.DrawString(font, "White", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2), Color.Black);
						}
						if (pattern[pattern.Count - 1] == buttonColors.purple)
						{
							spriteBatch.Draw(backgrounds[7], Vector2.Zero, Color.White);
							spriteBatch.DrawString(font, "Purple", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2), Color.Black);
						}
					}
				}
			}

			if (gameState == GameStateMemory.fail) 
			{
                spriteBatch.Draw(backgrounds[2], Vector2.Zero, Color.White);
                resultsTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (resultsTimer >= .5)
                {
                    showText = !showText;
                    resultsTimer -= .5;
                }
				if (showText)
				{
					spriteBatch.DrawString(font, "GAME OVER!!!", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2), Color.Black);
				}
            }

            if (gameState == GameStateMemory.win || gameState == GameStateMemory.secret)
            {
                spriteBatch.Draw(backgrounds[1], Vector2.Zero, Color.White);
                resultsTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (resultsTimer >= .5)
                {
                    showText = !showText;
                    resultsTimer -= .5;
                }
                if (showText)
                {
                    spriteBatch.DrawString(font, "YOU WIN!!!", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2), Color.Black);
                }
            }


            

            spriteBatch.Draw(buttonIcons[0], new Vector2(graphics.Viewport.Width / 2 - 500, graphics.Viewport.Height - 300), null, Color.White);
            spriteBatch.Draw(buttonIcons[1], new Vector2(graphics.Viewport.Width / 2 - 200, graphics.Viewport.Height - 300), null, Color.White);
            spriteBatch.Draw(buttonIcons[2], new Vector2(graphics.Viewport.Width / 2 + 200, graphics.Viewport.Height - 300), null, Color.White);
            spriteBatch.Draw(buttonIcons[3], new Vector2(graphics.Viewport.Width / 2 + 500, graphics.Viewport.Height - 300), null, Color.White);
			spriteBatch.Draw(buttonIcons[4], new Vector2(graphics.Viewport.Width / 2 - 500, 125), null, Color.White);
            spriteBatch.Draw(buttonIcons[5], new Vector2(graphics.Viewport.Width / 2 - 200, 125), null, Color.White);
            spriteBatch.Draw(buttonIcons[6], new Vector2(graphics.Viewport.Width / 2 + 200, 125), null, Color.White);
            spriteBatch.Draw(buttonIcons[7], new Vector2(graphics.Viewport.Width / 2 + 500, 125), null, Color.White);

            spriteBatch.Draw(Screens[0], Vector2.Zero, Color.White);
            spriteBatch.Draw(Screens[1], Vector2.Zero, Color.White);
            spriteBatch.Draw(Screens[2], Vector2.Zero, Color.White);

            if (gameState == GameStateMemory.secret)
            {
                bonTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (bonTimer >= 3 && bonCounter > 0)
                {
                    bonCounter--;
                    bonTimer -= 3;
                }

                if (bonCounter == 0)
                {
                    purpleDialogue = true;
                }

                spriteBatch.Draw(Bon[bonCounter], Vector2.Zero, Color.White);
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
