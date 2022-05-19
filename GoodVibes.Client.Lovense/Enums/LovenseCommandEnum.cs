namespace GoodVibes.Client.Lovense.Enums;

public enum LovenseCommandEnum
{
    /// <summary>
    /// Occurs if nothing is sent in.
    /// This will cause an exception
    /// </summary>
    None = 0,

    /// <summary>
    /// Vibrate the toy	
    /// v:Speed (Between 0 and 20)
    /// t:Toy's ID (optional)
    /// </summary>
    Vibrate = 10,

    /// <summary>
    /// Vibrate1 the toy (Edge)
    /// v:Speed (Between 0 and 20)
    /// t:Toy's ID (optional)
    /// </summary>
    Vibrate1 = 11,

    /// <summary>
    /// Vibrate2 the toy (Edge)
    /// v:Speed (Between 0 and 20)
    /// t:Toy's ID (optional)
    /// </summary>
    Vibrate2 = 12,

    /// <summary>
    /// Rotate the toy (Nora)
    /// v:Speed (Between 0 and 20)
    /// t:Toy's ID (optional)
    /// </summary>
    Rotate = 13,

    /// <summary>
    /// Rotate the toy anti-clockwise (Nora)
    /// v:Speed (Between 0 and 20)
    /// t:Toy's ID (optional)
    /// </summary>
    RotateAntiClockwise = 14,

    /// <summary>
    /// Rotate the toy clockwise (Nora)
    /// v:Speed (Between 0 and 20)
    /// t:Toy's ID (optional)
    /// </summary>
    RotateClockwise = 15,

    /// <summary>
    /// change the rotation direction (Nora)
    /// t:Toy's ID (optional)
    /// </summary>
    RotateChange = 16,

    /// <summary>
    /// Start contraction (Max)
    /// v:Speed (Between 0 and 3)
    /// t:Toy's ID (optional)
    /// </summary>
    AirAuto = 17,

    /// <summary>
    /// Pump in the air (Max)
    /// t:Toy's ID (optional)
    /// </summary>
    AirIn = 18,

    /// <summary>
    /// Start contraction (Max)
    /// t:Toy's ID (optional)
    /// </summary>
    AirOut = 19,

    /// <summary>
    /// Vibrate the toy by predefined patterns
    /// v:pattern (Between 0 and 3)
    /// t:Toy's ID (optional)
    /// </summary>
    Preset = 20,

    /// <summary>
    /// Get battery status. Return value is between 0 and 100.
    /// t:Toy's ID (optional)
    /// </summary>
    Battery = 21,

    /// <summary>
    /// Vibrate the toy for several seconds (Android Connect 2.3.8+, iOS Connect 2.2.6+)
    /// v:Speed (Between 0 and 20)
    /// sec:Time in Seconds
    /// t:Toy's ID (optional)
    /// </summary>
    AVibrate = 22,

    /// <summary>
    /// Rotate the toy for several seconds (Nora; Android Connect 2.3.8+, iOS Connect 2.2.6+)
    /// r:Speed (Between 0 and 20)
    /// sec:Time in Seconds
    /// t:Toy's ID (optional)
    /// </summary>
    ARotate = 23,

    /// <summary>
    /// Start contraction for several seconds (Max; Android Connect 2.3.8+, iOS Connect 2.2.6+)
    /// a:Speed (Between 0 and 3)
    /// sec:Time in Seconds
    /// t:Toy's ID (optional)
    /// </summary>
    AAirLevel = 24,

    /// <summary>
    /// Start vibration and rotation for several seconds (Nora; Android Connect 2.3.8+, iOS Connect 2.2.6+)
    /// v:Vibrate Speed (Between 0 and 20)
    /// r:Rotate Speed (Between 0 and 20)
    /// sec:Time in Seconds
    /// t:Toy's ID (optional)
    /// </summary>
    AVibRotate = 25,

    /// <summary>
    /// Start contraction for several seconds (Max; Android Connect 2.3.8+, iOS Connect 2.2.6+)
    /// v:Vibrate Speed (Between 0 and 20)
    /// a:Contraction Speed (Between 0 and 3)
    /// sec:Time in Seconds
    /// t:Toy's ID (optional)
    /// </summary>
    AVibAir = 26,

    /// <summary>
    /// Vibrate the toy for several seconds (Edge; Android Connect 2.3.8+, iOS Connect 2.2.6+)
    /// v:Speed (Between 0 and 20)
    /// sec:Time in Seconds
    /// t:Toy's ID (optional)
    /// </summary>
    AVibrate1 = 27,

    /// <summary>
    /// Vibrate the toy for several seconds (Edge; Android Connect 2.3.8+, iOS Connect 2.2.6+)
    /// v:Speed (Between 0 and 20)
    /// sec:Time in Seconds
    /// t:Toy's ID (optional)
    /// </summary>
    AVibrate2 = 28
}