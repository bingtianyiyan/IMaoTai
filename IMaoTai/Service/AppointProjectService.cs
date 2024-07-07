using IMaoTai.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IMaoTai.Service
{
    public class AppointProjectService:IAppointProjectService
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
