using System.Windows.Controls;

namespace GoodVibes.Client.Wpf.Modules.AvatarMapperModule.Views
{
    /// <summary>
    /// Interaction logic for AvatarMapperView.xaml
    /// </summary>
    public partial class AvatarMapperView : UserControl
    {
        public AvatarMapperView()
        {
            InitializeComponent();
        }

        private void OSCParamaterText_TextChanged(object sender, TextChangedEventArgs e)
        {
            hintOSCParamaterText.Visibility = System.Windows.Visibility.Visible;
            if (OSCParamaterText.Text.Length > 0)
                hintOSCParamaterText.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}