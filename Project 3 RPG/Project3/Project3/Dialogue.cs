using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project3
{
    public class Dialogue
    {
        /* Idea for General Dialogue Class */
        /* We read from the array of text just like a script. Index 0 
         would be the first line that gets written/displayed, and following that,
         if this dialogue instance receives player input, then it advances the dialogue. 
         May want to consider different versions of implementation.
         
         * GENERAL CONCEPT * 
        */

        // The array of dialogue that will be used.
        public List<String> text;
        // The current line the dialogue is on.
        public int currentLine;
        // May be used to set markers for where we want to pick up/leave off an NPC's dialogue. Optional(normal NPCs wouldn't use this, but questgivers would)
        // Consider this an ADVANCED CONCEPT. May not be even used in the final. 
        public int[] dialoguePoints;
        SpriteFont font;
        public Boolean canContinue = false;

        public Dialogue(SpriteFont font)
        {
            this.font = font;
        }

        public Dialogue(World world)
        {
            font = world.game.Content.Load<SpriteFont>("DialogueFont");
        }

        public Dialogue(List<String> text, SpriteFont font)
        {
            this.font = font;
            this.text = text;
        }

        public void SetDialogue(List<String> lines)
        {
            this.text = lines;
        }
        public void AdvanceLine()
        {
            currentLine++;
            if (currentLine == text.Count)
            {
                currentLine = text.Count - 1;
            }
        }

        public void ResetDialogue()
        {
            currentLine = 0;
        }

        public void AddLine(String line)
        {
            text.Add(line);
        }

        /* When player interacts with an interactable block, player will enter the isDialogue state. The player can only break out of it 
         if they are at the end of the dialogue option with that NPC/whatever. 
         
         Logic: 
         Player press enter in front of a interactable maptile, will check if maptile is interactable
         If maptile is interactable, load the dialogue up(assumes all interactable maptiles will have dialogue options)
         */
        public void Update(KeyboardState keyboard)
        {
            if (keyboard.IsKeyUp(Keys.Enter))
            {
                canContinue = true;
            }
            if (keyboard.IsKeyDown(Keys.Enter) && canContinue)
            {
                if (!isFinished())
                {
                    AdvanceLine();
                }
                canContinue = false;
            }
        }

        public Boolean isFinished()
        {
            if (currentLine >= text.Count - 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Draw(SpriteBatch sb)
        {
            Vector2 position = new Vector2(0, 0);//still figuring where to draw this guy//
            sb.DrawString(font, text.ElementAt(currentLine), position, Color.White);
        }

        /* ADVANCED CONCEPT -- MAY NOT HAVE TIME TO DO THIS! *
         For instance, NPCs will only ever advance the dialogue to a certain extent, 
         but questgivers may only go up to the pre-quest line.
         
         Here's an example of a sample interaction:
            0. *player interacts with QG* 
            1. Questgiver(QG) - "I need help!" //opens dialogue box 
            2. *player presses enter*
            3. QG - "I lost my precious axe out in the woods. Could you get it for me?" //advances dialogue index
            4. *player presses no* //resets dialogue index, closes dialogue box
            5. *player interacts with QG again*
            6. Questgiver(QG) - "I need help!"  //opens dialogue box
            7. *player presses enter*
            8. QG - "I lost my precious axe out in the woods. Could you get it for me?" //advances dialogue index
            9. *player presses yes*
            10. QG - "Thank you!" //dialogue box closes, sets dialogue index to get-quest-but-not-finished
            11. *player interacts with npc without getting axe*
            12. QG - "Have you found my dear axe yet?" //opens dialogue box
            13. *player presses enter* //dialogue box automatically closes because player doesn't have the item
            */
    }
}
