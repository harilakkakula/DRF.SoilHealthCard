using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DRF.SoilHealthCard.API.Model
{
    [Table("LoginHistory")]
    public class LoginHistory : LoginLogger
    {
        [Key]
        public int Id { get; set; }
    }

    public class LoginLogger
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
        public DateTime LoginDate { get; set; }
        public string? IPAddress { get; set; }
        public string? Source { get; set; }

    }
}
