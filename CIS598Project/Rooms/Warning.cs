using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS598Project.Game_Entities;
using CIS598Project.StateManagement;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SharpDX.Direct3D9;

namespace CIS598Project.Rooms
{
    public class Warning : GameScreen
    {
        Game game;

        Transition transition;

        SpriteFont font;

        ContentManager _content;

        double holdTimer = 0;
        public Warning(Game game) 
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
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            holdTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (holdTimer > 2.5) 
            {
                transition.shouldTransition = true;
            }

            if (transition.transitionToNextScreen)
            {
                foreach (var screen in ScreenManager.GetScreens())
                    screen.ExitScreen();

                ScreenManager.AddScreen(new MainMenu(game), PlayerIndex.One);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var graphics = ScreenManager.GraphicsDevice;
            var spriteBatch = ScreenManager.SpriteBatch;

            graphics.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.DrawString(font, "Warning", new Vector2(graphics.Viewport.Width / 2 - 125, graphics.Viewport.Height / 2 - 250), Color.White);

            spriteBatch.DrawString(font, "This game contains loud noises,\nsome disturbing themes,\nand hopefully lots of lots of fun.", new Vector2(graphics.Viewport.Width / 2 - 500, graphics.Viewport.Height / 2 - 100), Color.White);

            transition.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
    }
}
