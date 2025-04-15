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
	/// A bounding rectangle for collision
	/// </summary>
	public struct BoundingCircle
	{
        /// <summary>
        /// The center of the BoundingCircle
        /// </summary>
        public Microsoft.Xna.Framework.Vector2 Center;
        /// <summary>
        /// Radius of the BoundingCircle
        /// </summary>
        public float Radius;

        /// <summary>
        /// Constructs a new BoundingCircle
        /// </summary>
        /// <param name="center">The center of the circle</param>
        /// <param name="radius">The radius of the circle</param>
        public BoundingCircle(Microsoft.Xna.Framework.Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public bool collidesWith(BoundingRectangle rectangle)
        {
            return CollisionHelper.Collides(this, rectangle);
        }


    }
}
