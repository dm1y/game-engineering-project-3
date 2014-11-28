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
    /*
     * Notes: Maps are generated via text files and are found in the Maps folder of Content. 
     * 
     * It doesn't matter what size it is, but keep it consistent so that each line contains the same 
     * number of characters. Short example: 
     *                                             xxxxxxxxxxxxxxxxxxx
     *                                             xxxdddddddddddddxxx
     *                                             txxgggggggggggggxxt
     * 
     * More examples can be seen in the text files.
     * 
     * Current legend: 
     * 
     * d = dirt tile
     * x = danger tile 
     * t - transition tile 
     * g = grass tile 
     * 
     * If you add more tiles, update this and the MapTile class:
     *          What to update here: 
     *              - Create another Texture2D
     *              - Load it in LoadContent method 
     *              - Give a name for it 
     *              - Go to GenerateMap method 
     *              - Add a case character for it and follow the pattern in creating the mapTile 
     *              - When creating the MapTile, make sure you adjust its properties by its type
     *          What to update in MapTile class if applicable 
     *              - If it's a different transition tile, specify which map it transitions to 
     *                by adjusting its transitionTo attribute and define it by its name 
     *              - TBC when we have more tiles.
     * 
     */
    public class Map
    {
        public Game1 game;
        
        /* Size of each map */
        public int width {get; private set;}
        public int height { get; private set; }

        /* Map Textures */
        private Texture2D grassText;
        private Texture2D dirtText;
        private Texture2D redTransition;
        private Texture2D blueTransition;

        public Maptile[,] currentMap { get; private set; }

        public Map(Game1 g)
        {
            game = g;
        }

        public void LoadContent()
        {
            /* Loads textures */
            grassText = game.Content.Load<Texture2D>("MapTexture/grass");
            dirtText = game.Content.Load<Texture2D>("MapTexture/dirt");
            redTransition = game.Content.Load<Texture2D>("MapTexture/transition_red");
            blueTransition = game.Content.Load<Texture2D>("MapTexture/transition_blue");

            /* Gives textures names used to different special attributes if applicable */
            grassText.Name = "grass";
            dirtText.Name = "dirt";
            redTransition.Name = "danger";
            blueTransition.Name = "transition";
        }

        public void GenerateMap (int i)
        {
            List<string> lines = new List<string>();

            Stream stream = TitleContainer.OpenStream("Content/Maps/test" + i + ".txt");
            using (StreamReader reader = new StreamReader(stream))
            {
                string line = reader.ReadLine();
                width = line.Length;
                while (line != null)
                {
                    lines.Add(line);
                    line = reader.ReadLine();
                }
            }

            height = lines.Count;

            currentMap = new Maptile[width, height];

            Maptile newTile;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    char tileType = lines[y][x];
                    
                    switch (tileType)
                    
                    {
                        // If more tiles are added, give a case letter and create the tiles with the appropriate attributes 
                        case 'd':
                            /* Creates a dirt tile*/
                            newTile = new Maptile(dirtText, new Vector2(x, y), false, false, false, false );                            
                            currentMap[x, y] = newTile;
                            break;
                        case 'x':
                            /* Creates a transition tile to battle automagically */
                            newTile = new Maptile(redTransition, new Vector2(x, y), false, false, false, false);
                            currentMap[x, y] = newTile;
                            break;
                        case 't':
                            /* Creates a normal transition tile to a different map */
                            newTile = new Maptile(blueTransition, new Vector2(x, y), false, true, false, false);
                            currentMap[x, y] = newTile;
                            break;
                        case 'g':
                            /* Creates a grass tile */
                            newTile = new Maptile(grassText, new Vector2(x, y), false, false, false, false);
                            currentMap[x, y] = newTile;
                            break;
                    }
                }

                }
            }


        public void Draw(SpriteBatch sb)
        {

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    currentMap[x, y].Draw(sb);
                }
            }

        }

        
    }
}