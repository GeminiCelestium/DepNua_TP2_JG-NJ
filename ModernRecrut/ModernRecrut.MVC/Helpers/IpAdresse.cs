using System.Net;

namespace ModernRecrut.MVC.Helpers
{
    public class IpAdresse
    {
        public static string GetIpAdress()
        {
            string host = Dns.GetHostName();

            IPHostEntry ip = Dns.GetHostEntry(host);
            return ip.AddressList[2].ToString();
        }
    }
}
