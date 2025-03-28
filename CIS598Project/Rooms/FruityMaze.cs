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

		Texture2D Chica;

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

		GameState state;

		Emotion emote;

		bool flash;

		bool invert = false;

		bool crash = false;

		private int poppyAnimationFrame = 0;

		private double poppyAnimationTimer = 0;

		private double flashTimer = 0;

		private double lowerTime = 0;

		private double textTime = 0;

		private double startTime = 0;

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
			game.IsMouseVisible = false;
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
				}
				
			}
			else 
			{
				emote = Emotion.Happy;
			}
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
			Map = _content.Load<Texture2D>("Fruity_Maze/Textures/Maps/Map1");
			Chica = _content.Load<Texture2D>("Fruity_Maze/Textures/Chica/Chica1");
			Extended = _content.Load<SoundEffect>("Fruity_Maze/Sounds/Soundeffects/fruit4");
			font = _content.Load<SpriteFont>("MiniGame_Font");
            if (emote == Emotion.Happy)
			{
				backgroundMusic = _content.Load<Song>("Fruity_Maze/Sounds/Music/Four_Bits_to_the_left");
			}
			else if (emote == Emotion.Sad)
			{
                Chica = _content.Load<Texture2D>("Fruity_Maze/Textures/Chica/Chica2");
                Map = _content.Load<Texture2D>("Fruity_Maze/Textures/Maps/Map2");
                backgroundMusic = _content.Load<Song>("Fruity_Maze/Sounds/Music/FourBitsV2");
			}
			else 
			{
                Chica = _content.Load<Texture2D>("Fruity_Maze/Textures/Chica/Chica3");
                Map = _content.Load<Texture2D>("Fruity_Maze/Textures/Maps/Map3");
                poppy[0] = _content.Load<Texture2D>("Fruity_Maze/Textures/Poppy/SadPoppy1");
                poppy[1] = _content.Load<Texture2D>("Fruity_Maze/Textures/Poppy/SadPoppy2");
                backgroundMusic = _content.Load<Song>("Fruity_Maze/Sounds/Music/FourBitsV3");
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

			if (state == GameState.Play)
			{
				lowerTime += gameTime.ElapsedGameTime.TotalSeconds;

				if (lowerTime >= 5 && score != 500)
				{
					score -= 500;

					lowerTime -= 5;
				}
			}
			

			Vector2 mousePosition = new Vector2(currentMousePosition.X, currentMousePosition.Y);

			

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

			if (invert == false)
			{
				spriteBatch.Draw(poppy[poppyAnimationFrame], new Vector2(mouse.X, mouse.Y), null, Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.None, 0);
			}
			else if(invert)
			{
                spriteBatch.Draw(poppy[poppyAnimationFrame], new Vector2(mouse.X, mouse.Y), null, Color.White, 0f, Vector2.Zero, .5f, SpriteEffects.FlipHorizontally, 0 );
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

			spriteBatch.Draw(Screens[0], Vector2.Zero, Color.White);
			spriteBatch.Draw(Screens[1], Vector2.Zero, Color.White);
			spriteBatch.Draw(Screens[2], Vector2.Zero, Color.White);

			if (flash)
			{
                spriteBatch.Draw(Chica, Vector2.Zero, Color.White);
            }




			spriteBatch.End();

			

        }
	}
}
