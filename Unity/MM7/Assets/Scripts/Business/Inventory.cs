using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Business
{
    public class SlotPos
    {
        public int x;
        public int y;
        public SlotPos(int x, int y) { this.x = x; this.y = y; }
    }

    public class Inventory
    {
        private Item[,] BagItems { get; set; }

        public float SlotWidth { get; private set; }
        public float SlotHeight { get; private set; }

        public Inventory(int slotsH, int slotsV, float slotWidth, float slotHeight)
        {
            BagItems = new Item[slotsH, slotsV];
            SlotWidth = slotWidth;
            SlotHeight = slotHeight;
        }

        public Item GetItemAt(int x, int y) 
        {
            return BagItems[x, y];
        }

        public SlotPos GetSlotPos(Item item) 
        {
            for (int i = 0; i <= BagItems.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= BagItems.GetUpperBound(1); j++)
                {
                    if (BagItems[i, j] == item) // items instances should be different!
                        return new SlotPos(i, j);
                }
            }
            return null;
        }

        public Item RemoveItemAt(int x, int y) 
        {
            var item = GetItemAt(x, y);
            RemoveItem(item);
            return item;
        }

        public SlotPos RemoveItem(Item item) 
        {
            SlotPos cornerPosition = null;
            for (int i = 0; i <= BagItems.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= BagItems.GetUpperBound(1); j++)
                {
                    if (BagItems[i, j] == item)  // items instances should be different!
                    {
                        BagItems[i, j] = null;
                        if (cornerPosition == null)
                            cornerPosition = new SlotPos(i, j);
                    }
                }
            }
            return cornerPosition;
        }

        public bool TryInsertItemAt(Item item, int x, int y) 
        {
            var itemInventorySlotsRequiredH = GetSlotsNeeded(SlotWidth, item.Texture.width);
            var itemInventorySlotsRequiredV = GetSlotsNeeded(SlotHeight, item.Texture.height);

            if (x + itemInventorySlotsRequiredH > BagItems.GetUpperBound(0) + 1)
                return false;
            if (y + itemInventorySlotsRequiredV > BagItems.GetUpperBound(1) + 1)
                return false;

            for (int i = x; i < x + itemInventorySlotsRequiredH; i++)
                for (int j = y; j < y + itemInventorySlotsRequiredV; j++)
                    if (GetItemAt(i, j) != null)
                        return false; // occupied

            for (int i = x; i < x + itemInventorySlotsRequiredH; i++)
                for (int j = y; j < y + itemInventorySlotsRequiredV; j++)
                    BagItems[i, j] = item;

            return true;
        }

        public bool TryInsertItem(Item item)
        {
            for (int i = 0; i <= BagItems.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= BagItems.GetUpperBound(1); j++)
                {
                    if (TryInsertItemAt(item, i, j))
                        return true;
                }
            }
            return false;
        }

        public bool TryMoveItem(Item item, int x, int y) 
        {
            var originalPosition = RemoveItem(item);
            if (originalPosition == null)
                return false;
            var canMoveIt = TryInsertItemAt(item, x, y);
            if (!canMoveIt)
                TryInsertItemAt(item, originalPosition.x, originalPosition.y); // rollback
            return canMoveIt;
        }

        public bool TryExchangeItems(Item item1, Item item2)
        {
            var originalPosItem1 = RemoveItem(item1);
            var originalPosItem2 = RemoveItem(item2);
            if (TryInsertItemAt(item2, originalPosItem1.x, originalPosItem1.y))
            {
                if (TryInsertItemAt(item1, originalPosItem2.x, originalPosItem2.y))
                    return true;
                else
                    RemoveItem(item2);
            }
            // rollback
            TryInsertItemAt(item1, originalPosItem1.x, originalPosItem1.y);
            TryInsertItemAt(item2, originalPosItem2.x, originalPosItem2.y);
            return false;
        }

        public int GetTotalSlotsH() 
        {
            return BagItems.GetUpperBound(0) + 1;
        }

        public int GetTotalSlotsV() 
        {
            return BagItems.GetUpperBound(1) + 1;
        }

        public static int GetSlotsNeeded(float slotSize, float itemSize)
        {
            var fraction = itemSize / slotSize;
            var slotsRequired = Mathf.FloorToInt(fraction);
            var remainder = itemSize - slotsRequired * slotSize;
            if (remainder / slotSize > 0.2f)
                slotsRequired += 1;
            return slotsRequired;
        }
    }
}

