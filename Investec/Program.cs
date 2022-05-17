using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Investec
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://swapi.dev/api/people";

            List<Character> characterList = new List<Character>();

            CharacterResultList characterResultList = GetFullCharacterList(url);
            characterList.AddRange(characterResultList.results);

            while (characterResultList.next != null)
            {
                characterResultList = GetFullCharacterList(characterResultList.next);

                characterList.AddRange(characterResultList.results);
            }


            List<Character> characterBuddyList = new List<Character>();

            int mainListLength = characterList.Count;

            Character context = characterList[0];

            for (int i = 1; i < mainListLength; i++)
            {
                Character innerContext = characterList[i];

                List<string> joinedList = context.films.Intersect(innerContext.films).ToList();

                if (joinedList.Count == context.films.Count)
                {
                    characterBuddyList.Add(innerContext);
                }
            }


            foreach (Character item in characterBuddyList)
            {
                Console.Write($"{item.name} ");
            }

        }

        public static CharacterResultList GetFullCharacterList(string url)
        {
            HttpClient client = new HttpClient();

            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, url);

            string response = client.SendAsync(httpRequest).Result.Content.ReadAsStringAsync().Result;

            CharacterResultList list = JsonConvert.DeserializeObject<CharacterResultList>(response);

            return list;
        }
    }

    public class CharacterResultList
    {
        public int count { get; set; }
        public string next { get; set; }

        public string previous { get; set; }
        public List<Character> results { get; set; }
    }

    public class Character
    {
        public string name { get; set; }

        public List<string> films { get; set; }
    }

}
