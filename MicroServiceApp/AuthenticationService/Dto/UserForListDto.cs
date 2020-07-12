using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Dto
{
    public class UserForListDto
    {
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string[] Roles { get; set; }
    }
}
