using Microsoft.AspNetCore.Identity;

namespace ModernRecrut.MVC.Areas.Identity.Data;
public enum TypeOccupation
{
    Candidat,
    Employe,
}

// Add profile data for application users by adding properties to the ModernRecrutMVCUser class
public class ModernRecrutMVCUser : IdentityUser
{
    public TypeOccupation? Type { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
}

