using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project3
{
    class Shop
    {

        Player player;
        Inventory playerInventory;
        Inventory shopInventory;
        Boolean isBuying;
        Boolean isSelling;

        Boolean isConfirm;

        Boolean isErrorState;
        String displayText;
        int quantityOf;

        public Boolean isFinished;
        /* Possible states: 
            0 - Arrow is hovering over BUY
            1 - Arrow is hovering over SELL
            2 - Arrow is hovering over LEAVE*/
        int currentState;

        int currentItemSelect;

        public Shop(Inventory shopInventory)
        {
            this.shopInventory = shopInventory;
            isBuying = false;
            isSelling = false;
            isErrorState = false;
        }


        public void PlayerShop(Inventory playerInventory)
        {
            this.playerInventory = playerInventory;
            currentState = 0;
        }

        #region Update Handler
        public void Update(KeyboardState keyboard)
        {
            // If player is notBuying and notSelling, then they must be in selection
            if (!isBuying && !isSelling)
            {
                if (keyboard.IsKeyDown(Keys.W))
                {
                    currentState--;
                    if (currentState < 0)
                    {
                        currentState = 0;
                    }
                }
                else if (keyboard.IsKeyDown(Keys.S))
                {
                    currentState++;
                    if (currentState > 2)
                    {
                        currentState = 2;
                    }
                }
            }

            //If the player hits enter during selection and it is NOT selling or buying
            if (keyboard.IsKeyDown(Keys.Enter) && !isSelling && !isBuying)
            {
                if (currentState == 2)
                {
                    isFinished = true;
                    //Quit the shop, probably by calling it from player. 
                }
                else if (currentState == 1)
                {
                    //Enter SELL MODE
                    isSelling = true;
                    isBuying = false;
                }
                else if (currentState == 0)
                {
                    //Enter BUY MODE
                    isBuying = true;
                    isSelling = false;
                }
            }

            //If the player hits back during buy/sell and it is not a confirm state nor error, exit out of it.
            if (keyboard.IsKeyDown(Keys.Back) && !isConfirm && !isErrorState)
            {
                if (isSelling)
                {
                    isSelling = false;
                }
                else if (isBuying)
                {
                    isBuying = false;
                }
                currentItemSelect = 0;
                quantityOf = 0;
                displayText = "What will you be buying?";
            }

            if (isBuying)
            {
                HandlePurchases(keyboard);
            }

            else if (isSelling)
            {
                HandleSales(keyboard);
            }
        }
        #endregion

        #region Buying Items Region
        //If player is in buying mode, if the player presses W, move up, and presses S, move down. 
        //if press enter, inquire how much to buy, and if press enter again, calculates if player has
        //enough money to buy. if so, player gets x quantity of that item added to inventory? 
        public void HandlePurchases(KeyboardState kb)
        {
            if (!isErrorState)
            {
                if (kb.IsKeyDown(Keys.W) && !isConfirm)
                {
                    currentItemSelect--;
                    if (currentItemSelect < 0)
                    {
                        currentItemSelect = 0;
                    }
                }
                if (kb.IsKeyDown(Keys.S) && !isConfirm)
                {
                    currentItemSelect++;
                    if (currentItemSelect > shopInventory.items.Count() - 1)
                    {
                        currentItemSelect = shopInventory.items.Count - 1;
                    }
                }

                if (kb.IsKeyDown(Keys.Enter))
                {
                    isConfirm = true;
                    quantityOf = 1;
                }

                if (isConfirm == true)
                {
                    if (kb.IsKeyDown(Keys.W))
                    {
                        quantityOf++;
                    }
                    if (kb.IsKeyDown(Keys.S))
                    {
                        quantityOf--;
                        if (quantityOf < 1)
                        {
                            quantityOf = 1;
                        }
                    }
                    if (kb.IsKeyDown(Keys.Back))
                    {
                        isConfirm = false;
                    }
                    if (kb.IsKeyDown(Keys.Enter))
                    {
                        Item itemBuy = shopInventory.items.ElementAt(currentItemSelect);
                        //displayText = itemBuy.itemName + 
                        int totalPrice = itemBuy.getBuyPrice() * quantityOf;
                        int endAmount = playerInventory.money - totalPrice;
                        if (endAmount < 0)
                        {
                            //Set display message to not enough money error

                        }
                        else if (endAmount > 0 && playerInventory.InventoryIsFull())
                        {
                            //Set display message to inventory full error

                        }
                        else
                        {
                            playerInventory.money = playerInventory.money - totalPrice;
                            playerInventory.AddToInventory(itemBuy, quantityOf);
                        }
                    }
                }
            }
            //Otherwise, it is in the error state, and the only thing the player can do is press enter to go back 
            else
            {
                if (kb.IsKeyDown(Keys.Enter))
                {
                    isErrorState = false;
                }
            }
        }
        #endregion

        #region Selling Items Region
        public void HandleSales(KeyboardState kb)
        {
            if (kb.IsKeyDown(Keys.W) && !isConfirm)
            {
                currentItemSelect--;
                if (currentItemSelect < 0)
                {
                    currentItemSelect = 0;
                }
            }
            if (kb.IsKeyDown(Keys.S) && !isConfirm)
            {
                currentItemSelect++;
                if (currentItemSelect > playerInventory.items.Count() - 1)
                {
                    currentItemSelect = playerInventory.items.Count - 1;
                }
            }

            if (kb.IsKeyDown(Keys.Enter))
            {
                isConfirm = true;
                quantityOf = 1;
            }

            if (isConfirm == true)
            {
                Item itemSell = playerInventory.items.ElementAt(currentItemSelect);
                if (kb.IsKeyDown(Keys.W))
                {
                    quantityOf++;
                    if (quantityOf > itemSell.quantity)
                    {
                        quantityOf = itemSell.quantity;
                    }
                }
                if (kb.IsKeyDown(Keys.S))
                {
                    quantityOf--;
                    if (quantityOf < 1)
                    {
                        quantityOf = 1;
                    }
                }
                if (kb.IsKeyDown(Keys.Back))
                {
                    isConfirm = false;
                }
                if (kb.IsKeyDown(Keys.Enter))
                {
                    //Selling logic -- since we handle item limitations, we can freely sell via integer quantity.
                    int sellPrice = itemSell.getSellPrice();
                    playerInventory.money = playerInventory.money + sellPrice;
                    playerInventory.ConsumeFromInventory(itemSell, quantityOf);
                }

            }
        }
        #endregion

        public void Draw(SpriteBatch sb)
        {

            // First want to draw the box underlays based on states. 
            // If the player ISNT buying OR selling, then it is Buy/Sell/Leave draw.

            // If it is buying OR selling, we draw the item selections, money
            // Then the position of the selection arrow
        }

        /*
         * TODO: 
         * Constructor : 
         * NPC (for type of shop_ , NPC's dialogue, Player, HUD (money) 
         * list of items of things to sell
         * 
         * probably would be good to create an item class detailing price of each item 
         * its effects and attributes and what not. item class can be like main class that will have
         * sub class of types of items like potions/weapons/armory/etc.
         * 
         * item should have two prices
         * value for buying
         * value for selling
         */

        /*method ot buy stuff
         * 
         * ** use dialogue class to get player options , call his method if player decides to buy
         *   if player selects object 
         *     check to see if player has enough money to purchase
         *       if enough, subt money & add item to player's inventory 
         *     else 
         *       display message saying not enough money to buy
         */

        /*
         * method to sel stuff
         * 
         * use dialogue class to get player options, call if playe dceids to sell 
         * 
         *   bring up inventory
         *   have player slect from list 
         *   if player confirms sell
         *      add $$ to hud according to items worth 
         *      remove item from inventory 
         * 
         */
    }
}
