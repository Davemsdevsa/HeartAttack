using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartAttack
{
    public class AddonContainer
    {
        private Vector2D position;

        public Vector2D Position
        {
            get { return position; }
            set { position = value; }
        }

        private bool isUsed;

        public bool IsUsed
        {
            get { return isUsed; }
            set { isUsed = value; }
        }



        
    }
}
