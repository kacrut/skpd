using System;
using System.Collections.Generic;

namespace skpd.Models
{
    public partial class webpages_UsersInRoles
    {
        public int UserInRoleId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public virtual User User { get; set; }
        public virtual webpages_Roles webpages_Roles { get; set; }
    }
}
