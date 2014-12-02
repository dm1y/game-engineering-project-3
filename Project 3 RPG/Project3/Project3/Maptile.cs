﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project3
{
    public class Maptile
    {
        //The only blocks that CANNOT be walked through are blocks which we may later designate
        //as background props(ie, houses, walls, wells, anything you'd normally see in an RPG game 
        //that doesn't actually do anything and just looks pretty/limits the player's movement within
        //the map. For instance, if a block cannot be walked through then isCollidable will be TRUE.
        //This will apply to NPCs, since we most definitely don't want the player to walk through NPCS. 
        
        /* DA BLOCKS LEGEND N DA RULEZ
         * 
         * isCollidable - this boolean gets checked when the player is allowed to make a movement, which 
         *                is only done when the player is no longer moving to the next block. If the player
         *                attempts to make a move, the block is pre-emptively checked -- if isCollidable is false
         *                then player will move there.
         * 
         * isTransition - this boolean is to be used in conjunction with the integer transitionTo! It lets us know
         *                which map the tile that the player is standing on will be loaded. Note that this does mean
         *                when we make the GUI editor, we'll probably have to hardcode in the symbols, which limits us
         *                to the amount of transition zones we can make. May reconsider how we do this later on. 
         * 
         * isDangerous - very simple boolean, to be used for the battle system. if the player steps onto this block, we
         *               check ONCE if the block will spawn an enemy encounter. if it does, then we will pass the block's 
         *               "enemySpawnType" to the battle encounter class(this process may change depending on how we 
         *               implement it. however, i would like different tile types to spawn different enemies from a list! 
         *               for instance, a swamp tile might spawn three goos and 1 toothed plant, while a forest tile might 
         *               spawn two mushroom knights. :D
         *               
         * isInteract - this boolean gets checked if the player hits space in front of a block. if it's true, then we will
         *              check be used with a huge dialogue class which may have to hard-code in the dialogue. still thinking
         *              about how this will be implemented though, so don't bank too hard on this feature yet during the GUI
         *              I'm open to ideas!
         */

        public Boolean isCollidable;
        public Boolean isTransition;
        public Boolean isDangerous;
        public Boolean isInteract;

        public NPC npc;
        public Texture2D npcTexture;

        public int transitionTo;
        public List<Enemy> enemySpawnTypes;

        /* - Positions -
         * coordPosition - gives the position of the maptile in coordinate form(ie, "4,7")
         * truePosition - gives the position of the maptile in actual pixel position(ie, "64,512")
         */
        public Vector2 coordPosition;
        public Vector2 truePosition;

        /* - Drawing Variables - */
        public Texture2D texture;
        public Rectangle drawingRectangle;

        public Maptile(Texture2D texture, Vector2 position, Boolean collide, Boolean transition, Boolean dangerous, Boolean interact)
        {
            this.texture = texture;
            this.coordPosition = position;
            this.truePosition = new Vector2(texture.Width * position.X, texture.Height * position.Y);
            drawingRectangle = new Rectangle((int)truePosition.X, (int)truePosition.Y, texture.Width, texture.Height);
            
            /* Differentiates between the different kinds of tiles */
            isCollidable = collide;
            isTransition = transition;
            isDangerous = dangerous;
            isInteract = interact;

            /* Initializes other attributes that might not pertain to all tiles */
            transitionTo = 0;
            enemySpawnTypes = new List<Enemy>();
            npc = null;

            if (isCollidable)
                createCollidableTile();

            if (isTransition)
                createTransitionTile();

            if (isDangerous)
                createDangerousTile();

            if (isInteract)
                createInteractTile();
        }

        public void createTransitionTile()
        { 
            // TODO: Implement properties for isTransitionTile
            // should define which map it'll transition to depending on the texture 

            /* If the texture is considered a "blue transition texture", go to map 1 */
            if (texture.Name == "transition")
                transitionTo = 1;
        }

        public void createDangerousTile()
        { 
            // TODO: Implement properties for dangerousTile 
            // take array of monsters that is being passed in, 
            // depending on the type of tile, just hard code the string of monsters in 
            // otherwise make it data driven

            // easy run, 1 easy enemy 
            if (texture.Name == "danger")
            {
                // fill null in later once items are set 
                Enemy e = new Enemy(10, 2, 2, 100, 5, null);
                enemySpawnTypes.Add(e);
            }
            //    pass in list of enemies that are easy

            //if (texture.Name == "medium")
            //    pass in list of enemies that are challenging

            //if (texture.Name == "hard")
            //    maybe give it a boss

        }

        public void createInteractTile() 
        {
            // TODO: Implement properties for interactTile
            // this will rely on NPCs and items class , look into this later. 

            //if (texture.Name == "[type]NPC")
            //    define its NPC attributes 

            //if (texture.Name == "item")
            //    define item attributes
        }

        public void createCollidableTile()
        { 
            // TODO: Implement properties for collidableTile
            // not sure what this is supposed to do but perhaps create a border around the map to prevent out of bounds stuff 
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, drawingRectangle, Color.White);
            if (isInteract)
            {
                spritebatch.Draw(npcTexture, drawingRectangle, Color.White);
            }
        }
    }
}
