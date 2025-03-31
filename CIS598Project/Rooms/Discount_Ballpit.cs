using CIS598Project.Collisions;
using CIS598Project.Game_Entities;
using CIS598Project.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


namespace CIS598Project.Rooms
{
	public enum FredbearState
	{
		idle, 
		jumped,
		success,
		fail
	}
	public class Discount_Ballpit: GameScreen
	{
		Game game;

		Player PlayerRef;

		Song previousSong;

		ContentManager _content;

		Texture2D[] backgrounds = new Texture2D[2];

		Texture2D[] Fredbear_Idle = new Texture2D[5];

		Song[] songs = new Song[6];

		SpriteFont font;

		int misses = 0;

		int rounds = 5;

		int score = 0;

		int random = 0;

		double thoughtsTimer = 0;

		string[] thoughts = { "Loser.", "Has no friends.", "Moron.", "Fatty.", "Why would anyone be friends with you?", "Ha you're so pathetic."};

		public Discount_Ballpit(Game game, Player player) 
		{

			PlayerRef = player;

			this.game = game;


		}


		public override void Activate()
		{
			base.Activate();

			if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

			if (PlayerRef.ballpitPlays == 0 || PlayerRef.foundSecret[2])
			{
				backgrounds[0] = _content.Load<Texture2D>("Discount_Ballpit/Textures/Background/Ballpit_Background_1");
				backgrounds[1] = _content.Load<Texture2D>("Discount_Ballpit/Textures/Background/Ballpit_Background_1_purple");
			}
			else if (!PlayerRef.foundSecret[2]) 
			{
				if (PlayerRef.ballpitPlays == 1)
				{
					backgrounds[0] = _content.Load<Texture2D>("Discount_Ballpit/Textures/Background/Ballpit_Background_2");
					backgrounds[1] = _content.Load<Texture2D>("Discount_Ballpit/Textures/Background/Ballpit_Background_2_purple");
				}
				else if (PlayerRef.ballpitPlays == 2)
				{
					backgrounds[0] = _content.Load<Texture2D>("Discount_Ballpit/Textures/Background/Ballpit_Background_3");
					backgrounds[1] = _content.Load<Texture2D>("Discount_Ballpit/Textures/Background/Ballpit_Background_3_purple");
				}
				else 
				{
                    backgrounds[0] = _content.Load<Texture2D>("Discount_Ballpit/Textures/Background/Ballpit_Background_4");
                    backgrounds[1] = _content.Load<Texture2D>("Discount_Ballpit/Textures/Background/Ballpit_Background_4_purple");
                }
            }

			songs[0] = _content.Load<Song>("Discount_Ballpit/Sounds/Songs/A_slice_and_a_scoop");
            songs[1] = _content.Load<Song>("Discount_Ballpit/Sounds/Songs/A_slice_and_a_scoop_miss1");
            songs[2] = _content.Load<Song>("Discount_Ballpit/Sounds/Songs/A_slice_and_a_scoop_miss2");
            songs[3] = _content.Load<Song>("Discount_Ballpit/Sounds/Songs/A_slice_and_a_scoop_miss3");
            songs[4] = _content.Load<Song>("Discount_Ballpit/Sounds/Songs/A_slice_and_a_scoop_miss4");
            songs[5] = _content.Load<Song>("Discount_Ballpit/Sounds/Songs/Creme_De_La_Creme");

			font = _content.Load<SpriteFont>("MiniGame_Font");

			previousSong = songs[0];

			MediaPlayer.IsRepeating = true;
			MediaPlayer.Play(songs[0]);
        }

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, false);

			//pastKeyboardState = currentKeyboardState;
			//currentKeyboardState = Keyboard.GetState();



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

			if(misses == 0 && previousSong != songs[0]) 
			{
                MediaPlayer.Play(songs[0]);
                previousSong = songs[0];
            }
			else if(misses == 1 && previousSong != songs[1]) 
			{
				MediaPlayer.Play(songs[1]);
				previousSong = songs[1];
			}
            else if (misses == 2 && previousSong != songs[2])
            {
                MediaPlayer.Play(songs[2]);
                previousSong = songs[2];
            }
            else if (misses == 3 && previousSong != songs[3])
            {
                MediaPlayer.Play(songs[3]);
                previousSong = songs[3];
            }
            else if (misses == 4 && previousSong != songs[4])
            {
                MediaPlayer.Play(songs[4]);
                previousSong = songs[4];
            }
            else if (misses == 5 && previousSong != songs[5])
            {
                MediaPlayer.Play(songs[5]);
                previousSong = songs[5];
            }


        }

		public override void Draw(GameTime gameTime)
		{
			var graphics = ScreenManager.GraphicsDevice;
			var spriteBatch = ScreenManager.SpriteBatch;

			graphics.Clear(Color.Black);

			spriteBatch.Begin();

			if (misses != 5 && PlayerRef.foundSecret[2] != true)
			{
				spriteBatch.Draw(backgrounds[0], Vector2.Zero, Color.White);
			}
			else if (misses == 5 && PlayerRef.foundSecret[2] != true)
			{
				spriteBatch.Draw(backgrounds[1], Vector2.Zero, Color.White);
			}
			else 
			{
                spriteBatch.Draw(backgrounds[0], Vector2.Zero, Color.White);
            }

            if (misses != 5 && PlayerRef.foundSecret[2] != true)
            {
                spriteBatch.DrawString(font, "Score: " + score, Vector2.Zero, Color.White);
            }
            else if (misses == 5 && PlayerRef.foundSecret[2] != true)
            {
				thoughtsTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (thoughtsTimer >= 1.5)
				{
					int currentValue = random;
					while (random == currentValue)
					{
						random = new Random().Next(0, thoughts.Length);
					}
					if (random != currentValue)
					{
						thoughtsTimer -= 1.5;
					}
				}
                spriteBatch.DrawString(font, "Score: " + thoughts[random], Vector2.Zero, Color.Purple);
            }
            else
            {
                spriteBatch.Draw(backgrounds[0], Vector2.Zero, Color.White);
            }


			spriteBatch.End();
		}


	}
}
