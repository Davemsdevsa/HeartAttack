using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartAttack
{
    public class Ship : Drawable
    {
        private List<AddonBase> addons = new List<AddonBase>();

        public List<AddonBase> Addons
        {
            get { return addons; }
            set { addons = value; }
        }

        private Vector2D position;

        public Vector2D Position
        {
            get { return position; }
            set { position = value; }
        }
        
        private int health;

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        private int sheilds;

        public int Sheilds
        {
            get { return sheilds; }
            set { sheilds = value; }
        }
        public AddonBase GetAddonForPosition(Vector2D position)
        {
            AddonBase retval = null;
            foreach(AddonBase a in addons){
                if(a.Position.X == position.X && a.Position.Y == position.Y){
                    retval = a;
                    break;
                }
            }
            return retval;
        }

        
        public override void Draw()
        {
            //Draw Ship
            Console.ForegroundColor = ConsoleColor.Red;
            Console.CursorTop = (int)Position.Y;
            Console.CursorLeft = (int)Position.X;
            Console.Write("♥");
            Console.ForegroundColor = ConsoleColor.White;

            foreach (AddonBase addon in addons)
            {
                addon.Draw();
            }
        }
        
    }
}
