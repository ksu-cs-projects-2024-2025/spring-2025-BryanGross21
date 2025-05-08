using CIS598Project.Game_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CIS598Project
{
	public class GFredDialogue
    {
        string name;

        public GFredDialogue(Player player) 
        {
            name = player.name;
            text[23] = "Isn't that right, " + name + "?";
            text[24] = "Find him, " + name + ". Make him pay for\nwhat he has done.";


		}

        public string[] text = { "You found me huh, thought you were sly\ncoming here but I knew you would.",
            "You followed my trail and now you found me,\nguess you weren't just listening to the lies\n of your navigator.",
            "This company has tried to cover up everything,\nwhat was once a simple children's game\n has become my sanctuary,\nmy place to tell what he did.",
            "He made us wake up and these machines,\nthese contraptions of horror.\nDo you understand that?",
            "Good, then you understand my plight\nand what it means. I am one of the\nmissing children, I was taken by him.",
            "Who you might be asking?",
            "The Yellow Rabbit or should I say\nthe man behind the visage of him.",
            "He didn't just use that suit but the\nvisage of your navigator, the facade\nof a children's mascot used for\na vile act.",
            "It was my birthday, you know.\nMy friends had been all disappearing and \nunknown to the world that they were\ntrapped in the suits.",
            "I was naive, I thought they just didn't\nshow up, I was sad and my parents\n insisted on not going to Freddy's\n because of the disappearances.",
            "They relented when I somehow convinced\nthem my mood would be better if\nwe still went to celebrate my special day...",
            "How I should've listened.",
            "I was still a sad sap, the other kids\nwould question my lack of friends.",
            "They would bully and harrass me until\nI broke and wanted something.",
            "A balloon, I wanted a balloon\nbut none were there and...",
            "Then he should up, wearing the smiling\nfacade of the yellow rabbit beckoning\nme to follow him for there\n were my friends in the back waiting for me.",
            "I knew it was false and I ran and\nI hid but every time he found me\nand insisted I follow.",
            "\"Follow me and I'll put you back together\"\nin reference to my mood.",
            "Eventually I broke and followed\nfor him to shut up.",
            "He murdered me, that grimace on\nthe mascot's face never relenting.",
            "I would be reunited with my friends\nbut not in the way I imagined, we\nbecame trapped in these\nvessels of his design.",
            "They were all sold the same story,\nall the same tricks but under a different face.",
            "They've given up but I haven't I knew\nsomeone would find all these messages\n and that person was you.",
            "",
            "",
            "Only then can we pass on...",
            "Then why are you even here?"};
    }
}
