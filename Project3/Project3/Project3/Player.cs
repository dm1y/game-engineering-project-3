﻿using Microsoft.Xna.Framework;
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

        public World world;

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

        public int dimension;
        /* For deciding where the player is facing. 
         * If north, (0, -1). If south, (0, 1). If west, (-1, 0). If east, (0, 1)
         */
        private Vector2 frontOfPlayer;
        private Vector2 nextPosition;  /* Preemptive movement  */
        private Vector2 currPosition; /* Position of player IN WORLD SPACE */
        private Vector2 currPositionCoord; /* Position of player in MAP COORDINATE SPACE */

        public Vector2 position
        {
            get { return currPosition; }
        }


        public Maptile[,] map;
        public Player(Texture2D north, Texture2D south, Texture2D west, Texture2D east, Vector2 spawnPosition,
            Maptile[,] map, World world)
        {
            this.world = world;

            playerNorth = north;
            playerSouth = south;
            playerWest = west;
            playerEast = east;

            facingNorth = true;
            facingEast = false;
            facingWest = false;
            facingSouth = false;

            frontOfPlayer = new Vector2(0, -1);
            dimension = north.Height;
            this.map = map;
            currPositionCoord = spawnPosition;
            nextPosition = new Vector2(spawnPosition.X * dimension, spawnPosition.Y * dimension);
            currPosition = nextPosition;
        }

        /* Update order:
         *      World calls update(), which calls player.UpdateInput(). Input is received and will only move if
         *      currPosition = nextPosition. 
         *      Movement is called, which sets player's orientation and sets their currPositionCoord one over
         *      to the block they will move on.
         *      World's update() then calls player.UpdatePosition(), which will move the player over 1 pixel per update.
         *      This will keep happening until player reaches the nextPosition.
         */

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
         *      Once player's currPosition and nextPosition are lined up, then we check the tile properties
         *      and what to apply. 
         *      
         *      If player presses ENTER and they are lined up with an NPC in front of them(has to be 
         *      in front relative to the player), then it will initiate conversation and/or shop.
         */
        public void UpdateInput(GameTime gameTime, KeyboardState keyboard)
        {
            //If current position is already at next position, player can movei
            if (currPosition.X == nextPosition.X && currPosition.Y == nextPosition.Y)
            {
                if (keyboard.IsKeyDown(Keys.W))
                {
                    moveUp();
                }
                else if (keyboard.IsKeyDown(Keys.S))
                {
                    moveDown();
                }
                else if (keyboard.IsKeyDown(Keys.A))
                {
                    moveLeft();
                }
                else if (keyboard.IsKeyDown(Keys.D))
                {
                    moveRight();
                }

                if (keyboard.IsKeyDown(Keys.Enter))
                {
                    CheckInteract();
                }

                CheckTile();
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
            int yDifference = (int)(currPosition.Y - nextPosition.Y);
            int xDifference = (int)(currPosition.X - nextPosition.X);

            if (yDifference > 0)
            {
                currPosition.Y = currPosition.Y - 4;
            }
            if (yDifference < 0)
            {
                currPosition.Y = currPosition.Y + 4;
            }
            if (xDifference > 0)
            {
                currPosition.X = currPosition.X - 4;
            }
            if (xDifference < 0)
            {
                currPosition.X = currPosition.X + 4;
            }
        }

        private void StayWithinBounds()
        {
            /* TODO: Fill in later 
             * 
             * 
             * LEFTBOUND: 
             *    if x coordinate of the map that player is touching is less than or equal to the x position of player 
             *    i.e. [0, whatever] 
             *       have the player stay there at [0, y] if player is navigating towards the negatives in x axis
             * TOPBOUND: 
             *    if y coordinate of the map that player is touching is less than or equal to the y position of player 
             *    i.e. [whatever, 0]
             *       have the player stay there at [x, 0] if player is navigating towards the negatives in y axis
             * RIGHTBOUND:
             *    if x coordinate of the map that player is touching is greater than or equal to the x position of player 
             *    i.e. [boundary.x, whatever]; 
             *       have the player stay there at [boundary.x, y] if player is navigating towards out of range in x axis
             * BOTTOMBOUND:
             *    if x coordinate of the map that player is touching is greater than or equal to the x position of player 
             *    i.e. [whatever, boundary.y]
             *       have the player stay there at [x, boundary.y] if player is navigating towards out of range in y axis
             */
        }

        //Orients the player right, then sets the next X position to +tilewidth, and adds 1 to the relative X. 
        private void moveRight()
        {
            setFacingEast();
            frontOfPlayer = new Vector2(0, 1);
            if (map[(int)currPositionCoord.Y, (int)currPositionCoord.X + 1].isCollidable == false)
            {
                currPositionCoord.X = currPositionCoord.X + 1;
                nextPosition.X = nextPosition.X + dimension;
            }
        }

        //Orients the player left, then sets the next X position to -tilewidth, and subtracts 1 from the relative X. 
        private void moveLeft()
        {
            setFacingWest();
            frontOfPlayer = new Vector2(-1, 0);
            if (map[(int)currPositionCoord.Y, (int)currPositionCoord.X - 1].isCollidable == false)
            {
                currPositionCoord.X = currPositionCoord.X - 1;
                nextPosition.X = nextPosition.X - dimension;
            }
        }

        //Orients the player up, then sets the next Y position to -tilewidth, and subtracts 1 from relative Y. 
        private void moveUp()
        {
            setFacingNorth();
            frontOfPlayer = new Vector2(0, -1);
            if (map[(int)currPositionCoord.Y - 1, (int)currPositionCoord.X].isCollidable == false)
            {
                currPositionCoord.Y = currPositionCoord.Y - 1;
                nextPosition.Y = nextPosition.Y - dimension;
            }
        }

        //Orients the player down, then sets the next Y position to +tilewidth, and adds 1 to relative Y. 
        private void moveDown()
        {
            setFacingSouth();
            frontOfPlayer = new Vector2(0, 1);
            if (map[(int)currPositionCoord.Y + 1, (int)currPositionCoord.X].isCollidable == false)
            {
                currPositionCoord.Y = currPositionCoord.Y + 1;
                nextPosition.Y = nextPosition.Y + dimension;
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

        /* -- Interaction Methods -- */
        /* CheckTile() is used to check the properties of any tile that the player is 
         actually standing in. For instance, a player can actually stand in a transition tile,
         or in tall grass/swamp/cave when looking for monsters. */
        public void CheckTile()
        {
            Maptile tileToCheck = map[(int)currPositionCoord.Y, (int)currPositionCoord.X];


            /* Hard coded for testing purposes, we can generalize this later once we get XMLs working */
            /* If player hits the tile found in map[5,5] switch to the third map */
            /* TODO: Generalize the code above instead of hard coding, so we need to detect 
             * what our collisions will be that would cause the transition and the 
             * location of where those objects will be placed in the map */
            if (currPositionCoord.X == 5 && currPositionCoord.Y == 5)
                world.TransitionMap(3);
            /* If player hits the tile found in map[15,15] switch to the first map */
            if (currPositionCoord.X == 15 && currPositionCoord.Y == 15)
                world.TransitionMap(1);


            //if tileToCheck is a transition, then we figure out where the player's transition goes to. Then we call
            //load from world or something like that.
        }

        public void ChangeMap(Maptile[,] map)
        {
            this.map = map;
        }

        public void SpawnPlayerAt(Vector2 position)
        {

        }

        /* Handles iteration with NPCs, Merchants, and Objects*/
        /* CheckInteract() is used to check the properties of a tile that the player CAN'T
         stand on top of, but can still interact with(ie, loot chests, or signs, or people)*/
        public void CheckInteract()
        {
            Maptile tileToCheck = map[(int)(currPositionCoord.Y + frontOfPlayer.Y), (int)(currPositionCoord.X + frontOfPlayer.X)];

            //and then checking logic here
        }

        /* Basic Draw Method
         * Note: Texture2Ds will be replaced with animations later.
         */
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle playerSpriteBox = new Rectangle((int)currPosition.X, (int)currPosition.Y, dimension, dimension);
            if (facingNorth)
            {
                spriteBatch.Draw(playerNorth, playerSpriteBox, Color.White);
            }
            else if (facingEast)
            {
                spriteBatch.Draw(playerEast, playerSpriteBox, Color.White);
            }
            else if (facingSouth)
            {
                spriteBatch.Draw(playerSouth, playerSpriteBox, Color.White);
            }
            else if (facingWest)
            {
                spriteBatch.Draw(playerWest, playerSpriteBox, Color.White);
            }
        }
    }
}