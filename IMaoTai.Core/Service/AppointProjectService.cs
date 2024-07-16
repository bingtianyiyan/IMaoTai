using IMaoTai.Core.Domain;

namespace IMaoTai.Core.Service
{
    public class AppointProjectService : IAppointProjectService
    {
        public async Task RefreshProduct()
        {
            CommonX.MtSessionId = string.Empty;
            CommonX.WriteCache("mtSessionId.txt", string.Empty);
            AppointProjectViewModel.ProductList.Clear();
            await IMTService.GetCurrentSessionId();
        }
    }
}