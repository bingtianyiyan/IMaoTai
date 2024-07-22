using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace IMaoTai.Core.Domain
{
    public class UserLogin
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class CurrentUser
    {
        public string UserName { get; set; }
        public bool IsAuthenticated { get; set; }
        public IEnumerable<CaviarClaim> Claims { get; set; }
    }

    /// <summary>
    /// 防止递归循环
    /// </summary>
    public class CaviarClaim
    {
        public CaviarClaim(Claim claim)
        {
            Type = claim.Type;
            Value = claim.Value;
        }

        public CaviarClaim(string type, string value)
        {
            Type = type;
            Value = value;
        }

        public CaviarClaim()
        {

        }

        public string Type { get; set; }

        public string Value { get; set; }
    }
}