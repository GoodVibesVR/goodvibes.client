using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoodVibes.Client.Wpf.Modules.ContentHeaderModule.Views
{
    /// <summary>
    /// Interaction logic for Top.xaml
    /// </summary>
    public partial class ContentHeaderView : UserControl
    {
        public ContentHeaderView()
        {
            InitializeComponent();
        }

        private void CommandBinding_CanExecute_1(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CanExecuteCloseWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            Console.WriteLine("ContentHeaderView CanExecuteCloseWindow()");
            e.CanExecute = true;
        }

        private void onCloseWindowCommand(object sender, ExecutedRoutedEventArgs e)
        {
            Console.WriteLine("ContentHeaderView CloseWindowCommand()");
            SystemCommands.CloseWindow(Window.GetWindow(this));
        }

        private void CommandBinding_Executed_2(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(Window.GetWindow(this));
        }

        private void CommandBinding_Executed_3(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(Window.GetWindow(this));
        }
    }
}
