using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS598Project.Rooms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CIS598Project.Game_Entities
{
    public class MapNode
    {

        Player player;

        /// <summary>
        /// The position located on the map screen
        /// </summary>
        public Vector2 position;

        /// <summary>
        /// The image to represent the minigame
        /// </summary>
        Texture2D thumbnail;

        /// <summary>
        /// Used to reference which minigame this node will be associated with
        /// </summary>
        MinigameRef referenceType;

        public MapNode(int reference, Vector2 position, Game game, Player player) 
        {
            referenceType = (MinigameRef)reference;
            this.position = position;
            this.player = player;
        }

        /// <summary>
		/// Loads the content for the sprite
		/// </summary>
		/// <param name="content">The ContentManager to load with</param>
		public void LoadContent(ContentManager content)
        {
            if (referenceType == MinigameRef.duck_Pond) 
            {
                thumbnail = content.Load<Texture2D>("Desktop_Selection/Textures/map/path/duckpond_picture");
            }
            if (referenceType == MinigameRef.discount_Ballpit)
            {
                thumbnail = content.Load<Texture2D>("Desktop_Selection/Textures/map/path/ballpit_Picture");
            }
            if (referenceType == MinigameRef.balloon_Barrel)
            {
                thumbnail = content.Load<Texture2D>("Desktop_Selection/Textures/map/path/balloonBarrel_picture");
            }
            if (referenceType == MinigameRef.ballpit_Tower)
            {
                thumbnail = content.Load<Texture2D>("Desktop_Selection/Textures/map/path/ballpitTower_picture");
            }
            if (referenceType == MinigameRef.fruity_Maze)
            {
                thumbnail = content.Load<Texture2D>("Desktop_Selection/Textures/map/path/fruity_picture");
            }
            if (referenceType == MinigameRef.memory_Match)
            {
                thumbnail = content.Load<Texture2D>("Desktop_Selection/Textures/map/path/memory_picture");
            }
            if (referenceType == MinigameRef.security)
            {
                thumbnail = content.Load<Texture2D>("Desktop_Selection/Textures/map/path/security_picture");
            }
            if (referenceType == MinigameRef.fishing) 
            {
                thumbnail = content.Load<Texture2D>("Desktop_Selection/Textures/map/path/fish_picture");
            }
        }


        /// <summary>
        /// Draws the sprite on-screen
        /// </summary>
        /// <param name="gameTime">The GameTime object</param>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (player.foundSecret[(int)referenceType])
            {
                spriteBatch.Draw(thumbnail, position, Color.Gold);
            }
            else 
            {
                spriteBatch.Draw(thumbnail, position, Color.White);
            }
        }
    }
}

