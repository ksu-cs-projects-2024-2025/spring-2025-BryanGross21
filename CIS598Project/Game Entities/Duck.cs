using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIS598Project.Game_Entities
{
	public class Duck
	{
		public int points;

		public bool selected;

		public Duck(int duckNum, bool Point900Duck)
		{
			Random ran = new Random();

			//If at least 1 duck is worth 900 points
			if (Point900Duck)
			{
				int num = ran.Next(0, 101);
				if (num < 75)
				{
					points = 250;
				}
				else if (num >= 75 && num < 90)
				{
					points = 450;
				}
				else 
				{
					points = 900;
				}
			}
			else 
			{
				int num = ran.Next(duckNum * 7, 101);
				if (num < 75)
				{
					points = 250;
				}
				else if (num >= 75 && num < 90)
				{
					points = 450;
				}
				else
				{
					points = 900;
				}
			}
		}
	}
}
