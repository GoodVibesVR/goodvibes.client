using System;
using System.IO;
using GoodVibes.Client.Wpf.Services.Abstractions;
using Microsoft.Win32;

namespace GoodVibes.Client.Wpf.Services;

public class DialogService : IDialogService
{
    public string OpenJsonFileDialog()
    {
        var openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "GoodVibes Profile|*.json";
        return openFileDialog.ShowDialog() == true 
            ? File.ReadAllText(openFileDialog.FileName) : string.Empty;
    }
}