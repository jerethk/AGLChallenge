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

                        Console.WriteLine(name);
                    }
                }
                catch (JsonException e)
                {
                    Console.WriteLine(e.Message);
                }

            }

            Console.WriteLine("Done");

        }
    }
}
