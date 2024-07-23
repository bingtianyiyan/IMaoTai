using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql.DataAnnotations;

namespace IMaoTai.Core.Entity
{
    public class LoginUserEntity
    {
        [Column(IsPrimary = true)] // 指定这个属性是主键
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
