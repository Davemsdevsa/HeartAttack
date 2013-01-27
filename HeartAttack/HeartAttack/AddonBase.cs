using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartAttack
{
    public abstract class AddonBase
    {
        private Vector2D position;

        public Vector2D Position
        {
            get { return position; }
            set { position = value; }
        }

        private int cost;

        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }
        

        public abstract void Update();
        public abstract void Draw();
    }
}
