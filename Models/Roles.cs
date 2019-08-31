using System;
using System.Collections.Generic;

namespace campsApi
{
    public partial class Roles
    {
        public Roles()
        {
            Users = new HashSet<Users>();
        }

        public long Id { get; set; }
        public string Role { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
