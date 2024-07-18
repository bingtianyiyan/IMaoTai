using System.Windows;
using IMaoTai.Helpers;

namespace IMaoTai
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            await IocHelper.InitBusiness();
        }
    }
}