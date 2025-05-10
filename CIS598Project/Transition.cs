using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CIS598Project
{
    public class Transition
    {

        Texture2D[] transition = new Texture2D[5];

        double transitionTime = 0;

        int transitionFrame = 0;

        public bool transitionToNextScreen = false;

        public bool shouldTransition = false;

        /// <summary>
        /// Loads the content for the sprite
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            transition[0] = content.Load<Texture2D>("Transitions/transition_1");
            transition[1] = content.Load<Texture2D>("Transitions/transition_2");
            transition[2] = content.Load<Texture2D>("Transitions/transition_3");
            transition[3] = content.Load<Texture2D>("Transitions/transition_4");
            transition[4] = content.Load<Texture2D>("Transitions/transition_5");
        }

        /// <summary>
        /// Draws the sprite on-screen
        /// </summary>
        /// <param name="gameTime">The GameTime object</param>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (shouldTransition)
            {
                transitionTime += gameTime.ElapsedGameTime.TotalSeconds;

                if (transitionTime > .25 && transitionFrame != 5)
                {
                    transitionFrame++;
                    transitionTime -= .25;
                }

                if (transitionFrame == 5)
                {
                    transitionFrame--;
                    transitionToNextScreen = true;
                }

                spriteBatch.Draw(transition[transitionFrame], Vector2.Zero, Color.White);
            }
        }
    }
}

