using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS598Project.Game_Entities;
using CIS598Project.StateManagement;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using SharpDX.Direct3D9;
using Microsoft.Xna.Framework.Input;

namespace CIS598Project.Rooms
{
    public class MainMenu : GameScreen
    {

		string fileName = Environment.CurrentDirectory + "\\savedata.txt";

		Game game;

        Texture2D background;

        Texture2D logo;

        Song backgroundMusic;

        SoundEffect continued;

        SpriteFont font;

        ContentManager _content;

        Transition transition;

        double colorChange = 0;

        int colorOption = 0;

        Color colorForInstruction = Color.White;

        bool isLogoTransitioning = true;

        Vector2 logoPosition;

        KeyboardState pastKeyboardState;

        KeyboardState currentKeyboardState;

        bool[] endings = { false, false };

        Texture2D[] endingIcons = new Texture2D[2];



        public MainMenu(Game game)
        {
            this.game = game;
            int readLine = 0;
            int sawEndings = 0;
            StreamReader reader = new(fileName);
			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine();

				if (readLine >= 20 && readLine < 22)
				{
					if (String.Equals("True", line))
					{
                        endings[sawEndings] = true;
					}
					else
					{
						endings[sawEndings] = false;
					}
					sawEndings++;
				}
				
				readLine++;
			}
		}

        public override void Activate()
        {
            base.Activate();

            if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");
            transition = new Transition();

            transition.LoadContent(_content);

            logoPosition = new Vector2(0, game.GraphicsDevice.Viewport.Height + 200);

            font = _content.Load<SpriteFont>("Minigame_Font");
            background = _content.Load<Texture2D>("Title_Screen/Textures/Menu_background");
            logo = _content.Load<Texture2D>("Title_Screen/Textures/Title_Screen");
            backgroundMusic = _content.Load<Song>("Title_Screen/Sounds/Song/TitleScreen-FFPS");
            continued = _content.Load<SoundEffect>("Game_Selection/Sounds/Soundeffects/gameSelect");

            endingIcons[0] = _content.Load<Texture2D>("Title_Screen/Textures/Normal_Ending");
            endingIcons[1] = _content.Load<Texture2D>("Title_Screen/Textures/secret_Ending");


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

            if (isLogoTransitioning == false)
            {
                if (currentKeyboardState.IsKeyDown(Keys.Enter) && pastKeyboardState.IsKeyUp(Keys.Enter) && transition.shouldTransition == false)
                {
                    continued.Play();
                    MediaPlayer.Stop();
                    transition.shouldTransition = true;
                }

                if (transition.transitionToNextScreen)
                {
                    foreach (var screen in ScreenManager.GetScreens())
                        screen.ExitScreen();

                    ScreenManager.AddScreen(new GameSelection(game), PlayerIndex.One);
                }
            }
            else 
            {
                if (logoPosition == Vector2.Zero)
                {
                    isLogoTransitioning = false;
                }
                else 
                {
                    logoPosition.Y -= 5;
                }
            }

        }

        public override void Draw(GameTime gameTime)
        {
            var graphics = ScreenManager.GraphicsDevice;
            var spriteBatch = ScreenManager.SpriteBatch;

            graphics.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.Draw(background, Vector2.Zero, Color.White);

            spriteBatch.Draw(logo, logoPosition, Color.White);

            if (isLogoTransitioning == false) 
            {
                colorChange += gameTime.ElapsedGameTime.TotalSeconds;

                if (colorChange > .75)
                {
                    colorOption++;
                    if (colorOption == 4)
                    {
                        colorOption = -1;
                    }
                    else if (colorOption == 3)
                    {
                        colorForInstruction = Color.Yellow;
                    }
                    else if (colorOption == 2)
                    {
                        colorForInstruction = Color.Red;
                    }
                    else if (colorOption == 1)
                    {
                        colorForInstruction = Color.Blue;
                    }
                    else
                    {
                        colorForInstruction = Color.White;
                    }
                    colorChange -= .75;
                }

                spriteBatch.DrawString(font, "Press Enter", new Vector2(graphics.Viewport.Width / 2 - 125, graphics.Viewport.Height / 2 + 450), colorForInstruction, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }

            if (endings[0]) 
            {
                spriteBatch.Draw(endingIcons[0], new Vector2(45, 318), Color.White);
            }
            if (endings[1]) 
            {
				spriteBatch.Draw(endingIcons[1], new Vector2(1620, 318), Color.White);
			}

            transition.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
    }
}
