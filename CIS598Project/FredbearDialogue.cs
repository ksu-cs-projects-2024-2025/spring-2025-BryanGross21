using CIS598Project.Game_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CIS598Project
{
    public class FredbearDialogue
    {
        Player player;

        public string[] welcome = { "Hi there, you called?\nWhat can I help you with?", "Hi pal, Fredbear here.\nWhat can I do to help?", "I'm Fredbear and I'm here to help." };

        public string askAgain = "Anything else I can help with?";

        public string goodBye = "Until next time, friend.";

        public string[] questions = { "0. What does this screen do?", "1. Can you provide me a hint?", "2. Bye bye" };

        public string[] answers = { "This is the map screen, you move\nmy nephew, Freddy, around to different\nnodes. These nodes represent different minigames,\nwhen you want to play one\nhit 'e' on your keyboard and\na new screen will tell you\neverything you need to know.",
        "This is the stage screen, this is\nwhere all your purchases from the\nshop will show up. Keep in mind\nsome items only appear after you\nhave bought other items.",
        "This is the shop screen, this is\nwhere my buddy, Springbonnie, sells you items\nfor the stage show. Hmm...       \n\n\nGuess, he has stage fright right now.",
        "This is the save screen, it does\nas you probably think it does it saves the game." };

        public string[][] hints = new string[2][];


        public FredbearDialogue(Player player)
        {
            this.player = player;


            hints[0] = new string[8];
            hints[1] = new string[9];

            hints[0][0] = "They say that the Ballpit Tower\nprovides the most tickets but\nyou have to get lucky.";
			hints[0][1] = "The Balloon Barrel gives you the\nmost consistent tickets but it's the\nlowest amount.";
			hints[0][2] = "Each Duck Pond playthrough will give\nyou at least a single 900\npoint duck.";
			hints[0][3] = "Security and Memory Match give consistent\nticket outputs if you're good enough.";
			hints[0][4] = "They say the pearl at the bottom\nof Freddy's Fishing Hole gives a \ngood jackpot.";
			hints[0][5] = "In the Discount Ballpit you don't\nhave to 100% line-up your jump.";
			hints[0][6] = "Save whenever you get the chance.";
			hints[0][7] = "There are secrets hidden in the\ngames, I might tell you about\nthem later.";

			hints[1][0] = "Keep picking ducks, don't play any\nother games.";
			hints[1][1] = "Hey, I would really hate if\nyou kept missing the jump.";
			hints[1][2] = "Do you like balloons? I do,\n so just keep grabbing them.";
			hints[1][3] = "Keep fishing up prize balls, eventually\n something else will happen.";
			hints[1][4] = "If you're good at the maze,\nbeat it more.";
			hints[1][5] = "Match all the sequences.";
			hints[1][6] = "You haven't fully kept an eye\n on all the children, do that.";
			hints[1][7] = "Fish up the highest score you can.";
            hints[1][8] = "Go see them....";
		}

    }
}
