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
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace CIS598Project.Rooms
{

	public enum ballpitState 
	{
		wait,
		play,
		fredbear
	}
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

		Texture2D[] Fredbear_Cry = new Texture2D[5];

		Texture2D[] Golden_Cry = new Texture2D[5];

		Texture2D crashScreen;

		Texture2D overlay;

		Texture2D arrow;

		Texture2D jumped;

		Song[] songs = new Song[6];

		SpriteFont font;

		bool devCheat = false; //sets arrow to center

		FredbearState fredbear = FredbearState.idle;

		ballpitState ballpit = ballpitState.wait;

		BoundingRectangle[] bounds = new BoundingRectangle[3];

		Vector2 arrowPosition = new(0, 387);

		Vector2 fredbearPosition = new(0, 0);

		BoundingRectangle fredbearHitBox = new(0, 0, 145, 275);

        KeyboardState pastKeyboardState;

        KeyboardState currentKeyboardState;

		SoundEffect jump;

		SoundEffect balls;

		SoundEffect fail;

		SoundEffect crash;

		SoundEffect crying;

		SoundEffect completed;

		SoundEffectInstance complete;

		SoundEffectInstance cry;

		SoundEffectInstance crashed;

        bool goRight = true;

		int misses = 0;

		int roundNum = 1;

		int score = 0;

		int random = 0;

		double reactionTimer;

		double thoughtsTimer = 0;

		double finalThoughtsTime = 0;



		double buffer = 0;

		double fredbearTimer = 0;

		int fredbear_Frame = 0;

		bool showFinalThought;

		string[] thoughts = { "Loser.", "Has no friends.", "Moron.", "Fatty.", "Why would anyone be friends with you?", "Ha you're so pathetic."};

		string finalThought = "I would like a balloon...";

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

			for (int i = 0; i < Fredbear_Idle.Length; i++) 
			{
				Fredbear_Idle[i] = _content.Load<Texture2D>("Discount_Ballpit/Textures/Gameplay_elements/Fredbear/Readying/000" + (i + 1));
                Fredbear_Cry[i] = _content.Load<Texture2D>("Discount_Ballpit/Textures/Gameplay_elements/Fredbear/Failed/000" + (i + 1));
                Golden_Cry[i] = _content.Load<Texture2D>("Discount_Ballpit/Textures/Gameplay_elements/Golden_Freddy/Crying/000" + (i + 1));
            }

			jumped = _content.Load<Texture2D>("Discount_Ballpit/Textures/Gameplay_elements/Fredbear/Falling/Falling");

			jump = _content.Load<SoundEffect>("Discount_Ballpit/Sounds/Soundeffects/jump");

			arrow = _content.Load<Texture2D>("Discount_Ballpit/Textures/Gameplay_elements/Arrow");

			font = _content.Load<SpriteFont>("MiniGame_Font");

            crash = _content.Load<SoundEffect>("Duck_Pond/Duck_Pond_Sounds/df");

            balls = _content.Load<SoundEffect>("Discount_Ballpit/Sounds/Soundeffects/balls");

			fail = _content.Load<SoundEffect>("Discount_Ballpit/Sounds/Soundeffects/landing");

			crying =  _content.Load<SoundEffect>("Balloon_Barrel/Sounds/Soundeffect/cry");

			completed = _content.Load<SoundEffect>("Fruity_Maze/Sounds/Soundeffects/20");

            crashScreen = _content.Load<Texture2D>("Duck_Pond/Crash");

            overlay = _content.Load<Texture2D>("Balloon_Barrel/Backgrounds/Overlay");

            previousSong = songs[0];

			bounds[0] = new(0, 788, 818, 68);
			bounds[1] = new(818, 622, 277, 260);
			bounds[2] = new(1091, 788, 829, 68);


			MediaPlayer.IsRepeating = true;
			MediaPlayer.Play(songs[0]);
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

			if (ballpit == ballpitState.play) {
				if (devCheat != true)
				{
					if (arrowPosition.X >= game.GraphicsDevice.Viewport.Width)
					{
						goRight = false;
					}
					else if (arrowPosition.X <= 0)
					{
						goRight = true;
					}

					if (goRight)
					{
						arrowPosition.X += 50;
					}
					else
					{
						arrowPosition.X -= 50;
					}
				}
				else 
				{
					arrowPosition.X = 939;
				}

                if (currentKeyboardState.IsKeyDown(Keys.Space) && pastKeyboardState.IsKeyUp(Keys.Space)) 
				{
					fredbearPosition.X = arrowPosition.X;
					fredbearPosition.Y = 0;
					ballpit = ballpitState.fredbear;
					fredbear = FredbearState.jumped;
					jump.Play();
					arrowPosition.X = 0;
				}

            }

			if (fredbear == FredbearState.jumped && ballpit == ballpitState.fredbear) 
			{
				fredbearPosition.Y += 7.5f;
				fredbearHitBox.X = fredbearPosition.X;
				fredbearHitBox.Y = fredbearPosition.Y;
				for (int i = 0; i < bounds.Length; i++) 
				{
					if (fredbearHitBox.collidesWith(bounds[i])) 
					{
						if (i == 1)
						{
							fredbear = FredbearState.success;
							score += 250;
							misses = 0;
							balls.Play();
						}
						else 
						{
							MediaPlayer.Stop();
							fredbear = FredbearState.fail;
							misses += 1;
							fail.Play();
						}
						fredbearTimer = 0;
						roundNum += 1;
					}
				}
			}

			if (fredbear == FredbearState.success || fredbear == FredbearState.fail) 
			{
				reactionTimer += gameTime.ElapsedGameTime.TotalSeconds;

				if (reactionTimer >= 3) 
				{
					reactionTimer -= 3;

					if (PlayerRef.foundSecret[2] == true)
					{
						fredbear = FredbearState.idle;
						ballpit = ballpitState.wait;
						MediaPlayer.Play(songs[0]);
					}
					else if (roundNum != 6 || misses == 5 && PlayerRef.foundSecret[2] != true)
					{
						if (misses == 0 && previousSong != songs[0] && PlayerRef.foundSecret[2] == false)
						{
							MediaPlayer.Play(songs[0]);
							previousSong = songs[0];
						}
						else if (misses == 1 && previousSong != songs[1] && PlayerRef.foundSecret[2] == false)
						{
							MediaPlayer.Play(songs[1]);
							previousSong = songs[1];
						}
						else if (misses == 2 && previousSong != songs[2] && PlayerRef.foundSecret[2] == false)
						{
							MediaPlayer.Play(songs[2]);
							previousSong = songs[2];
						}
						else if (misses == 3 && previousSong != songs[3] && PlayerRef.foundSecret[2] == false)
						{
							MediaPlayer.Play(songs[3]);
							previousSong = songs[3];
						}
						else if (misses == 4 && previousSong != songs[4] && PlayerRef.foundSecret[2] == false)
						{
							MediaPlayer.Play(songs[4]);
							previousSong = songs[4];
						}
						else if (misses == 5 && previousSong != songs[5] && PlayerRef.foundSecret[2] == false)
						{
							MediaPlayer.Play(songs[5]);
							previousSong = songs[5];
						}
						if (misses != 5)
						{
							fredbear = FredbearState.idle;
							ballpit = ballpitState.wait;
						}
					}

				}

			}

			if (roundNum == 6 && misses == 5 && PlayerRef.foundSecret[2] == false)
			{
				if (cry == null)
				{
					cry = crying.CreateInstance();
					cry.Play();
				}
				if (cry.State != SoundState.Playing)
				{
					showFinalThought = true;
				}

			}
			else if (roundNum == 6 && misses != 5 && PlayerRef.foundSecret[2] == false)
			{
				if (complete == null)
				{
					complete = completed.CreateInstance();
					complete.Play();
				}
				if (complete.State != SoundState.Playing)
				{
					PlayerRef.ticketAmount += score;
					PlayerRef.ballpitPlays += 1;
					game.Exit();
				}
			}
			else if (roundNum == 6 && PlayerRef.foundSecret[2] == true) 
			{
				if (complete == null)
				{
					complete = completed.CreateInstance();
					complete.Play();
				}
				if (complete.State != SoundState.Playing)
				{
					PlayerRef.ticketAmount += score;
					PlayerRef.ballpitPlays += 1;
					game.Exit();
				}
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

            if (misses != 5 || PlayerRef.foundSecret[2] == true)
            {
                spriteBatch.DrawString(font, "Score: " + score, Vector2.Zero, Color.White);
            }
            else if (misses == 5 && PlayerRef.foundSecret[2] != true && showFinalThought == false)
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

			if (fredbear == FredbearState.idle) 
			{
				fredbearTimer += gameTime.ElapsedGameTime.TotalSeconds;

				double gate = 0; //How fast the animation should go based off the amount of misses
				if (misses == 0 || PlayerRef.foundSecret[2] == true)
				{
					gate = .5;
				}
				else if (misses == 1)
				{
					gate = .65;
				}
				else if (misses == 2)
				{
					gate = .75;
				}
				else if (misses == 3)
				{
					gate = .85;
				}
				else 
				{
					gate = 1;
				}

				if (fredbearTimer >= gate) 
				{
					fredbear_Frame++;
					if (fredbear_Frame == 5) 
					{
						fredbear_Frame = 0;
					}
					fredbearTimer -= gate;
				}

				spriteBatch.Draw(Fredbear_Idle[fredbear_Frame], Vector2.Zero, Color.White);
			}

			if (ballpit == ballpitState.wait) 
			{
				buffer += gameTime.ElapsedGameTime.TotalSeconds;

				if (buffer <= 3)
				{
					spriteBatch.DrawString(font, "ROUND " + roundNum + "!!!", new Vector2(graphics.Viewport.Width / 2 - 150, graphics.Viewport.Height / 2 - 100), Color.White);
				}
				else if (buffer > 3 && buffer < 6)
				{
                    spriteBatch.DrawString(font, "READY!!!", new Vector2(graphics.Viewport.Width / 2 - 150, graphics.Viewport.Height / 2 - 100), Color.White);
                }
				else if (buffer >= 6 && buffer < 9)
				{
                    spriteBatch.DrawString(font, "GO!!!", new Vector2(graphics.Viewport.Width / 2 - 150, graphics.Viewport.Height / 2 - 100), Color.White);
                }
				else 
				{
					ballpit = ballpitState.play;
					buffer = 0;
				}
			}

			if (ballpit == ballpitState.play) 
			{
				buffer += gameTime.ElapsedGameTime.TotalSeconds;
				if (buffer >= 2 && buffer <= 4)
				{
					spriteBatch.DrawString(font, "Press Space!!!", new Vector2(graphics.Viewport.Width / 2 - 250, 0), Color.White);
				}
				else if (buffer > 4) 
				{
					buffer -= 4;
				}
				spriteBatch.Draw(arrow, arrowPosition, Color.White);
			}

			if (fredbear == FredbearState.jumped) 
			{
					spriteBatch.Draw(jumped, fredbearPosition, Color.White);
			}

			if (fredbear == FredbearState.fail && PlayerRef.foundSecret[2] == true )
			{
                fredbearTimer += gameTime.ElapsedGameTime.TotalSeconds;

                double gate = 0; //How fast the animation should go based off the amount of misses
                if (misses == 0 || PlayerRef.foundSecret[2] == true)
                {
                    gate = .5;
                }
                else if (misses == 1)
                {
                    gate = .65;
                }
                else if (misses == 2)
                {
                    gate = .75;
                }
                else if (misses == 3)
                {
                    gate = .85;
                }
                else
                {
                    gate = 1;
                }

                if (fredbearTimer >= gate)
                {
                    fredbear_Frame++;
                    if (fredbear_Frame >= 5)
                    {
                        fredbear_Frame = 0;
                    }
                    fredbearTimer -= gate;
                }

                spriteBatch.Draw(Fredbear_Cry[fredbear_Frame], new Vector2(fredbearPosition.X, fredbearPosition.Y + 100), Color.White);
            }
			else if (fredbear == FredbearState.fail && misses == 5 && PlayerRef.foundSecret[2] == false) 
			{
				fredbearTimer += gameTime.ElapsedGameTime.TotalSeconds;
				if (fredbearTimer >= .3) 
				{
                    fredbear_Frame++;
                    if (fredbear_Frame >= 5)
                    {
                        fredbear_Frame = 0;
                    }
                    fredbearTimer -= .3;
                }
                spriteBatch.Draw(Golden_Cry[fredbear_Frame], new Vector2(fredbearPosition.X, fredbearPosition.Y + 200), null, Color.White, 0f, new Vector2(194 / 2, 325f / 2f), .75f, SpriteEffects.None, 0);
            }

			if (showFinalThought) 
			{
				MediaPlayer.Stop();

				spriteBatch.Draw(overlay, Vector2.Zero, Color.White);

				spriteBatch.DrawString(font, finalThought, new Vector2(graphics.Viewport.Width / 2 - 100, graphics.Viewport.Height / 2 - 100), Color.Yellow);

				finalThoughtsTime += gameTime.ElapsedGameTime.TotalSeconds;

				if (finalThoughtsTime >= 5) 
				{
					spriteBatch.Draw(crashScreen, Vector2.Zero, Color.White);
					if (crashed == null)
					{
						crashed = crash.CreateInstance();
						crashed.Play();
					}
					if(crashed.State != SoundState.Playing)
					{ 
						PlayerRef.foundSecret[2] = true;
						game.Exit();
					}
				}
			}


			spriteBatch.End();
		}


	}
}
