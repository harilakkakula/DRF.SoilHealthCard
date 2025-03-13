using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DRF.SoilHealthCard.API.Model
{
    public class User : IdentityUser<int>
    {
        public virtual ICollection<IdentityUserClaim<int>>? Claims { get; set; }
        [Column("IsActive", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsActive { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string? Name { get; set; }
        public List<RefreshToken>? RefreshTokens { get; set; }
        public string? RoleName { get; set; }
        public int Role { get; set; }
        public string? RoUserName { get; set; }
        public int? RoUserId { get; set; }

    }
}
