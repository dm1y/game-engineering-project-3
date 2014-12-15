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
        public Texture2D inv;

        //public BattleSystem battleSys;

        public Inventory playerInventory;
        public Boolean mayContinue = false;
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
        public Boolean isBattling;
        public Boolean isDisplayInventory;
        
        public NPC currentNPC;

        public int baseAtk;
        public int basedef;

        /* For the battle system aspect */
        public int atk;
        public int def;
        public int speed;

        public Item weapon;
        public Item shield;
        public Item consumable;

        public int dimension;
        /* For deciding where the player is facing. 
         * If north, (0, -1). If south, (0, 1). If west, (-1, 0). If east, (0, 1)
         */
        private Vector2 frontOfPlayer;
        private Vector2 nextPosition;  /* Preemptive movement  */
        private Vector2 currPosition; /* Position of player IN WORLD SPACE */
        private Vector2 currPositionCoord; /* Position of player in MAP COORDINATE SPACE */

        public Boolean hasChecked;

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
            isBattling = false;
            isDisplayInventory = false;
            /* Default stats for battle system */
            atk = 3;
            def = 2;
            //Don't let speed be less than 11 -- the max weight for a weapon/shield is 5(so player's slowest speed is 1);
            speed = 11;
            hasChecked = false;

            inv = world.game.Content.Load<Texture2D>("Overlays/inventorybg");
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
            //TODO: Colin -- Can check keyboard inputs for "1,2,3,4,5,6,7,8,9,0" to equip items.
            //Note: Be sure to check type of item so that player doesn't end up equipping junk, 
            //or items in the wrong area. Methods for equip are at the bottom(lines 430 and onwards)

            #region Battle System Control
            if (isBattling)
            {

                if (world.battleSystem.exitBattle)
                {
                    isBattling = false;
                    world.battleSystem.ResetBattle();
                }
                if ((keyboard.IsKeyDown(Keys.Enter) || keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Back)) && mayContinue)
                {
                    playSound(keyboard);
                    world.battleSystem.Update(keyboard);
                    mayContinue = false;
                }
                else if (keyboard.IsKeyUp(Keys.Enter) && keyboard.IsKeyUp(Keys.W) && keyboard.IsKeyUp(Keys.S)
                    && keyboard.IsKeyUp(Keys.Back) && !mayContinue)
                {
                    mayContinue = true;
                }
                
            }
            #endregion

            #region NPC Interaction region
            //If the player is interacting, delegate all keyboard commands into the NPC. 
            if (isInteracting)
            {
                //if current NPC is finished,
                if (currentNPC.isFinished)
                {
                    isInteracting = false;
                    currentNPC.isFinished = false;
                }
                else
                {
                    if ((keyboard.IsKeyDown(Keys.Enter) || (keyboard.IsKeyDown(Keys.W))
                        || (keyboard.IsKeyDown(Keys.S)) || (keyboard.IsKeyDown(Keys.Back))) && mayContinue)
                    {
                        playSound(keyboard);

                        currentNPC.Update(keyboard);
                        mayContinue = false;
                    }
                    else if (keyboard.IsKeyUp(Keys.Enter) && keyboard.IsKeyUp(Keys.W) && keyboard.IsKeyUp(Keys.S)
                        && keyboard.IsKeyUp(Keys.Back) && !mayContinue)
                    {
                        playSound(keyboard);   //CHECK!
                        mayContinue = true;
                    }
                }
            }
#endregion

            #region Inventory Manipulation 
            if (isDisplayInventory)
            {
                if (keyboard.IsKeyDown(Keys.I) && mayContinue)
                {
                    isDisplayInventory = false;
                }
                if (keyboard.IsKeyUp(Keys.I) && !mayContinue)
                {
                    mayContinue = true;
                }
                //Currently will just display what the player has equipped
            }
            #endregion
            #region Player movement and Tile Check region
            if (!isInteracting && !isBattling && !isDisplayInventory)
            {
                //If current position is already at next position, player can move
                if (currPosition.X == nextPosition.X && currPosition.Y == nextPosition.Y)
                {

                    if (keyboard.IsKeyDown(Keys.W))
                    {
                        hasChecked = false;
                        moveUp();
                    }
                    else if (keyboard.IsKeyDown(Keys.S))
                    {
                        hasChecked = false;
                        moveDown();
                    }
                    else if (keyboard.IsKeyDown(Keys.A))
                    {
                        hasChecked = false;
                        moveLeft();
                    }
                    else if (keyboard.IsKeyDown(Keys.D))
                    {
                        hasChecked = false;
                        moveRight();
                    }
                    else {
                        SetIdle();
                    }
                    
                    if (keyboard.IsKeyUp(Keys.Enter) && keyboard.IsKeyUp(Keys.I) && !mayContinue)
                    {
                        mayContinue = true;
                    }
                    if (keyboard.IsKeyDown(Keys.I) && mayContinue)
                    {
                        isDisplayInventory = true;
                        mayContinue = false;
                    }
                    if (keyboard.IsKeyDown(Keys.Enter) && mayContinue && !isDisplayInventory )
                    {
                        CheckInteract();
                    }

                    CheckTile();
                }
            }
        #endregion

            // Equips the item 
            EquipItem(keyboard);

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
            frontOfPlayer = new Vector2(1, 0);

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

            if (tileToCheck.isDangerous && !hasChecked)
            {
                if (!isBattling)
                {
                    if (tileToCheck.battleType == "easy") 
                        world.battleSystem.GenerateBattle(tileToCheck.easyEnemies);

                    if (tileToCheck.battleType == "medium")
                        world.battleSystem.GenerateBattle(tileToCheck.mediumEnemies);

                    if (tileToCheck.battleType == "hard")
                        world.battleSystem.GenerateBattle(tileToCheck.mediumEnemies);
                }
                hasChecked = true;
                isBattling = true;
                //Console.WriteLine("Player is now in battle!");
                //Console.WriteLine("Easy Enemies: ");
                //foreach (Enemy e in tileToCheck.easyEnemies)
                //{
                //    Console.Write(e.enemyName + ", ");
                //}
                //Console.WriteLine("\nMedium Enemies: ");
                //foreach (Enemy e in tileToCheck.mediumEnemies)
                //{
                //    Console.Write(e.enemyName + ", ");
                //}
                //Console.WriteLine("\nHard Enemies: ");
                //foreach (Enemy e in tileToCheck.hardEnemies)
                //{
                //    Console.Write(e.enemyName + ", ");
                //}

                // TODO: Fill in. 
                // transition to battle sys map 
                // or draw battle sys map 
                // will fill in after the UX/UI is done for battle sys
            }
            
        }

        public void ChangeMap(Map map)
        {
            this.map = map;
        }

        public void SpawnPlayerAt(Vector2 position)
        {

        }

        public void SetInventory(Inventory inv)
        {
            this.playerInventory = inv;
        }
        /* Handles iteration with NPCs, Merchants, and Objects*/
        /* CheckInteract() is used to check the properties of a tile that the player CAN'T
         stand on top of, but can still interact with(ie, loot chests, or signs, or people)*/
        public void CheckInteract()
        {
            Maptile tileToCheck = map.currentMap[(int)(currPositionCoord.X + frontOfPlayer.X), (int)(currPositionCoord.Y + frontOfPlayer.Y)];
            if (tileToCheck.isInteract)
            {
                isInteracting = true;
                mayContinue = false;
                currentNPC = tileToCheck.npc;
            }
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
                
            }
            else if (facingEast)
            {
                playerEast.Draw(spriteBatch);
                
            }
            else if (facingSouth)
            {
                playerSouth.Draw(spriteBatch);
                
            }
            else if (facingWest)
            {
                playerWest.Draw(spriteBatch);
                
            }

            if (isDisplayInventory)
            {

                Vector2 overall = new Vector2(world.camera.Position.X / 2, world.camera.Position.Y / 2);

                spriteBatch.Draw(inv, overall, Color.White);

                Vector2 wep = overall + new Vector2(8, 8);
                Vector2 shield = overall + new Vector2(8, 32);
                Vector2 consumable = overall + new Vector2(8, 56);

                spriteBatch.DrawString(world.shopDialogueFont, "Weapon: ", wep, Color.White);
                spriteBatch.DrawString(world.shopDialogueFont, "Shield: ", shield, Color.White);
                spriteBatch.DrawString(world.shopDialogueFont, "Consumable: ", consumable, Color.White);

                //Draw a small display box, with
                //Weapon: <Item Name>, Dmg: <Item Dmg>
                //Shield: <Item Name>, Def: <Item def>
                //Consumable: <Item Name>, Heals: <Item heal>
            }
            if (isInteracting)
            {
                currentNPC.Draw(spriteBatch);
            }
        }

        // adjusts stats according to weapon item properties 
        public void setAtk(Item item)
        {
            weapon = item;
            atk = item.damage + baseAtk;
            speed -= item.weight;
        }

        // adjusts stats according to defense item properties 
        public void setDef(Item item)
        {
            shield = item;
            def = item.block + basedef;
            speed -= item.weight;
        }

        // sets the current equipped consumable to item being passed in 
        public void setConsumable(Item item)
        {
            consumable = item;
        }

        public void playSound(KeyboardState keyboard)    //CHECK NEEDS WORK!
        {
            if ( isInteracting && !isBattling && !currentNPC.isFinished)
            {
                if (keyboard.IsKeyDown(Keys.Enter))
                {
                    world.talkToNPCInstance.Volume = 0.5f;
                    world.talkToNPCInstance.Pan = 0.5f;
                    world.talkToNPCInstance.Play();
                    
                }

                if (keyboard.IsKeyDown(Keys.Back))
                {
                    world.hitBackSpaceSoundInstance.Volume = 0.2f;
                    world.hitBackSpaceSoundInstance.Pan = 0.2f;
                    world.hitBackSpaceSoundInstance.Play();
                }

                if (keyboard.IsKeyDown(Keys.W))
                {
                    world.menuSoundInstance.Volume = 0.3f;
                    world.menuSoundInstance.Pan = 0.5f;
                    world.menuSoundInstance.Play();
                }
                if (keyboard.IsKeyDown(Keys.S))
                {
                    world.menuSoundInstance.Volume = 0.3f;
                    world.menuSoundInstance.Pan = 0.5f;
                    world.menuSoundInstance.Play();
                }
            }

            else if (isBattling && !world.battleSystem.inEnemySelect && !world.battleSystem.exitBattle && world.battleSystem.inChoices)
            {
                if (world.battleSystem.currentSelect == 0)
                {
                    if (keyboard.IsKeyDown(Keys.Enter))
                    {
                        world.attackSoundInstance.Volume = 0.8f;
                        world.attackSoundInstance.Pan = 0.9f;
                        world.attackSoundInstance.Play();
                    }


                    if (keyboard.IsKeyDown(Keys.Delete))
                    {
                        world.hitBackSpaceSoundInstance.Volume = 0.2f;
                        world.hitBackSpaceSoundInstance.Pan = 0.2f;
                        world.hitBackSpaceSoundInstance.Play();
                    }

                    if (keyboard.IsKeyDown(Keys.W))
                    {
                        world.menuSoundInstance.Volume = 0.3f;
                        world.menuSoundInstance.Pan = 0.5f;
                        world.menuSoundInstance.Play();
                    }
                    if (keyboard.IsKeyDown(Keys.S))
                    {
                        world.menuSoundInstance.Volume = 0.3f;
                        world.menuSoundInstance.Pan = 0.5f;
                        world.menuSoundInstance.Play();
                    }
                }


            }
            
           
        }

        #region Equipping of items
        private void EquipItem(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.NumPad1) || keyboard.IsKeyDown(Keys.D1))
            {
                Equip(0);
            }

            if (keyboard.IsKeyDown(Keys.NumPad2) || keyboard.IsKeyDown(Keys.D2))
            {
                Equip(1);
            }

            if (keyboard.IsKeyDown(Keys.NumPad3) || keyboard.IsKeyDown(Keys.D3))
            {
                Equip(2);
            }

            if (keyboard.IsKeyDown(Keys.NumPad4) || keyboard.IsKeyDown(Keys.D4))
            {
                Equip(3);
            }

            if (keyboard.IsKeyDown(Keys.NumPad5) || keyboard.IsKeyDown(Keys.D5))
            {
                Equip(4);
            }

            if (keyboard.IsKeyDown(Keys.NumPad6) || keyboard.IsKeyDown(Keys.D6))
            {
                Equip(5);
            }

            if (keyboard.IsKeyDown(Keys.NumPad7) || keyboard.IsKeyDown(Keys.D7))
            {
                Equip(6);
            }

            if (keyboard.IsKeyDown(Keys.NumPad8) || keyboard.IsKeyDown(Keys.D8))
            {
                Equip(7);
            }

            if (keyboard.IsKeyDown(Keys.NumPad9) || keyboard.IsKeyDown(Keys.D9))
            {
                Equip(8);
            }

            if (keyboard.IsKeyDown(Keys.NumPad0) || keyboard.IsKeyDown(Keys.D0))
            {
                Equip(9);
            }
        }

        private void Equip(int i)
        {
            if (playerInventory.items[i] != null)
            {
                if (playerInventory.items[i].isPotion)
                {
                    setConsumable(playerInventory.items[i]);
                    // TODO: Update display of equipped consumable item 
                }

                if (playerInventory.items[i].isShield)
                {
                    setDef(playerInventory.items[i]);
                    // TODO: Update display of defense item/shield if applicable 
                }

                if (playerInventory.items[i].isWeapon)
                {
                    setAtk(playerInventory.items[i]);
                    // TODO: Update player's equipped weapon
                }
            }
        }
        #endregion
    }
}
