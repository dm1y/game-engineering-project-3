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
        World world;
        int playerHealth;

        bool endOfBattle;
        bool win;
        bool lose;
        Boolean canFight;
        Boolean inEnemySelect;
        Boolean inChoices;

        int expGained;
        int levelGained;
        int moneyGained;

        int displayCounter;

        List<Enemy> enemyList;
        List<Item> itemGained;

        List<string> combatHistory;

        public Boolean exitBattle;
        public int currentSelect;
        public int enemySelect;

        Texture2D background;
        Texture2D playersprite;
        Texture2D combattext;
        Texture2D options;
        Texture2D playerdisplay;
        Texture2D target;
        Texture2D enemydisplay;
        Texture2D arrow;

        /* Information needed: 
            Player : attack power, defense power, speed, item consumable 
            Enemy : need its health, attack power, defense power, speed, exp given
            Display : need to adjust/set level / exp / money if it improves */
        public BattleSystem(Player player, Display display, World world)
        {
            this.world = world;
            this.player = player;
            this.HUD = display;

            expGained = 0;
            levelGained = 0;
            moneyGained = 0;
            itemGained = new List<Item>();
            enemyList = new List<Enemy>();
            combatHistory = new List<string>();
            playerHealth = display.HP;
            endOfBattle = false;
            win = false;
            lose = false;
            displayCounter = 0;
            exitBattle = false;
            inChoices = false;
        }

        public void LoadTextures()
        {
            Game1 g = world.game;
            arrow = g.Content.Load<Texture2D>("Overlays/arrow select");
            background = g.Content.Load<Texture2D>("Battle/background");
            playersprite = g.Content.Load<Texture2D>("Battle/playerbattlesprite");
            combattext =  g.Content.Load<Texture2D>("Battle/combattext");
            options = g.Content.Load<Texture2D>("Battle/options");
            playerdisplay = g.Content.Load<Texture2D>("Battle/playerdisplay");
            target = g.Content.Load<Texture2D>("Battle/target");
            enemydisplay = g.Content.Load<Texture2D>("Battle/enemydisplay");
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

        public void GenerateBattle(List<Enemy> enemies)
        {
            playerHealth = HUD.HP;
            Random r = new Random();
            int num = r.Next(enemies.Count);
            Enemy e = enemies.ElementAt(num);
            enemyList.Add(e);

            combatHistory.Add("A " + e.enemyName + " appeared!");
            if (IsSuccessful())
            {
                num = r.Next(enemies.Count);
                e = enemies.ElementAt(num);
                enemyList.Add(e);
                combatHistory.Add("A " + e.enemyName + " appeared!");
            }
            if (IsSuccessful())
            {
                num = r.Next(enemies.Count);
                e = enemies.ElementAt(num);
                enemyList.Add(e);
                combatHistory.Add("A " + e.enemyName + " appeared!");
            }
            inChoices = false;
        }
        // Update method 
        public void Update(KeyboardState keyboard)
        {
            //If 'W' is down, we want to move the selection arrow up. IE, from "HEAL" to "FIGHT".
            if (keyboard.IsKeyDown(Keys.W) && !inEnemySelect && !exitBattle && inChoices)
            {
                currentSelect--;
                if (currentSelect < 0)
                {
                    currentSelect = 0;
                }
                return;
            }
            //Otherwise, if 'S' is down, we want to move the selection arrow down. IE, from "HEAL" to "RUN"
            else if (keyboard.IsKeyDown(Keys.S) && !inEnemySelect && !exitBattle && inChoices)
            {
                currentSelect++;
                if (currentSelect > 2)
                {
                    currentSelect = 2;
                }
                return;
            }
            //Else, if ENTER is down, we want to engage in whatever option was chosen.
            else if (keyboard.IsKeyDown(Keys.Enter) && !inEnemySelect && !exitBattle && inChoices)
            {
                //If it's 0, we want to fight. Enter enemy selection state.
                if (currentSelect == 0)
                {
                    inEnemySelect = true;
                    inChoices = false;
                }
                //If it's 1, we want to heal. Consume current healing item.
                if (currentSelect == 1)
                {
                    UseConsumable();
                    inChoices = false;
                }
                //If it's 2, we want to run. Try running.
                if (currentSelect == 2)
                {
                    Escape();
                    inChoices = false;
                }
            }
            //If we are in enemy selection, delegate control to SelectEnemies()
            else if (inEnemySelect)
            {
                SelectEnemies(keyboard);
            }

            //Otherwise, if not in any of the above states, we must have computed battle 
            else
            {
                //Assumes that combat calculation has been done
                if (keyboard.IsKeyDown(Keys.Enter) && !endOfBattle)
                {
                    if (displayCounter < combatHistory.Count)
                    {
                        displayCounter++;
                    }
                    else
                    {
                        combatHistory.Clear();
                        combatHistory.Add("What will you do?");
                        displayCounter = 1;
                        inChoices = true;
                    }
                }
                else if (keyboard.IsKeyDown(Keys.Enter) && endOfBattle)
                {
                    if (displayCounter < combatHistory.Count)
                    {
                        displayCounter++;
                    }
                    else
                    {
                        transition(keyboard);
                    }
                }
                //On every update, we check if the whole battle is over. If not, we continue as usual. 
                //Note that you can only get endOfBattle to TRUE under three conditions:
                //1. Kill all monsters
                //2. Player dies
                //3. Player runs

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

        private void SelectEnemies(KeyboardState kb)
        {
            if (kb.IsKeyDown(Keys.Back))
            {
                inEnemySelect = false;
                return;
            }

            if (kb.IsKeyDown(Keys.W))
            {
                if (enemySelect < 2)
                {
                    if (enemySelect != enemyList.Count - 1)
                    {
                        enemySelect++;
                    }
                }
            }
            else if (kb.IsKeyDown(Keys.S))
            {
                if (enemySelect > 1)
                {
                    enemySelect--;
                }
            }
            else if (kb.IsKeyDown(Keys.Enter))
            {
                inEnemySelect = false;
                Battle();
            }
        }
        private void UseConsumable()
        {
            if (player.consumable != null)
            {
                playerHealth += player.consumable.heal;
                player.playerInventory.RemoveFromInventory(player.consumable);
                player.consumable = null;
                canFight = false;
                combatHistory.Add("You used " + player.consumable.itemName + " and healed for " + player.consumable.heal); 
                Battle();
            }
            else
            {
                combatHistory.Clear();
                combatHistory.Add("You're out of healing items!");
                displayCounter = 1;

                // output message saying that there are no consumnable items to use 
            }
        }

        private void Escape()
        {
            if (IsSuccessful())
            {
                //William added this -- sets endOfBattle to true, player neither lost or won. 
                combatHistory.Clear();
                combatHistory.Add("You successfully escaped!");
                endOfBattle = true;
                lose = false;
                win = false;
            }
            else
            {
                combatHistory.Clear();
                combatHistory.Add("You failed to escape!");
                canFight = false;
            }
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
                    if (e.Equals(enemyList.ElementAt(enemySelect)) && canFight)
                    {
                        if (IsSuccessful())
                        {
                            combatHistory.Add("You hit the " + e.enemyName + " for " + player.atk);
                            e.HP = e.HP - player.atk;
                        }
                    }
                }
                else
                {
                    if (IsSuccessful())
                    {
                        int damage = e.Damage - player.def;
                        playerHealth = playerHealth - damage;
                    }

                    if (e.Equals(enemyList.ElementAt(enemySelect)) && canFight)
                    {
                        if (IsSuccessful())
                        {
                            combatHistory.Add("You hit the " + e.enemyName + " for " + player.atk);
                            e.HP = e.HP - player.atk;
                        }
                    }
                }
            }

            enemySelect = 0;
            checkOutcome();
        }

        private void checkOutcome()
        {
            if (playerHealth <= 0)
            {
                lose = true;
                endOfBattle = true;
            }
            else
            {
                foreach (Enemy e in enemyList)
                {
                    if (e.HP <= 0)
                    {
                        expGained += e.Experience;
                        moneyGained += e.bounty;
                        gainItemSpoils();
                        enemyList.Remove(e);
                    }

                    if (enemyList.Count == 0)
                    {
                        win = true;
                        endOfBattle = true;

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
            }
            inChoices = true;
        }

        // parameter is either enemy or enemy list
        // if list, see below if statement 
        private void checkBattleWin(Enemy e)
        {
            if (e.HP <= 0)
            {
                expGained += e.Experience;
                moneyGained += e.bounty;
                gainItemSpoils();
                enemyList.Remove(e);
            }

            if (enemyList.Count == 0)
            {
                win = true;
                endOfBattle = true;

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
            if (playerHealth <= 0)
            {
                lose = true;
                endOfBattle = true;
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


        private void transition(KeyboardState keyboard)
        {
            if (lose)
            {
                combatHistory.Clear();

                //Display's Player's loss
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

            else if (!lose && !win)
            {
                // only reaches here if player decides to escape and is successful
                expGained = 0;
                levelGained = 0;
                moneyGained = 0;

                // displays 0 exp gained message ?
            }

            if (keyboard.IsKeyDown(Keys.Enter))
            {
                exitBattle = true;
                //Now we can exit.
            }
        }

        public void ResetBattle()
        {
            enemyList.Clear();

            currentSelect = 0;
            enemySelect = 0;
            exitBattle = false;
            displayCounter = 0;
            combatHistory.Clear();
            endOfBattle = false;
            canFight = true;
            win = false;
            lose = false;

            expGained = 0;
            levelGained = 0;
            moneyGained = 0;
            itemGained.Clear();
        }

        public void Draw(SpriteBatch sb)
        {
            Vector2 origin = new Vector2(world.camera.Position.X / 2, world.camera.Position.Y / 2);

            //Background forest
            sb.Draw(background, origin, Color.White);
            //Combat History
            Vector2 combatpos = origin;
            sb.Draw(combattext, combatpos, Color.White);
            //Player Display
            Vector2 playerdisplaypos = origin + new Vector2(0, 128);
            sb.Draw(playerdisplay, playerdisplaypos, Color.White);
            //Options
            Vector2 optionpos = origin + new Vector2(416, 0);
            sb.Draw(options, optionpos, Color.White);
            //Enemy Name
            Vector2 enemynamepos = origin + new Vector2(64, 320);
            Vector2 enemydisplaypos = enemynamepos + new Vector2(192, 0);
            sb.Draw(enemydisplay, enemynamepos, Color.White);
            sb.Draw(enemydisplay, enemydisplaypos, Color.White);
            //Player Sprite
            Vector2 playerpos = origin + new Vector2(112, 224);
            sb.Draw(playersprite, playerpos, Color.White);

            #region Options Draw
            Vector2 optionTextPos = optionpos + new Vector2(18, 16);
            sb.DrawString(world.battleFont, "FIGHT", optionTextPos + new Vector2(0, 0), Color.White);
            sb.DrawString(world.battleFont, "HEAL", optionTextPos + new Vector2(0, 16), Color.White);
            sb.DrawString(world.battleFont, "RUN", optionTextPos + new Vector2(0, 32), Color.White);
            Vector2 optionSelPos = optionTextPos + new Vector2(-12, currentSelect * 16);
            sb.Draw(arrow, optionSelPos, Color.White);
            #endregion
            #region Enemy Draw
            //Drawing Enemies
            if (enemyList.Count >= 1)
            {
                Vector2 enemy1 = origin + new Vector2(256, 240);
                sb.Draw(enemyList.ElementAt(0).enemyTexture, enemy1, Color.White);
            }
            if (enemyList.Count >= 2)
            {
                Vector2 enemy2 = origin + new Vector2(320, 256);
                sb.Draw(enemyList.ElementAt(1).enemyTexture, enemy2, Color.White);
            }
            if (enemyList.Count >= 3)
            {
                Vector2 enemy3 = origin + new Vector2(384, 240);
                sb.Draw(enemyList.ElementAt(2).enemyTexture, enemy3, Color.White);
            }
            #endregion

            #region Combat Text Draw
            Vector2 combatTextPos = origin + new Vector2(16, 16);
            for (int i = 0; i < displayCounter; i++)
            {
                Vector2 textPos = combatTextPos + new Vector2(0, i*16);
                sb.DrawString(world.shopDialogueFont, combatHistory.ElementAt(i), textPos, Color.White);
            }
            #endregion
        }
    }
}
