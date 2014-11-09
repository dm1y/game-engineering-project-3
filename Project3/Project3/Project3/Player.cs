using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project3
{
    class Player
    {
        /* - Directional Textures - 
         * Using the booleans, update() and draw() will handle
         * the cases for how the player moves in each direction.
         */
        public Texture2D playerNorth;
        public Texture2D playerSouth;
        public Texture2D playerWest;
        public Texture2D playerEast;

        public Boolean facingNorth;
        public Boolean facingSouth;
        public Boolean facingWest;
        public Boolean facingEast;

        private KeyboardState lastState;
        private Vector2 nextPosition;
        private Vector2 currPosition;
        private Vector2 currPositionCoord;

        public Maptile[][] map;
        public Player(Texture2D north, Texture2D south, Texture2D west, Texture2D east, Vector2 spawnPosition, 
            Maptile[][] map)
        {
            playerNorth = north;
            playerSouth = south;
            playerWest = west;
            playerEast = east;

            facingNorth = true;
            facingEast = false;
            facingWest = false;
            facingSouth = false;

            this.map = map;
            currPositionCoord = spawnPosition;
            nextPosition = new Vector2(spawnPosition.X * north.Width, spawnPosition.Y * north.Height);
            currPosition = nextPosition;
        }

        /* Input Update Method
         * Rules: 
         *      If current position and next position are the same, then the player
         *      is allowed to move. Player will receive a keyboard command, which means that they will
         *      turn in the given direction and move with it. Must move in increments which will
         *      reach the next block without going over. One pixel per update should be ok, meaning the
         *      player will move to the next block in 0.516 seconds.
         *      
         *      nextPosition is determined by the player's CURRENT position & where it is on the map.
         *      For instance, if the keyboard input is UP and the block UP is not walkable, then the
         *      next position will be the same as curr position. 
         *      
         *      Account for cases: Wall to left, right, top, below. Use IFS, not elifs! 
         */
        public void UpdateInput(GameTime gameTime, KeyboardState keyboard)
        {
            //If current position is already at next position, player can move
            if (currPosition.X == nextPosition.X && currPosition.Y == nextPosition.Y)
            {
                if (keyboard.IsKeyDown(Keys.W))
                {
                    moveUp();
                }
                if (keyboard.IsKeyDown(Keys.S))
                {
                    moveDown();
                }
                if (keyboard.IsKeyDown(Keys.A))
                {
                    moveLeft();
                }
                if (keyboard.IsKeyDown(Keys.D))
                {
                    moveRight();
                }
            }
        }

        /* Player Position Update Method 
         * 
         * Updates the player's current position based off of where the next move is.
         * Will be player.Y - nextMove.Y, and player.X - nextMove.X
         * Difference guideline:
         *      if Y-DIFFERENCE is positive, player needs to move up
         *      if Y-DIFFERENCE is negative, player needs to move down
         *      if X-DIFFERENCE is positive, player needs to move left
         *      if X-DIFFERENCE is negative, player needs to move right
         */
        public void UpdatePosition(GameTime gameTime)
        {
            //TODO after dinner
        }

        //Orients the player right, then sets the next X position to +tilewidth, and adds 1 to the relative X. 
        private void moveRight()
        {
            setFacingEast();
            if (map[(int)currPositionCoord.Y][(int)currPositionCoord.X + 1].isCollidable == false)
            {
                currPositionCoord.X = currPositionCoord.X + 1;
                nextPosition.X = nextPosition.X + playerNorth.Width;
            }
        }

        //Orients the player left, then sets the next X position to -tilewidth, and subtracts 1 from the relative X. 
        private void moveLeft()
        {
            setFacingWest();
            if (map[(int)currPositionCoord.Y][(int)currPositionCoord.X - 1].isCollidable == false)
            {
                currPositionCoord.X = currPositionCoord.X - 1;
                nextPosition.X = nextPosition.X - playerNorth.Width;
            }
        }

        //Orients the player up, then sets the next Y position to -tilewidth, and subtracts 1 from relative Y. 
        private void moveUp()
        {
            setFacingNorth();
            if (map[(int)currPositionCoord.Y - 1][(int)currPositionCoord.X].isCollidable == false)
            {
                currPositionCoord.Y = currPositionCoord.Y - 1;
                nextPosition.Y = nextPosition.Y - playerNorth.Width;
            }
        }

        //Orients the player down, then sets the next Y position to +tilewidth, and adds 1 to relative Y. 
        private void moveDown()
        {
            setFacingSouth();
            if (map[(int)currPositionCoord.Y + 1][(int)currPositionCoord.X].isCollidable == false)
            {
                currPositionCoord.Y = currPositionCoord.Y + 1;
                nextPosition.Y = nextPosition.Y + playerNorth.Width;
            }
        }

        private void setFacingEast()
        {
            facingEast = true;
            facingNorth = facingSouth = facingWest = false;
        }

        private void setFacingWest()
        {
            facingWest = true;
            facingNorth = facingSouth = facingEast = false;
        }

        private void setFacingNorth()
        {
            facingNorth = true;
            facingSouth = facingEast = facingWest = false;
        }

        private void setFacingSouth()
        {
            facingSouth = true;
            facingNorth = facingEast = facingWest = false;
        }
        public void ChangeMap(Maptile[][] map)
        {
            this.map = map;
        }

        public void SpawnPlayerAt(Vector2 position)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            

        }
    }
}
