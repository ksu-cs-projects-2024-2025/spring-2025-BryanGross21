using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS598Project.Game_Entities;
using CIS598Project.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CIS598Project.Rooms
{
    public class GameSelection : GameScreen
    {
        string fileName = Environment.CurrentDirectory + "\\savedata.txt";

        Game game;

        Player player;

        Transition transition;

        Texture2D selection;

        Texture2D frame;

        SpriteFont font;

        ContentManager _content;

        KeyboardState pastKeyboardState;

        KeyboardState currentKeyboardState;

        Song backgroundMusic;

        SoundEffect option;

        SoundEffect gameSelected;

        bool topSelected = true;

        int readLine = 0;

        int foundSecret = 0;

        int consecutivePlays = 0;

        int sawEndings = 0;

        int itemsUnlockedRow = 0;

        int itemsUnlockedColumn = 0;

        public GameSelection(Game game) 
        {
            this.game = game;
        }


        public override void Activate()
        {
            base.Activate();

            if (_content == null) _content = new ContentManager(ScreenManager.Game.Services, "Content");

            transition = new Transition();

            transition.LoadContent(_content);

            font = _content.Load<SpriteFont>("Minigame_Font");

            selection = _content.Load<Texture2D>("Game_Selection/Textures/Fredbear_Selection");

            frame = _content.Load<Texture2D>("Game_Selection/Textures/load_border");

            backgroundMusic = _content.Load<Song>("Game_Selection/Sounds/Song/Creme_De_La_Creme");

            option = _content.Load<SoundEffect>("Game_Selection/Sounds/Soundeffects/optionSelect");

            gameSelected = _content.Load<SoundEffect>("Game_Selection/Sounds/Soundeffects/gameSelect");

            player = new Player();

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

            if (File.Exists(fileName))
            {
                if (currentKeyboardState.IsKeyDown(Keys.W) && pastKeyboardState.IsKeyUp(Keys.W) || currentKeyboardState.IsKeyDown(Keys.S) && pastKeyboardState.IsKeyUp(Keys.S))
                {
                    option.Play();
                    topSelected = !topSelected;
                }
            }

            if (currentKeyboardState.IsKeyDown(Keys.E) && pastKeyboardState.IsKeyUp(Keys.E))
            {
                transition.shouldTransition = true;
                gameSelected.Play();
                MediaPlayer.Stop();
            }

            if (transition.transitionToNextScreen)
            {
                if (topSelected)
                {
                    foreach (var screen in ScreenManager.GetScreens())
                        screen.ExitScreen();

                    ScreenManager.AddScreen(new MainGame_Screen(new Player(), game), PlayerIndex.One);
                }
                else
                {

                    StreamReader reader = new(fileName);
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (readLine < 8)
                        {
                            player.consecutivePlays[readLine] = Convert.ToInt32(line);
                        }
                        else if (readLine == 8)
                        {
                            player.name = line;
                        }
                        else if (readLine == 9)
                        {
                            player.fruityMazeWins = Convert.ToInt32(line);
                        }
                        else if (readLine == 10)
                        {
                            player.ballpitPlays = Convert.ToInt32(line);
                        }
                        else if (readLine == 11)
                        {
                            player.ballpitTowerLosses = Convert.ToInt32(line);
                        }
                        else if (readLine >= 12 && readLine < 20)
                        {
                            if (String.Equals("True", line))
                            {
                                player.foundSecret[foundSecret] = true;
                            }
                            else
                            {
                                player.foundSecret[foundSecret] = false;
                            }
                            foundSecret++;
                        }
                        else if (readLine >= 20 && readLine < 22)
                        {
                            if (String.Equals("True", line))
                            {
                                player.sawEnding[sawEndings] = true;
                            }
                            else
                            {
                                player.sawEnding[sawEndings] = false;
                            }
                            sawEndings++;
                        }
                        else if (readLine >= 22 && readLine < 43)
                        {
                            if (itemsUnlockedColumn == 13 && itemsUnlockedRow == 0)
                            {
                                itemsUnlockedColumn = 0;
                                itemsUnlockedRow++;
                            }
                            else if (itemsUnlockedColumn == 5 && itemsUnlockedRow == 1)
                            {
                                itemsUnlockedColumn = 0;
                                itemsUnlockedRow++;
                            }
                            if (String.Equals("True", line))
                            {
                                player.itemsUnlocked[itemsUnlockedRow][itemsUnlockedColumn] = true;
                            }
                            else
                            {
                                player.itemsUnlocked[itemsUnlockedRow][itemsUnlockedColumn] = false;
                            }
                            itemsUnlockedColumn++;
                        }
                        else if (readLine == 43)
                        {
                            player.ticketAmount = Convert.ToInt32(line);
                        }
                        readLine++;
                    }
                }
                foreach (var screen in ScreenManager.GetScreens())
                    screen.ExitScreen();

                ScreenManager.AddScreen(new MainGame_Screen(player, game), PlayerIndex.One);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var graphics = ScreenManager.GraphicsDevice;
            var spriteBatch = ScreenManager.SpriteBatch;

            graphics.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.Draw(frame, new Vector2(graphics.Viewport.Width / 2 - 500, graphics.Viewport.Height / 2 - 250), Color.White);

            spriteBatch.DrawString(font, "New Game", new Vector2(graphics.Viewport.Width / 2 - 300, graphics.Viewport.Height / 2 - 200), Color.White, 0f, Vector2.Zero, 1.25f, SpriteEffects.None, 0);

            if (File.Exists(fileName))
            {
                spriteBatch.DrawString(font, "Continue", new Vector2(graphics.Viewport.Width / 2 - 300, graphics.Viewport.Height / 2 - 50), Color.White, 0f, Vector2.Zero, 1.25f, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.DrawString(font, "Continu", new Vector2(graphics.Viewport.Width / 2 - 300, graphics.Viewport.Height / 2 - 50), Color.Gray, 0f, Vector2.Zero, 1.25f, SpriteEffects.None, 0);
            }

            if (topSelected)
            {
                spriteBatch.Draw(selection, new Vector2(graphics.Viewport.Width / 2 - 525, graphics.Viewport.Height / 2 - 200), Color.White);
            }
            else 
            {
                spriteBatch.Draw(selection, new Vector2(graphics.Viewport.Width / 2 - 525, graphics.Viewport.Height / 2 - 50), Color.White);
            }

            transition.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
    }
}
