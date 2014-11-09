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

        /* Movement Updates
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
        public void Update(GameTime gameTime, KeyboardState keyboard)
        {
            //If current position is already at next position, player can move
            if (currPosition == nextPosition)
            {
                if (keyboard.IsKeyDown(Keys.Right))
                {
                    moveRight();
                }

            }
        }

        private void moveRight()
        {

        }

        private void moveLeft()
        {
        }

        private void moveUp()
        {
        }

        private void moveDown()
        {
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
