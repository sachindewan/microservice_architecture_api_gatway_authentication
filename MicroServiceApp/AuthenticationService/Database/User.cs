using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Database
{
    public class User:IdentityUser<int>
    {
        public User()
        {
            CreatedDate = DateTime.Now;
        }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
