using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure;

namespace Business
{
    public class PartyStats
    {
        public List<PlayingCharacter> Chars { get; set; }

        public int Gold { get; set; }
        public int Food { get; set; }
     
        public PartyStats()
        {
            Gold = 200;
            Food = 7;
        }
    }
}

