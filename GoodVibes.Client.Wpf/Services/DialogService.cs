using System.IO;
using Microsoft.Win32;
using Prism.Services.Dialogs;
using IDialogService = GoodVibes.Client.Wpf.Services.Abstractions.IDialogService;

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

    public void OpenJsonFileSaveDialog(string avatarId, string content)
    {
        var saveFileDialog = new SaveFileDialog()
        {
            Title = "Save GoodVibes mapping profile",
            CheckFileExists = false,
            CheckPathExists = true,
            DefaultExt = "json",
            FileName = $"GoodVibes-Profile_{avatarId}.json",
            Filter = "GoodVibes Profile(*.json)|*.json|All files (*.*)|*.*"
        };
        
        if (saveFileDialog.ShowDialog() == true)
        {
            File.WriteAllText(saveFileDialog.FileName, content);
        }
    }
}