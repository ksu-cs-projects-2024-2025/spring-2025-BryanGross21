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

		Texture2D[] backgrounds = new Texture2D[2];

		Texture2D[] specs = new Texture2D[3];

		int specsFrame = 0;

		double specsTimer;

		SoundEffect talk;

		Song[] songs = new Song[2];

		bool isSpeaking = true;

		KeyboardState pastKeyboardState;

		KeyboardState currentKeyboardState;


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

			backgrounds[0] = _content.Load<Texture2D>("Endings/Textures/normal/Happy_birthday");
			backgrounds[1] = _content.Load<Texture2D>("Endings/Textures/secret/gfred");

			specs[0] = _content.Load<Texture2D>("Endings/Textures/secret/specs_1");
			specs[1] = _content.Load<Texture2D>("Endings/Textures/secret/specs_2");
			specs[2] = _content.Load<Texture2D>("Endings/Textures/secret/specs_3");

			songs[0] = _content.Load<Song>("Endings/Soundeffects/Songs/Where_Dreams_Die");
			songs[1] = _content.Load<Song>("Endings/Soundeffects/Songs/Void");

			talk = _content.Load<SoundEffect>("Endings/Soundeffects/Soundeffects/gfred_speak");

			MediaPlayer.IsRepeating = true;
			if (secretEnding)
			{
				MediaPlayer.Play(songs[1]);
				MediaPlayer.IsRepeating = true;
			}
			else 
			{
				MediaPlayer.Play(songs[0]);
				MediaPlayer.IsRepeating = true;
			}
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

			if (secretEnding) 
			{
				if (isSpeaking == false) 
				{

				}
			}
		}

		public override void Draw(GameTime gameTime)
		{
			var graphics = ScreenManager.GraphicsDevice;
			var spriteBatch = ScreenManager.SpriteBatch;

			graphics.Clear(Color.Black);

			spriteBatch.Begin();

			if (secretEnding == false)
			{
				spriteBatch.Draw(backgrounds[0], Vector2.Zero, Color.White);
			}
			else 
			{
				spriteBatch.Draw(backgrounds[1], Vector2.Zero, Color.White);
				specsTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (specsTimer > 2.5) 
				{
					Random ran = new();
					specsFrame = ran.Next(0, 3);
					specsTimer -= 2.5;
				}
				spriteBatch.Draw(specs[specsFrame], Vector2.Zero, Color.White);
			}
			
			spriteBatch.End();
		}


	}
}
