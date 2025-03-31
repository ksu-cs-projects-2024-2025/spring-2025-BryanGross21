using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CIS598Project.Game_Entities
{
	public class Player
	{
		public int[] consecutivePlays = new int[16];

		public int fruityMazeWins = 2;

		/// <summary>
		/// 0 represents the ballpit tower, 1 represents prize king
		/// </summary>
		public int[] losses = new int[2];

		public bool[] foundSecret = new bool[16];

		public int ticketAmount;

		public Player() 
		{
			for (int i = 0; i < consecutivePlays.Length; i++) 
			{
				consecutivePlays[i] = 0;
				foundSecret[i] = false;
			}
			losses[0] = 0;
		}
	}
}
