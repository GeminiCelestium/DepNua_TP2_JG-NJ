using System.ComponentModel.DataAnnotations;

namespace ModernRecrut.MVC.Utilities.Validation
{
    public class Extensions : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            IFormFile extension = (IFormFile)value;

            if (extension.FileName.Split(".").Last().ToLower() == "pdf" || extension.FileName.Split(".").Last().ToLower() == "docx" || extension.FileName.Split(".").Last().ToLower() == "doc")
            {
                return true;
            }

            return false;
        }
    }
}
