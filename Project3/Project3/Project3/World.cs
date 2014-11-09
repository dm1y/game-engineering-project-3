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
    class World
    {
        public Game1 game;
        //Two dimensional array of maptiles, so that we only have to check certain areas around the player

        /* (Goal for 11/12/2014)
         *  Have player moving around town and able to cross into transition zones, which will
         *  take the player to a new map. Since we're using a 2D data structure and the most number 
         *  of blocks we'll be using is in the thousands, the system should be pretty straightforward
         *  and efficient. 
         *  
         *  Implementation Ideas 
         *  We first load up all the maptiles with a texture(I added a 32x32 texture called 'grass' into the content)
         *  and assign them positions via coordinate system. The maptiles will handle the math and relative position to 
         *  be drawn in(Already done!) For instance, if a maptile is created with coordinates (1, 3), then it will be at 
         *  (32, 96) in real pixel coordinates. 
         *  
         *  William - Implementing how player will move around on the tiles. Player should only move roughly about
         *            one block at a time(Pokemon style). Player's current direction should also be kept track of,
         *            since during NPC checking, we only want the player to be able to interact with an NPC if they're 
         *            facing it! Additionally, need to make sure that player is unable to move any other direction
         *            mid-movement, or else player will end up out of bounds of the block collisions.
         *            
         *  Colin - Create three maps for the time being -- one is the "town" that the player spawns at. 
         *          Then create the two other transition zones -- the red transition blocks(the texture is "transition_red")
         *          go to a zone on the right side of town. The blue blocks(name is "transition_blue") take the player to an area south
         *          of town. Each zone should be able to transition back to the zone that the player just came from.
         *          
         *          We can probably do this by giving maptiles a "isTransition" variable, which the player checks during collision
         *          correction. We can give that block a transition state/path integer, which tells us where the player will load to
         *          when they step on that block. The player will then probably call a method from the world that will switch up the
         *          blocks to be loaded.
         *          
         *          Current suggestion is for TOWN to be 1, RED TRANSITION to be 2, and BLUE TRANSITION to be 3. 
         *          
         *          Note that we probably want to spawn the player 2-3 blocks away from the transition blocks so that they dont get stuck
         *          in a loop(ie, player steps on transition red, lands on transition reds in zone 2, teleports back, and gets stuck in an
         *          infinite loop)
         *          
         */
        public Maptile[,] currentMap;
        public Maptile[,] mapTwo;
        public Maptile[,] mapThree;   //Not being used yet.
        Texture2D grassText;
        Texture2D redTransition;
        Texture2D blueTransition;
        int size = 50;
        Player player;

        public World(Game1 game)
        {
            this.game = game;
        }

        //WORK IN PROGRESS
        public void LoadContent(ContentManager Content)
        {
            
            
            currentMap = new Maptile[size, size];
            mapTwo = new Maptile[size, size];
            mapThree = new Maptile[size, size];
            grassText = game.Content.Load<Texture2D>("grass");
            redTransition = game.Content.Load<Texture2D>("transition_red");
            blueTransition = game.Content.Load<Texture2D>("transition_blue");
            LoadMap(2);

            //for (int x = 0; x < size; x++)
            //{
            //    for (int y = 0; y < size; y++)
            //    {
            //        if (x > size-32)
            //        {
            //            newTile = new Maptile(redTransition, new Vector2(x, y));
            //            currentMap[x, y] = newTile;
            //        }
            //        else if (y > size - 32)
            //        {
            //            newTile = new Maptile(blueTransition,new Vector2(x,y));
            //            currentMap[x, y] = newTile;
            //        }
            //        else
            //        {
            //            newTile = new Maptile(grassText, new Vector2(x, y));
            //            currentMap[x, y] = newTile;
            //        }
                    
               // }
                
            //}
        }
        
        //default sets to 1
        public void LoadMap(int path = 1)
        {
            Maptile newTile;
            Maptile [,] tempMap = new Maptile[size,size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
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
                            newTile = new Maptile(grassText,new Vector2(x, y));
                            tempMap[x, y] = newTile;
                            break;
                    }
                    currentMap = tempMap;
                }
            }
           
        }

        public void Update(GameTime gametime)
        {

        }
        public void Draw(SpriteBatch sb)
        {


            sb.Begin();
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    
                    currentMap[x, y].Draw(sb);
                }
            }
           
            sb.End();

        }
    }
}
