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

        public void EquipItem(Item item) {
            if (item.EquipSlot == EquipSlot.None)
                return;

            // TODO: check already equipped items
            switch (item.EquipSlot)
            {
                case EquipSlot.Weapon:
                case EquipSlot.Weapon1or2:
                case EquipSlot.Weapon2:
                case EquipSlot.WeaponW:
                    Weapon1 = item;
                    break;
                case EquipSlot.Missile:
                    Missile = item;
                    break;
                case EquipSlot.Armor:
                    Armor = item;
                    break;
                case EquipSlot.Shield:
                    Shield = item;
                    break;
                case EquipSlot.Boots:
                    Boots = item;
                    break;
                case EquipSlot.Helm:
                    Helm = item;
                    break;
                case EquipSlot.Belt:
                    Belt = item;
                    break;
                case EquipSlot.Cloak:
                    Cloak = item;
                    break;
                    // TODO: others
            }

        }

    }
}

