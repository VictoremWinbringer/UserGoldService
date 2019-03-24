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

        [Fact]
        public async Task RegisterShouldReturnValidToken()
        {
            string userName = "Victor";
            HttpResponseMessage response = await _client.PutAsync(_root + $"register/{userName}", null);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            Assert.Equal(userName, result);
        }

        [Fact]
        public async Task GetMyGoldShouldReturnValidGold()
        {
            _client.DefaultRequestHeaders.Add("token", new[] { "1111" });
            HttpResponseMessage response = await _client.GetAsync(_root + "mygold/");
            response.EnsureSuccessStatusCode();
            string str = await response.Content.ReadAsStringAsync();
            decimal result = decimal.Parse(str, CultureInfo.InvariantCulture);
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task AddGoldShouldAddValidGold()
        {
            var content = new StringContent("");
            content.Headers.Add("token", new[] { "1111" });
            HttpResponseMessage response = await _client.PutAsync(_root + "gold/111", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
