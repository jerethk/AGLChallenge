using System;
using System.Collections.Generic;
using System.Text;

namespace AGLChallenge
{
    public class Owner
    {
        /// <summary>
        /// This class represents the owners of pets
        /// </summary>
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public List<Pet> Pets { get; set; }

        // Constructor
        public Owner(string name, string gender, int age, List<Pet> pets)
        {
            this.Name = name;
            this.Gender = gender;
            this.Age = age;
            this.Pets = pets;
        }
    }
}
