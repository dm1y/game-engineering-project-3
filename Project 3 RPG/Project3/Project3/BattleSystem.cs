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

        bool endOfBattle;
        bool win;
        bool lose;

        Item itemGained;
        int expGained;
        int levelGained;
        int moneyGained;

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
            endOfBattle = false;
            win = false;
            lose = false;
        }

        /*
         * TODO: [[Front end]] implements the scrolling text dialogue stuff. 
         * 
         * Basically we'll use the logic of the menu we did in class for this part. 
         * This will display the options the user can choose from 
         * ** Attack -> transition to option of which enemy to attack
         * ** Defend 
         * ** Consumable -> will disappear if player is only allowed to use one consumable, else chooses from list 
         * ** Escape 
         * 
         * logic of battle sys is found below, most still in pseudo code form since player/display/enemy classes 
         * do not have those added yet (will add soon after laying out all the logic so its easier to implement)
         */

        // Update method 
        public void Update()
        {
            // call backend methods after getting user selection implemented 

            if (endOfBattle)
            {
                transition();
            }
        }

        // Random generator. 
        // Might need to use a seed as a parameter 
        private Boolean IsSuccessful() 
        {
            Random r = new Random();
            int num = r.Next(99);

            if (num % 2 == 0)
                return true;
            else
                return false;
        }

        private void Escape()
        {
            if (IsSuccessful())
            {
                // end battle 
                // output "escape successful"
                transition();
            } 
            // else, does nothing, makes no impact on battle except maybe display a message 
            // saying escape was unsuccessful
        }

        /*
         * Method that actually subtracts the health and whatnot 
         * Parameters might change into: Player, Enemy
         */
        private void Battle()
        {
            // Note: player needs a default speed. 

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
                    playerHealth = playerHealth - enemy.Damage; //+ player.def 
                }
            // else, do the same above but reverse the order so enemy attacks first 
        }

        // parameter is either enemy or enemy list
        // if list, see below if statement 
        private void checkBattleWin(Enemy e)
        {
            if (e.HP == 0)
            {
                win = true;
                // player wins so we have to recalculate player stats
                expGained += e.Experience;

                // if display.exp reaches % exp.threshold == 0
                    // levelGained = display.Level++;
                    // increase level and reset exp and increase threshold in Display class 
                    // so something like setLevel() (basically an incrementor)
                    //                   setExp() - (adds it if threshold isn't reached, sets to 0 if threshold reached and adds on remainder)
                    //                   setThreshold() will be needed in display class to do these calculations and save them
                    // note: threshold will not be viewable to users 
                // increase display.money by monster worth (e.Worth)
                    // moneyGained = e.Worth;
                    // add this to current player money using setMoney() so it stays in Display class 

                // call gainItemSpoils(e)  

                transition();
            }
            
            // if its a list of enemies, duplicate a list in the initalization and call that checkEnemyList
            // iterate through enemy list, checking HP health, if it's 0, add exp then from remove it 
            // so basically for (enemy e in enemylist) 
            //                      if (e.HP == 0)
           //                            expGained += e.exp
            //                           remove(e)
            // outside for loop, have an if statement. if checkEnemyList is empty, then do the win logic found inside if (e.HP = 0). 
            
        }
        private void checkBattleLose(int health)
        {
            if (playerHealth == 0)
            {
                lose = true;
                transition();
            }
        }

        private void gainItemSpoils(Enemy e)
        { 
           // assumes enemy will have a list of items that can be dropped 
           // Random r = new Random();
           // int num = r.Next(list size - 1)
           
           // get the num index of the list of item 
           // set it equal to itemGained. 
           // add to player inventory 
        }

        private void transition()
        {
            if (lose)
            {
                // really depends on our game play / how we want to handle this 
            }
            else if (win)
            {
                // what will be displayed in the transition: gains 
                // so if it's only experienced gained, display that 
                // if level, show a level up
                // if item, do that.. or all three? 
               
                // so code wise:
                // if (expGained > 0)
                    // display it 
                // if (levelGained > 0)
                    // display it 
                // if (moneyGained > 0)
                    // display it 
                // if (itemGained != null)
                    // display it 

                // hitting enter or spacebar to go back to map (?)
                // insert function to transition back to map here 
            }
            else 
            {
                // only reaches here if player decides to escape and is successful
                expGained = 0;
                levelGained = 0;
                moneyGained = 0;
                itemGained = null;

                // displays 0 exp gained message ?
            }
        }

    }
}
