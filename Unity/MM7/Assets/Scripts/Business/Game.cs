using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Business
{
    public class Game
    {
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

