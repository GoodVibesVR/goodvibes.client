using GoodVibes.Client.Settings.Enums;
using Newtonsoft.Json;

namespace GoodVibes.Client.Settings
{
    public class SettingsManager<T> where T : class
    {
        private readonly string _filePath;

        public SettingsManager(string fileName, SettingsLocationEnum settingsLocation = SettingsLocationEnum.AppData)
        {
            _filePath = GetLocalFilePath(settingsLocation, fileName);
        }

        private string GetLocalFilePath(SettingsLocationEnum settingsLocation, string fileName)
        {
            var path = string.Empty;
            switch (settingsLocation)
            {
                case SettingsLocationEnum.AppData:
                    path = @$"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\GoodVibes";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    break;
                case SettingsLocationEnum.ApplicationDirectory:
                    path = AppDomain.CurrentDomain.BaseDirectory;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return Path.Combine(path, fileName);
        }

        public T? LoadSettings() =>
            File.Exists(_filePath) ? JsonConvert.DeserializeObject<T>(File.ReadAllText(_filePath)) : null;

        public void SaveSettings(T settings)
        {
            string json = JsonConvert.SerializeObject(settings);
            File.WriteAllText(_filePath, json);
        }
    }
}
