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
        public string[] welcome = { "Hi there, you called?\nWhat can I help you with?", "Hi pal, Fredbear here.\nWhat can I do to help?", "I'm Fredbear and I'm here to help." };

        public string askAgain = "Anything else I can help with?";

        public string goodBye = "Until next time, friend.\nRemember if you see something\nthat you shouldn't see\nexit the game.";

        public string[][] questions = new string[4][];

        public string[][] answers = new string[4][];

        public string[][] hints = new string[2][];


    }
}
