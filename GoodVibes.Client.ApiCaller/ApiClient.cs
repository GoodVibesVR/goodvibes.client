﻿using System.Net;
using System.Net.Http.Headers;
using GoodVibes.Client.ApiCaller.Abstractions;
using Newtonsoft.Json;

namespace GoodVibes.Client.ApiCaller;

public class ApiClient : IApiClient, ILovenseApiClient
{
    public string ApiRoot { get; }

    public ApiClient(string apiRoot)
    {
        ApiRoot = apiRoot;
    }

    public async Task<T?> PostAsync<T>(string path, HttpContent content)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri($"{ApiRoot}");
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await client.PostAsync(path, content);
        var json = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(json))
            return JsonConvert.DeserializeObject<T>(json);

        return default;
    }

    public async Task<MemoryStream> GetImageAsync(string path)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await client.GetAsync($"{ApiRoot}{path}");
        var responseStream = await response.Content.ReadAsStreamAsync();
        var dataStream = new MemoryStream();
        await responseStream.CopyToAsync(dataStream);
        dataStream.Position = 0;

        return dataStream;
    }
}