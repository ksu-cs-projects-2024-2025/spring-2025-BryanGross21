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
	public enum gfredState 
	{
		delay,
		speak,
		end
	}
	public class Ending_Screen : GameScreen
	{
		Game game;

		Player player;

		Texture2D[] backgrounds = new Texture2D[2];

		Texture2D[] specs = new Texture2D[3];

		int specsFrame = 0;

		double specsTimer;

		double delayTimer;

		double textTimer;

		SoundEffect talk;

		//The text on the screen
		string textOnScreen = "";

		//The length of the currently spoken line
		int currentLineLength = 0;

		//The current character of the line
		int currentCharacter = 0;

		int currentLine = 0;

		GFredDialogue secretDialogue = new();

		gfredState state = gfredState.delay;

		Song[] songs = new Song[2];

		bool understands = false;

		KeyboardState pastKeyboardState;

		KeyboardState currentKeyboardState;

		SpriteFont font;


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

			font = _content.Load<SpriteFont>("MiniGame_Font");

			talk = _content.Load<SoundEffect>("Endings/Soundeffects/Soundeffects/gfred_speak");

			MediaPlayer.IsRepeating = true;
			if (secretEnding)
			{
				player.sawEnding[1] = true;
				MediaPlayer.Play(songs[1]);
				MediaPlayer.IsRepeating = true;
			}
			else 
			{
				player.sawEnding[0] = true;
				MediaPlayer.Play(songs[0]);
				MediaPlayer.IsRepeating = true;
			}
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

			if (secretEnding) 
			{
				if (state == gfredState.delay) 
				{
					delayTimer += gameTime.ElapsedGameTime.TotalSeconds;
					if (delayTimer > 1) 
					{
						if (understands)
						{
							currentLineLength = secretDialogue.text[currentLine].Count();
							state = gfredState.speak;
							delayTimer -= 1;
						}
						else
						{
							if (currentLine == secretDialogue.text.Length - 1 || currentLine == secretDialogue.text.Length)
							{
								state = gfredState.end;
							}
							else
							{
								currentLineLength = secretDialogue.text[currentLine].Count();
								state = gfredState.speak;
								delayTimer -= 1;
							}
						}
					}
				}
				if (state == gfredState.speak) 
				{
					textTimer += gameTime.ElapsedGameTime.TotalSeconds;
					if (currentLineLength != textOnScreen.Count())
					{
						if (currentKeyboardState.IsKeyDown(Keys.X) && pastKeyboardState.IsKeyUp(Keys.X))
						{
							textOnScreen = secretDialogue.text[currentLine];
						}
					}
                    if (textTimer > .1 && currentLineLength != textOnScreen.Count())
					{
						if (char.IsLetter(secretDialogue.text[currentLine][currentCharacter]))
						{
							talk.Play();
						}
						textOnScreen += secretDialogue.text[currentLine][currentCharacter];
						currentCharacter += 1;
						textTimer -= .1;
					}
					else
					{
						if (currentLine != 3)
						{
							if (currentKeyboardState.IsKeyDown(Keys.E) && pastKeyboardState.IsKeyUp(Keys.E))
							{
								textOnScreen = "";
								understands = false;
								currentCharacter = 0;
								currentLine++;
								state = gfredState.delay;
							}
						}
						else 
						{
                            if (currentKeyboardState.IsKeyDown(Keys.Y) && pastKeyboardState.IsKeyUp(Keys.Y))
                            {
                                textOnScreen = "";
                                currentCharacter = 0;
                                currentLine++;
                                state = gfredState.delay;
                            }
                            if (currentKeyboardState.IsKeyDown(Keys.N) && pastKeyboardState.IsKeyUp(Keys.N))
                            {
								understands = true;
                                textOnScreen = "";
                                currentCharacter = 0;
                                currentLine = secretDialogue.text.Length - 1;
                                state = gfredState.delay;
                            }
                        }
					}
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
				if (state != gfredState.end)
				{
					specsTimer += gameTime.ElapsedGameTime.TotalSeconds;
					if (specsTimer > 2.5)
					{
						Random ran = new();
						specsFrame = ran.Next(0, 3);
						specsTimer -= 2.5;
					}
					spriteBatch.Draw(specs[specsFrame], Vector2.Zero, Color.White);
				}
				if (state == gfredState.speak)
				{
					if(currentLine == 3 && textOnScreen.Count() == currentLineLength) 
					{
						spriteBatch.DrawString(font, "Y for yes, N for n", new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2), Color.White);
					}
					spriteBatch.DrawString(font, textOnScreen, new Vector2(graphics.Viewport.Width / 2 - 750, 25), Color.White);
				}
				if (state == gfredState.end) 
				{
                    spriteBatch.DrawString(font, "The End.", new Vector2(graphics.Viewport.Width / 2 - 125, 25), Color.White);
                }
			}
			
			spriteBatch.End();
		}


	}
}
