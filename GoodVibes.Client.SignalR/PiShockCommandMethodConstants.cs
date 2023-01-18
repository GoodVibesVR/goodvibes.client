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
    /// Used to fetch PiShock information. Response will be returned on <see cref="GetPiShockInformationResponse"/>
    /// </summary>
    public static string GetPiShockInformation => "GetPiShockInformation";

    /// <summary>
    /// Used to receive the response from <see cref="GetPiShockInformation"/>
    /// </summary>
    public static string GetPiShockInformationResponse => "GetPiShockInformationResponse";

    /// <summary>
    /// Used to pause PiShock from receiving any more actions. Response will be returned on <see cref="PausePiShockResponse"/>
    /// </summary>
    public static string PausePiShock => "PausePiShock";

    /// <summary>
    /// Used to receive the response from <see cref="PausePiShock"/>
    /// </summary>
    public static string PausePiShockResponse => "PausePiShockResponse";

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

    /// <summary>
    /// Used to fetch the status of the PiVault. Response will be returned on <see cref="GetPiVaultStatusResponse"/>
    /// </summary>
    public static string GetPiVaultStatus => "GetPiVaultStatus";

    /// <summary>
    /// Used to receive the response from <see cref="GetPiVaultStatus"/>
    /// </summary>
    public static string GetPiVaultStatusResponse => "GetPiVaultStatusResponse";

    /// <summary>
    /// Used to fetch the API key permissions. Response till be returned on <see cref="GetApiKeyPermissionsResponse"/>
    /// </summary>
    public static string GetApiKeyPermissions => "GetApiKeyPermissions";

    /// <summary>
    /// Used to receive the response from <see cref="GetApiKeyPermissions"/>
    /// </summary>
    public static string GetApiKeyPermissionsResponse => "GetApiKeyPermissionsResponse";

    /// <summary>
    /// Used to set the unlock time of the PiVault. Response will be returned on <see cref="SetUnlockTimeResponse"/>
    /// </summary>
    public static string SetUnlockTime => "SetUnlockTime";

    /// <summary>
    /// Used to receive the response from <see cref="SetUnlockTime"/>
    /// </summary>
    public static string SetUnlockTimeResponse => "SetUnlockTimeResponse";

    /// <summary>
    /// Used to clear the current PiVault session. Response will be returned on <see cref="ClearCurrentSessionResponse"/>
    /// </summary>
    public static string ClearCurrentSession => "ClearCurrentSession";

    /// <summary>
    /// Used to receive the response from <see cref="ClearCurrentSession"/>
    /// </summary>
    public static string ClearCurrentSessionResponse => "ClearCurrentSessionResponse";

    /// <summary>
    /// Used to unlock the PiVault. Response will be returned on <see cref="UnlockPiVaultResponse"/>
    /// This either unlocks the lockbox (if "Mode" is UnlockNextPull or UnlockAfterDelay) or allows the lockbox to be manually unlocked with the button (if "Mode" is AllowManualUnlock)
    /// </summary>
    public static string UnlockPiVault => "UnlockPiVault";

    /// <summary>
    /// Used to receive the response from <see cref="UnlockPiVault"/>
    /// </summary>
    public static string UnlockPiVaultResponse => "UnlockPiVaultResponse";

    /// <summary>
    /// Adds a set amount of minutes to the PiVault session. Response will be returned on <see cref="AddMinutesToSessionResponse"/>
    /// </summary>
    public static string AddMinutesToSession => "AddMinutesToSession";

    /// <summary>
    /// Used to receive the response from <see cref="AddMinutesToSession"/>
    /// </summary>
    public static string AddMinutesToSessionResponse => "AddMinutesToSessionResponse";

    /// <summary>
    /// Removes a set amount of minutes to the PiVault session. Response will be returned on <see cref="RetractMinutesFromSessionResponse"/>
    /// </summary>
    public static string RetractMinutesFromSession => "RetractMinutesFromSession";

    /// <summary>
    /// Used to receive the response from <see cref="RetractMinutesFromSession"/>
    /// </summary>
    public static string RetractMinutesFromSessionResponse => "RetractMinutesFromSessionResponse";

    /// <summary>
    /// Adds a set amount of hours to the PiVault session. Response will be returned on <see cref="AddHoursToSessionResponse"/>
    /// </summary>
    public static string AddHoursToSession => "AddHoursToSession";

    /// <summary>
    /// Used to receive the response from <see cref="AddHoursToSession"/>
    /// </summary>
    public static string AddHoursToSessionResponse => "AddHoursToSessionResponse";

    /// <summary>
    /// Removes a set amount of hours from the PiVault session. Response will be returned on <see cref="RetractHoursFromSessionResponse"/>
    /// </summary>
    public static string RetractHoursFromSession => "RetractHoursFromSession";

    /// <summary>
    /// Used to receive the response from <see cref="RetractHoursFromSession"/>
    /// </summary>
    public static string RetractHoursFromSessionResponse => "RetractHoursFromSessionResponse";

    /// <summary>
    /// Adds a set amount of days to the PiVault session. Response will be returned on <see cref="AddDaysToSessionResponse"/>
    /// </summary>
    public static string AddDaysToSession => "AddDaysToSession";

    /// <summary>
    /// Used to receive the response from <see cref="AddDaysToSession"/>
    /// </summary>
    public static string AddDaysToSessionResponse => "AddDaysToSessionResponse";

    /// <summary>
    /// Removes a set amount of days from the PiVault session. Response will be returned on <see cref="RetractDaysFromSessionResponse"/>
    /// </summary>
    public static string RetractDaysFromSession => "RetractDaysFromSession";

    /// <summary>
    /// Used to receive the response from <see cref="RetractDaysFromSession"/>
    /// </summary>
    public static string RetractDaysFromSessionResponse => "RetractDaysFromSessionResponse";
}