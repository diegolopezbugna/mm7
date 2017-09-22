﻿using System;
using System.Collections.Generic;

namespace Business
{
    public class NpcTopic 
    {
        public List<NpcTopic> Subtopics { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ShopActionType ShopActionType { get; set; }

        public NpcTopic() {
            Subtopics = new List<NpcTopic>();
        }

        // TODO: gold topics BARD Buy Lute for 500 gold
        public NpcTopic(string title) : this() {
            Title = title;
        }

        public NpcTopic(string title, string description) : this() {
            Title = title;
            Description = description;
        }

        public NpcTopic(string title, string description, List<NpcTopic> subtopics) : this(title, description) {
            Subtopics = subtopics;
        }

        public NpcTopic(string title, ShopActionType shopActionType) : this() {
            Title = title;
            ShopActionType = shopActionType;
        }

        public string GetTitleFor(Shop shop, PlayingCharacter playingCharacter)
        {
            // TODO: merchant skill of playingCharacter
            if (shop.ShopType == ShopType.Healer && ShopActionType == ShopActionType.Heal)
            {
                var playingCharacterHealsUseCase = new PlayingCharacterHealsUseCase(null);
                var cost = playingCharacterHealsUseCase.GetHealingAtHealerCost(shop.ShopMultiplier, playingCharacter);
                return string.Format(Title, cost);
            }

            return Title;
        }
    }
}

