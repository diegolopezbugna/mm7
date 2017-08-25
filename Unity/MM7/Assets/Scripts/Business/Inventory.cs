using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Business
{
    public class Inventory
    {
        //public List<EquippedItem> EquippedItems { get; set; }

        private Item[,] BagItems { get; set; }

        public float SlotWidth { get; private set; }
        public float SlotHeight { get; private set; }

        public Inventory(int slotsH, int slotsV, float slotWidth, float slotHeight)
        {
            BagItems = new Item[slotsH, slotsV];
            SlotWidth = slotWidth;
            SlotHeight = slotHeight;
        }

        public Item GetItemAt(int x, int y) {
            return BagItems[x, y];
        }

        public Item RemoveItemAt(int x, int y) {
            var item = GetItemAt(x, y);
            if (item == null)
                return null;
            for (int i = 0; i <= BagItems.GetUpperBound(0); i++)
            {
                for (int j = 0; i <= BagItems.GetUpperBound(1); i++)
                {
                    if (BagItems[i, j] == item) // items instances should be different!
                        BagItems[i, j] = null;
                }
            }
            return item;
        }

        public bool TryInsertItemAt(Item item, int x, int y) {
            var itemInventorySlotsRequiredH = GetSlotsNeeded(SlotWidth, item.Texture.width);
            var itemInventorySlotsRequiredV = GetSlotsNeeded(SlotHeight, item.Texture.height);

            if (x + itemInventorySlotsRequiredH > BagItems.GetUpperBound(0) + 1)
                return false;
            if (y + itemInventorySlotsRequiredV > BagItems.GetUpperBound(1) + 1)
                return false;

            for (int i = x; i < x + itemInventorySlotsRequiredH; i++)
                for (int j = y; j < y + itemInventorySlotsRequiredV; j++)
                    if (GetItemAt(i, j) != null)
                        return false; // occupied. TODO: exchange items

            for (int i = x; i < x + itemInventorySlotsRequiredH; i++)
                for (int j = y; j < y + itemInventorySlotsRequiredV; j++)
                    BagItems[i, j] = item;

            return true;
        }

        public int GetTotalSlotsH() {
            return BagItems.GetUpperBound(0) + 1;
        }

        public int GetTotalSlotsV() {
            return BagItems.GetUpperBound(1) + 1;
        }

        private int GetSlotsNeeded(float slotSize, float itemSize)
        {
            var fraction = itemSize / slotSize;
            var slotsRequired = Mathf.FloorToInt(fraction);
            var remainder = fraction - slotsRequired * slotSize;
            if (remainder > 0.2f)
                slotsRequired += 1;
            return slotsRequired;
        }
    }
}

