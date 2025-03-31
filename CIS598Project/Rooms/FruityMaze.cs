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

	public enum Emotion 
	{
		Happy,
		Sad,
		Creepy
	}
	public enum GameState 
	{
		Start = 0,
		Play,
		GameOver,
		Win
	}
	public class FruityMaze : GameScreen
	{

		/// <summary>
		/// The game this class belongs to
		/// </summary>
		Game game;

		Texture2D backgrounds;

		Texture2D[] Fruits = new Texture2D[4];

		Texture2D[] poppy = new Texture2D[2];

		Texture2D[] Screens = new Texture2D[3];

		Texture2D[] signs = new Texture2D[2];

		Texture2D[] Chica = new Texture2D[6];

		Texture2D Map;

		Texture2D banner;

		Texture2D crashScreen;

		Song backgroundMusic;

		SoundEffect horn;

		SoundEffect GameStateChange;

		SoundEffect Extended;

		SoundEffect Crash;

		SoundEffectInstance StateChange;

		SoundEffectInstance flashing;

		SoundEffectInstance crashing;

		SoundEffectInstance airHorn;

		BoundingRectangle startSign;

		BoundingRectangle endSign;

		BoundingRectanglePriority[] checkpoints = new BoundingRectanglePriority[27];

		Color purple;

		GameState state;

		Emotion emote;

		bool flash;

		bool invert = true;

		bool crash = false;

		bool end = false;

		bool showPoppy = true;

		bool[] flashed = new bool[9];

		int j = 0;

		private int poppyAnimationFrame = 0;

		private double poppyAnimationTimer = 0;

		private double flashTimer = 0;

		private double lowerTime = 0;

		private double textTime = 0;

		private double startTime = 0;

		string[] ourple = { "Hello there,\nwhat's wrong?", "Hmm? You don't want to\ntalk? Why not?", "Your parents say you\ncan't talk to strangers?", "Well they are smart, you \nshouldn't talk to strangers.", "However, you want to \nknow something cool?", "I'm not a stranger,\nI'm a friend of yours.", "Okay, you'll talk.\nThat's great!", "What was your problem...", "Hmm, none of your friends showed\nup for your birthday?", "Well, who needs them?\nI'm your friend and I know \njust how to celebrate your birthday.", "Just follow me and I'll take you\nto a special party room just for you." };

		int ourpleCount = 0;

		bool purpleDialogue = false;

		double ourpleTimer = 0;

		int chicaCount = 5;

		double chicaTimer = 0;

		int currentCheckpoint = 0;

		private MouseState pastMousePosition;
		private MouseState currentMousePosition;

		SpriteFont font;

		ContentManager _content;

		int count = 0;
			
		BoundingRectangle mouse = new(0, 0, 64, 64);

		int score = 5000;

		Player playerRef;

		public FruityMaze(Game game,Player player) 
		{
			this.game = game;
			playerRef = player;
			//game.IsMouseVisible = false;
			if (player.foundSecret[5] == false)
			{
				if (player.fruityMazeWins == 0)
				{
					emote = Emotion.Happy;
				}
				else if (player.fruityMazeWins == 1)
				{
					emote = Emotion.Sad;
				}
				else 
				{
					emote = Emotion.Creepy;
					game.Window.Title = "Don't listen to him...";
				}
				
			}
			else 
			{
				emote = Emotion.Happy;
			}

			startSign = new BoundingRectangle(51, 222, 128, 128);
			endSign = new BoundingRectangle(1853, 85, 128, 128);
			checkpoints[0] = new BoundingRectanglePriority(0, 228, 734, 113, 0, false);
			checkpoints[1] = new BoundingRectanglePriority(728, 0, 96, 339, 1, false);
			checkpoints[2] = new BoundingRectanglePriority(819, 0, 274, 89, 2, true);
			checkpoints[3] = new BoundingRectanglePriority(1088, 0, 71, 136, 3, false);
			checkpoints[4] = new BoundingRectanglePriority(1091, 136, 65, 203,4, false);
			checkpoints[5] = new BoundingRectanglePriority(1098, 339, 51, 237, 5, false);
			checkpoints[6] = new BoundingRectanglePriority(828, 521, 270, 51, 6, false);
			checkpoints[7] = new BoundingRectanglePriority(828, 390, 65, 131, 7, false);
			checkpoints[8] = new BoundingRectanglePriority(527, 390, 302, 66, 8, false);
			checkpoints[9] = new BoundingRectanglePriority(528, 456, 80, 275, 9, false);
			checkpoints[10] = new BoundingRectanglePriority(535, 731, 65, 181, 10, false);
			checkpoints[11] = new BoundingRectanglePriority(600, 801, 229, 52, 11, false);
			checkpoints[12] = new BoundingRectanglePriority(829, 807, 331, 39, 12, false);
			checkpoints[13] = new BoundingRectanglePriority(1064, 636, 96, 171, 13, false);
			checkpoints[14] = new BoundingRectanglePriority(1160, 641, 516, 76, 14, false);
			checkpoints[15] = new BoundingRectanglePriority(1579, 438, 100, 203, 15, false);
			checkpoints[16] = new BoundingRectanglePriority(1327, 441, 252, 77, 16, false);
			checkpoints[17] = new BoundingRectanglePriority(1331, 337, 99, 104, 17, false);
			checkpoints[18] = new BoundingRectanglePriority(1241, 242, 109, 133, 18, false);
			checkpoints[19] = new BoundingRectanglePriority(1325, 207, 51, 57, 19, false);
			checkpoints[20] = new BoundingRectanglePriority(1365, 174, 53, 50, 20, false);
			checkpoints[21] = new BoundingRectanglePriority(1418, 187, 239, 28, 21, false);
			checkpoints[22] = new BoundingRectanglePriority(1611, 215, 46, 57, 22, false);
			checkpoints[23] = new BoundingRectanglePriority(1643, 234, 55, 90, 23, false);
			checkpoints[24] = new BoundingRectanglePriority(1686, 284, 80, 101, 24, false);
			checkpoints[25] = new BoundingRectanglePriority(1754, 167, 52, 144, 25, false);
			checkpoints[26] = new BoundingRectanglePriority(1754, 113, 166, 55, 26, false);
		}

		public override void Activate()
		{
			base.Activate();

			if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

            poppy[0] = _content.Load<Texture2D>("Fruity_Maze/Textures/Poppy/Poppy1");
            poppy[1] = _content.Load<Texture2D>("Fruity_Maze/Textures/Poppy/Poppy2");
			Screens[0] = _content.Load<Texture2D>("Fruity_Maze/Textures/Screen/scanlines");
			Screens[1] = _content.Load<Texture2D>("Fruity_Maze/Textures/Screen/Screen");
			Screens[2] = _content.Load<Texture2D>("Fruity_Maze/Textures/Screen/ScreenBorder");
			Fruits[0] = _content.Load<Texture2D>("Fruity_Maze/Textures/Fruit/Coconut");
			Fruits[1] = _content.Load<Texture2D>("Fruity_Maze/Textures/Fruit/Grape");
			Fruits[2] = _content.Load<Texture2D>("Fruity_Maze/Textures/Fruit/Melon");
			Fruits[3] = _content.Load<Texture2D>("Fruity_Maze/Textures/Fruit/Orange");
			signs[0] = _content.Load <Texture2D>("Fruity_Maze/Textures/Signs/start");
			signs[1] = _content.Load<Texture2D>("Fruity_Maze/Textures/Signs/End");
			Map = _content.Load<Texture2D>("Fruity_Maze/Textures/Maps/Map1");
			Extended = _content.Load<SoundEffect>("Fruity_Maze/Sounds/Soundeffects/fruit4");
			horn = _content.Load<SoundEffect>("Balloon_Barrel/Sounds/Soundeffect/win");
			GameStateChange = _content.Load<SoundEffect>("Fruity_Maze/Sounds/Soundeffects/20");
			Crash = _content.Load<SoundEffect>("Duck_Pond/Duck_Pond_Sounds/df");
			crashScreen = _content.Load<Texture2D>("Duck_Pond/Crash");
			font = _content.Load<SpriteFont>("MiniGame_Font");
            if (emote == Emotion.Happy)
			{
				backgroundMusic = _content.Load<Song>("Fruity_Maze/Sounds/Music/Four_Bits_to_the_left");
			}
			else if (emote == Emotion.Sad)
			{
                Map = _content.Load<Texture2D>("Fruity_Maze/Textures/Maps/Map2");
                backgroundMusic = _content.Load<Song>("Fruity_Maze/Sounds/Music/FourBitsV2");
			}
			else 
			{
                Map = _content.Load<Texture2D>("Fruity_Maze/Textures/Maps/Map3");
                poppy[0] = _content.Load<Texture2D>("Fruity_Maze/Textures/Poppy/SadPoppy1");
                poppy[1] = _content.Load<Texture2D>("Fruity_Maze/Textures/Poppy/SadPoppy2");
                backgroundMusic = _content.Load<Song>("Fruity_Maze/Sounds/Music/FourBitsV3");
			}

			for (int i = 5; i >= 0; i--) 
			{
				Chica[i] = _content.Load<Texture2D>("Fruity_Maze/Textures/Chica/Chica" + (i + 1));
			}

			MediaPlayer.Play(backgroundMusic);
			MediaPlayer.IsRepeating = true;
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

			if (state == GameState.Start && mouse.collidesWith(startSign)) 
			{
				state = GameState.Play;
				GameStateChange.Play();
			}

			end = true;


			if (state == GameState.Play)
			{
				lowerTime += gameTime.ElapsedGameTime.TotalSeconds;


				if (lowerTime >= 10 && score != 1000)
				{
					score -= 1000;

					lowerTime -= 10;
				}


				for (int i = 0; i < checkpoints.Length; i++) 
				{
					if (mouse.collidesWith(checkpoints[i])) 
					{
						currentCheckpoint = i;
						end = false;
						invert = true;
						if (currentCheckpoint >= 3 && currentCheckpoint <= 8 || currentCheckpoint == 15 || currentCheckpoint == 16 || currentCheckpoint == 17)
						{
							invert = false;
						}
					}
				}
			}



			if (end == true && state == GameState.Play) 
			{
				state = GameState.GameOver;
				showPoppy = false;
			}


			if (state == GameState.Play && mouse.collidesWith(endSign) && currentCheckpoint != 26 && emote != Emotion.Creepy)
			{
				state = GameState.GameOver;
				showPoppy = false;
			}
			else if (state == GameState.Play && mouse.collidesWith(endSign) && currentCheckpoint == 26 && emote != Emotion.Creepy) 
			{
				state = GameState.Win;
				showPoppy = false;
			}
			else if(state == GameState.GameOver || mouse.collidesWith(endSign) && currentCheckpoint == 26)
			{
				showPoppy = false;
				flash = true;
				if (mouse.collidesWith(endSign) && currentCheckpoint == 26) 
				{
					state = GameState.Win;
				}
				if (StateChange == null)
				{
					MediaPlayer.Stop();
					StateChange = Extended.CreateInstance();
					StateChange.Play();
				}
				if (StateChange.State != SoundState.Playing)
				{
					flash = true;
				}
			}

			Vector2 mousePosition = new Vector2(currentMousePosition.X, currentMousePosition.Y);

			if (state == GameState.GameOver && emote != Emotion.Creepy)
			{
				if (StateChange == null)
				{
					MediaPlayer.Stop();
					StateChange = Extended.CreateInstance();
					StateChange.Play();
				}
				if (StateChange.State != SoundState.Playing)
				{
					
					game.Exit();
				}
			}

			if (state == GameState.Win && emote != Emotion.Creepy) 
			{
				if (airHorn == null) 
				{
					MediaPlayer.Stop();
					airHorn = horn.CreateInstance();
					airHorn.Play();
				}
				if (airHorn.State != SoundState.Playing) 
				{
					playerRef.ticketAmount += score;
					game.Exit();
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

			spriteBatch.Draw(Map, Vector2.Zero, Color.White);

			if (emote == Emotion.Creepy)
			{
				poppyAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (poppyAnimationTimer > .5)
				{
					poppyAnimationFrame++;
					if (poppyAnimationFrame == 2)
					{
						poppyAnimationFrame = 0;
					}
					poppyAnimationTimer -= .5;
				}
			}
			else if (emote == Emotion.Sad)
			{
				poppyAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (poppyAnimationTimer > .35)
				{
					poppyAnimationFrame++;
					if (poppyAnimationFrame == 2)
					{
						poppyAnimationFrame = 0;
					}
					poppyAnimationTimer -= .35;
				}
                spriteBatch.Draw(Fruits[1], new Vector2(1266, 777), null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
                spriteBatch.Draw(Fruits[3], new Vector2(778, 135), null, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0);
            }
			else 
			{
                poppyAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (poppyAnimationTimer > .25)
                {
                    poppyAnimationFrame++;
                    if (poppyAnimationFrame == 2)
                    {
                        poppyAnimationFrame = 0;
                    }
                    poppyAnimationTimer -= .25;
                }

				spriteBatch.Draw(Fruits[0], new Vector2(100, 458), null, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0);
                spriteBatch.Draw(Fruits[1], new Vector2(1266, 777), null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
                spriteBatch.Draw(Fruits[2], new Vector2(941, 617), null, Color.White, 90f, Vector2.Zero, 2f, SpriteEffects.None, 0);
                spriteBatch.Draw(Fruits[3], new Vector2(778, 135), null, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0);
            }

			if (state == GameState.Start) 
			{
				spriteBatch.Draw(signs[0], new Vector2(51, 222), Color.White);
			}

			if (state == GameState.Play)
			{
				spriteBatch.Draw(signs[1], new Vector2(1853, 85), Color.White);
			}

			if (showPoppy)
			{
				if (invert == false)
				{
					spriteBatch.Draw(poppy[poppyAnimationFrame], new Vector2(mouse.X, mouse.Y), null, Color.White, 0f, new Vector2(64, 64), .5f, SpriteEffects.None, 0);
				}
				else if (invert)
				{
					spriteBatch.Draw(poppy[poppyAnimationFrame], new Vector2(mouse.X, mouse.Y), null, Color.White, 0f, new Vector2(64, 64), .5f, SpriteEffects.FlipHorizontally, 0);
				}
			}

			spriteBatch.DrawString(font, "Score: " + score, new Vector2(graphics.Viewport.Width - 650, 50), Color.White);

			if (state == GameState.Start && emote != Emotion.Creepy) 
			{
				startTime += gameTime.ElapsedGameTime.TotalSeconds;

				if (startTime > 1) 
				{
					textTime += gameTime.ElapsedGameTime.TotalSeconds;
					if ((int)textTime % 3 != 0)
					{
                        spriteBatch.DrawString(font, "Go to the start sign to start going through the maze,\nmake it the end in the fastest time possible to score\nthe most points.", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2 + 300), Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
                    }
                    
				}
			}


			if (state == GameState.Start && emote == Emotion.Creepy)
			{
				startTime += gameTime.ElapsedGameTime.TotalSeconds;

				if (startTime > 1)
				{
					textTime += gameTime.ElapsedGameTime.TotalSeconds;
					if ((int)textTime % 3 != 0)
					{
						spriteBatch.DrawString(font, "Go to the start sign...", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2 + 300), Color.Purple, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
					}

				}
			}

			if (state == GameState.Win) 
			{
				textTime += gameTime.ElapsedGameTime.TotalSeconds;
				spriteBatch.DrawString(font, "YOU WIN!!!", new Vector2(graphics.Viewport.Width / 2 - 300, graphics.Viewport.Height / 2 - 100), Color.Blue, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
			}

			if (state == GameState.GameOver )
			{
				textTime += gameTime.ElapsedGameTime.TotalSeconds;
				spriteBatch.DrawString(font, "OUT OF BOUNDS!!!", new Vector2(graphics.Viewport.Width / 2 - 500, graphics.Viewport.Height / 2 - 100), Color.Red, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
			}

			spriteBatch.Draw(Screens[0], Vector2.Zero, Color.White);
			spriteBatch.Draw(Screens[1], Vector2.Zero, Color.White);
			spriteBatch.Draw(Screens[2], Vector2.Zero, Color.White);

			if (flash) 
			{
				chicaTimer += gameTime.ElapsedGameTime.TotalSeconds;

				if (chicaTimer >= 3 && chicaCount > 0) 
				{
					chicaCount--;
					chicaTimer -= 3;
				}

				if (chicaCount == 0) 
				{
					purpleDialogue = true;
				}

				spriteBatch.Draw(Chica[chicaCount], Vector2.Zero, Color.White);
			}

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
				if (crashing == null) 
				{
					crashing = Crash.CreateInstance();
					crashing.Play();
				}
				if (crashing.State != SoundState.Playing) 
				{
					playerRef.foundSecret[5] = true;
					if (state == GameState.Win) 
					{
						playerRef.ticketAmount += score;
					}
					game.Exit();
				}
			}



			spriteBatch.End();

			

        }
	}
}
