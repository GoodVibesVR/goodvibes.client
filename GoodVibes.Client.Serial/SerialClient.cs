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

    private SerialPort _serialPort = null!;

    public SerialClient(ApplicationSettings applicationSettings)
    {
        _serialSettings = applicationSettings.SerialSettings!;

        var comPorts = new List<string>();
        foreach (var serialSettingsDeviceId in _serialSettings.DeviceIds!)
        {
            var vidAndPid = serialSettingsDeviceId.Split(":");
            comPorts.AddRange(GetPiShockComPorts(Convert.ToInt32(vidAndPid[0]), Convert.ToInt32(vidAndPid[1])));
        }

        if (comPorts.Any())
        {
            ConnectToPort(comPorts[0]);
        }
    }

    private static IEnumerable<string> GetPiShockComPorts(int vid, int pid)
    {
        var pattern = $"^VID_{vid:X}.PID_{pid:X}";
        var rx = new Regex(pattern, RegexOptions.IgnoreCase);
        var comports = new List<string>();

        var rk1 = Registry.LocalMachine;
        var rk2 = rk1.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum");
        if (rk2 == null) return comports;

        foreach (var s3 in rk2.GetSubKeyNames())
        {
            var rk3 = rk2.OpenSubKey(s3);
            if (rk3 == null) continue;

            foreach (var s in rk3.GetSubKeyNames())
            {
                if (!rx.Match(s).Success) continue;

                var rk4 = rk3.OpenSubKey(s);
                if (rk4 == null) continue;

                foreach (var s2 in rk4.GetSubKeyNames())
                {
                    var rk5 = rk4.OpenSubKey(s2);
                    var rk6 = rk5?.OpenSubKey("Device Parameters");
                    if (rk6 == null) continue;

                    var portName = (string)rk6.GetValue("PortName")!;
                    if (string.IsNullOrEmpty(portName) || !SerialPort.GetPortNames().Contains(portName)) continue;

                    var comPort = (string)rk6.GetValue("PortName")!;
                    if (string.IsNullOrEmpty(comPort)) continue;

                    comports.Add(comPort);
                }
            }
        }

        return comports;
    }

    private void ConnectToPort(string comPort)
    {
        _serialPort = new SerialPort();
        _serialPort.BaudRate = _serialSettings.BaudRate;
        _serialPort.PortName = comPort;
        _serialPort.Parity = Parity.None;
        _serialPort.DataBits = 8;
        _serialPort.StopBits = StopBits.One;
        _serialPort.ReadBufferSize = 20000000;
        _serialPort.DataReceived += SerialPort_DataReceived;
        _serialPort.ErrorReceived += _serialPort_ErrorReceived;

        _serialPort.Open();
    }

    private void _serialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
    {
        // TODO: At this point dispose and reinitialize?
    }

    private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        var data = _serialPort.ReadLine();
        var correctJson = data.TryParseJson<PiShockSerialCallback>(out var correctData);
        if (correctJson)
        {
            if (correctData.Commands.Any())
            {
                // TODO: Do stuff
            }
        }
    }
}
