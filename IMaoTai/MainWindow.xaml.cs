using IMaoTai.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace IMaoTai
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Resources.SetIoc();
            InitializeComponent();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddWpfBlazorWebView();
            serviceCollection.AddMasaBlazor();

#if DEBUG
            serviceCollection.AddBlazorWebViewDeveloperTools();
#endif
        }
    }
}