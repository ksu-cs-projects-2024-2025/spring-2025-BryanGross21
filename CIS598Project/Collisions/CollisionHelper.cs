using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CIS598Project.Collisions
{
	public static class CollisionHelper
	{

		/// <summary>
		/// Detects collision between two bounding rectangles
		/// </summary>
		/// <param name="a">the first rectangle</param>
		/// <param name="b">the second rectangle</param>
		/// <returns>True if there is a collision, false if there isn't one</returns>
		public static bool Collides(BoundingRectangle a, BoundingRectangle b) 
		{
			return !(a.Right < b.Left || a.Left > b.Right || a.Top > b.Bottom || a.Bottom < b.Top);
		}

		/// <summary>
		/// Detects collision between two bounding rectangles
		/// </summary>
		/// <param name="a">the first rectangle</param>
		/// <param name="b">the second rectangle</param>
		/// <returns>True if there is a collision, false if there isn't one</returns>
		public static bool Collides(BoundingRectangle a, BoundingRectanglePriority b)
		{
			return !(a.Right < b.Left || a.Left > b.Right || a.Top > b.Bottom || a.Bottom < b.Top);
		}

        public static bool Collides(BoundingCircle circle, BoundingRectangle rectangle)
        {
            float nearestX = MathHelper.Clamp(circle.Center.X, rectangle.Left, rectangle.Right);
            float nearestY = MathHelper.Clamp(circle.Center.Y, rectangle.Top, rectangle.Bottom);
            return Math.Pow(circle.Radius, 2) >=
            Math.Pow(circle.Center.X - nearestX, 2) +
            Math.Pow(circle.Center.Y - nearestY, 2);
        }

        public static bool Collides(BoundingRectangle r, BoundingCircle c) => Collides(c, r);
    }
}
