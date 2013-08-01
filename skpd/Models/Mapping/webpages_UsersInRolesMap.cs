using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace skpd.Models.Mapping
{
    public class webpages_UsersInRolesMap : EntityTypeConfiguration<webpages_UsersInRoles>
    {
        public webpages_UsersInRolesMap()
        {
            // Primary Key
            this.HasKey(t => t.UserInRoleId);

            // Properties
            // Table & Column Mappings
            this.ToTable("webpages_UsersInRoles");
            this.Property(t => t.UserInRoleId).HasColumnName("UserInRoleId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.webpages_UsersInRoles)
                .HasForeignKey(d => d.UserId);
            this.HasRequired(t => t.webpages_Roles)
                .WithMany(t => t.webpages_UsersInRoles)
                .HasForeignKey(d => d.RoleId);

        }
    }
}
