using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Audio;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using CIS598Project.Rooms;
using System.Collections.Generic;


namespace CIS598Project.Game_Entities
{
	public enum FredbearState
	{
		entering,
		idle,
		talking,
		leaving
	}

	public enum answerState
	{
		enter,
		question,
		answer
	}

	public class Fredbear
	{
		Player player;

		public FredbearState state = FredbearState.entering;

		SoundEffect talk;

		double talkTimer;

		FredbearDialogue fredbearDialogue;

		Texture2D[] entering = new Texture2D[31];

		double enterAnimationTimer;

		int enterFrame;

		Texture2D idle;

		Texture2D[] talking = new Texture2D[2];

		double talkAnimationTimer;

		bool firstTalkFrame;

		bool isTalking = false;

		Texture2D[] leaving = new Texture2D[10];

		bool welcomeState = false;

		double leaveAnimationTimer;

		int leaveFrame;

		Texture2D overlay;

		SpriteFont font;

		KeyboardState pastKeyboardState;

		KeyboardState currentKeyboardState;

		public MainGame_ScreenState screenState;

		answerState aS = answerState.enter;

		string textOnScreen = "";

		string hint = "";

		//The length of the currently spoken line
		int currentLineLength = 0;

		//The current character of the line
		int currentCharacter = 0;

		int currentLine = 0;

		int lineAnswer = 3;

		int randomOpening;

		public bool hasLeft = false;

		bool foundSecrets = false;

		bool hasAllUnlocks = false;

		int previousHint;

		//0 for map, 1 for stage, 2 for shop, 3 for save
		int screen = 0;

		public Fredbear(Player player)
		{
			this.player = player;
			fredbearDialogue = new(player);
			Random ran = new Random();
			randomOpening = ran.Next(0, 3);
			foundSecrets = checkFoundSecret();
			hasAllUnlocks = checkUnlocks();
		}

		/// <summary>
		/// Loads the content for the sprite
		/// </summary>
		/// <param name="content">The ContentManager to load with</param>
		public void LoadContent(ContentManager content)
		{
			for (int i = 4; i < 35; i++)
			{
				string add = "";
				if ((i) < 10)
				{
					add = ("000" + (i));
				}
				if ((i) >= 10)
				{
					add = ("00" + (i));
				}
				entering[i - 4] = content.Load<Texture2D>("Fredbear/Textures/coming_in/" + add);
			}
			idle = content.Load<Texture2D>("Fredbear/Textures/coming_in/0034");

			talking[0] = content.Load<Texture2D>("Fredbear/Textures/talking/0035");
			talking[1] = content.Load<Texture2D>("Fredbear/Textures/talking/0036");

			for (int i = 37; i < 47; i++)
			{
				string add = "";
				if ((i) >= 10)
				{
					add = ("00" + (i));
				}
				leaving[i - 37] = content.Load<Texture2D>("Fredbear/Textures/leaving/" + add);
			}

			talk = content.Load<SoundEffect>("Fredbear/Sounds/fredbear_talk");

			font = content.Load<SpriteFont>("Minigame_Font");

			overlay = content.Load<Texture2D>("Balloon_Barrel/Backgrounds/Overlay");
		}

		/// <summary>
		/// Updates the sprite
		/// </summary>
		/// <param name="gameTime">The GameTime object</param>
		public void Update(GameTime gameTime)
		{
			pastKeyboardState = currentKeyboardState;
			currentKeyboardState = Keyboard.GetState();

			if (aS == answerState.question && welcomeState == false)
			{
				if (screenState == MainGame_ScreenState.map)
				{
					screen = 0;
				}
				else if (screenState == MainGame_ScreenState.stage)
				{
					screen = 1;
				}
				else if (screenState == MainGame_ScreenState.shop)
				{
					screen = 2;
				}
				else 
				{
					screen = 3;
				}
				talkTimer += gameTime.ElapsedGameTime.TotalSeconds;

				if (talkTimer > .15)
				{
					firstTalkFrame = !firstTalkFrame;
					if (char.IsLetter(fredbearDialogue.welcome[randomOpening][currentLineLength]))
					{
						talk.Play();
					}
					textOnScreen += fredbearDialogue.welcome[randomOpening][currentLineLength];
					currentLineLength++;
					talkTimer -= .15;
				}

				if (currentLineLength == fredbearDialogue.welcome[randomOpening].Count())
				{
					welcomeState = true;
					currentLineLength = 0;
					aS = answerState.answer;
					state = FredbearState.idle;
				}
			}
			else if (aS == answerState.question && welcomeState == true)
			{
				talkTimer += gameTime.ElapsedGameTime.TotalSeconds;

				if (talkTimer > .15)
				{
					firstTalkFrame = !firstTalkFrame;
					if (char.IsLetter(fredbearDialogue.askAgain[currentLineLength]))
					{
						talk.Play();
					}
					textOnScreen += fredbearDialogue.askAgain[currentLineLength];
					currentLineLength++;
					talkTimer -= .15;
				}

				if (currentLineLength == fredbearDialogue.askAgain.Count())
				{
					lineAnswer = 3;
					currentLineLength = 0;
					aS = answerState.answer;
					state = FredbearState.idle;
				}
			}
			else if(aS == answerState.answer)
			{
				if (currentKeyboardState.IsKeyDown(Keys.D0) && pastKeyboardState.IsKeyUp(Keys.D0) && lineAnswer == 3)
				{
					textOnScreen = "";
					state = FredbearState.talking;
					lineAnswer = 0;
				}
				if (currentKeyboardState.IsKeyDown(Keys.D1) && pastKeyboardState.IsKeyUp(Keys.D1) && lineAnswer == 3)
				{
					textOnScreen = "";
					hint = grabHint();
					state = FredbearState.talking;
					lineAnswer = 1;
				}
				if (currentKeyboardState.IsKeyDown(Keys.D2) && pastKeyboardState.IsKeyUp(Keys.D2) && lineAnswer == 3)
				{
					textOnScreen = "";
					state = FredbearState.talking;
					lineAnswer = 2;
				}
			}

			if (lineAnswer == 0)
			{
				talkTimer += gameTime.ElapsedGameTime.TotalSeconds;

				if (talkTimer > .15)
				{
					firstTalkFrame = !firstTalkFrame;
					if (char.IsLetterOrDigit(fredbearDialogue.answers[screen][currentLineLength]))
					{
						talk.Play();
					}
					textOnScreen += fredbearDialogue.answers[screen][currentLineLength];
					currentLineLength++;
					talkTimer -= .15;
				}

				if (currentLineLength == fredbearDialogue.answers[screen].Count())
				{
					lineAnswer = 3;
					currentLineLength = 0;
					aS = answerState.question;
					state = FredbearState.talking;
					textOnScreen = "";
				}
			}
			else if (lineAnswer == 1)
			{
				talkTimer += gameTime.ElapsedGameTime.TotalSeconds;

				if (talkTimer > .15)
				{
					firstTalkFrame = !firstTalkFrame;
					if (char.IsLetterOrDigit(hint[currentLineLength]))
					{
						talk.Play();
					}
					textOnScreen += hint[currentLineLength];
					currentLineLength++;
					talkTimer -= .15;
				}

				if (currentLineLength == hint.Count())
				{
					lineAnswer = 3;
					currentLineLength = 0;
					aS = answerState.question;
					state = FredbearState.talking;
					textOnScreen = "";
				}
			}
			else if(lineAnswer == 2)
			{
				talkTimer += gameTime.ElapsedGameTime.TotalSeconds;

				if (talkTimer > .15)
				{
					if (currentLineLength != fredbearDialogue.goodBye.Count())
					{
						firstTalkFrame = !firstTalkFrame;
						if (char.IsLetter(fredbearDialogue.goodBye[currentLineLength]))
						{
							talk.Play();
						}
						textOnScreen += fredbearDialogue.goodBye[currentLineLength];
						currentLineLength++;
						talkTimer -= .15;
					}
				}

				if (currentLineLength == fredbearDialogue.goodBye.Count())
				{
					state = FredbearState.leaving;
				}
			}

		}

		private string grabHint()
		{
			Random ran = new();
			if (hasAllUnlocks)
			{
				if (foundSecrets)
				{
					return fredbearDialogue.hints[1][8];
				}
				else
				{
					List<string> list = new();
					if (player.foundSecret[0] == false) 
					{
						list.Add(fredbearDialogue.hints[1][0]);
					}
					if (player.foundSecret[1] == false)
					{
						list.Add(fredbearDialogue.hints[1][1]);
					}
					if (player.foundSecret[2] == false)
					{
						list.Add(fredbearDialogue.hints[1][2]);
					}
					if (player.foundSecret[3] == false)
					{
						list.Add(fredbearDialogue.hints[1][3]);
					}
					if (player.foundSecret[4] == false)
					{
						list.Add(fredbearDialogue.hints[1][4]);
					}
					if (player.foundSecret[5] == false)
					{
						list.Add(fredbearDialogue.hints[1][5]);
					}
					if (player.foundSecret[6] == false)
					{
						list.Add(fredbearDialogue.hints[1][6]);
					}
					if (player.foundSecret[7] == false)
					{
						list.Add(fredbearDialogue.hints[1][7]);
					}
					return list[ran.Next(0, list.Count())];
				}
			}
			else 
			{
				if (foundSecrets) 
				{
					return fredbearDialogue.hints[1][8];
				}
				return fredbearDialogue.hints[0][ran.Next(0, 8)];
			}
		}

		private bool checkUnlocks()
		{
			for (int i = 0; i < player.itemsUnlocked.Length; i++)
			{
				for (int j = 0; j < player.itemsUnlocked[i].Length; j++)
				{
					if (player.itemsUnlocked[i][j] == false)
					{
						return false;
					}
				}
			}
			return true;
		}


		private bool checkFoundSecret()
		{
			for (int i = 0; i < player.foundSecret.Length; i++)
			{
				if (player.foundSecret[i] == false)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Draws the sprite on-screen
		/// </summary>
		/// <param name="gameTime">The GameTime object</param>
		/// <param name="spriteBatch">The SpriteBatch to draw with</param>
		public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphics)
		{
			spriteBatch.Draw(overlay, Vector2.Zero, Color.White);

			if (state == FredbearState.entering)
			{
				enterAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;

				if (enterAnimationTimer > .75)
				{
					enterFrame++;
					if (enterFrame == 31)
					{
						enterFrame = 30;
						state = FredbearState.talking;
						aS = answerState.question;
					}
					enterAnimationTimer -= .75;
				}
				spriteBatch.Draw(entering[enterFrame], Vector2.Zero, Color.White);
			}
			if (state == FredbearState.talking) 
			{
				if (firstTalkFrame)
				{
					spriteBatch.Draw(talking[0], Vector2.Zero, Color.White);
				}
				else 
				{
					spriteBatch.Draw(talking[1], Vector2.Zero, Color.White);
				}
			}
			if (state == FredbearState.idle)
			{
				spriteBatch.Draw(idle, Vector2.Zero, Color.White);
				spriteBatch.DrawString(font, fredbearDialogue.questions[0], new Vector2(graphics.Viewport.Width / 2 - 750, graphics.Viewport.Height / 2 - 250), Color.White);
				spriteBatch.DrawString(font, fredbearDialogue.questions[1], new Vector2(graphics.Viewport.Width / 2 - 750, graphics.Viewport.Height / 2 - 50), Color.White);
				spriteBatch.DrawString(font, fredbearDialogue.questions[2], new Vector2(graphics.Viewport.Width / 2 - 750, graphics.Viewport.Height / 2 + 150), Color.White);
			}
			if (state == FredbearState.leaving)
			{
				leaveAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;

				if (leaveAnimationTimer > .5)
				{
					leaveFrame++;
					if (leaveFrame == 10)
					{
						leaveFrame = 9;
						hasLeft = true;
					}
					leaveAnimationTimer -= .5;
				}
				spriteBatch.Draw(leaving[leaveFrame], Vector2.Zero, Color.White);
			}
			spriteBatch.DrawString(font, textOnScreen, new Vector2(graphics.Viewport.Width / 2 - 750, 25), Color.White);
		}
	}
}
	