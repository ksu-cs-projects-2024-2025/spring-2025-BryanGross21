using CIS598Project.Game_Entities;
using CIS598Project.Rooms;
using CIS598Project.StateManagement;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace CIS598Project
{
    /// <summary>
    /// Gets the name, description, and controls for each 
    /// </summary>
    public class MinigameDictionary
    {
        string[] names = { "Duck Pond", "Discount Ballpit", "Balloon Barrel", "Ballpit Tower", "Fruity Maze", "Memory Match", "Security", "Freddy's Fishing Hole" };

		string[] description = { "Congratulations, as the birthday child you get to partake\nin a Freddy's traditon, pulling ducks from the pond! Take\n3 ducks from the pond and whatever you score is the am-\nount of tickets you get!!! \n(Note this game has gotten the experimental 3D treatment)", "Take a dive into our classic (discount) Fazbear ballpit\nwhich was present at our New Harmony, Utah location!\nExperience the fun of a tried and true Fazbear tradition.\n(Note this game has gotten the experimental 3D treatment)", "Everyone is a winner at our balloon barrel, remember\nto take a single balloon and claim it in for 500 points!\nFun fact: Did you know that at Fredbear's Family Diner the balloon \nbarrel was one of the original attractions! \n(Note this game has gotten the experimental 3D treatment)",
			"Fish out a prize at our Ballpit Tower, for some\nreason prize balls love this tower! Not every time you play you can win\nbut the more you play the better odds you'll get!\n(Note this game has gotten the experimental 3D treatment)",
			"The classic Fazbear era arcade game that was originally ported to home PCs in \n1985. Help the girl reach the end of the maze, if you need more help there\nwill be instructions in the game after some time that will tell you more!\n(Note this game is still using the original pixel style PLEASE UPDATE)",
			"The classic Fazbear era arcade game based on Simon\noriginally developed 1979 for Fredbear's Family Diner.\nListen to the robotic voice to know the pattern to match,\n there are 5 rounds the more rounds you beat the more points you get!\n(Note this game is still using the original pixel style PLEASE UPDATE)",
			"This classic Fazbear arcade game developed in 1984 to help promote the\nsafety and awareness of guests at Freddy's. Each round you will be presented \na child and a sequence of children will run across the screen.\nWhen the children run across the screen you will look for the child presented\nand then at the end of the sequence guess the position of \nthat child in the sequence. This game runs for 4 rounds, so best of luck!\n(Note this game is still using the original pixel style PLEASE UPDATE)",
			"In this classic Fazbear game you play as Freddy Fazbear\nas he fishes for a pearl at the bottom of the ocean.\nDrop your plunger and try to fish up the pearl,\navoid any fish on the way or you will fish them up instead.\nYou will go on for 3 rounds and you get as many tickets\nas your score so best of luck!\n(Note this game is still using the original pixel style PLEASE UPDATE)" };

		string[] controls = { "Use the mouse to point and click on objects", "Use the Spacebar", "Use the Spacebar", "Use the Spacebar", "Use the mouse to trace the maze", "Use the mouse to point and click on objects", "Use the mouse to point and click on objects", "A and D to move left and right, Spacebar to release the plunger" };


		SpriteFont font;



		public void LoadContent(ContentManager content)
		{
			font = content.Load<SpriteFont>("MiniGame_Font");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="spriteBatch">the spritebatch to draw the textures</param>
		/// <param name="node">The current node the player is on used to grab the text</param>
		/// <param name="width">The width of the screen</param>
		/// <param name="height">The height of the screen</param>
		public void Draw(SpriteBatch spriteBatch, int node, int width, int height)
		{
			spriteBatch.DrawString(font, "Escape to go back <---", Vector2.Zero, Color.White, 0, Vector2.Zero, .5f, SpriteEffects.None, 0);
			spriteBatch.DrawString(font, names[node], new Vector2(width / 2 - 250, 0), Color.White);

			spriteBatch.DrawString(font, description[node], new Vector2(width / 4, height / 4), Color.White,  0, Vector2.Zero, .5f, SpriteEffects.None, 0);

			spriteBatch.DrawString(font, "Controls:", new Vector2(width / 2 - 250, height / 4 + 350), Color.White);


			spriteBatch.DrawString(font, controls[node], new Vector2(width / 2 - 250, height / 4 + 450), Color.White, 0, Vector2.Zero, .5f, SpriteEffects.None, 0);


			spriteBatch.DrawString(font, "Space to continue --->", new Vector2(width - 425, height - 50), Color.White, 0, Vector2.Zero, .5f, SpriteEffects.None, 0);
		}
	}
}
