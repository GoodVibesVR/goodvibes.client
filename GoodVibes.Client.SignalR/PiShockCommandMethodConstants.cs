namespace GoodVibes.Client.SignalR;

public static class PiShockCommandMethodConstants
{
    /// <summary>
    /// Used to receive the connection acknowledgement.
    /// </summary>
    public static string ConnectionAck => "ConnectionAck";

    /// <summary>
    /// Used to send a ping request to the server. The response will be returned on <see cref="Pong"/>
    /// </summary>
    public static string Ping => "Ping";

    /// <summary>
    /// Used to receive the response from <see cref="Ping"/>
    /// </summary>
    public static string Pong => "Pong";

    /// <summary>
    /// Used to send a Shock request to a PiShock shocker. Response will be returned on <see cref="ShockResponse"/>
    /// </summary>
    public static string Shock => "Shock";

    /// <summary>
    /// Used to receive the response from <see cref="Shock"/>
    /// </summary>
    public static string ShockResponse => "ShockResponse";

    /// <summary>
    /// Used to send a Vibrate request to a PiShock shocker. Response will be returned on <see cref="VibrateResponse"/>
    /// </summary>
    public static string Vibrate => "Vibrate";

    /// <summary>
    /// Used to receive the response from <see cref="Vibrate"/>
    /// </summary>
    public static string VibrateResponse => "VibrateResponse";

    /// <summary>
    /// Used to send a Beep request to a PiShock shocker. Response will be returned on <see cref="BeepResponse"/>
    /// </summary>
    public static string Beep => "Beep";

    /// <summary>
    /// Used to receive the response from <see cref="Beep"/>
    /// </summary>
    public static string BeepResponse => "BeepResponse";
}