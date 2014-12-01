using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project3
{
    public class Player
    {

        public World world; 

        /* - Directional Textures - 
         * Using the booleans, update() and draw() will handle
         * the cases for how the player moves in each direction.
         */
        public Animation playerNorth;
        public Animation playerSouth;
        public Animation playerWest;
        public Animation playerEast;

        public Boolean facingNorth;
        public Boolean facingSouth;
        public Boolean facingWest;
        public Boolean facingEast;

        public Boolean isInteracting;
        public NPC currentNPC;

        public int dimension;
        /* For deciding where the player is facing. 
         * If north, (0, -1). If south, (0, 1). If west, (-1, 0). If east, (0, 1)
         */
        private Vector2 frontOfPlayer;
        private Vector2 nextPosition;  /* Preemptive movement  */
        private Vector2 currPosition; /* Position of player IN WORLD SPACE */
        private Vector2 currPositionCoord; /* Position of player in MAP COORDINATE SPACE */

        /* For Battle System */
        public int atk;
        public int def;
        public int speed; 

        public Vector2 position
        {
            get { return currPosition; }
        }


        //public Maptile[,] map;
        public Map map;
        public Player(Texture2D north, Texture2D south, Texture2D west, Texture2D east, Vector2 spawnPosition, 
            Map map, World world)
        {
            this.world = world; 

            playerNorth = new Animation();
            playerSouth = new Animation();
            playerWest = new Animation();
            playerEast = new Animation();

            playerNorth.Initialize(north, currPosition, 32, 32, 3, 100, Color.White, 1, true, false);
            playerSouth.Initialize(south, currPosition, 32, 32, 3, 100, Color.White, 1, true, false);
            playerWest.Initialize(west, currPosition, 32, 32, 3, 100, Color.White, 1, true, false);
            playerEast.Initialize(east, currPosition, 32, 32, 3, 100, Color.White, 1, true, false);

            facingNorth = false;
            facingEast = true;
            facingWest = false;
            facingSouth = false;

            frontOfPlayer = new Vector2(0, -1);
            dimension = north.Height;
            this.map = map;
            currPositionCoord = spawnPosition;
            nextPosition = new Vector2(spawnPosition.X * dimension, spawnPosition.Y * dimension);
            currPosition = nextPosition;

            isInteracting = false;
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
            //If the player is interacting, delegate all keyboard commands into the NPC. 
            if (isInteracting)
            {
                if (currentNPC.isFinished)
                {
                    isInteracting = false;
                    currentNPC.isFinished = false;
                }
                else
                {
                    currentNPC.Update(keyboard);
                }
            }

            else
            {
                //If current position is already at next position, player can move
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
                    else {
                        SetIdle();
                    }
                    if (keyboard.IsKeyDown(Keys.Enter))
                    {
                        CheckInteract();
                    }

                    CheckTile();
                }
            }
        }

        public void UpdateAnimations(GameTime gametime)
        {
            playerNorth.Update(currPosition + new Vector2(16, 16), gametime);
            playerSouth.Update(currPosition + new Vector2(16, 16), gametime);
            playerWest.Update(currPosition + new Vector2(16, 16), gametime);
            playerEast.Update(currPosition + new Vector2(16, 16), gametime);
        }

        public void SetIdle()
        {
            playerNorth.setFrame(0);
            playerSouth.setFrame(0);
            playerWest.setFrame(0);
            playerEast.setFrame(0);
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
                UpdateAnimations(gameTime);
            }
            if (yDifference < 0)
            {
                currPosition.Y = currPosition.Y + 4;
                UpdateAnimations(gameTime);
            }
            if (xDifference > 0)
            {
                currPosition.X = currPosition.X - 4;
                UpdateAnimations(gameTime);
            }
            if (xDifference < 0)
            {
                currPosition.X = currPosition.X + 4;
                UpdateAnimations(gameTime);
            }

        }

        //Orients the player right, then sets the next X position to +tilewidth, and adds 1 to the relative X. 
        private void moveRight()
        {
            setFacingEast();
            frontOfPlayer = new Vector2(0, 1);

            if ((int)currPositionCoord.X != map.width - 1)
            {
                if (map.currentMap[(int)currPositionCoord.X + 1, (int)currPositionCoord.Y].isCollidable == false)
                {
                    currPositionCoord.X = currPositionCoord.X + 1;
                    nextPosition.X = nextPosition.X + dimension;
                }
            }
        }

        //Orients the player left, then sets the next X position to -tilewidth, and subtracts 1 from the relative X. 
        private void moveLeft()
        {
            setFacingWest();
            frontOfPlayer = new Vector2(-1, 0);

            if ((int)currPositionCoord.X != 0)
            {
                if (map.currentMap[(int)currPositionCoord.X - 1, (int)currPositionCoord.Y].isCollidable == false)
                {
                    currPositionCoord.X = currPositionCoord.X - 1;
                    nextPosition.X = nextPosition.X - dimension;
                }
            }


        }

        //Orients the player up, then sets the next Y position to -tilewidth, and subtracts 1 from relative Y. 
        private void moveUp()
        {
            setFacingNorth();
            frontOfPlayer = new Vector2(0, -1);

            if ((int)currPositionCoord.Y != 0)
            {
                if (map.currentMap[(int)currPositionCoord.X, (int)currPositionCoord.Y - 1].isCollidable == false)
                {
                    currPositionCoord.Y = currPositionCoord.Y - 1;
                    nextPosition.Y = nextPosition.Y - dimension;
                }
            }
        }

        //Orients the player down, then sets the next Y position to +tilewidth, and adds 1 to relative Y. 
        private void moveDown()
        {
            setFacingSouth();
            frontOfPlayer = new Vector2(0, 1);

            /* Move if not on the border */
            if ((int)currPositionCoord.Y != map.height - 1)
            {
                if (map.currentMap[(int)currPositionCoord.X, (int)currPositionCoord.Y + 1].isCollidable == false)
                {
                    currPositionCoord.Y = currPositionCoord.Y + 1;
                    nextPosition.Y = nextPosition.Y + dimension;
                }
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
            Maptile tileToCheck = map.currentMap[(int)currPositionCoord.X,(int)currPositionCoord.Y];


            /* Checks if the tile is a transition tile and transitions to the map defined by the tile */
            if (tileToCheck.isTransition)
                world.TransitionMap(tileToCheck.transitionTo);

            if (tileToCheck.isDangerous)
            {
                // TODO: Fill in. 
            }
            
        }

        public void ChangeMap(Map map)
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
            Maptile tileToCheck = map.currentMap[(int)(currPositionCoord.X + frontOfPlayer.X), (int)(currPositionCoord.Y + frontOfPlayer.Y)];

            //if tile is interactable, override controls with isInteracting, then
            //pull current reference to the NPC over
            if (tileToCheck.isInteract)
            {
                isInteracting = true;
                currentNPC = tileToCheck.npc;
            }
            //and then checking logic here
        }

        /* Basic Draw Method
         * Note: Texture2Ds will be replaced with animations later.
         */
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle playerSpriteBox = new Rectangle((int)currPosition.X, (int)currPosition.Y, dimension,dimension);
            if (facingNorth)
            {

                playerNorth.Draw(spriteBatch);
                //spriteBatch.Draw(playerNorth, playerSpriteBox, Color.White);
            }
            else if (facingEast)
            {
                Console.Write(true);
                Console.Write(currPosition);
                playerEast.Draw(spriteBatch);
                //spriteBatch.Draw(playerEast, playerSpriteBox, Color.White);
            }
            else if (facingSouth)
            {
                playerSouth.Draw(spriteBatch);
                //spriteBatch.Draw(playerSouth, playerSpriteBox, Color.White);
            }
            else if (facingWest)
            {
                playerWest.Draw(spriteBatch);
                //spriteBatch.Draw(playerWest, playerSpriteBox, Color.White);
            }

            if (isInteracting)
            {
                currentNPC.Draw(spriteBatch);
            }
        }

        // used when player equips different item, changes attack & speed
        public void setAttack(Item item)
        { 
            // set atk equal to the atk power of item 
        }

        // used when player equips different item, changes defense & speed 
        public void setDefense(Item item)
        { 
            // set def equal to the def power of item
        }
    }
}
