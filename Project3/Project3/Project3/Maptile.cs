using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        /* - Positions -
         * coordPosition - gives the position of the maptile in coordinate form(ie, "4,7")
         * truePosition - gives the position of the maptile in actual pixel position(ie, "64,512")
         */
        public Vector2 coordPosition;
        public Vector2 truePosition;

        /* - Drawing Variables - */
        public Texture2D texture;
        public Rectangle drawingRectangle;

        public Maptile(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.coordPosition = position;
            this.truePosition = new Vector2(texture.Width * position.X, texture.Height * position.Y);
            drawingRectangle = new Rectangle((int)truePosition.X, (int)truePosition.Y, texture.Width, texture.Height);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, drawingRectangle, Color.White);
        }
    }
}
