using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project3
{
    class Map
    {

        /* TODO: Move level creation logic here instead of having it in World 
         TODO: Use StreamReaders to create the levels via ascii 
         TODO: to access texts/textures, specify directory or folder name/then the actual name 
         Ex: "Levels/level1.txt" when doing Content.Load , this will keep things organized
         * 
         
         Pseudo logic / code ;
         * 
         This will be in World class: 
         string levelPath = "Content/Levels/ " + index of level specifying type of level ".txt"
         using (Stream file = TitleContainer.OpenStream(levelPath))
                level = new Level(file, levelIndex);
         

         ** use this in the load method 
         List<string> line = new List<string>();
         using (StreamReader reader = new StreamReader(file))
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    lines.Add(line);
         * // should probably check to make sure the width of each text file is the same, will figure out logic for that  later 
                    line = reader.ReadLine();
                }
         * 
         * 
         * use the line.Count information to initialize the size of the grid 
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


    }
}
