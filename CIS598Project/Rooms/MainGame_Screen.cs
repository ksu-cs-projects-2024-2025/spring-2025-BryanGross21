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
using CIS598Project.Collisions;

namespace CIS598Project.Rooms
{
	public enum MainGame_ScreenState 
	{
		map,
		stage,
		shop,
		save
	}
	public class MainGame_Screen : GameScreen
	{
		SpriteFont font;

		Game game;

		Player player;

		Song[] backgroundMusic = new Song[5];

		Texture2D[] controls = new Texture2D[5];

		Texture2D[] Fred = new Texture2D[3];

		Texture2D[] mapElements = new Texture2D[3];

		Texture2D[] TaskBar = new Texture2D[8];

		MapNode[] gameSelectors = new MapNode[8];

		double fredAnimationTimer = 0;

		int fredAnimationCount = 0;

		bool showFredbear = false;

		bool preventMove = false; //Prevents Freddy from moving until he reaches his next position

		/// <summary>
		/// 0 - 3 are for the screens, 4 is for the fredbear helper
		/// </summary>
		BoundingRectangle[] selections = new BoundingRectangle[5];

		int nodePosition = 0; //The node that the player is on.

		Vector2 fredPosition;

		ContentManager _content;

		KeyboardState pastKeyboardState;

		KeyboardState currentKeyboardState;

        private MouseState pastMousePosition;

        private MouseState currentMousePosition;

		MainGame_ScreenState state = MainGame_ScreenState.map;

		int currentGame = 0;

		public MainGame_Screen(Player player, Game game) 
		{
			this.game = game;
			this.player = player;
			game.IsMouseVisible = true;
			game.Window.Title = "Fredbear and Friends: Arcade";
			/*for (int i = 0; i < gameSelectors.Length; i++) 
			{
				gameSelectors
			}*/
			gameSelectors[0] = new(0, new Vector2(250, 250), game, player);
            gameSelectors[1] = new(1, new Vector2(450, 250), game, player);
			gameSelectors[2] = new(2, new Vector2(450, 450), game, player);
            gameSelectors[3] = new(3, new Vector2(650, 450), game, player);
            gameSelectors[4] = new(4, new Vector2(650, 650), game, player);
			gameSelectors[5] = new(5, new Vector2(450, 650), game, player);
            gameSelectors[6] = new(6, new Vector2(250, 650), game, player);
            gameSelectors[7] = new(7, new Vector2(250, 450), game, player);

			fredPosition = new Vector2(450, 250);
        }

		public override void Activate()
		{
			base.Activate();

			if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

			foreach (MapNode node in gameSelectors) 
			{
				if (node != null) 
				{
					node.LoadContent(_content);
				}
			}

			//The music for each screen
			backgroundMusic[0] = _content.Load<Song>("Desktop_Selection/Sounds/Songs/8BitTravel(Map)");
            backgroundMusic[1] = _content.Load<Song>("Desktop_Selection/Sounds/Songs/Broken8Bit(Save)");
            backgroundMusic[2] = _content.Load<Song>("Desktop_Selection/Sounds/Songs/RunningLights(Store)");
            backgroundMusic[3] = _content.Load<Song>("Desktop_Selection/Sounds/Songs/stage(no_act)");
            backgroundMusic[4] = _content.Load<Song>("Desktop_Selection/Sounds/Songs/stage(acting)");

			TaskBar[0] = _content.Load<Texture2D>("Desktop_Selection/Textures/taskbar/map");
            TaskBar[1] = _content.Load<Texture2D>("Desktop_Selection/Textures/taskbar/map_select_fredbear");
            TaskBar[2] = _content.Load<Texture2D>("Desktop_Selection/Textures/taskbar/stage");
            TaskBar[3] = _content.Load<Texture2D>("Desktop_Selection/Textures/taskbar/stageFredb");
            TaskBar[4] = _content.Load<Texture2D>("Desktop_Selection/Textures/taskbar/store");
            TaskBar[5] = _content.Load<Texture2D>("Desktop_Selection/Textures/taskbar/storeFredb");
            TaskBar[6] = _content.Load<Texture2D>("Desktop_Selection/Textures/taskbar/save");
            TaskBar[7] = _content.Load<Texture2D>("Desktop_Selection/Textures/taskbar/saveFredb");

			mapElements[0] = _content.Load<Texture2D>("Desktop_Selection/Textures/map/path/horizonal_path");
            mapElements[1] = _content.Load<Texture2D>("Desktop_Selection/Textures/map/path/vertical_path");
            mapElements[2] = _content.Load<Texture2D>("Desktop_Selection/Textures/map/path/coin");

			Fred[0] = _content.Load<Texture2D>("Desktop_Selection/Textures/map/Fred/Fred_Frame_1");
            Fred[1] = _content.Load<Texture2D>("Desktop_Selection/Textures/map/Fred/Fred_Frame_2");
            Fred[2] = _content.Load<Texture2D>("Desktop_Selection/Textures/map/Fred/Fred_Frame_3");

			controls[0] = _content.Load<Texture2D>("Desktop_Selection/Textures/map/controls/w_key");
            controls[1] = _content.Load<Texture2D>("Desktop_Selection/Textures/map/controls/a_key");
            controls[2] = _content.Load<Texture2D>("Desktop_Selection/Textures/map/controls/s_key");
            controls[3] = _content.Load<Texture2D>("Desktop_Selection/Textures/map/controls/d_key");
            controls[4] = _content.Load<Texture2D>("Desktop_Selection/Textures/map/controls/e_key");

			font = _content.Load<SpriteFont>("MiniGame_Font");

            //MediaPlayer.Play(backgroundMusic[(int)state]);
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
		}

		/*private bool fredUpdate() 
		{
			if (currentKeyboardState.IsKeyDown(Keys.A) && pastKeyboardState.IsKeyDown(Keys.A)) 
			{

			}
		}*/

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

			if (showFredbear)
			{
				spriteBatch.Draw(TaskBar[(int)state + 1], Vector2.Zero, Color.White);
			}
			else 
			{
                spriteBatch.Draw(TaskBar[(int)state], Vector2.Zero, Color.White);
            }

			if (state == MainGame_ScreenState.map) 
			{
				spriteBatch.DrawString(font, "Movement: ", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2 - 250), Color.White, 0f, Vector2.Zero, .75f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, "Selection: ", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2), Color.White, 0f, Vector2.Zero, .75f, SpriteEffects.None, 0);
                spriteBatch.Draw(controls[4], new Vector2(graphics.Viewport.Width / 2 + 250, graphics.Viewport.Height / 2), Color.White);


                spriteBatch.Draw(mapElements[0], new Vector2(350, 250), Color.White);
                spriteBatch.Draw(mapElements[1], new Vector2(450, 350), Color.White);
                spriteBatch.Draw(mapElements[0], new Vector2(550, 450), Color.White);
                spriteBatch.Draw(mapElements[1], new Vector2(650, 550), Color.White);
                spriteBatch.Draw(mapElements[0], new Vector2(550, 650), Color.White);
                spriteBatch.Draw(mapElements[0], new Vector2(350, 650), Color.White);
                spriteBatch.Draw(mapElements[1], new Vector2(250, 550), Color.White);
                spriteBatch.Draw(mapElements[1], new Vector2(250, 350), Color.White);

                foreach (MapNode node in gameSelectors)
                {
                    if (node != null)
                    {
						node.Draw(gameTime, spriteBatch);
                    }
                }

				fredAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (fredAnimationTimer > .3) 
				{
                    fredAnimationCount++;
                    if (fredAnimationCount == 3) 
					{
						fredAnimationCount = 0;
					}
					fredAnimationTimer -= .3;
				}

				spriteBatch.Draw(Fred[fredAnimationCount], fredPosition, Color.White);

                spriteBatch.Draw(controls[1], new Vector2(graphics.Viewport.Width / 2 + 300, graphics.Viewport.Height / 2 - 250), Color.White);
                spriteBatch.Draw(controls[3], new Vector2(graphics.Viewport.Width / 2 + 400, graphics.Viewport.Height / 2 - 260), Color.White);


				
            }

			spriteBatch.End();
		}


	}
}
