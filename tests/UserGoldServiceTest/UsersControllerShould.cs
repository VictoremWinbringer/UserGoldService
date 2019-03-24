using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UserGoldService;
using Xunit;

namespace UserGoldServiceTest
{
    public class UsersControllerShould
    {
        private readonly HttpClient _client;
        private readonly string _root;
        private readonly TestServer _server;

        public UsersControllerShould()
        {
            TestServer server = new TestServer(WebHost.CreateDefaultBuilder().UseStartup<Startup>());

            HttpClient client = server.CreateClient();

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            _client = client;

            _server = server;

            _root = "api/v1/users/";
        }

        private async Task<string> Register(string userName)
        {
            HttpResponseMessage response = await _client.PutAsync(_root + $"register/{userName}", null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }


        private async Task<decimal> GetMyGold(string token)
        {
            _client.DefaultRequestHeaders.Remove("token");
            _client.DefaultRequestHeaders.Add("token", new[] { token });
            HttpResponseMessage response = await _client.GetAsync(_root + "mygold/");
            response.EnsureSuccessStatusCode();
            string str = await response.Content.ReadAsStringAsync();
            return decimal.Parse(str, CultureInfo.InvariantCulture);
        }

        private async Task AddGold(decimal count, string token)
        {
            StringContent content = new StringContent("");
            _client.DefaultRequestHeaders.Remove("token");
            _client.DefaultRequestHeaders.Add("token", new[] { token });
            HttpResponseMessage response = await _client.PutAsync(_root + $"gold/{count}", content);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task WorkCorrect()
        {
            decimal added = 1.1M;
            var token = await Register("Victor");
            var oldGold = await GetMyGold(token);
            await AddGold(added, token);
            var newGold = await GetMyGold(token);
            Assert.Equal(oldGold+added, newGold);
        }
    }
}
