using System.Collections.Concurrent;
using GoodVibes.Client.Cache.Abstractions;
using Newtonsoft.Json;

namespace GoodVibes.Client.Cache
{
    public class GoodVibesCacheManager<T> where T : IApplicationCache, new()
    {
        private readonly string _filePath;
#pragma warning disable CS8618
        private readonly ConcurrentQueue<string> _textToWrite;
#pragma warning restore CS8618

        private CancellationTokenSource _source;
#pragma warning disable CS0649
        private CancellationToken _token;
#pragma warning restore CS0649

        private T _cache;

        public GoodVibesCacheManager()
        {
            _textToWrite = new ConcurrentQueue<string>();
            _source = new CancellationTokenSource();

            var temp = new T();
            _filePath = GetLocalFilePath(temp.FileName);

            _cache = ReadCache();
            Task.Run(WriteCache, _token);
        }

        public T GetCache()
        {
            return _cache;
        }

        public T SaveCache(T cache)
        {
            _textToWrite.Enqueue(JsonConvert.SerializeObject(cache,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto }));

            _cache = cache;
            return _cache;
        }

        private static string GetLocalFilePath(string fileName)
        {
            var path = @$"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\GoodVibes";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return Path.Combine(path, fileName);
        }

        private T ReadCache()
        {
            return (File.Exists(_filePath)
                ? JsonConvert.DeserializeObject<T>(File.ReadAllText(_filePath),
                    new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto })
                : new T())!;
        }

        private async void WriteCache()
        {
            while (true)
            {
                if (_token.IsCancellationRequested)
                {
                    return;
                }

                while (_textToWrite.TryDequeue(out var cacheStr))
                {
                    try
                    {
                        // ReSharper disable once MethodSupportsCancellation
                        await File.WriteAllTextAsync(_filePath, cacheStr);
                    }
                    catch (Exception e)
                    {
                        // Whelp, nothing more we can do if an error occurs here.
                        Console.WriteLine("Failed write cache to disk", e);
                    }
                }

                Thread.Sleep(100);
            }
        }
    }
}