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
using System.IO;

namespace Project3
{
    public class Map
    {
        Game1 game;
        World world;
        
        /* Size of each map */
        public int width {get; private set;}
        public int height { get; private set; }

        /* Map Textures */
        Texture2D grassText;
        Texture2D redTransition;
        Texture2D blueTransition;

        public Maptile[,] currentMap;

        public Map(Game1 g, World w)
        {
            game = g;
            world = w;
        }

        public void LoadContent(ContentManager Content)
        {
            grassText = game.Content.Load<Texture2D>("MapTexture/grass");
            redTransition = game.Content.Load<Texture2D>("MapTexture/transition_red");
            blueTransition = game.Content.Load<Texture2D>("MapTexture/transition_blue");

        }

        public void LoadMap(int i)
        {
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(TitleContainer.OpenStream("Content/MapTexture/test" + i + ".txt")))
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    lines.Add(line);
                }
            }
        }
/*        * use the line.Count information to initialize the size of the grid 
         * 
         * use colin's nested for loop here, 
         * but when reaching the switch statement, create a new tile based on the input of the ascii . 
         * 
         * also redo some logic for map tile that has "subclasses"/different types of tiles created
         * 
         * And this should populate the grid. 
         * 
         * Transitions will still remain the world class.
         * 
         * 
            }*/

        /*
 *  TODO: 
 *  Make the map data driven. 
 *  
 */
        //default sets to 1
        public void LoadMap(int path = 1)
        {
            Maptile newTile;
            Maptile[,] tempMap = new Maptile[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //                    if (x % 5 == 0)
                    //                        path = 2; 

                    /* TODO 1: Make the map data driven by creating either an xml or ascii based level 
                     * info loader thing */
                    /* TODO 2: Instead of having the switch be path, set it to whatever the input file is
                     *  to determine which tile to create and add to the tempMap */
                    /* TODO 3: In the player class, check the collisions by seeing which tile the player is 
                     currently interacting with. If it is a transition tile, transitions to the next map. 
                     If it's an NPC , interact with NPC, and etc. If it's an enemy, transition to
                     battle system. */

                    switch (path)
                    {
                        case 1:
                            newTile = new Maptile(grassText, new Vector2(x, y));
                            tempMap[x, y] = newTile;

                            break;
                        case 2:
                            newTile = new Maptile(redTransition, new Vector2(x, y));
                            tempMap[x, y] = newTile;
                            break;
                        case 3:
                            newTile = new Maptile(blueTransition, new Vector2(x, y));
                            tempMap[x, y] = newTile;
                            break;
                        default:
                            newTile = new Maptile(grassText, new Vector2(x, y));
                            tempMap[x, y] = newTile;
                            break;
                    }
                    currentMap = tempMap;
                    path = 1;
                }
            }

        }
    }
}