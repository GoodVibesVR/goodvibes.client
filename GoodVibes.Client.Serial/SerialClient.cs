using System.IO.Ports;
using System.Text.RegularExpressions;
using GoodVibes.Client.Common.Extensions;
using GoodVibes.Client.Serial.Callbacks;
using GoodVibes.Client.Settings.Models;
using Microsoft.Win32;

namespace GoodVibes.Client.Serial;

public class SerialClient
{
    private readonly SerialSettings _serialSettings;

    private SerialPort serialPort = null!;
    private Thread _thread = null!;
    //private string receivedData = null!;

    public SerialClient(ApplicationSettings applicationSettings)
    {
        _serialSettings = applicationSettings.SerialSettings!;

        var comPorts = new List<string>();
        foreach (var serialSettingsDeviceId in _serialSettings.DeviceIds)
        {
            var vidAndPid = serialSettingsDeviceId.Split(":");
            comPorts.AddRange(GetPiShockComPorts(Convert.ToInt32(vidAndPid[0]), Convert.ToInt32(vidAndPid[1])));
        }

        if (comPorts.Any())
        {
            ConnectToPort(comPorts[0]);
        }
    }

    static IEnumerable<string> GetPiShockComPorts(int vid, int pid)
    {
        var pattern = $"^VID_{vid:X}.PID_{pid:X}";
        var rx = new Regex(pattern, RegexOptions.IgnoreCase);
        var comports = new List<string>();

        var rk1 = Registry.LocalMachine;
        var rk2 = rk1.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum");

        foreach (var s3 in rk2.GetSubKeyNames())
        {
            var rk3 = rk2.OpenSubKey(s3);
            foreach (var s in rk3.GetSubKeyNames())
            {
                if (rx.Match(s).Success)
                {
                    var rk4 = rk3.OpenSubKey(s);
                    foreach (var s2 in rk4.GetSubKeyNames())
                    {
                        var rk5 = rk4.OpenSubKey(s2);
                        var location = (string)rk5.GetValue("LocationInformation");
                        var rk6 = rk5.OpenSubKey("Device Parameters");
                        var portName = (string)rk6.GetValue("PortName");
                        if (!string.IsNullOrEmpty(portName) && SerialPort.GetPortNames().Contains(portName))
                            comports.Add((string)rk6.GetValue("PortName"));
                    }
                }
            }
        }

        return comports;
    }

    private void ConnectToPort(string comPort)
    {
        serialPort = new SerialPort();
        serialPort.BaudRate = _serialSettings.BaudRate;
        serialPort.PortName = comPort;
        serialPort.Parity = Parity.None;
        serialPort.DataBits = 8;
        serialPort.StopBits = StopBits.One;
        serialPort.ReadBufferSize = 20000000;
        serialPort.DataReceived += SerialPort_DataReceived;

        serialPort.Open();
    }

    private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        var data = serialPort.ReadLine();
        var correctJson = data.TryParseJson<PiShockSerialCallback>(out var correctData);
        if (correctJson)
        {
            if (correctData.Commands.Any())
            {
                
            }
        }
    }
}
