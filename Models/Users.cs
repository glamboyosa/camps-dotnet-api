using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace campsApi
{
    public partial class Users
    {
        public long Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
    
        public string Passwordhash { get; set; }
        public string Passwordsalt { get; set; }
        public long Role { get; set; }
        public string Token { get; set; }

        public virtual Roles RoleNavigation { get; set; }
    }
}
