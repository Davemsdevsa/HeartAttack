using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartAttack
{
    public class Enemy : Drawable  
    {
        private bool _positionChanged = true;
        private Vector2D _lastDrawPos = new Vector2D();
        private Vector2D position = new Vector2D();

        public Vector2D Position
        {
            get { return position; }
            set { position = value; _positionChanged = true; }
        }
             

        private Vector2D velocity;

        public Vector2D Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }        

        public override void Draw()
        {
            try
            {
                if (_positionChanged)
                {
                    Console.CursorLeft = (int)_lastDrawPos.X;
                    Console.CursorTop = (int)_lastDrawPos.Y;
                    Console.Write(" ");

                    Console.CursorLeft = (int)position.X;
                    Console.CursorTop = (int)position.Y;
                    Console.Write("☺");
                    _positionChanged = false;
                    _lastDrawPos = position;
                }
            }catch
            {
                
            }
        }
    }
}
