using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CIS598Project.Game_Entities
{
	public class Player
	{
		public int[] consecutivePlays = new int[8];

		public int fruityMazeWins = 0;

		public int ballpitPlays = 0;

		public int ballpitTowerLosses;

		public bool[] foundSecret = new bool[8];

		public int ticketAmount;

		public Player() 
		{
			for (int i = 0; i < consecutivePlays.Length; i++) 
			{
				consecutivePlays[i] = 0;
				foundSecret[i] = false;
			}
			ballpitTowerLosses = 0;
		}
	}
}
