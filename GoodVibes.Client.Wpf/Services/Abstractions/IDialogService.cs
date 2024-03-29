﻿using System.Drawing;

namespace GoodVibes.Client.Wpf.Services.Abstractions;

public interface IDialogService
{
    string OpenJsonFileDialog();
    void OpenJsonFileSaveDialog(string avatarId, string content);
}