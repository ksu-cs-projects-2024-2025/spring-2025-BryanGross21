using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using CIS598Project.Collisions;

namespace CIS598Project.Game_Entities
{
    public class Fish
    {

        Texture2D[] fish = new Texture2D[3];

        Vector2 position;

        public BoundingRectangle bounds = new(0,0, 128, 128);

        private int sideOfScreen;

        private int fishAnimationFrame = 0;

        double fishAnimationTimer = 0;

        public int score = 100;

        double speedMultiplier = 1;

        /// <summary>
        /// Moves the fish to the left of the screen
        /// </summary>
        bool movingLeft = true;

        public Fish(Vector2 position, int score, int sideOfScreen) 
        {
            this.position = position;
            this.score = score;
            this.sideOfScreen = sideOfScreen;
        }

        /// <summary>
		/// Loads the content for the sprite
		/// </summary>
		/// <param name="content">The ContentManager to load with</param>
		public void LoadContent(ContentManager content)
        {
            if (score == 100)
            {
                fish[0] = content.Load<Texture2D>("Fishing/Textures/Fish/Orange_2");
                fish[1] = content.Load<Texture2D>("Fishing/Textures/Fish/Orange_1");
                fish[2] = content.Load<Texture2D>("Fishing/Textures/Fish/Orange_3");
            }
            else if (score == 200) 
            {
                fish[0] = content.Load<Texture2D>("Fishing/Textures/Fish/Green_2");
                fish[1] = content.Load<Texture2D>("Fishing/Textures/Fish/Green_1");
                fish[2] = content.Load<Texture2D>("Fishing/Textures/Fish/Green_3");
                speedMultiplier = 1.1;
            }
            else if (score == 300)
            {
                fish[0] = content.Load<Texture2D>("Fishing/Textures/Fish/Purple_2");
                fish[1] = content.Load<Texture2D>("Fishing/Textures/Fish/Purple_1");
                fish[2] = content.Load<Texture2D>("Fishing/Textures/Fish/Purple_3");
                speedMultiplier = 1.2;
            }
            else if (score == 400)
            {
                fish[0] = content.Load<Texture2D>("Fishing/Textures/Fish/Blue_2");
                fish[1] = content.Load<Texture2D>("Fishing/Textures/Fish/Blue_1");
                fish[2] = content.Load<Texture2D>("Fishing/Textures/Fish/Blue_3");
                speedMultiplier = 1.3;
            }
            else if (score == 500)
            {
                fish[0] = content.Load<Texture2D>("Fishing/Textures/Fish/Pink_2");
                fish[1] = content.Load<Texture2D>("Fishing/Textures/Fish/Pink_1");
                fish[2] = content.Load<Texture2D>("Fishing/Textures/Fish/Pink_3");
                speedMultiplier = 1.4;
            }
            else
            {
                fish[0] = content.Load<Texture2D>("Fishing/Textures/Fish/Gold_2");
                fish[1] = content.Load<Texture2D>("Fishing/Textures/Fish/Gold_1");
                fish[2] = content.Load<Texture2D>("Fishing/Textures/Fish/Gold_3");
                speedMultiplier = 1.5;
            }
        }

        /// <summary>
        /// Updates the sprite
        /// </summary>
        /// <param name="gameTime">The GameTime object</param>
        public void Update(GameTime gameTime)
        {

            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (position.X >= sideOfScreen)
            {
                movingLeft = true;
            }
            else if (position.X <= 0) 
            {
                movingLeft = false;
            }

            if (movingLeft)
            {
                position.X -= ((float)speedMultiplier * 4);
            }
            else 
            {
                position.X += ((float)speedMultiplier * 4);
            }
           
            bounds.X = position.X;  // Update bounds position X
            bounds.Y = position.Y;  // Update bounds position Y

        }

        /// <summary>
        /// Draws the sprite on-screen
        /// </summary>
        /// <param name="gameTime">The GameTime object</param>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            fishAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (fishAnimationTimer > .5) 
            {
                fishAnimationFrame++;
                if (fishAnimationFrame == 2) 
                {
                    fishAnimationFrame = 0;
                }
            }
            if (movingLeft)
            {
                spriteBatch.Draw(fish[fishAnimationFrame], position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            else {
				spriteBatch.Draw(fish[fishAnimationFrame], position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0);
			}
        }
    }
}
