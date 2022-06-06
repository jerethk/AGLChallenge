using System;
using System.Collections.Generic;
using System.Text;

namespace AGLChallenge
{
    public class Pet
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public Pet(string name, string type)
        {
            this.Name = name;
            this.Type = type;
        }
    }
}
