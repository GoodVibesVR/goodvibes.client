namespace GoodVibes.Client.ApiCaller.Abstractions;

public interface IApiClient
{
    /// <summary>
    /// Post command asynchronously to an API
    /// </summary>
    /// <typeparam name="T">Output object type</typeparam>
    /// <param name="path">Path to add to the root. Needs a leading slash</param>
    /// <param name="content">The kind of content you need to POST to the API.</param>
    /// <returns></returns>
    Task<T?> PostAsync<T>(string path, HttpContent content);

    /// <summary>
    /// Fetches an image from an API
    /// </summary>
    /// <param name="path">Path to the image. Needs a leading slash</param>
    /// <returns></returns>
    Task<MemoryStream> GetImageAsync(string path);

    /// <summary>
    /// The current set API root path
    /// </summary>
    string ApiRoot { get; }
}