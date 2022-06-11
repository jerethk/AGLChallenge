using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AGLChallenge
{
    public class Program
    {
        private static HttpClient client = new HttpClient();
        private static bool isFinished = false;
        private const string Uri = "http://agl-developer-test.azurewebsites.net/people.json";
        private const int TimeOutInterval = 5000;       // Allow 5 seconds before timing out.

        static void Main(string[] args)
        {
            Console.WriteLine($"Fetching data from web service. Process will time out in {TimeOutInterval} ms.\n");

            // Note- This is an async method so will run on a separate thread.
            RunApplicationAsync();       

            // Code for timer.
            DateTime startTime = DateTime.Now;
            double timeInterval = 0;
            bool isTimedOut = false;

            // This loop will exit either when isFinished == true or the process times out
            while (!isFinished)
            {
                timeInterval = (DateTime.Now - startTime).TotalMilliseconds;

                if (timeInterval > TimeOutInterval)
                {
                    isTimedOut = true;
                    break;
                }
            }

            if (isTimedOut)
            {
                Console.WriteLine("\nTimed out");
            }
            else
            {
                Console.WriteLine($"\nProcess finished in {Math.Floor(timeInterval)} ms.");
            }
        }

        static async void RunApplicationAsync()
        {
            // Obtain the JSON from the web service.
            string jsonString = await GetJsonStringAsync(Uri);

            if (jsonString == null)
            {
                Console.WriteLine("Failed to get JSON");
                isFinished = true;
            }
            else
            {
                // Parse the JSON string and convert the data into a List of Owners.
                List<Owner> ownerList = ConvertJsonToList(jsonString);

                if (ownerList == null)
                {
                    Console.WriteLine("Failed to parse JSON");
                    isFinished = true;
                }
                else
                {
                    OutputCatsByOwnerGender(ownerList);
                    isFinished = true;

                    /*
                    // Display all data in list, only for testing
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
        static async public Task<string> GetJsonStringAsync(string uri)
        {
            string result;

            try
            {
                result = await client.GetStringAsync(uri);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Http Request Exception: {e.Message}");
                result = null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                result = null;
            }

            return result;
        }

        // Parses the JSON string and returns a List of Owners. Returns null if fails.
        static public List<Owner> ConvertJsonToList(string jsonString)
        {
            List<Owner> ownerList = new List<Owner>();
            
            try
            {
                JsonDocument document = JsonDocument.Parse(jsonString);

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
                Console.WriteLine($"Json Exception: {e.Message}");
                return null;
            }
            catch (Exception e)
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
                switch (owner.Gender)
                {
                    case "Male":
                        foreach (Pet pet in owner.Pets)
                        {
                            if (pet.Type == "Cat")
                            {
                                catsOwnedByMales.Add(pet.Name);
                            }
                        }

                        break;

                    case "Female":
                        foreach (Pet pet in owner.Pets)
                        {
                            if (pet.Type == "Cat")
                            {
                                catsOwnedByFemales.Add(pet.Name);
                            }
                        }

                        break;
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
