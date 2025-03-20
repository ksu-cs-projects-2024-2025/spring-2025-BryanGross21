using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIS598Project.Game_Entities;

namespace CIS598Project
{
	public class Pity_System
	{
		Player player;
		public Pity_System(Player player) 
		{
			this.player = player;
		}

		/// <summary>
		/// Returns a random value that is dependent upon the player's current losses in a given minigame
		/// </summary>
		/// <returns>a random value between 1 and 100</returns>
		public int randomVal() 
		{
			Random ran = new();

			return ran.Next(95 + (player.losses[0] * 5), 101);
		}
	}
}
