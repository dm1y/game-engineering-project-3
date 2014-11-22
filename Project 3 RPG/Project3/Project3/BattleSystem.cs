using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project3
{
    class BattleSystem
    {
        /*
         * TODO (before we get into this): Create the enemy class. 
         * Possibly create the HUD class / Weapon / Armory class (we can just hardcode for now?)
         * 
         * TODO: 
         * In the constructor, pass in the Player and the Enemy so we can get the following attributes:
         * ** Health of player 
         * ** Attack of player
         * ** Defense of player 
         * ** Optional: consumables 
         * ** Current experience, level
         * ** List of techniques player has so player can select which attack to use
         * 
         * ** Health of enemy 
         * ** Attack of enemy 
         * ** Enemy resistance/defense 
         * ** Experience gained 
         * ** List of techniques enemy has (randomized attacks depending on complexity of enemy)
         * ** Number of enemies
         */

        /*
         * TODO: 
         * This method will implement the scrolling text. 
         * 
         * Basically we'll use the logic of the menu we did in class for this part. 
         * This will display the options the user can choose from 
         * ** Attack -> Techniques
         * ** Defend 
         * ** Consumable
         * ** Escape 
         * ** Optional: Special moves 
         * 
         * Depending on what course of action the player takes, the enemy will respond in kind 
         * using a random generator. 
         * 
         * This method will also incorporate the following helper methods which are seen below.
         * 
         */

        /*
         * TODO: 
         * Method that randomizes enemy attacks
         * Parameters: Enemy, possibly a range for the random generator but that can be 
         *              incorporated into the enemy information
         * 
         * Implement a random generator using Random r = new Random(); 
         * Depending on how we want the likelyhood of each attack/miss to be, we can change the range
         * For now we can just use mod. If whatever number r is divisible by 2, attack -- else, miss. 
         * 
         * if r is divisble by 2
         *    go through the list randomly, choose an attack and go to method below
         * else 
         *    display a message saying the enemy has missed
         * 
         */

        /*
         * TODO: 
         * Method that actually subtracts the health and whatnot 
         * Parameters: Player, Enemy
         * 
         * if player is attacking 
         *    if player did not miss
         *       subtract enemy health by player attack points - enemy defense/resistance
         * 
         * if enemy is attacking 
         *    if enemy did not miss
         *       subtract player health by enemy attack points - player shield 
         * 
         */

        /*
         * TODO: 
         * Method for item drops / spoils after defeating enemy 
         * Parameter: Enemy 
         * ** Assumes enemy has a list of dropped items incorporated within its class
         * 
         * Have a random generator set up 
         * 
         * if enemy health is at 0
         *   Add exp to player 
         *   check if player levels up, 
         *      if player levels up, increase attack points and defense 
         *   Randomly pick an item from the item drop list and return it 
         */

        /**
         * TODO: 
         * Method for escape option 
         * Returns a boolean 
         * 
         * Use a random generator
         * Depending on range.
         * 
         * If  r % 2 == 0
         *  return true -- escape successful
         * else 
         *  return false 
         * /

        /*
         * TODO 
         * Method for transition screen 
         * Should be placed after enemy health is at 0 in update method...
         * can actually be combined with method above depending on how we want to implement transition screen 
         * 
         * has similar logic to menu screen 
         * display spoils, then transitions back to map 
         * transitioning back to map is similar to transition found in world
         * 
         * we need to figure out logic in how to have a delay time in the update so there will be a flowy transition 
         * 
         */
    }
}
