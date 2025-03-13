using Microsoft.AspNetCore.Identity;

namespace DRF.SoilHealthCard.API.Model
{
    public class Role : IdentityRole<int>
    {
        public virtual ICollection<UserRole>? UserRoles { get; set; }

    }
}
