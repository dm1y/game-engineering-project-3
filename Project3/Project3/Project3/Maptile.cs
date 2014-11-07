using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project3
{
    class Maptile
    {
        //Maptiles can tell us if a block can be walked on, 
        //as well as if they're transition/AI/building/etc. 
        public Boolean isCollidable;
        public int transitionTo;

        public Maptile()
        {
                
        }
    }
}
