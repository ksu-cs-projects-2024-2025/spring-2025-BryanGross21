using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

//Reuses the code from the collision exercise from CIS 580, just worked for my purposes
namespace CIS598Project.Collisions
{
	/// <summary>
	/// A bounding rectangle for collision used for FruityMaze
	/// </summary>
	public struct BoundingRectanglePriority
	{
		public float X;

		public float Y;

		public float Width;

		public float Height;

		public float Left => X;

		public float Right => X + Width;

		public float Top => Y;

		public float Bottom => Y + Height;

		/// <summary>
		/// Ensures that the player can only go to and from check-points that are either 1 behind or 1 ahead
		/// </summary>
		public int checkpoint = 0;

		/// <summary>
		/// Checks to see if a flash event should happen
		/// </summary>
		public bool chicaFlash = false;

		public BoundingRectanglePriority(float x, float y, float width, float height, int checkpoint, bool chicaFlash) 
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
			this.checkpoint = checkpoint;
			this.chicaFlash = chicaFlash;
		}


	}
}
