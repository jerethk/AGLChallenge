using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace AGLChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            string JsonString = GetJsonString("people.json");

            if (JsonString == null)
            {
                Console.WriteLine("Failed to get JSON");
            }
            else
            {
                List<Owner> ownerList = ConvertJsonToList(JsonString);

                if (ownerList == null)
                {
                    Console.WriteLine("Failed to parse JSON");
                }
                else
                {
                    OutputCatsByOwnerGender(ownerList);

                    /*
                    foreach (Owner o in ownerList)
                    {
                        Console.WriteLine($"\n{o.Name} {o.Age} {o.Gender}");

                        foreach (Pet p in o.Pets)
                        {
                            Console.WriteLine($"     {p.Name} {p.Type}");
                        }
                    } */
                }
            }
        }

        static private string GetJsonString(string path)
        {
            string input;
            
            using (StreamReader JsonReader = new StreamReader(path))
            {
                try
                {
                    input = JsonReader.ReadToEnd();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    input = null;
                }
            }

            return input;
        }

        static private List<Owner> ConvertJsonToList(string JsonString)
        {
            List<Owner> ownerList = new List<Owner>();
            
            try
            {
                JsonDocument document = JsonDocument.Parse(JsonString);

                foreach (JsonElement element in document.RootElement.EnumerateArray())
                {
                    string name = element.GetProperty("name").GetString();
                    string gender = element.GetProperty("gender").GetString();
                    int age = element.GetProperty("age").GetInt32();
                    List<Pet> pets = new List<Pet>();

                    JsonElement petsElement = element.GetProperty("pets");
                    if (petsElement.ValueKind == JsonValueKind.Array)
                    {
                        foreach (JsonElement petElement in petsElement.EnumerateArray())
                        {
                            string petName = petElement.GetProperty("name").GetString();
                            string petType = petElement.GetProperty("type").GetString();

                            pets.Add(new Pet(petName, petType));
                        }
                    }

                    ownerList.Add(new Owner(name, gender, age, pets));
                }

                return ownerList;
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        static private void OutputCatsByOwnerGender(List<Owner> ownerList)
        {
            List<string> catsOwnedByMales = new List<string>();
            List<string> catsOwnedByFemales = new List<string>();

            foreach (Owner owner in ownerList)
            {
                if (owner.Gender == "Male")
                {
                    foreach (Pet pet in owner.Pets)
                    {
                        if (pet.Type == "Cat")
                        {
                            catsOwnedByMales.Add(pet.Name);
                        }
                    }
                }
                else if (owner.Gender == "Female")
                {
                    foreach (Pet pet in owner.Pets)
                    {
                        if (pet.Type == "Cat")
                        {
                            catsOwnedByFemales.Add(pet.Name);
                        }
                    }
                }
            }

            catsOwnedByMales.Sort();
            catsOwnedByFemales.Sort();

            Console.WriteLine("LIST OF CATS, GROUPED BY GENDER OF OWNER");
            Console.WriteLine("Male owners:");
            foreach (string name in catsOwnedByMales) Console.WriteLine($"· {name}");

            Console.WriteLine("\nFemale owners:");
            foreach (string name in catsOwnedByFemales) Console.WriteLine($"· {name}");
        }
    }
}
