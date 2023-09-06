namespace ModernRecrut.MVC.Helpers
{
    public class ConversionDocument
    {
        public static string ConvertirDocumentEnString(IFormFile image)
        {
            using (var ms = new MemoryStream())
            {
                image.CopyTo(ms);
                var fileBytes = ms.ToArray();
                string str = Convert.ToBase64String(fileBytes);
                // act on the Base64 data

                return str;
            }
        }
    }
}
