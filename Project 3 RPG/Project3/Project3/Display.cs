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
    class Display
    {
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

        public Display(Player player)
        {
            p = player;
            font = null;
            ShowHUD = true;
            //Need a HP property for player
            //HP = player.
            //need to make money property for player
            //money = 
            //need to make level property
            //level =
        }
        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("DialogueFont");
            HUDBackGround = content.Load<Texture2D>("Overlays/hud_maindisplay_96x96");
        }

        public void Update(GameTime gametime)
        {
            KeyboardState keyState = Keyboard.GetState();
        }

        public void Draw(SpriteBatch sb)
        {
            //sb.End();
            //sb.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, null, null, null, null, p.world.camera.transform);
            if (ShowHUD)
            {
                sb.Draw(HUDBackGround, new Vector2(p.world.camera.Position.X/2 + 205,p.world.camera.Position.Y/2 + 205), Color.White);
                sb.DrawString(font, "HP: " + "/", new Vector2(55,60), Color.Red);
                sb.DrawString(font, "Level: " + "/", new Vector2(55, 80), Color.Red);
                sb.DrawString(font, "Money: " + "/", new Vector2(55, 100), Color.Red);

                
            }
        }
    }
}
