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
        public Camera camera;

        /* TODO: Create boundaries variable stating the map size 
         * 
         * Legend: Game Window is 960 x 640 
         * Each tile window is a 30 x 20. 
         * 
         * So Map Size of 60 x 60 is 960*2 by 640*3. 
         * That way we can keep track of player movements. 
         */

        /*
         * HIERARCHY NOTES: 
         * 
         * Game1 -> Menu Screen 
         * Menu Screen -> Transitions to World 
         * World creates Player / Enemies / Maps / Quests based on XML stuff 
         * **(Diana shall do data driven stuff soon I swear -- BY THIS WEEKEND PLS)
         * Camera is independent for the most part but relies on position of player 
         * 
         * TODO: Create an Enemy class
         * In world, create a List of enemies for each different map 
         *  so when player encounters a tile that is an enemy tile (random generator implemented to determine if enemy is encountered or not)
         *  go through list of enemies, randomly pick one, and pass it into battle system 
         *  
         * ** Alternative method, pass in the list of enemies to the battle system class instead and pick enemy randomly from there 
         *  ** Might be better to do list so we can determine quantity and variety. 
         *  ** constructor changes of enemy will be adding a quantity of enemies (1 for different types, 2 for total num of enemies)
         * ** details on enemies should be in battle sys clss
         *
         * BattleSystem(player, enemy) -> Dialogue class
         * TODO: Implement Battle System Class 
         * ** see battle sys class for details 
         *       ---- HUD CLASS --- 
         *          Note that we should reset the player's health back to max after a battle!
         * 
         * TODO: Implement Quest class 
         * Create a List of Quests in World (relies on map details + data driven stuff) 
         * ** see quest class for details 
         * ** if quest[i] is active/inprogress, set items to visible 
         * ** if quest[i] is inactive/completed, set items to not visible
         * ** is visible/not visible can be handled multiple ways, easier way can be found in quest class. 
         * depends what ya'll want to do to implement
         * 
         * TODO: Dialogue screen (logic similar to menu screen done in class)
         * 
         * TODO: NPC class 
         *      would probably have a type of NPC , it's own dialogue class, it's own shop thing 
         * 
         * NPC -> Shop 
         * NPC -> Dialogue 
         * TODO: Shop/Item class  : parameters (details found in shop class)
         *          Dialogue class displays sell, shop, leave (maybe have a talk option) 
         *          List of items to sell with prices 
         *          details for items class found in shop class.
         *          
         * TODO: Possibly an inventory class or we can just keep an arraylist of items the player has. 
         *       need to somehow create some kind of overlay to display this information as well as a keystroke
         * 
         * HEADS UP DISPLAY - 
         *          Idea - Display $$$, exp, lvl
         *          Near bottom right - display three boxes - Weapon, Shield, Potion. Pressing 1, 2, or 3 will
         *                              switch up the weapon/shield/potion by iterating through player's inventory
         */

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

        //Player Textures//
        Texture2D playerup;
        Texture2D playerdown;
        Texture2D playerleft;
        Texture2D playerright;

        //Tile Textures
        Texture2D grassText;
        Texture2D redTransition;
        Texture2D blueTransition;

        //// ---- MOVEMENT ---- //
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        int size = 50;
        Player player;

        public World(Game1 game, Camera c)
        {
            this.game = game;
            camera = c;
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

            playerleft = game.Content.Load<Texture2D>("playerleft");
            playerright = game.Content.Load<Texture2D>("playerright");
            playerup = game.Content.Load<Texture2D>("playerup");
            playerdown = game.Content.Load<Texture2D>("playerdown");

            LoadMap(1);

            player = new Player(playerup, playerdown, playerleft, playerright, new Vector2(10, 10), currentMap, this);

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

        /* This method can be called from the player class using the world instance.
         * TransitionMap (path depending on object interacted with). 
         * 
         * Refer to Player class -> CheckTile for example. 
         */
        public void TransitionMap(int path)
        {
            /* Clears the current map */
            currentMap = null;

            /* [Optional] TODO:  Add cut scene here */

            /* Loads new map */
            LoadMap(path);
        }


        /*
         *  TODO: 
         *  Make the map data driven. 
         *  
         */
        //default sets to 1
        public void LoadMap(int path = 1)
        {
            Maptile newTile;
            Maptile [,] tempMap = new Maptile[size,size]; 
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
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
                            newTile = new Maptile(grassText,new Vector2(x, y));
                            tempMap[x, y] = newTile;
                            break;
                    }
                    currentMap = tempMap;
                    path = 1;
                }
            }
           
        }

        public void Update(GameTime gametime)
        {
            currentKeyboardState = Keyboard.GetState();


            /* Temporary for now to make it easier for debugging. 
             Basically exits the game when the [ESCAPE] key is pressed. */
            if (currentKeyboardState.IsKeyDown(Keys.Escape))
                game.Exit();

            player.UpdateInput(gametime, currentKeyboardState);
            player.UpdatePosition(gametime);

            camera.Update(gametime, player);

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

            player.Draw(sb);
            sb.End();

        }
    }
}
