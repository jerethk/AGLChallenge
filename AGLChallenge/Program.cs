using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AGLChallenge
{
    class Program
    {
        private static HttpClient client = new HttpClient();
        private const string uri = "http://agl-developer-test.azurewebsites.net/people.json";

        static void Main(string[] args)
        {
            Console.WriteLine("Fetching data from web... (press any key to exit)\n");
            RunApplication();
            Console.ReadKey();
        }

        static async void RunApplication()
        {
            // Obtain the JSON from the web service
            string JsonString = await GetJsonString(uri);

            if (JsonString == null)
            {
                Console.WriteLine("Failed to get JSON");
            }
            else
            {
                // Parse the JSON string and convert the data into a List of Owners
                List<Owner> ownerList = ConvertJsonToList(JsonString);

                if (ownerList == null)
                {
                    Console.WriteLine("Failed to parse JSON");
                }
                else
                {
                    OutputCatsByOwnerGender(ownerList);

                    /*
                    // Display all data, for testing
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

        // Gets JSON from the web service, and returns it as a string. Returns null if fails.
        static async private Task<string> GetJsonString(string uri)
        {
            string result;

            try
            {
                result = await client.GetStringAsync(uri);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                result = null;
            }

            return result;
        }

        // Parses the JSON string and returns a List of Owners. Returns null if fails.
        static private List<Owner> ConvertJsonToList(string JsonString)
        {
            List<Owner> ownerList = new List<Owner>();
            
            try
            {
                JsonDocument document = JsonDocument.Parse(JsonString);

                foreach (JsonElement element in document.RootElement.EnumerateArray())
                {
                    // Retrieve the owner's name, gender and age properties
                    string name = element.GetProperty("name").GetString();
                    string gender = element.GetProperty("gender").GetString();
                    int age = element.GetProperty("age").GetInt32();

                    // Create a list of pets, retrieve the pet data
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

                    // Add the owner to the list
                    ownerList.Add(new Owner(name, gender, age, pets));
                }

                return ownerList;
            }
            catch (JsonException e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                return null;
            }
        }

        // Creates a list of cats owned by each gender of owner, sorts them alphabetically, then prints to console
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

            // Sort lists
            catsOwnedByMales.Sort();
            catsOwnedByFemales.Sort();

            // Print to console
            Console.WriteLine("LIST OF CATS, GROUPED BY GENDER OF OWNER");
            Console.WriteLine("Male owners:");
            foreach (string name in catsOwnedByMales) Console.WriteLine($"· {name}");

            Console.WriteLine("\nFemale owners:");
            foreach (string name in catsOwnedByFemales) Console.WriteLine($"· {name}");
        }
    }
}
