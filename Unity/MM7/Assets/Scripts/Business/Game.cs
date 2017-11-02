using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Business
{
    public class Game
    {
        public bool IsBuyStandardEnabled = false; // TODO: move to config
        public bool IsIdentifyEnabled = false; // TODO: move to config
        public bool IsRepairEnabled = false; // TODO: move to config

        private PartyStats _partyStats;
        public PartyStats PartyStats { 
            get { 
                if (_partyStats == null)
                    _partyStats = CreatePartyUseCase.CreateDummyParty();
                return _partyStats; 
            } 
            set { _partyStats = value; } 
        }
        
        private static Game instance;
        public static Game Instance {
            get {
                if (instance == null) {
                    instance = new Game();
                }
                return instance;
            }
        }

        public Game()
        {
        }
    }
}

