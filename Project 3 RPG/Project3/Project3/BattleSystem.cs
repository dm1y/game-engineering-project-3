using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project3
{
    public class BattleSystem
    {
        Display display;
        Player player;
        Enemy enemy;

        int playerHealth;

        /* Information needed: 
            Player : attack power, defense power, speed, item consumable 
            Enemy : need its health, attack power, defense power, speed, exp given
            Display : need to adjust/set level / exp / money if it improves */
        public BattleSystem(Player player, Enemy enemy, Display display)
        {
            this.player = player;
            this.enemy = enemy;
            this.display = display;

            playerHealth = display.HP;
        }

        /*
         * TODO: [[Front end]]  
         * implements the scrolling text dialogue stuff. 
         * 
         * Basically we'll use the logic of the menu we did in class for this part. 
         * This will display the options the user can choose from 
         * ** Attack -> transition to option of which enemy to attack
         * ** Defend 
         * ** Consumable -> will disappear if player is only allowed to use one consumable, else chooses from list 
         * ** Escape 
         */

        // Random generator. 
        // Might need to use a seed as a parameter 
        public Boolean IsSuccessful() 
        {
            Random r = new Random();
            int num = r.Next(99);

            if (num % 2 == 0)
                return true;
            else
                return false;
        }

        public void Escape()
        {
            if (IsSuccessful())
            {
                // end battle 
                // "escape successful"
                // transition to end screen displaying 0 exp/money rewarded and no items gained 
            } 
            // else, does nothing, makes no impact on battle except maybe display a message 
            // saying escape was unsuccessful
        }

        /*
         * Method that actually subtracts the health and whatnot 
         * Parameters it will change into: Player, Enemy
         */
        public void Battle()
        {
            // Player needs a default speed. 

            // if player speed is greater than enemy speed 
            // player attacks first 
                if (IsSuccessful())
                {
                    // currently there is one enemy so we just use enemy 
                    enemy.HP = enemy.HP; // - player.atk + enemy.def; 

                    // else we would use this instead 
                    // for (Enemy e : enemyList) 
                    //     if e == enemy selected by player 
                            // enemy.HP -= player.atk 
                }

                // use a for loop like the above if we use enmylist instead of enemy 
                // now enemy attacks 
                if (IsSuccessful())
                {
                    playerHealth = playerHealth; //- currenemy.atk + player.def 
                }
            // else, do the same above but reverse the order so enemy attacks first 
        }

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
