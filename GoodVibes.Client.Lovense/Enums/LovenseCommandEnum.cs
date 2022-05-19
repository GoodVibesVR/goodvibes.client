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
    /// Starts the pump (Max)
    /// v:Strength (Between 0 and 3)
    /// /// t:Toy's ID (optional)
    /// </summary>
    Pump = 15
}