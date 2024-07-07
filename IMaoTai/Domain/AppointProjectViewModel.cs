using IMaoTai.Entity;

namespace IMaoTai.Domain
{
    /// <summary>
    /// 预约项目页面对应的VM
    /// </summary>
    public class AppointProjectViewModel
    {
        public static List<ProductEntity> ProductList { get; set; } = new List<ProductEntity>();
    }

    public class AppointProjectListModel
    {
        public List<ProductEntity> ProductList { get; set; } = new List<ProductEntity>();
    }
}