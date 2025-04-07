using CIS598Project.Game_Entities;
using CIS598Project.StateManagement;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace CIS598Project.Rooms
{
	public class GameSelect : GameScreen
	{
		SpriteFont font;

		Game game;

		Player player;

		Song backgroundMusic;

		ContentManager _content;

		KeyboardState pastKeyboardState;

		KeyboardState currentKeyboardState;

		int currentGame = 0;

		public GameSelect(Player player, Game game) 
		{
			this.game = game;
			this.player = player;
			game.IsMouseVisible = true;
			game.Window.Title = "Fredbear and Friends: Arcade";
		}

		public override void Activate()
		{
			base.Activate();

			if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

			font = _content.Load<SpriteFont>("MiniGame_Font");
			backgroundMusic = _content.Load<Song>("Thank_You_For_Your_Patience_2_");




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

			if (currentKeyboardState.IsKeyDown(Keys.Up) && pastKeyboardState.IsKeyUp(Keys.Up)) 
			{
				currentGame++;
				if (currentGame == 6) 
				{
					currentGame = 0;
				}
			}

			if (currentKeyboardState.IsKeyDown(Keys.Down) && pastKeyboardState.IsKeyUp(Keys.Down))
			{
				currentGame--;
				if (currentGame == -1)
				{
					currentGame = 6;
				}
			}

			if (currentKeyboardState.IsKeyDown(Keys.E) && pastKeyboardState.IsKeyUp(Keys.E))
			{
				if (currentGame == 0)
				{
					ScreenManager.AddScreen(new DuckPond(game, player), PlayerIndex.One);
				}
				else if (currentGame == 1)
				{
					ScreenManager.AddScreen(new Discount_Ballpit(game, player), PlayerIndex.One);
				}
				else if (currentGame == 2)
				{
					ScreenManager.AddScreen(new BalloonBarrel(game, player), PlayerIndex.One);
				}
				else if (currentGame == 3)
				{
					ScreenManager.AddScreen(new Ballpit_Tower(game, player), PlayerIndex.One);
				}
				else if (currentGame == 4)
				{
					ScreenManager.AddScreen(new FruityMaze(game, player), PlayerIndex.One);
				}
				else 
				{
					ScreenManager.AddScreen(new Security(game, player), PlayerIndex.One);
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

			string name = " ";

			if (currentGame == 0)
			{
				name = "Duck Pond";
			}
			else if (currentGame == 1)
			{
				name = "Discount Ballpit";
			}
			else if (currentGame == 2)
			{
				name = "Balloon Barrel";
			}
			else if (currentGame == 3)
			{
				name = "Ballpit Tower";
			}
			else if (currentGame == 4)
			{
				name = "Fruity Maze";
			}
			else
			{
				name = "Security";
			}

			spriteBatch.DrawString(font, "Tickets: " + player.ticketAmount, Vector2.Zero, Color.White);
			if (currentGame == 0)
			{
				spriteBatch.DrawString(font, "Consecutive Plays: " + player.consecutivePlays[1], new Vector2(0, 125), Color.White);
				spriteBatch.DrawString(font, "Found Secret? " + player.foundSecret[1], new Vector2(0, 250), Color.White);
			}
			else if (currentGame == 1)
			{
				spriteBatch.DrawString(font, "Plays: " + player.ballpitPlays, new Vector2(0, 125), Color.White);
				spriteBatch.DrawString(font, "Found Secret? " + player.foundSecret[2], new Vector2(0, 250), Color.White);
			}
			else if (currentGame == 2)
			{
				spriteBatch.DrawString(font, "Consecutive Plays: " + player.consecutivePlays[0], new Vector2(0, 125), Color.White);
				spriteBatch.DrawString(font, "Found Secret? " + player.foundSecret[0], new Vector2(0, 250), Color.White);
			}
			else if (currentGame == 3)
			{
				spriteBatch.DrawString(font, "Losses: " + player.losses[0], new Vector2(0, 125), Color.White);
				spriteBatch.DrawString(font, "Found Secret? " + player.foundSecret[11], new Vector2(0, 250), Color.White);
			}
			else if (currentGame == 4)
			{
				spriteBatch.DrawString(font, "Wins: " + player.fruityMazeWins, new Vector2(0, 125), Color.White);
				spriteBatch.DrawString(font, "Found Secret? " + player.foundSecret[5], new Vector2(0, 250), Color.White);
			}
			else
			{
				spriteBatch.DrawString(font, "Found Secret? " + player.foundSecret[5], new Vector2(0, 125), Color.White);
			}
			spriteBatch.DrawString(font, name, new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2), Color.White);



			spriteBatch.End();



		}


	}
}
