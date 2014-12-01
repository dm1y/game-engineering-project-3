﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Project3
{
    public class ObjectLoader
    {
        /* Not the best for encapsulation, but works. Loads up the items that exist in the game and stores references.
         * Will load basic gear for player(sword, shield, 3 potions) and basic money
         Will then load enemy drop table, shopkeeper's inventory.
         */
        public ObjectLoader()
        {
        }

        public void Initialize(World world)
        {
            List<string> lines = new List<string>();

            #region Player Load
            /* Loads player inventory */
            Inventory player_inv;
            List<Item> player_items = new List<Item>();

            //[Texture Path] [Item Name] [Price] [Effect Int] [Shield?] [Potion?] [Weapon?]
            Stream stream_player = TitleContainer.OpenStream("Content/Items/playerinventory.txt");
            using (StreamReader reader = new StreamReader(stream_player))
            {
                string line = reader.ReadLine();
                while (line != null)
                {

                    string[] p = line.Split(' ');
                    Texture2D texture = world.game.Content.Load<Texture2D>(p[0]);
                    int price = int.Parse(p[2]);
                    int effect = int.Parse(p[3]);
                    Boolean shield = Boolean.Parse(p[4]);
                    Boolean potion = Boolean.Parse(p[5]);
                    Boolean weapon = Boolean.Parse(p[6]);
                    Item currItem = new Item(texture, p[1], price, effect, shield, potion, weapon);
                    player_items.Add(currItem);

                    line = reader.ReadLine();
                }

                player_inv = new Inventory(player_items);
                player_inv.money = 5;
                world.player.playerInventory = player_inv;
            }
            stream_player.Close();
            #endregion

            #region Enemy Loot Load
            /* Loads enemy loot table*/
            //Inventory enemy_inv;
            List<Item> enemy_loot = new List<Item>();
            Stream stream_loot = TitleContainer.OpenStream("Content/Items/enemyloot.txt");
            using (StreamReader reader = new StreamReader(stream_loot))
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    string[] p = line.Split(' ');
                    Texture2D texture = world.game.Content.Load<Texture2D>(p[0]);
                    int price = int.Parse(p[2]);
                    int effect = int.Parse(p[3]);
                    Boolean shield = Boolean.Parse(p[4]);
                    Boolean potion = Boolean.Parse(p[5]);
                    Boolean weapon = Boolean.Parse(p[6]);
                    Item currItem = new Item(texture, p[1], price, effect, shield, potion, weapon);
                    enemy_loot.Add(currItem);

                    line = reader.ReadLine();
                }
            }
            stream_loot.Close();
            #endregion

            #region Load Merchant
            /* Loads merchant inventory & places it on the map*/
            Inventory merch_inv;
            List<Item> merch_items = new List<Item>();
            Stream stream_merchant = TitleContainer.OpenStream("Content/Items/merchant.txt");
            using (StreamReader reader = new StreamReader(stream_merchant))
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    string[] p = line.Split(' ');
                    Texture2D texture = world.game.Content.Load<Texture2D>(p[0]);
                    int price = int.Parse(p[2]);
                    int effect = int.Parse(p[3]);
                    Boolean shield = Boolean.Parse(p[4]);
                    Boolean potion = Boolean.Parse(p[5]);
                    Boolean weapon = Boolean.Parse(p[6]);
                    Item currItem = new Item(texture, p[1], price, effect, shield, potion, weapon);
                    merch_items.Add(currItem);

                    line = reader.ReadLine();
                }

                merch_inv = new Inventory(merch_items);
            }
            Shop merchant_shop = new Shop(merch_inv);
            NPC merchant = new NPC(merchant_shop);

            Texture2D merch_skin = world.game.Content.Load<Texture2D>("Items/merchant");
            world.currentMap.currentMap[6, 6].isInteract = true;
            world.currentMap.currentMap[6, 6].npc = merchant;
            world.currentMap.currentMap[6, 6].npcTexture = merch_skin;
            world.currentMap.currentMap[6, 6].isCollidable = true;
            stream_merchant.Close();

            #endregion

            #region Load NPCs
            /* Loads NPCs */
            /* Format: 
             NPC Coordinate - up to 4 chars. anything over len 4 is a line.
             Number of lines
                Lines(corresponding to # of lines)
             Repeat */

            List<String> npc_lines = new List<String>();
            Dialogue npc_dialogue;
            Vector2 coord = new Vector2(0, 0);
            int num = 0;

            Stream stream_npc = TitleContainer.OpenStream("Content/Items/npcs.txt");
            using (StreamReader reader = new StreamReader(stream_npc))
            {
                Boolean isCoord = true;
                string line = reader.ReadLine();
                while (line != null)
                {
                    // First line will be coords, followed by length of dialogue.
                    if (isCoord)
                    {
                        npc_lines = new List<String>();
                        isCoord = false;
                        string[] m = line.Split(',');
                        int x = int.Parse(m[0]);
                        int y = int.Parse(m[1]);
                        coord = new Vector2(x, y);
                        line = reader.ReadLine();
                        num = int.Parse(line);
                    }
                    //if not isCoord, then we are reading lines
                    if (!isCoord)
                    {
                        for (int i = 0; i < num; i++)
                        {
                            line = reader.ReadLine();
                            npc_lines.Add(line);
                        }
                        isCoord = true;
                        npc_dialogue = new Dialogue(world);
                        npc_dialogue.SetDialogue(npc_lines);
                        NPC townsperson = new NPC(npc_dialogue);

                        world.currentMap.currentMap[(int)coord.X, (int)coord.Y].isInteract = true;
                        world.currentMap.currentMap[(int)coord.X, (int)coord.Y].npc = townsperson;
                        Texture2D npc_skin = world.game.Content.Load<Texture2D>("Items/npc");
                        world.currentMap.currentMap[(int)coord.X, (int)coord.Y].isCollidable = true;
                        world.currentMap.currentMap[(int)coord.X, (int)coord.Y].npcTexture = npc_skin;

                        line = reader.ReadLine();
                    }
                }
            }
            stream_npc.Close();
            #endregion

        }
    }
}
