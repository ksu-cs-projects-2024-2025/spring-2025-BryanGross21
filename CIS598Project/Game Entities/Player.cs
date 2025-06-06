﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CIS598Project.Game_Entities
{
	public class Player
	{
		public int[] consecutivePlays = new int[8];

        public string name = "Bryan";

		public int fruityMazeWins = 0;

		public int ballpitPlays = 0;

		public int ballpitTowerLosses;

		public bool[] foundSecret = new bool[8];

        /// <summary>
        /// 0 is the normal ending, 1 is the secret ending
        /// </summary>
        public bool[] sawEnding = new bool[2];

        /// <summary>
        /// The items present in the showroom view
        /// [0][0] = stage
        /// [0][1] = sanitation station
        /// [0][2] = paper pals
        /// [0][3] = tables
        /// [0][4] = hats
        /// [0][5] = present
        /// [0][6] = children's drawings
        /// [0][7] = traffic light
        /// [0][8] = celebrate poster
        /// [0][9] = fazerblast poster
        /// [0][10] = fan
        /// [0][11] = monitor
        /// [0][12] = gumball swivelhands
        /// Upgrades
        /// [1][0] = curtains
        /// [1][1] = Confetti Floor
        /// [1][2] = Guitar
        /// [1][3] = Mic
        /// [1][4] = Cupcake
        /// Characters
        /// [2][0] = Bonnie
        /// [2][1] = Freddy
        /// [2][2] = Chica
        /// </summary>
        public bool[][] itemsUnlocked = new bool[3][];

		public int ticketAmount = 0;

		public Player() 
		{
            itemsUnlocked[0] = new bool[13];
            itemsUnlocked[1] = new bool[5];
            itemsUnlocked[2] = new bool[3];
		}
	}
}
