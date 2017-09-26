using System;
using System.Collections.Generic;

namespace Business
{
    public class OpenChestUseCase
    {
        public OpenChestUseCase()
        {
        }

        public List<Item> GetItems(int[] treasureLevels, int[] fixedItemIds) 
        {
            var items = GenerateChestRandomItems(treasureLevels);

            foreach (var itemId in fixedItemIds)
                items.Add(Item.GetByCode(itemId));

            return items;
        }

        private List<Item> GenerateChestRandomItems(int[] treasureLevels)
        {
            List<Item> items = new List<Item>();
            
            foreach (var treasureLevel in treasureLevels)
            {
                if (treasureLevel == 7)
                {
                    // TODO: artifact
                }
                else
                {
                    var additionalCount = UnityEngine.Random.Range(1, 6);
                    for (int i = 0; i < 1 + additionalCount; i++)
                    {
                        var whatToGenerateProb = UnityEngine.Random.Range(1, 101);
                        if (whatToGenerateProb < 20)
                            break;
                        else if (whatToGenerateProb < 60)
                            items.Add(Item.GenerateGold(treasureLevel));
                        else
                            items.Add(Item.GenerateItem(treasureLevel));
                    }
                }
            }

            return items;
        }
    }
}

