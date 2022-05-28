using System.Windows.Controls;
using Prism.Regions;

namespace GoodVibes.Client.Wpf.Modules.ContentModule.Views
{
    /// <summary>
    /// Interaction logic for ContentView.xaml
    /// </summary>
    public partial class ContentView : UserControl
    {
        public ContentView(IRegionManager regionManager)
        {
            InitializeComponent();
        }
    }
}
