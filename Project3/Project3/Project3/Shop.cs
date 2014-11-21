using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project3
{
    class Shop
    {

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
