using System.Net;
using System.Net.Http.Headers;
using System.Text;
using GoodVibes.Client.ApiCaller.Abstractions;
using GoodVibes.Client.ApiCaller.Requests;
using GoodVibes.Client.ApiCaller.Responses;
using Newtonsoft.Json;

namespace GoodVibes.Client.ApiCaller
{
    public class GoodVibesClient : IGoodVibesClient
    {
        private readonly string _apiRoot;
        private DateTime _lastCall = DateTime.MinValue;

        public GoodVibesClient()
        {
            _apiRoot = "https://goodvibes.miwca.se/api/"; // TODO: Fix me
        }

        public void SendCommand(CommandRequest request)
        {
            var now = DateTime.Now;
            if (now < _lastCall.AddSeconds(1))
            {
                return;
            }

            var path = "v1/lovense/command";
            var result = PostAsync<CommandResponse>(path,
                    new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"))
                .GetAwaiter()
                .GetResult();

            _lastCall = now;
        }

        private async Task<T?> PostAsync<T>(string path, HttpContent content)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri($"{_apiRoot}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.PostAsync(path, content);
            var json = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(json))
                return JsonConvert.DeserializeObject<T>(json);

            return default;
        }
    }
}