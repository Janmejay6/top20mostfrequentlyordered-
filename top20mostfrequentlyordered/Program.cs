using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace top20mostfrequentlyordered
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var pizzasJson =  httpClient.GetAsync("http://files.olo.com/pizzas.json").Result;

                    if (pizzasJson.IsSuccessStatusCode)
                    {
                        var content = pizzasJson.Content.ReadAsStringAsync().Result;
                        var toppings = JsonConvert.DeserializeObject<Pizzas[]>(content).Select(x => string.Join(",", x.Toppings.OrderBy(y => y)));

                        Dictionary<string, int> top20Toppings = toppings.GroupBy(x => x).OrderByDescending(y=> y.Count()).Take(20)  
                                                                                   .ToDictionary(grp =>grp.Key, grp=> grp.Count());

                        Console.WriteLine(" top 20 most frequently ordered pizzas and combinations");

                        foreach (var item in top20Toppings)
                        {
                            Console.WriteLine("Topping: " + item.Key + "\t Frequency: " + item.Value);
                        }

                        Console.ReadLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


    }
    class Pizzas
    {
        public string [] Toppings { get; set; }
    }
}
