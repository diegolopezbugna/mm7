using System;
using System.Collections.Generic;
using System.Linq;

namespace Business
{
    public class Inventory
    {
        //public List<EquippedItem> EquippedItems { get; set; }

        private Item[,] BagItems { get; set; }

        public Inventory(int slotsH, int slotsV)
        {
            BagItems = new Item[slotsH, slotsV];
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
            if (x + item.InventorySlotsRequiredH > BagItems.GetUpperBound(0) + 1)
                return false;
            if (y + item.InventorySlotsRequiredV > BagItems.GetUpperBound(1) + 1)
                return false;

            for (int i = x; i < x + item.InventorySlotsRequiredH; i++)
                for (int j = y; j < y + item.InventorySlotsRequiredV; j++)
                    if (GetItemAt(i, j) != null)
                        return false; // occupied. TODO: exchange items

            for (int i = x; i < x + item.InventorySlotsRequiredH; i++)
                for (int j = y; j < y + item.InventorySlotsRequiredV; j++)
                    BagItems[i, j] = item;

            return true;
        }

        public int GetTotalSlotsH() {
            return BagItems.GetUpperBound(0) + 1;
        }

        public int GetTotalSlotsV() {
            return BagItems.GetUpperBound(1) + 1;
        }

    }
}

