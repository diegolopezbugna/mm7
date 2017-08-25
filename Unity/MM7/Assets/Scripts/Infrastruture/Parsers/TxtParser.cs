using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Business
{
    public abstract class TxtParser<T>
    {
        public List<T> Entities = new List<T>();

        public TxtParser(string filepath) {
            var reader = new StreamReader(filepath, Encoding.Default);

            using (reader)
            {
                reader.ReadLine(); // header
                var line = reader.ReadLine();
                if (line != null){
                    do
                    {
                        string[] values = line.Split('\t');
                        var entity = ParseValues(values);
                        if (entity != null)
                            Entities.Add(entity);
                        line = reader.ReadLine();
                    }
                    while (line != null);
                } 
                reader.Close();
            }
        }

        public abstract T ParseValues(string[] values);
    }
}

