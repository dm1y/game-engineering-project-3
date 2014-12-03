using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Project3
{
    public class BattleSystem
    {
        Display HUD;
        Player player;

        int playerHealth;

        bool endOfBattle;
        bool win;
        bool lose;


        int expGained;
        int levelGained;
        int moneyGained;

        List<Enemy> enemyList;
        List<Item> itemGained;

        /* Information needed: 
            Player : attack power, defense power, speed, item consumable 
            Enemy : need its health, attack power, defense power, speed, exp given
            Display : need to adjust/set level / exp / money if it improves */
        public BattleSystem(Player player, List<Enemy> list, Display display, int currentMap)
        {
            this.player = player;
            enemyList = list;
            this.HUD = display;

            expGained = 0;
            levelGained = 0;
            moneyGained = 0;
            itemGained = new List<Item>();

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
 
            // for testing purposes to try and get map/battle sys transitions. 
            //have no idea how this will work. 
            KeyboardState currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            if (currentKeyboardState.IsKeyDown(Keys.F))
            { 
                // go back to world somehow 
            }

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

        
        private void UseConsumable()
        {
            if (player.consumable != null)
            {
                playerHealth += player.consumable.heal;
                player.playerInventory.RemoveFromInventory(player.consumable);
                player.consumable = null;
            }
            else
            {
                // output message saying that there are no consumnable items to use 
            }
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
            foreach (Enemy e in enemyList)
            {
                if (player.speed > e.enemySpeed)
                {
                    // tweak after UX/UI: if e is equal to the enemy the player wants to attack 

                    // if player does not miss, enemy HP goes down 
                    if (IsSuccessful())
                    {
                        e.HP = e.HP - player.atk;
                    }
                    checkBattleWin(e);

                    // if enemy isn't dead 
                    if (e.HP != 0)
                    {
                        if (IsSuccessful())
                        {
                            playerHealth = playerHealth - e.Damage + player.def;
                        }
                    }

                    checkBattleLose(playerHealth);
                }
                else
                {
                    // enemy does damage first 
                    if (IsSuccessful())
                    {
                        playerHealth = playerHealth - e.Damage + player.def;
                    }

                    checkBattleLose(playerHealth);
                    // TWEAK HERE WHEN UI/UX IS DONE: if e is equal to the enemy the player wants to attack 

                    // if player does not miss, enemy HP goes down 
                    if (IsSuccessful())
                    {
                        e.HP = e.HP - player.atk;
                    }

                    checkBattleWin(e);
                }
            }
        }

        // parameter is either enemy or enemy list
        // if list, see below if statement 
        private void checkBattleWin(Enemy e)
        {
            if (e.HP == 0)
            {
                expGained += e.Experience;
                moneyGained += e.bounty;
                gainItemSpoils();
                enemyList.Remove(e);
            }

            if (enemyList.Count == 0)
            {
                transition();
                win = true;

                int total = expGained + HUD.experience;

                // Checks if it has reached above threshold 
                if (total > HUD.threshold)
                {
                    // Increases level 
                    HUD.increaseLevel();
                    levelGained = HUD.level;

                    // Increases threshold by 50
                    HUD.increaseThreshold();
                }

                // Sets experience 
                int remainder = total - HUD.threshold;
                HUD.setExperience(remainder);

                // Increases player money 
                player.playerInventory.money += moneyGained;
            }
                        
        }
        private void checkBattleLose(int health)
        {
            if (playerHealth == 0)
            {
                lose = true;
                transition();
            }
        }

        private void gainItemSpoils()
        {
            Random r = new Random();

            // If there is only one enemy 
            if (enemyList.Count == 1)
            {
                if (enemyList[0].EnemyItemsList == null || enemyList[0].EnemyItemsList.Count == 0)
                    return;
                else
                {
                    int random = r.Next(enemyList[0].EnemyItemsList.Count);
                    if (random != enemyList[0].EnemyItemsList.Count)
                    {
                        itemGained.Add(enemyList[0].EnemyItemsList[random]);
                        player.playerInventory.AddToInventory(enemyList[0].EnemyItemsList[random], 1);
                    }
                }
            }
            
            // If there are multiple 
            else
            {
                int rand = r.Next(enemyList.Count);
                if (enemyList[rand].EnemyItemsList == null || enemyList[rand].EnemyItemsList.Count == 0)
                    return;
                else
                {
                    int random = r.Next(enemyList[rand].EnemyItemsList.Count);
                    if (random != enemyList[rand].EnemyItemsList.Count)
                    {
                        itemGained.Add(enemyList[rand].EnemyItemsList[random]);
                        player.playerInventory.AddToInventory(enemyList[rand].EnemyItemsList[random], 1);
                    }
                }
            }

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
                // if (itemGained.Count > 0)
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


        public void Draw(SpriteBatch sb)
        {
        }
    }
}
