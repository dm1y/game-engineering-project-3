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
            grassText = game.Content.Load<Texture2D>("MapTexture/grass");
            dirtText = game.Content.Load<Texture2D>("MapTexture/dirt");
            redTransition = game.Content.Load<Texture2D>("MapTexture/transition_red");
            blueTransition = game.Content.Load<Texture2D>("MapTexture/transition_blue");

        }

        public void GenerateMap (int i)
        {
            List<string> lines = new List<string>();

            Stream stream = TitleContainer.OpenStream("Content/test" + i + ".txt");
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