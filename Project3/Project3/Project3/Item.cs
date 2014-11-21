using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project3
{
    class Item
    {
        Texture2D itemTexture;

        /* Basic INTs for properties - 
         itemBuyPrice - self explanatory
         itemSellPrice - always set to 60% of the buying price
         block - used to calculate how much damage mitigation the player gets when struck in battle. only intended for SHIELDS
         heal - used to calculate how much a potion can heal
         damage - added to player's base damage when they attack*/
        int itemBuyPrice;
        int itemSellPrice;
        int block;
        int heal;
        int damage;

        Boolean isShield = false;
        Boolean isPotion = false;
        Boolean isWeapon = false;
        String itemName;

        public Item(Texture2D item, String itemName,
            int itemBuyPrice, int ItemGeneralEffect, Boolean isShield, Boolean isPotion, Boolean isWeapon)
        {
            itemTexture = item;
            this.itemBuyPrice = itemBuyPrice;
            itemSellPrice = (int)(itemBuyPrice * 0.60);
            if (isShield)
            {
                block = ItemGeneralEffect;
                this.isShield = true;
            }
            else if (isPotion)
            {
                heal = ItemGeneralEffect;
                this.isPotion = true;
            }
            else if (isWeapon)
            {
                damage = ItemGeneralEffect;
                this.isWeapon = true;
            }

            this.itemName = itemName;
        }

        public String GetItemType()
        {
            if (isShield)
            {
                return "shield";
            }
            else if (isPotion)
            {
                return "potion";
            }
            else if (isWeapon)
            {
                return "weapon";
            }
            return "misc";
        }
    }
}