using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS598Project.Game_Entities
{
	public enum childShirt 
	{
		grey = 0,
		blue,
		red,
		green
	}
	public enum childBand 
	{
		blue = 0,
		green,
		red,
		yellow
	}
	public enum childEyes 
	{
		Blue = 0,
		Green,
		Hazel,
		White
	}
	public enum childBody 
	{
		white = 0,
		pale_white,
		chocolate_brown,
		brown
	}
	public class Security_Children
	{
		public childBody color;

		public childShirt shirtColor;

		public childBand bandColor;

		public childEyes eyeColor;
		public Security_Children generateChild(int round) 
		{
			Security_Children child = new();
			Random ran = new();
			int randomint = 0;
			child.shirtColor = childShirt.grey;
			child.bandColor = childBand.blue;
			child.eyeColor = childEyes.White;
			child.color = childBody.white;
			if (round >= 1) 
			{
				randomint = ran.Next(0, 4);
				if (randomint == 1)
				{
					child.bandColor = childBand.green;
				}
				else if (randomint == 2)
				{
					child.bandColor = childBand.red;
				}
				else if (randomint == 3)
				{
					child.bandColor = childBand.yellow;
				}
			}
			if (round >= 2)
			{
				randomint = ran.Next(0, 4);
				if (randomint == 1)
				{
					child.color = childBody.pale_white;
				}
				else if (randomint == 2)
				{
					child.color = childBody.chocolate_brown;
				}
				else if (randomint == 3)
				{
					child.color = childBody.brown;
				}
			}
			if (round >= 3)
			{
				randomint = ran.Next(0, 4);
				if (randomint == 1)
				{
					child.shirtColor = childShirt.blue;
				}
				else if (randomint == 2)
				{
					child.shirtColor = childShirt.red;
				}
				else if (randomint == 3)
				{
					child.shirtColor = childShirt.green;
				}
			}
			if (round >= 4) 
			{
				randomint = ran.Next(0, 4);
				if (randomint == 1)
				{
					child.eyeColor = childEyes.Blue;
				}
				else if (randomint == 2)
				{
					child.eyeColor = childEyes.Green;
				}
				else if (randomint == 3)
				{
					child.eyeColor = childEyes.Hazel;
				}
			}

			return child;
		}
	}
}
