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
    public class Display
    {
        Game game;
        Player p;
        public int HP { set; get; }
        public int level { set; get; }
        public int money { set; get; }
        //public int ScreenWidth;
        //public int ScreenHeight;

        //Will probably make a custom font
        public SpriteFont font;

        public bool ShowHUD;
        //Background Texture
        public Texture2D HUDBackGround;
        public List<Item> items;

        public Display(Player player, Game game)
        {
            game = this.game;
            p = player;
            font = null;
            ShowHUD = true;
            items = new List<Item>();
            HP = 0;
            level = 1;
            money = 0;

        }
        public void LoadContent(ContentManager content)
        {
            //for (int i = 0; i < p.playerInventory.items.Count; i++)
            //{
            //    items[i] = p.playerInventory.items[i];
            //}
            // Console.WriteLine(items.Count);
            font = content.Load<SpriteFont>("Displayfont");
            HUDBackGround = content.Load<Texture2D>("Overlays/hud_maindisplay_96x96");
        }

        public void Update(GameTime gametime)
        {
            KeyboardState keyState = Keyboard.GetState();
        }

        public void Draw(SpriteBatch sb)
        {

            if (ShowHUD)
            {
                // Initial draw state 
                // Always use the camera position formula, then offset it to where you want the things to be located 
                // formula (p.world.camera.Position.X/2, p.world.camera.Position.Y/2)
                // example here, offset for x is 0 since it's at the very left
                // offset for y is the screen size height (320) - texture height 
                sb.Draw(HUDBackGround, new Vector2(p.world.camera.Position.X / 2, p.world.camera.Position.Y / 2 + 320 - HUDBackGround.Height), Color.White);

                // for words, you'll have to adjust y coordinates manually 
                sb.DrawString(font, "HP: " + HP + "/", new Vector2(p.world.camera.Position.X / 2 + 9, p.world.camera.Position.Y / 2 + 235), Color.Red);
                sb.DrawString(font, "Level: " + level + "/", new Vector2(p.world.camera.Position.X / 2 + 7, p.world.camera.Position.Y / 2 + 255), Color.Red);
                sb.DrawString(font, "Money: " + money + "/", new Vector2(p.world.camera.Position.X / 2 + 8, p.world.camera.Position.Y / 2 + 275), Color.Red);

                Console.WriteLine(p.playerInventory.items.Count);
                //for (int i = 0; i < p.playerInventory.items.Count; i++)
                // {
                //  sb.Draw(HUDBackGround, new Vector2(p.world.camera.Position.X / 2 + 10, p.world.camera.Position.Y / 2 + 320 - HUDBackGround.Height), Color.White);

                //}


            }
        }
    }
}
