using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartAttack
{
    public class Vector2D
    {
        public Vector2D()
        {
        }

        public Vector2D(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2D(Vector2D vector)
        {
            this.x = vector.x;
            this.y = vector.y;
        }

        private float x;

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        private float y;

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public Vector2D Add(Vector2D vector)
        {
            return new Vector2D(x + vector.X, y + vector.Y);
        }

        public Vector2D Subtract(Vector2D vector)
        {
            return new Vector2D(vector.X - x, vector.Y - y);
        }

        public float Length()
        {
            return (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }

        public Vector2D ScalarMultiply(float multiplier)
        {
            return new Vector2D(x * multiplier, y * multiplier);
        }
        public Vector2D Unit()
        {
            float length = Length();
            return new Vector2D(x / length, y / length);
        }

        public int DistanceTo(Vector2D vector)
        {
            int retval = (int)Math.Sqrt(Math.Pow(this.X - vector.X, 2) + Math.Pow(this.Y - vector.Y, 2));
            return retval ;
        }
        
    }
}
