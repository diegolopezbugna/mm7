using System;

namespace Business
{
    public class EquippedItems
    {
        public Item Weapon1 { get; private set; }
        public Item Weapon2 { get; private set; }
        public Item Missile { get; private set; }
        public Item Helm { get; private set; }
        public Item Armor { get; private set; }
        public Item Shield { get; private set; }
        public Item Amulet { get; private set; }
        public Item Belt { get; private set; }
        public Item Cloak { get; private set; }
        public Item Boots { get; private set; }
        public Item[] Gauntlets { get; private set; }
        public Item[] Rings { get; private set; }

        public EquippedItems() {
            Gauntlets = new Item[2];
            Rings = new Item[6];
        }

        public Item GetItemEquipped(EquipSlot equipSlot) {
            switch (equipSlot)
            {
                case EquipSlot.Weapon:
                case EquipSlot.Weapon1or2:
                case EquipSlot.Weapon2:
                case EquipSlot.WeaponW:
                    return Weapon1; // TODO: second weapon
                case EquipSlot.Missile:
                    return Missile;
                case EquipSlot.Armor:
                    return Armor;
                case EquipSlot.Shield:
                    return Shield;
                case EquipSlot.Helm:
                    return Helm;
                case EquipSlot.Belt:
                    return Belt;
                case EquipSlot.Boots:
                    return Boots;
                case EquipSlot.Cloak:
                    return Cloak;
                    // TODO: rest

            }
            return null;
        }

        public Item EquipItem(Item item) {
            if (item.EquipSlot == EquipSlot.None)
                return null;

            Item oldEquippedItem = null;

            switch (item.EquipSlot)
            {
                case EquipSlot.Weapon:
                case EquipSlot.Weapon1or2:
                case EquipSlot.Weapon2:
                case EquipSlot.WeaponW:
                    oldEquippedItem = Weapon1;
                    Weapon1 = item;
                    break;
                case EquipSlot.Missile:
                    oldEquippedItem = Missile;
                    Missile = item;
                    break;
                case EquipSlot.Armor:
                    oldEquippedItem = Armor;
                    Armor = item;
                    break;
                case EquipSlot.Shield:
                    oldEquippedItem = Shield;
                    Shield = item;
                    break;
                case EquipSlot.Boots:
                    oldEquippedItem = Boots;
                    Boots = item;
                    break;
                case EquipSlot.Helm:
                    oldEquippedItem = Helm;
                    Helm = item;
                    break;
                case EquipSlot.Belt:
                    oldEquippedItem = Belt;
                    Belt = item;
                    break;
                case EquipSlot.Cloak:
                    oldEquippedItem = Cloak;
                    Cloak = item;
                    break;
                    // TODO: others RINGS GAUNTLETS
            }

            return oldEquippedItem;

        }

        public void UnequipItem(Item item) {
            // TODO: rings? gauntlets?
//            if (Weapon1 == item)  // TODO: better??
//                Weapon1 = null;
            // TODO: better to have a list of items??
            switch (item.EquipSlot)
            {
                case EquipSlot.Weapon:
                case EquipSlot.Weapon1or2:
                case EquipSlot.Weapon2:
                case EquipSlot.WeaponW:
                    Weapon1 = null;
                    break;
                case EquipSlot.Missile:
                    Missile = null;
                    break;
                case EquipSlot.Armor:
                    Armor = null;
                    break;
                case EquipSlot.Shield:
                    Shield = null;
                    break;
                case EquipSlot.Helm:
                    Helm = null;
                    break;
                case EquipSlot.Belt:
                    Belt = null;
                    break;
                case EquipSlot.Boots:
                    Boots = null;
                    break;
                case EquipSlot.Cloak:
                    Cloak = null;
                    break;
                    // TODO: rest

            }
        }

    }
}

