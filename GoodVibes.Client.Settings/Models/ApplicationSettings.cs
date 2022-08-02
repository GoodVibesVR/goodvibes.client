﻿namespace GoodVibes.Client.Settings.Models
{
    public class ApplicationSettings
    {
        public string? GoodVibesRoot { get; set; }
        public SignalRSettings? SignalRSettings { get; set; }
    }

    public class GoodVibesCache
    {
        public LovenseCache? LovenseCache { get; set; }
        public PiShockCache? PiShockCache { get; set; }
        public AvatarMapperCache? AvatarMapperCache { get; set; }
    }

    public class LovenseCache
    {

    }

    public class PiShockCache
    {

    }

    public class AvatarMapperCache
    {

    }
}
