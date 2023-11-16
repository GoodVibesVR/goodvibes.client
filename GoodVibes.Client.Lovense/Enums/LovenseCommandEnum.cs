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
    /// Vibrate3 the toy (Lapis)
    /// v:Speed (Between 0 and 20)
    /// t:Toy's ID (optional)
    /// </summary>
    Vibrate3 = 19,

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
    Pump = 15,

    /// <summary>
    /// Alternative to Pump (Max)?
    /// v:Strength (Between 0 and 20)
    /// </summary>
    Suction = 16,

    /// <summary>
    /// Starts the thrusting motion (Gravity)
    /// v:Strength (Between 0 and 20)
    /// </summary>
    Thrusting = 17,

    /// <summary>
    /// Starts the fingering motion (Flexer)
    /// v:Strength (Between 0 and 10)
    /// </summary>
    Fingering = 18,

    /// <summary>
    /// Sets the depth (Solace)
    /// v:Strength (Between 0 and 3)
    /// </summary>
    Depth = 20
}