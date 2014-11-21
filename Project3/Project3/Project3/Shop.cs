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

        String displayText;
        int quantityOf;

        /* Possible states: 
            0 - Arrow is hovering over BUY
            1 - Arrow is hovering over SELL
            2 - Arrow is hovering over LEAVE*/
        int currentState;

        public Shop(Inventory shopInventory)
        {
            this.shopInventory = shopInventory;
            isBuying = false;
            isSelling = false;
        }


        public void PlayerShop(Inventory playerInventory)
        {
            this.playerInventory = playerInventory;
            currentState = 0;
        }

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

            //If the player hits enter during selection.
            if (keyboard.IsKeyDown(Keys.Enter))
            {
                if (currentState == 2)
                {
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

            //If the player hits back during buy/sell, exit out of it.
            if (keyboard.IsKeyDown(Keys.Back))
            {
                if (isSelling)
                {
                    isSelling = false;
                }
                else if (isBuying)
                {
                    isBuying = false;
                }
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

        public void HandlePurchases(KeyboardState kb)
        {

        }

        public void HandleSales(KeyboardState kb)
        {

        }


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
