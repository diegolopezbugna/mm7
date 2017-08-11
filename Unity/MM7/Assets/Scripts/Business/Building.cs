using System;
using System.Collections.Generic;

namespace Business
{
    public class Building
    {
        public string Name { get; set; }
        public string VideoFilename { get; set; }

        public Building()
        {
        }

        public static Building GetByLocationCode(string locationCode) {

            // TODO: remove hardcoding! Read from TXTs

            if (locationCode == "225")
            {
                return new Building() { Name = "Mia Lucille' Home", VideoFilename = "Human Poor House 2" };
            }

            return null;
        }
    }
}

