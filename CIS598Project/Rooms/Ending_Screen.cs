using CIS598Project.StateManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CIS598Project.Game_Entities;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace CIS598Project.Rooms
{
	public class Ending_Screen : GameScreen
	{
		Game game;

		Player player;



		ContentManager _content;

		/// <summary>
		/// If true shows the secret ending else the normal ending
		/// </summary>
		bool secretEnding;

		public Ending_Screen(Game game, Player player, bool ending) 
		{
			this.game = game;
			this.player = player;
			secretEnding = ending;
		}

		public override void Activate()
		{
			base.Activate();

			if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

		

			MediaPlayer.IsRepeating = true;
			//MediaPlayer.Play(backgroundSong);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, false);


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

		public override void Draw(GameTime gameTime)
		{
			var graphics = ScreenManager.GraphicsDevice;
			var spriteBatch = ScreenManager.SpriteBatch;

			graphics.Clear(Color.Black);

			spriteBatch.Begin();
			
			spriteBatch.End();
		}


	}
}
