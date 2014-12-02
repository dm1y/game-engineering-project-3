﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project3
{
    public class NPC
    {
        World world;

        Texture2D speechBubble;
        //Texture2D 
        /* NPC methods -
         General format
            Two types of NPCs - Random Dialogue(talk) or Merchant(implements shop)
            
         If it's just a dialogue npc, we just give it standard dialogue options and give it
         a dialogue to run with. 
         
         Otherwise, if it's a shop NPC, we simply give it the shop options and run with that.
         Will probably need to just have an "npc_generator"*/
        public Boolean isDialogueNPC;
        public Boolean isShopNPC;
        public Boolean isFinished;

        public Dialogue dialogue;
        public Shop shop;
        public NPC(Dialogue dialogue, World world)
        {
            this.world = world;
            this.dialogue = dialogue;
            isDialogueNPC = true;
            isShopNPC = false;
            isFinished = false;
            speechBubble = world.game.Content.Load<Texture2D>("Overlays/speech_maindialogue_416x96");
        }

        public NPC(Shop shop, World world)
        {
            this.world = world;
            this.shop = shop;
            isShopNPC = true;
            isDialogueNPC = false;
            isFinished = false;
            shop.LoadTextures(world);
            shop.PlayerShop(world.player.playerInventory);
        }

        public void Update(KeyboardState keyboard)
        {
            if (isShopNPC)
            {
                if (shop.isFinished)
                {
                    isFinished = true;
                    shop.ResetShop();
                    //set this npc to finish, player gets it, turns interaction w/ npc off, npc isFinished goes back to false
                }
                else
                {
                    shop.Update(keyboard);
                }
            }
            else if (isDialogueNPC)
            {
                //if (keyboard.IsKeyUp(Keys.Enter) && dialogue.isFinished() && !dialogue.canContinue)
                //{
                //    dialogue.canContinue = true;
                //    canGo = true;
                //}
                //if (keyboard.IsKeyDown(Keys.Enter) && dialogue.isFinished() && canGo)
                //{
                //    dialogue.ResetDialogue();
                //    isFinished = true;
                //    canGo = false;
                //}
                //set this one to finished, player gets the message, turns it's finished on, then turns this one back off

                //if the dialogue is finished and enter is UP, then we have just hit enter onto the last line. make it possible
                //to continue.
                //if enter is DOWN and we can go, we will reset the dialogue, set isFinished = true, and turn canGo off
                if (keyboard.IsKeyDown(Keys.Enter) && dialogue.isFinished())
                {
                    dialogue.ResetDialogue();
                    isFinished = true;
                }
                else
                {
                    //update
                    if (keyboard.IsKeyDown(Keys.Enter))
                    {
                        dialogue.Update(keyboard);
                    }
                }

            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (isDialogueNPC)
            {
                Vector2 position = new Vector2(dialogue.world.camera.Position.X / 2 + 32, dialogue.world.camera.Position.Y / 2);
                sb.Draw(speechBubble, position, Color.White);
                dialogue.Draw(sb);
            }
            else if (isShopNPC)
            {
                shop.Draw(sb);
            }
        }
    }
}
