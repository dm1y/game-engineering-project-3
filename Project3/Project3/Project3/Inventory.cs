using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project3
{
    /* For displaying player inventory during gameplay. IE, if the player presses "i", they can 
     change what they have equipped.*/
    class Inventory
    {
        public List<Item> items;

        public int money;

        public int currentItemIndex;
        public Item currentItem;
        /* These attributes will *only* be used for PLAYER. It will technically
         be possible for a merchant to "equip" items, but it wont have any functionality. */
        public Item equippedShield;
        public Item equippedWeapon;
        public Item equippedPotion;

        public Inventory(List<Item> items)
        {
            currentItemIndex = 0;
            this.items = items;
        }

        public Inventory()
        {
            currentItemIndex = 0;
            items = new List<Item>();
        }

        public void AddToInventory(Item item)
        {
            items.Add(item);
        }

        public void RemoveFromInventory(Item item)
        {
            items.Remove(item);
        }

        public void BrowseNextItem()
        {
            currentItemIndex++;
            if (currentItemIndex > items.Count())
            {
                currentItemIndex--;
            }
        }

        public void BrowsePreviousItem()
        {
            currentItemIndex--;
            if (currentItemIndex < 0)
            {
                currentItemIndex++;
            }
        }





    }
}