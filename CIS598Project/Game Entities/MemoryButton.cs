using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using SharpDX.Direct2D1;


namespace CIS598Project.Game_Entities
{
    public enum buttonColors 
    {
        blue,
        green,
        red,
        yellow,
        orange,
        white,
        black,
        purple
    }
    public class MemoryButton
    {
        public SoundEffect sound;

        public buttonColors color;

        public MemoryButton(int color) 
        {
            this.color = (buttonColors)color;
        }

        /// <summary>
        /// Loads the content for the sprite
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            if (color == buttonColors.blue)
            {
                sound = content.Load<SoundEffect>("Memory/Sounds/Soundeffects/Colors/blue");
            }
            else if (color == buttonColors.green)
            {
                sound = content.Load<SoundEffect>("Memory/Sounds/Soundeffects/Colors/green");
            }
            else if (color == buttonColors.red)
            {
                sound = content.Load<SoundEffect>("Memory/Sounds/Soundeffects/Colors/red");
            }
            else if (color == buttonColors.yellow)
            {
                sound = content.Load<SoundEffect>("Memory/Sounds/Soundeffects/Colors/yellow");
            }
            else if (color == buttonColors.orange)
            {
                sound = content.Load<SoundEffect>("Memory/Sounds/Soundeffects/Colors/orange");
            }
            else if (color == buttonColors.white)
            {
                sound = content.Load<SoundEffect>("Memory/Sounds/Soundeffects/Colors/white");
            }
            else if (color == buttonColors.black)
            {
                sound = content.Load<SoundEffect>("Memory/Sounds/Soundeffects/Colors/black");
            }
            else 
            {
                sound = content.Load<SoundEffect>("Memory/Sounds/Soundeffects/Colors/purple");
            }
        }
    }
}
