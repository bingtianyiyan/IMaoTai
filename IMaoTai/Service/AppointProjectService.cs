using IMaoTai.Domain;

namespace IMaoTai.Service
{
    public class AppointProjectService : IAppointProjectService
    {
        public async Task RefreshProduct()
        {
            App.MtSessionId = string.Empty;
            App.WriteCache("mtSessionId.txt", string.Empty);
            AppointProjectViewModel.ProductList.Clear();
            await IMTService.GetCurrentSessionId();
        }
    }
}