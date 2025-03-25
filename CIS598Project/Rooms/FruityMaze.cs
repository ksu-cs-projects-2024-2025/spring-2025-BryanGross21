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

		Texture2D Map;

		Texture2D banner;

		Texture2D crashScreen;

		Song backgroundMusic;

		SoundEffect GameStateChange;

		SoundEffect Extended;

		SoundEffect Crash;

		SoundEffectInstance StateChange;

		SoundEffectInstance crashing;

		GameState state;

		Emotion emote;

		bool crash = false;

		private int poppyAnimationFrame = 0;

		private double poppyAnimationTimer = 0;

		private MouseState pastMousePosition;
		private MouseState currentMousePosition;

		SpriteFont font;

		ContentManager _content;

		BoundingRectangle[] ducksCollision = new BoundingRectangle[16];
			
		BoundingRectangle mouse = new(0, 0, 32, 32);

		int score = 5000;


		Player playerRef;

		public FruityMaze(Game game,Player player) 
		{
			this.game = game;
			if (player.foundSecret[5] == false)
			{
				if (player.consecutivePlays[5] == 0) 
				{

				}
			}
			else 
			{

			}
		}

		public override void Activate()
		{
			base.Activate();

			if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

			
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

			

        }
	}
}
