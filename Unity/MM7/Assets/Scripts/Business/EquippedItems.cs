using System;

namespace Business
{
    public class EquippedItems
    {
        public Item WeaponRight { get; private set; }
        public Item WeaponLeft { get; private set; }
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

        public bool IsDualHandWeaponEquipped {
            get { return WeaponRight != null && WeaponRight == WeaponLeft; }
        }

        public bool IsDualWeaponsWielding {
            get { return WeaponRight != null && WeaponLeft != null && WeaponRight != WeaponLeft; }
        }

        public EquippedItems() {
            Gauntlets = new Item[2];
            Rings = new Item[6];
        }


        public bool TryEquipItem(Item item, out Item oldEquippedItem) {
            oldEquippedItem = null;

            if (item.EquipSlot == EquipSlot.None)
                return false;

            switch (item.EquipSlot)
            {
                case EquipSlot.Weapon:
                case EquipSlot.WeaponW:
                    if (IsDualHandWeaponEquipped)
                        WeaponLeft = null;
                    oldEquippedItem = WeaponRight;
                    // TODO: dual weapons wielding, how to put the second weapon?
                    WeaponRight = item;
                    break;

                case EquipSlot.Weapon1or2:
                    if (IsDualHandWeaponEquipped)
                        WeaponLeft = null;
                    oldEquippedItem = WeaponRight;
                    WeaponRight = item;
                    if (Shield != null)
                        WeaponLeft = item;
                    break;

                case EquipSlot.Weapon2:
                    if (IsDualWeaponsWielding)
                        return false; // can't exchange both at the same time
                    if (WeaponRight != null && Shield != null)
                        return false; // can't exchange both at the same time
                    if (Shield != null)
                        oldEquippedItem = Shield;
                    else
                        oldEquippedItem = WeaponRight;
                    WeaponRight = item;
                    WeaponLeft = item;
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
                    if (IsDualHandWeaponEquipped)
                    {
                        oldEquippedItem = WeaponRight;
                        WeaponRight = null;
                        WeaponLeft = null;
                    }
                    else if (IsDualWeaponsWielding)
                    {
                        oldEquippedItem = WeaponLeft;
                        WeaponLeft = null;
                    }
                    else
                    {
                        oldEquippedItem = Shield;
                    }
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

            return true;
        }

        public void UnequipItem(Item item) {
            // TODO: rings? gauntlets?
            if (Missile == item)
                Missile = null;
            else if (Armor == item)
                Armor = null;
            else if (Shield == item)
                Shield = null;
            else if (Helm == item)
                Helm = null;
            else if (Belt == item)
                Belt = null;
            else if (Boots == item)
                Boots = null;
            else if (Cloak == item)
                Cloak = null;
            else
            {
                if (WeaponRight == item) WeaponRight = null;
                if (WeaponLeft == item) WeaponLeft = null;
            }
        }

    }
}

