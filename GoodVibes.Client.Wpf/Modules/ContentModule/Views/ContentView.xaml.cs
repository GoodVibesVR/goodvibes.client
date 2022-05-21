using System.Windows.Controls;
using GoodVibes.Client.Core;
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

            //RegionManager.SetRegionName(this.HeaderContent, "ContentHeaderView");
            //RegionManager.SetRegionManager(this.HeaderContent, regionManager);
        }
    }
}
