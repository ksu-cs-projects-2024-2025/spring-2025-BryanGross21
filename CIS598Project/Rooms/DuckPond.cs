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
	public enum DuckType 
	{
		Fred = 0,
		GFred,
		SadFred
	}
	public class DuckPond : GameScreen
	{

		/// <summary>
		/// The game this class belongs to
		/// </summary>
		Game game;

		Texture2D[] backgrounds = new Texture2D[3];

		Texture2D[] pointsOverlay = new Texture2D[2];

		Texture2D[] idle = new Texture2D[20];

		Texture2D[] selected = new Texture2D[15];

		Texture2D overlay;

		Texture2D banner;

		Song[] songs = new Song[3];

		SoundEffect select;

		SoundEffect chosen;

		SoundEffect Crash;

		SoundEffect Finished;

		DuckType duckType;

		Duck[] ducks = new Duck[13];

		bool[] showDucks = new bool[13];

		bool canSelect = true;

		bool crash = false;

		bool close = false;

		Rectangle whereToDraw;

		private int duckIdleAnimationFrame = 0;

		private int duckSelectedAnimationFrame = 0;

		private MouseState pastMousePosition;
		private MouseState currentMousePosition;

		SpriteFont font;

		ContentManager _content;

		BoundingRectangle[] ducksCollision = new BoundingRectangle[16];
			
		BoundingRectangle mouse = new(0, 0, 32, 32);

		int count = 3;

		int score = 0;

		int timeToShowScore;

		Player playerRef;

		public DuckPond(Game game,Player player) 
		{
			this.game = game;
			if (player.consecutivePlays[1] == 1 && player.foundSecret[1] == false)
			{
				duckType = DuckType.GFred;
				game.Window.Title = "Fredbear and Friends";
			}
			else if (player.consecutivePlays[1] == 2 && player.foundSecret[1] == false)
			{
				duckType = DuckType.SadFred;
				game.Window.Title = "Fredbear";
			}
			else 
			{
				duckType = DuckType.Fred;
			}

			for (int i = 0; i < player.consecutivePlays.Length; i++) 
			{
				if (i != 1) 
				{
					player.consecutivePlays[i] = 0;
				}
			}
			playerRef = player;

			bool has900Points = false;

			for (int i = 0; i < ducks.Length; i++) 
			{
				ducks[i] = new Duck(i, has900Points);
				if (ducks[i].points == 900 && has900Points == false) 
				{
					has900Points = true;
				}
				showDucks[i] = true;
			}

			if (duckType == DuckType.SadFred) 
			{
				count = 5;
			}
		}

		public override void Activate()
		{
			base.Activate();

			if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

			backgrounds[0] = _content.Load<Texture2D>("Duck_Pond/Duck_pond_background");
			backgrounds[1] = _content.Load<Texture2D>("Duck_Pond/Duck_pond_background_final");
			backgrounds[2] = _content.Load<Texture2D>("Duck_Pond/Crash");


			songs[0] = _content.Load<Song>("Duck_Pond/Duck_Pond_Music/Minor_Corrosion_of_the_Bizet");
			songs[1] = _content.Load<Song>("Duck_Pond/Duck_Pond_Music/Minor_Corrosion_of_the_Bizet_Reverbed");
			songs[2] = _content.Load<Song>("Duck_Pond/Duck_Pond_Music/Minor_Corrosion_of_the_Bizet_Spooky");

			overlay = _content.Load<Texture2D>("Duck_Pond/Other_Sprites/Duck_Select");
			banner = _content.Load<Texture2D>("Duck_Pond/Other_Sprites/Point_Border");
			if (duckType == DuckType.Fred)
			{
				for (int i = 0; i < idle.Length; i++)
				{
					string add = "";
					if ((i + 1) < 10)
					{
						add = ("000" + (i + 1));
					}
					if ((i + 1) >= 10 )
					{
						add = ("00" + (i + 1));
					}

					if (i < 15)
					{
						selected[i] = _content.Load<Texture2D>("Duck_Pond/Selections/Fredbear_Duck_1/" + add);
					}
					idle[i] = _content.Load<Texture2D>("Duck_Pond/Idles/Fredbear_Duck_1/" + add);
				}
			}
			else if (duckType == DuckType.GFred)
			{
					for (int i = 0; i < idle.Length; i++)
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
					if (i < 15)
						{
							selected[i] = _content.Load<Texture2D>("Duck_Pond/Selections/Fredbear_Duck_2/" + add);
						}
						idle[i] = _content.Load<Texture2D>("Duck_Pond/Idles/Fredbear_Duck_2/" + add);
					}
			}
			else 
			{
					for (int i = 0; i < idle.Length; i++)
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
					if (i < 15)
						{
							selected[i] = _content.Load<Texture2D>("Duck_Pond/Selections/Fredbear_Duck_3/" + add);
						}
						idle[i] = _content.Load<Texture2D>("Duck_Pond/Idles/Fredbear_Duck_3/" + add);
					}

				banner = _content.Load<Texture2D>("Duck_Pond/Other_Sprites/He");
			}

			font = _content.Load<SpriteFont>("MiniGame_Font");
			select = _content.Load<SoundEffect>("Duck_Pond/Duck_Pond_Sounds/pickupCoin");
			chosen = _content.Load<SoundEffect>("Duck_Pond/Duck_Pond_Sounds/powerUp");
			Crash = _content.Load<SoundEffect>("Duck_Pond/Duck_Pond_Sounds/df");
			Finished = _content.Load<SoundEffect>("Duck_Pond/Duck_Pond_Sounds/jump");

			ducksCollision[0] = new BoundingRectangle(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2, ScreenManager.GraphicsDevice.Viewport.Height / 2), 193 / 2, 134 / 2);
			ducksCollision[1] = new BoundingRectangle(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 - 250, ScreenManager.GraphicsDevice.Viewport.Height / 2 - 200), 193 / 2, 134 / 2);
			ducksCollision[2] = new BoundingRectangle(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2, ScreenManager.GraphicsDevice.Viewport.Height / 2 - 200), 193 / 2, 134 / 2);
			ducksCollision[3] = new BoundingRectangle(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 - 350, ScreenManager.GraphicsDevice.Viewport.Height / 2 + 100), 193 / 2, 134 / 2);
			ducksCollision[4] = new BoundingRectangle(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 + 350, ScreenManager.GraphicsDevice.Viewport.Height / 2 + 100), 193 / 2, 134 / 2);
			ducksCollision[5] = new BoundingRectangle(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 + 175, ScreenManager.GraphicsDevice.Viewport.Height / 2 - 300), 193 / 2, 134 / 2);
            ducksCollision[6] = new BoundingRectangle(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 + 100, ScreenManager.GraphicsDevice.Viewport.Height / 2 + 250), 193 / 2, 134 / 2);
            ducksCollision[7] = new BoundingRectangle(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 - 100, ScreenManager.GraphicsDevice.Viewport.Height / 2 - 350), 193 / 2, 134 / 2);
            ducksCollision[8] = new BoundingRectangle(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 + 300, ScreenManager.GraphicsDevice.Viewport.Height / 2 - 50), 193 / 2, 134 / 2);
            ducksCollision[9] = new BoundingRectangle(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 - 100, ScreenManager.GraphicsDevice.Viewport.Height / 2 + 350), 193 / 2, 134 / 2);
            ducksCollision[10] = new BoundingRectangle(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 - 200, ScreenManager.GraphicsDevice.Viewport.Height / 2), 193 / 2, 134 / 2);
            ducksCollision[11] = new BoundingRectangle(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 - 100, ScreenManager.GraphicsDevice.Viewport.Height / 2 + 150), 193 / 2, 134 / 2);
            ducksCollision[12] = new BoundingRectangle(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2 + 100, ScreenManager.GraphicsDevice.Viewport.Height / 2 + 100), 193 / 2, 134 / 2);

            MediaPlayer.IsRepeating = true;
			MediaPlayer.Play(songs[(int)duckType]);
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
			for (int i = 0; i < ducks.Length; i++) {
				if (mouse.collidesWith(ducksCollision[i]) && showDucks[i] && canSelect)
				{
					if (currentMousePosition.LeftButton == ButtonState.Pressed && pastMousePosition.LeftButton == ButtonState.Released)
					{
						canSelect = false;
						chosen.Play();
						whereToDraw = new Rectangle(0, 0, 0, 0);
						ducks[i].selected = true;
					}
				}

			}

			if (duckType != DuckType.SadFred && close == true)
			{
				canSelect = false;
				for (int i = 0; i < playerRef.consecutivePlays.Length; i++) 
				{
					if (i != 1) 
					{
						playerRef.consecutivePlays[i] = 0;
					}
				}
				timeToShowScore += 1;
				if (timeToShowScore == 1) 
				{
					Finished.Play();
				}
				if (timeToShowScore == 100)
				{
					playerRef.consecutivePlays[1] += 1;
					playerRef.ticketAmount += score;
					foreach (var screen in ScreenManager.GetScreens())
						screen.ExitScreen();

					ScreenManager.AddScreen(new GameSelect(playerRef, game), PlayerIndex.One);
				}
			}
			else if (duckType == DuckType.SadFred && close == true)
			{


                timeToShowScore += 1;

				if (timeToShowScore >= 2) 
				{
                    MediaPlayer.Pause();
                    if (timeToShowScore == 2)
					{
                        Crash.Play();
						playerRef.foundSecret[1] = true;
						for (int i = 0; i < playerRef.consecutivePlays.Length; i++)
						{
							if (i != 1)
							{
								playerRef.consecutivePlays[i] = 0;
							}
						}
					}
                    crash = true;
                }

                if (timeToShowScore == 100)
                {
					playerRef.ticketAmount += score;
					foreach (var screen in ScreenManager.GetScreens())
						screen.ExitScreen();

					ScreenManager.AddScreen(new GameSelect(playerRef, game), PlayerIndex.One);
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

			if (duckType != DuckType.SadFred)
			{
				spriteBatch.Draw(backgrounds[0], Vector2.Zero, Color.White);
			}
			else 
			{
				spriteBatch.Draw(backgrounds[1], Vector2.Zero, Color.White);
			}

			spriteBatch.DrawString(font, "Duck pulls: " + count, new Vector2(50, 0), Color.White);

			if (duckType != DuckType.SadFred && crash == false)
			{
				spriteBatch.DrawString(font, "Score: " + score, new Vector2(50, graphics.Viewport.Height - 100), Color.White);
			}
			else if (duckType == DuckType.SadFred && crash == false)
			{
				if (count == 5)
				{
					spriteBatch.DrawString(font, "Score: " + score, new Vector2(50, graphics.Viewport.Height - 100), Color.White);
				}
				else if (count == 4)
				{
					spriteBatch.DrawString(font, "Score: Susie", new Vector2(50, graphics.Viewport.Height - 100), Color.White);
				}
				else if (count == 3) 
				{
					spriteBatch.DrawString(font, "Score: Jeremy", new Vector2(50, graphics.Viewport.Height - 100), Color.White);
				}
				else if (count == 2)
				{
					spriteBatch.DrawString(font, "Score: Gabriel", new Vector2(50, graphics.Viewport.Height - 100), Color.White);
				}
				else if (count == 1)
				{
					spriteBatch.DrawString(font, "Score: Fritz", new Vector2(50, graphics.Viewport.Height - 100), Color.White);
				}
				else if (count == 0)
				{
					spriteBatch.DrawString(font, "Score: Cassidy", new Vector2(50, graphics.Viewport.Height - 100), Color.White);
				}
			}



			duckAnimationTimer_idle += gameTime.ElapsedGameTime.TotalSeconds;
			if (duckAnimationTimer_idle > .25)
			{
				duckIdleAnimationFrame++;
				if (duckIdleAnimationFrame == 19) 
				{
					duckIdleAnimationFrame = 0;
				}
				duckAnimationTimer_idle -= .25;
			}

            drawDuck(0, 0, spriteBatch, gameTime, 0, graphics);
            drawDuck(-250, -200, spriteBatch, gameTime, 1, graphics);
            drawDuck(0, -200, spriteBatch, gameTime, 2, graphics);
            drawDuck(-350, 100, spriteBatch, gameTime, 3, graphics);
            drawDuck(350, 100, spriteBatch, gameTime, 4, graphics);
            drawDuck(175, -300, spriteBatch, gameTime, 5, graphics);
            drawDuck(100, 250, spriteBatch, gameTime, 6, graphics);
            drawDuck(-100, -350, spriteBatch, gameTime, 7, graphics);
            drawDuck(300, -50, spriteBatch, gameTime, 8, graphics);
            drawDuck(-100, 350, spriteBatch, gameTime, 9, graphics);
            drawDuck(-200, 0, spriteBatch, gameTime, 10, graphics);
            drawDuck(-100, 150, spriteBatch, gameTime, 11, graphics);
            drawDuck(100, 100, spriteBatch, gameTime, 12, graphics);

            if (count == 0) 
			{
				close = true;
			}

			if (crash) 
			{
				//canSelect = false;
				spriteBatch.Draw(backgrounds[2], Vector2.Zero, Color.White);
			}



			spriteBatch.End();

		}

		private void drawDuck(int x, int y, SpriteBatch spriteBatch, GameTime gameTime, int i, GraphicsDevice graphics) 
		{
			Color color = Color.White;
            if (!ducks[i].selected && showDucks[i])
            {
				if (ducks[i].points == 900)
				{
					color = Color.Green;
				}
				else if (ducks[i].points == 500) 
				{
					color = Color.Red;
				}
                spriteBatch.Draw(idle[duckIdleAnimationFrame], new Vector2(graphics.Viewport.Width / 2 + x, graphics.Viewport.Height / 2 + y), null, color, 0f, new Vector2(97.3f, 76.6f), 1f, SpriteEffects.None, 0);
                spriteBatch.Draw(overlay, whereToDraw, null, Color.White, 0f, new Vector2(42, 42), SpriteEffects.None, 0);
            }
            else if (ducks[i].selected && showDucks[i])
            {
                duckAnimationTimer_select += gameTime.ElapsedGameTime.TotalSeconds;
                if (duckAnimationTimer_select > .25)
                {

                    duckSelectedAnimationFrame++;
                    if (duckSelectedAnimationFrame >= 14)
                    {
                        duckSelectedAnimationFrame = 14;
                    }
                    duckAnimationTimer_select -= .25;
                }
                spriteBatch.Draw(selected[duckSelectedAnimationFrame], new Vector2(graphics.Viewport.Width / 2 + x, graphics.Viewport.Height / 2 + y), null, Color.White, 0f, new Vector2(97.3f, 76.6f), 1f, SpriteEffects.None, 0);

                if (duckSelectedAnimationFrame == 14)
                {
                    spriteBatch.Draw(banner, new Vector2(graphics.Viewport.Width / 2 + x - 50, graphics.Viewport.Height / 2 + y - 12.5f), null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);

                    spriteBatch.DrawString(font, ducks[i].points.ToString(), new Vector2(graphics.Viewport.Width / 2 + x - 10, graphics.Viewport.Height / 2 + y - 15), Color.White, 0f, Vector2.Zero, .25f, SpriteEffects.None, 0);
                    timeToShowScore += 1;

                    if (timeToShowScore == 100)
                    {
                        showDucks[i] = false;
                        count--;
                        score += ducks[i].points;
                        duckSelectedAnimationFrame = 0;
                        timeToShowScore = 0;
                        canSelect = true;
                    }
                }

            }
        }
	}
}
