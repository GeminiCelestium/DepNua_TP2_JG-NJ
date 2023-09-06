using System.ComponentModel;

namespace ModernRecrut.MVC.Models
{
    public class RoleViewModel
    {
        [DisplayName("ID")]
        public string? RoleId { get; set; }
        [DisplayName("Nom")]
        public string RoleName { get; set; }
    }
}
