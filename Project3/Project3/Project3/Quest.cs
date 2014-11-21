using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Project3
{
    class Quest
    {
        /*
         * Constructor details:
         * 
         * Boolean determining type of quest (defeating enemies, retreiving items)
         * Classifying type of enemy -- default to null
         * Classifying type of items -- default to null
         * (Optional) Number of types of items/enemies if we want to make it more complex
         * Quantity of numbers needed to satisfy quest 
         *  ex: # of enemies defeated, # of items retrieved 
         *  use this quantity as a counter 
         * Boolean if quest is active 
         * Boolean if quest is in progress
         * Boolean if quest has been completed 
         * List of rewards for completion (determined by random gen unless we want to hardcode a specific reward per quest, then no list is needed)
         */
        public Quest(bool type, bool isActive, bool isInProgress, bool isCompleted)
        {

        }

        /*
         * 
         * Update method perhaps;
         * 
         * check if quest has been activated 
         * 
         * if not activated, don't do anything 
         * 
         * if activated: check to see if quest is in progress
         * 
         * if quest is in progress 
         *   // keep checking to see if counter is at 0 yet 
         *      // if at zero, display message saying to go back to NPC to complete quest 
         *         // when quest is completed 
         */

        public void Update(GameTime gametime)
        {
            //if quest has not been updated
            //do nothing
            //Otherwise, keep checking to see if counter is at 0

            //if zero is reached, display message saying to go back to NPC to finish quest
        }

        public void EnableQuest()
        {

        }
        /*
         * Method to activate/deactivate quest 
         * 
         * if player encounters the NPC
         *    bring up dialogue screen that displays choices 
         *    if quest has not been completed 
         *    *    if player chooses to accept quest 
         *    *       set boolean to active 
         *            set boolean to in progress
         *    *       make items visible / spawn enemies 
         *    else if quest has been completed
         *          if player chooses to finish quest 
         *              set boolean to inactive 
         *              make items not visible 
         *              give player reward (incorporated here or having a separate method depending on implementation)
         * 
         *    
         */

        public void DisableQuest()
        {

        }

        public void CheckForCompletion()
        {

        }
        /*
         * method checking to see if quest has been completed 
         * 
         * if player has defeated enemy / retrieved item 
         *    decrement counter 
         *    
         * notes: we can handle record keeping of defeated enemies by having a list in the class of enemies spawned and remove 
         * each enemy from the list or item from the list (so that way they won't be visible in the world either, and decrement this 
         * counter after each defeat. we'll need to incorporate a boolean parameter in battle sys class though, like 
         * if this is a quest battle, then check the counter that's being passed in and etc. ) 
         */

        public void Spawn()
        {

        }
        /*
         * method to spawn enemies / items 
         * 
         * ... just pretty much note which tiles in the map the enemies/items are located at. 
         */
    }
}