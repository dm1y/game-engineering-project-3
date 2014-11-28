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
        public Map currentMap;
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

        int width = 50;
        int height = 40;

        Map map;

        Player player;

        public World(Game1 game, Camera c)
        {
            this.game = game;
            camera = c;
            map = new Map(game);
        }

        //WORK IN PROGRESS
        public void LoadContent(ContentManager Content)
        {
            map.LoadContent();
            map.GenerateMap(0);
            
            currentMap = map;

            width = map.width;
            height = map.height;

            camera.setBoundaries(width * 32, height * 32);

            playerleft = game.Content.Load<Texture2D>("Player/playerleft");
            playerright = game.Content.Load<Texture2D>("Player/playerright");
            playerup = game.Content.Load<Texture2D>("Player/playerup");
            playerdown = game.Content.Load<Texture2D>("Player/playerdown");

            player = new Player(playerup, playerdown, playerleft, playerright, new Vector2(1, 1), currentMap, this);

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
            map.GenerateMap(path);
//            LoadMap(path);
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
            //sb.Begin();

            map.Draw(sb);
            
            player.Draw(sb);
            
            sb.End();
        }
    }
}
