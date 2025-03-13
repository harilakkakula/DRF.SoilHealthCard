using Microsoft.AspNetCore.Identity;

namespace DRF.SoilHealthCard.API.Model
{
    public class UserRole : IdentityUserRole<int>
    {
        public int Id { get; set; }
        public virtual User? User { get; set; }
        public virtual Role? Role { get; set; }
    }
}
