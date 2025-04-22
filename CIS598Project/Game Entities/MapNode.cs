using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS598Project.Rooms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CIS598Project.Game_Entities
{
    public class MapNode
    {

        /// <summary>
        /// This and player are used simply for passing the game and player entities into the minigames
        /// </summary>
        Game game;

        Player player;

        /// <summary>
        /// The position located on the map screen
        /// </summary>
        Vector2 position;

        /// <summary>
        /// The image to represent the minigame
        /// </summary>
        Texture2D thumbnail;

        /// <summary>
        /// Used to reference which minigame this node will be associated with
        /// </summary>
        MinigameRef referenceType;
    }
}
