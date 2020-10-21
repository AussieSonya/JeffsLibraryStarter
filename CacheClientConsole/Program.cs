using CacheCow.Client;
using CacheCow.Client.RedisCacheStore;
using System;
using System.Net.Http;

namespace CacheClientConsole
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.Clear();
            var uri = new Uri("http://localhost:3000");
            var client = ClientExtensions.CreateClient(new RedisStore("localhost:6379"));
            client.BaseAddress = uri;

            while (true)
            {
                Console.WriteLine("Hit Enter to get the data");
                Console.ReadLine();
                var response = await client.GetAsync("/caching/info");
                response.EnsureSuccessStatusCode();
                Console.WriteLine(response.Headers.CacheControl?.ToString());
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content); 
            }
        }
    }
}
