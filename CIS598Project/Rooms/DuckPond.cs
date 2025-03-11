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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar;

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

		DuckType duckType;

		Duck[] ducks = new Duck[16];

		bool[] showDucks = new bool[16];

		private int duckIdleAnimationFrame = 0;

		private int duckSelectedAnimationFrame = 0;

		private MouseState pastMousePosition;
		private MouseState currentMousePosition;

		SpriteFont font;

		ContentManager _content;

		BoundingRectangle[] ducksCollision = new BoundingRectangle[16];
			
		BoundingRectangle mouse = new(0, 0, 32, 32);

		public BoundingRectangle Bounds => mouse;

		Player playerRef;

		public DuckPond(Player player, Game game) 
		{
			this.game = game;
			if (player.consecutivePlays[1] == 1 && player.foundSecret[1] == false)
			{
				duckType = DuckType.GFred;
				game.Window.Title = "I'M SO LONELY...";
			}
			else if (player.consecutivePlays[1] == 2 && player.foundSecret[1] == false)
			{
				duckType = DuckType.SadFred;
				game.Window.Title = "DO YOU HEAR ME?";
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

			MediaPlayer.IsRepeating = true;
			//MediaPlayer.Play(songs[(int)duckType]);
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

			if (duckType != DuckType.SadFred)
			{
				spriteBatch.Draw(backgrounds[0], Vector2.Zero, Color.White);
			}
			else 
			{
				spriteBatch.Draw(backgrounds[1], Vector2.Zero, Color.White);
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


			if (!ducks[0].selected)
			{
				spriteBatch.Draw(idle[duckIdleAnimationFrame], new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2), Color.White);
			}
			else if (ducks[0].selected) 
			{

			}
			spriteBatch.End();

		}
	}
}
