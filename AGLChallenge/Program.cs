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
            string input = "";
            bool isInputFailed = false;

            using (StreamReader JsonReader = new StreamReader("people.json"))
            {
                try
                {
                    input = JsonReader.ReadToEnd();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    isInputFailed = true;
                }
            }

            if (!isInputFailed)
            {
                List<Owner> ownerList = new List<Owner>();

                try
                {
                    JsonDocument document = JsonDocument.Parse(input);

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

                    foreach (Owner o in ownerList) 
                    {
                        Console.WriteLine($"\n{o.Name} {o.Age} {o.Gender}");
                        
                        foreach (Pet p in o.Pets)
                        {
                            Console.WriteLine($"     {p.Name} {p.Type}");
                        }
                    }
                }
                catch (JsonException e)
                {
                    Console.WriteLine(e.Message);
                }

            }

            Console.WriteLine("\nDone");
        }
    }
}
