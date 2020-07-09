using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcWebUIClient.Models
{
    public class UserViewModel
    {
        [Required]
        [StringLength(maximumLength:20,MinimumLength =8,ErrorMessage ="please specified agreed leanth")]
        public string UserName { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
